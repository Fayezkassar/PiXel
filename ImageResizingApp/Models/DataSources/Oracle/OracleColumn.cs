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
using static ImageResizingApp.Models.ResizeConfig;
using System.ComponentModel;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    internal class OracleColumn : IColumn
    {
        public ITable Table { get; set; }
        public string Name { get; set; }
        public string ColumnType { get; set; }
        public bool CanResize { get; set; }

        public event EventHandler<ResizeConfig.ProgressChangedEventHandler> ProgressChanged;

        private readonly OracleConnection _connection;

        public OracleColumn(ITable table, OracleConnection connection, string name, string columnType)
        {
            Name = name;
            ColumnType = columnType;
            CanResize = columnType == "BLOB";
            Table = table;
            _connection = connection;
        }
        public void Resize(int? from, int? to, int? minSize, int? maxSize, IFilter filter, string backupDestination, object sender, DoWorkEventArgs e, IQualityAssessment iqa)
        {

            BackgroundWorker bwAsync = sender as BackgroundWorker;
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
                double spaceGain = 0;
                int totalSuccess = 0;
                while (dr.Read())
                {
                    if (bwAsync.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    counter++;
                    ResizeConfig config = new ResizeConfig();
                    config.totalCount = totalCount;
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
                        img = new MagickImage("C:\\Users\\Paola\\Desktop\\2817292.jpg");
                        originalImg = new MagickImage("C:\\Users\\Paola\\Desktop\\2817292.jpg");
                    }
                    catch
                    {
                        if (ProgressChanged != null)
                        {
                            config.progressPercentage = (int)(counter / totalCount * 100);
                            ProgressChanged(this, new ResizeConfig.ProgressChangedEventHandler(config));
                        }
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
                    double[] a = new double[100];
                    MagickImage img1 = new MagickImage("C:\\Users\\Paola\\Desktop\\scale\\original.png");
                    MagickImage img2;
                    int x = 0;
                    string[] b = { "C:\\Users\\Paola\\Desktop\\scale\\original.png", "C:\\Users\\Paola\\Desktop\\scale\\300x412.png", "C:\\Users\\Paola\\Desktop\\scale\\400x550.png", "C:\\Users\\Paola\\Desktop\\scale\\473x650.png", "C:\\Users\\Paola\\Desktop\\scale\\509x700.png", "C:\\Users\\Paola\\Desktop\\scale\\545x749.png", "C:\\Users\\Paola\\Desktop\\scale\\582x800.png", "C:\\Users\\Paola\\Desktop\\scale\\900x1237.png", "C:\\Users\\Paola\\Desktop\\scale\\946x1300.png", "C:\\Users\\Paola\\Desktop\\scale\\1164x1600.png", "C:\\Users\\Paola\\Desktop\\scale\\1455x2000.png", "C:\\Users\\Paola\\Desktop\\scale\\2000x2750.png" };
                    for (int i= 0;i < b.Length*4; i=i+4)
                    {
                        //create a bad quality image fhigh resolution and check it's score
                        
                        img2 = new MagickImage(b[x]);
                        double[] res = iqa.Compare(img1, img2);
                        a[i] = res[0];
                        a[i+1]= res[1];
                        a[i + 2] = res[2];
                        a[i + 3] = res[3];
                        ++x;
                    }
                    if (iqa.Compare(originalImg, img)[0] > 0){
                        string finalPks = Utilities.GeneratePrimaryKeyValuePairsString(Table.PrimaryKeys, pKs);
                        string sqlUpdate = "UPDATE " + Table.Name + " SET " + Name + " = :pBlob" + " WHERE " + finalPks;
                        OracleParameter param = new OracleParameter("pBlob", OracleDbType.Blob);
                        param.Direction = ParameterDirection.Input;
                        param.Value = finalBytes;

                        OracleTransaction transaction = _connection.BeginTransaction();
                        //OracleCommand updateCommand = _connection.CreateCommand();
                        //updateCommand.Transaction = transaction;
                        //updateCommand.CommandText = sqlUpdate;
                        //updateCommand.Parameters.Add(param);
                        try
                        {
                            //updateCommand.ExecuteNonQuery();
                            //transaction.Commit();
                            totalSuccess++;
                            config.successNumber = totalSuccess;
                            spaceGain += (originalImg.Width * originalImg.Height * originalImg.BitDepth()) - (img.Width * img.Height*img.BitDepth());
                            config.spaceGain = spaceGain;
                        }
                        catch
                        {
                             transaction.Rollback();
                        }
                        finally
                        {
                            transaction.Dispose();
                            if (ProgressChanged != null)
                            {
                                config.progressPercentage = (int)(counter / totalCount * 100);
                                ProgressChanged(this, new ResizeConfig.ProgressChangedEventHandler(config));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public IImage GetImageForPrimaryKeysValues(IEnumerable<string> primaryValues)
        {
            return new OracleImage(this, _connection, primaryValues);
        }
    }
}
