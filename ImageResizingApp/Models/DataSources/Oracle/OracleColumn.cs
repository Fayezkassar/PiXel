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
using ImageResizingApp.Helpers;

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
        public void Resize(int? from, int? to, int? minSize, int? maxSize, IFilter filter, string backupDestination)
        {

                OracleTransaction transaction = _connection.BeginTransaction();

            OracleCommand updateCommand = _connection.CreateCommand();
            updateCommand.Transaction = transaction;
            try
            {
                var finalFrom = from ?? 0;
                string minSizeCondition = minSize!=null ? ("dbms_lob.getlength(" + Name + ")>" + minSize) : "";
                string finalSizeCondition = "";
                string maxSizeCondition = maxSize != null ? ("dbms_lob.getlength(" + Name + ")<" + maxSize) : "";
                if (minSizeCondition != "")
                {
                    finalSizeCondition+= minSizeCondition;
                    if (maxSizeCondition != "")
                    {
                        finalSizeCondition += " AND " + maxSizeCondition;
                    }
                }
                else if (maxSizeCondition != "")
                {
                    finalSizeCondition += maxSizeCondition;
                }


                string sqlSelect = "SELECT " + String.Join(",", Table.PrimaryKeys) + ", " + Name + " FROM (SELECT ROWNUM RNUM, a.* FROM " + Table.Name + " a" + (to == null ? (finalSizeCondition!="" ? (" WHERE " +finalSizeCondition) : "") : (" WHERE ROWNUM<=" + to + " AND "+ finalSizeCondition )) + ")";
                sqlSelect += " WHERE RNUM>=" + finalFrom;

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

                        MagickImage img;
                        byte[] bytes = new byte[blobSize];
                        blob.Read(bytes, 0, (int)blobSize);
                        img = new MagickImage(bytes);

                        if (backupDestination != null && backupDestination.Length > 0)
                        {
                            try
                            {
                                img.Write(backupDestination + "\\" + string.Join("-", pKs));
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        filter.Process(img);
                        byte[] finalBytes = img.ToByteArray();

                        string finalPks = Utilities.GeneratePrimaryKeyValuePairs(Table.PrimaryKeys, pKs);
                        string sqlUpdate = "UPDATE " + Table.Name + " SET " + Name + " = :pBlob" + " WHERE " + finalPks;
                        OracleParameter param = new OracleParameter("pBlob", OracleDbType.Blob);
                        param.Direction = ParameterDirection.Input;
                        param.Value = finalBytes;

                        updateCommand.Parameters.Clear();
                        updateCommand.CommandText = sqlUpdate;
                        updateCommand.Parameters.Add(param);
                        updateCommand.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public IImage GetImageWithPrimaryKeysValues(IEnumerable<string> primaryValues)
        {
            return new OracleImage(this, _connection, primaryValues);
        }
    }
}
