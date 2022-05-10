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
using System.Threading.Tasks;
using static ImageResizingApp.Models.Interfaces.IColumn;
using ImageResizingApp.Models.QualityAssessment;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    internal class OracleColumn : IColumn
    {
        public ITable Table { get; set; }

        public string Name { get; set; }

        public string ColumnType { get; set; }

        public bool Resizable { get; set; }

        private readonly OracleConnection _connection;

        public event EventHandler<IColumn.ProgressChangedEventHandler> ProgressChanged;

        public OracleColumn(ITable table, OracleConnection connection)
        {
            Table = table;
            _connection = connection;
        }
        public void Resize(int? from, int? to, int? minSize, int? maxSize, IFilter filter, string backupDestination)
        {
            ImageQualityAssessment iqa = new HotelDieuIQA();
            var finalFrom = from ?? 0;
            string minSizeCondition = minSize != null ? ("dbms_lob.getlength(" + Name + ")>" + minSize) : "";
            string finalSizeCondition = "";
            string maxSizeCondition = maxSize != null ? ("dbms_lob.getlength(" + Name + ")<" + maxSize) : "";
            if (minSizeCondition != "")
            {
                finalSizeCondition += minSizeCondition;
                if (maxSizeCondition != "")
                {
                    finalSizeCondition += " AND " + maxSizeCondition;
                }
            }
            else if (maxSizeCondition != "")
            {
                finalSizeCondition += maxSizeCondition;
            }

            List<string> pKs = new List<string>();
            int n = Table.PrimaryKeys.Count();

            string sqlSelect = "SELECT " + String.Join(",", Table.PrimaryKeys) + ", " + Name + " FROM (SELECT ROWNUM RNUM, a.* FROM " + Table.Name + " a" + (to == null ? (finalSizeCondition != "" ? (" WHERE " + finalSizeCondition) : "") : (" WHERE ROWNUM<=" + to + (finalSizeCondition!="" ? (" AND " + finalSizeCondition): "" ))) + ")";
            sqlSelect += " WHERE RNUM>=" + finalFrom;
            OracleCommand selectCmd = new OracleCommand(sqlSelect, _connection);

            string sqlCount = "SELECT COUNT(*) FROM (SELECT ROWNUM RNUM, a.* FROM " + Table.Name + " a" + (to == null ? (finalSizeCondition != "" ? (" WHERE " + finalSizeCondition) : "") : (" WHERE ROWNUM<=" + to + (finalSizeCondition != "" ? (" AND " + finalSizeCondition) : ""))) + ")";
            sqlCount += " WHERE RNUM>=" + finalFrom;
            OracleCommand cmd = new OracleCommand(sqlCount, _connection);

            try
            {
                decimal totalCount = (decimal)(cmd.ExecuteScalar());
                OracleDataReader dr = selectCmd.ExecuteReader();
                int counter = 0;
                while (dr.Read())
                {
                    counter++;
                    if (ProgressChanged != null)
                    {
                        int res = (int)(counter / totalCount * 100);
                        ProgressChanged(this, new ProgressChangedEventHandler(res));
                    }
                    pKs.Clear();
                    for (int i = 0; i < n; i++)
                    {
                        pKs.Add(dr.GetString(i));
                    }

                    OracleBlob blob = dr.GetOracleBlob(n);

                    long blobSize = blob.Length;

                    MagickImage img;
                    MagickImage originalImg;
                    byte[] bytes = new byte[blobSize];
                    blob.Read(bytes, 0, (int)blobSize);
                    try
                    {
                        img = new MagickImage(bytes);
                        originalImg = new MagickImage(bytes);
                    }catch
                    {
                        continue;
                        //originalImg = new MagickImage();
                        //img = new MagickImage();
                    }

                    if (backupDestination != null && backupDestination.Length > 0)
                    {
                        try
                        {
                            originalImg.Write(backupDestination + "\\" + string.Join("-", pKs));
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    filter.Process(img);
                    byte[] finalBytes = img.ToByteArray();
                    if (iqa.Compare(originalImg, img)){
                        string finalPks = Utilities.GeneratePrimaryKeyValuePairs(Table.PrimaryKeys, pKs);
                        string sqlUpdate = "UPDATE " + Table.Name + " SET " + Name + " = :pBlob" + " WHERE " + finalPks;
                        OracleParameter param = new OracleParameter("pBlob", OracleDbType.Blob);
                        param.Direction = ParameterDirection.Input;
                        param.Value = finalBytes;

                        OracleTransaction transaction = _connection.BeginTransaction();
                        OracleCommand updateCommand = _connection.CreateCommand();
                        updateCommand.Transaction = transaction;
                        updateCommand.CommandText = sqlUpdate;
                        updateCommand.Parameters.Add(param);
                        try
                        {
                            updateCommand.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                        }
                        finally
                        {
                            transaction.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public IImage GetImageWithPrimaryKeysValues(IEnumerable<string> primaryValues)
        {
            return new OracleImage(this, _connection, primaryValues);
        }
    }
}
