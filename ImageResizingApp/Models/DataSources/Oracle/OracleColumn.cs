using ImageResizingApp.Models.Interfaces;
using System;
using System.Linq;
using Oracle.ManagedDataAccess.Types;
using ImageMagick;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media.Imaging;
using System.IO;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    internal class OracleColumn : IColumn
    {
        public ITable Table { get; set; }

        public string Name { get; set; }

        public string ColumnType { get; set; }

        public bool Resizable { get; set; }

        private readonly OracleConnection _connection;

        public OracleColumn(ITable table, OracleConnection connection)
        {
            Table = table;
            _connection = connection;
        }
        public bool Resize(int? rowNumber, int? from, int? to, int? minSize, int? maxSize, IFilter filter, string backupDestination)
        {

            OracleTransaction transaction = _connection.BeginTransaction();

            OracleCommand updateCommand = _connection.CreateCommand();
            updateCommand.Transaction = transaction;
            try
            {
                string sqlSelect;
                if (rowNumber != null)
                {
                    sqlSelect = "SELECT " + String.Join(",", Table.PrimaryKeys) + ", DOC FROM (SELECT ROWNUM RNUM, a.* FROM " + Table.Name + " a  WHERE NIDOC=" + rowNumber + ")"; // TO REPLACE NIDOC WITH ROWNNUM MAYBE and DOC WITH NAME PARAM
                } else
                {
                    var finalFrom = from ?? 0;
                    sqlSelect = "SELECT " + String.Join(",", Table.PrimaryKeys) + ", " + Name + " FROM (SELECT ROWNUM RNUM, a.* FROM " + Table.Name + " a" + (to == null ? "" : (" WHERE ROWNUM<=" + to)) + ")";
                    sqlSelect += " WHERE RNUM>=" + finalFrom;
                    sqlSelect += " AND NIDOC=103896"; // to remove
                }

                OracleCommand cmd = new OracleCommand(sqlSelect, _connection);
                OracleDataReader dr = cmd.ExecuteReader();

                List<string> pKs = new List<string>();
                int n = Table.PrimaryKeys.Count();

                while (dr.Read())
                {
                    try
                    {
                        pKs.Clear();
                        for (int i = 0; i < n; i++)
                        {
                            pKs.Add(dr.GetString(i));
                        }

                        OracleBlob blob = dr.GetOracleBlob(n);

                        long blobSize = blob.Length;
                        if ((minSize != null && blobSize < minSize) || (maxSize != null && blobSize > maxSize))
                        {
                            continue;
                        }

                        MagickImage img;
                        byte[] bytes = new byte[blobSize];
                        blob.Read(bytes, 0, (int)blobSize);
                        img = new MagickImage(bytes);

                        if (backupDestination != null && backupDestination.Length > 0)
                        {
                            try
                            {
                                img.Write(backupDestination + "\\" + string.Join("-", pKs) + ".png"); //to remove
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        filter.Process(img);
                        byte[] finalBytes = img.ToByteArray();

                        string finalPks = "";
                        int j = 0;
                        foreach (string key in Table.PrimaryKeys)
                        {
                            if (j != 0) finalPks += " AND ";
                            finalPks += key + "=" + pKs[j];
                            ++j;
                        }
                        string sqlUpdate = "UPDATE " + Table.Name + " SET " + Name + " = :pBlob" + " WHERE " + finalPks;
                        OracleParameter param = new OracleParameter("pBlob", OracleDbType.Blob);
                        param.Direction = ParameterDirection.Input;
                        param.Value = finalBytes;

                        updateCommand.Parameters.Clear();
                        updateCommand.CommandText = sqlUpdate;
                        updateCommand.Parameters.Add(param);
                        updateCommand.ExecuteNonQuery();
                        transaction.Rollback(); //to remove
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public BitmapImage GetBitmapImage(DataRowView row)
        {
            string sql = "SELECT DOC FROM " + Table.Name + " WHERE NIDOC=103896";
            OracleCommand cmd = new OracleCommand(sql, _connection);
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                OracleBlob blob = dr.GetOracleBlob(0);
                byte[] bytes = new byte[blob.Length];
                blob.Read(bytes, 0, (int)blob.Length);
                //MagickImage img;
                //img = new MagickImage(bytes);
                return LoadImage(bytes);
            }
            return null;
        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
