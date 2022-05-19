using ImageResizingApp.Models.Interfaces;
using System;
using System.Linq;
using Oracle.ManagedDataAccess.Types;
using ImageMagick;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using ImageResizingApp.Helpers;
using System.ComponentModel;
using System.Threading;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    internal class OracleColumn : IColumn
    {
        public ITable Table { get; set; }
        public string Name { get; set; }
        public string ColumnType { get; set; }
        public bool CanResize { get; set; }

        private static Mutex counterMutex = new Mutex();
        private static Mutex spaceMutex = new Mutex();
        private static Mutex successMutex = new Mutex();

        public event EventHandler<ResizeConfig.ProgressChangedEventHandler> ProgressChanged;

        private readonly OracleConnection _connection;

        private OracleDataSource _dataSources;

        public OracleColumn(ITable table, OracleConnection connection, string name, string columnType, OracleDataSource oracleDataSource)
        {
            Name = name;
            ColumnType = columnType;
            CanResize = columnType == "BLOB";
            Table = table;
            _connection = connection;
            _dataSources = oracleDataSource;
        }

        public void ResizeImage(object obj)
        {
            object[] array = obj as object[];
            OracleBlob blob = (OracleBlob)array[0];
            IFilter filter = (IFilter)array[1];
            List<string> pKs = (List<string>)array[2];
            IQualityAssessment iqa = (IQualityAssessment)array[5];
            BackgroundWorker bwAsync = array[3] as BackgroundWorker;
            DoWorkEventArgs e = (DoWorkEventArgs)array[4];
            string backupDestination = (string)array[6];

            if (bwAsync.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            ResizeConfig config = new ResizeConfig(0, filter.SuccessNumber, filter.TotalCount, Utilities.GetFormatedSize(filter.SpaceGain));
            long blobSize = blob.Length;


            MagickImage img;
            MagickImage originalImg;
            byte[] bytes = new byte[blobSize];
            blob.Read(bytes, 0, (int)blobSize);
            try
            {
                img = new MagickImage(bytes);
                originalImg = new MagickImage(bytes);
            }
            catch
            {
                counterMutex.WaitOne();
                filter.Counter++;
                counterMutex.ReleaseMutex();
                if (ProgressChanged != null)
                {
                    config.progressPercentage = (int)(filter.Counter / filter.TotalCount * 100);
                    ProgressChanged(this, new ResizeConfig.ProgressChangedEventHandler(config));
                }
                return;
            }

            if (backupDestination != null && backupDestination.Length > 0)
            {
                try
                {
                    originalImg.Write(backupDestination + "\\" + string.Join("-", pKs) + ".png");
                }
                catch
                {
                    counterMutex.WaitOne();
                    filter.Counter++;
                    counterMutex.ReleaseMutex();
                    if (ProgressChanged != null)
                    {
                        config.progressPercentage = (int)(filter.Counter / filter.TotalCount * 100);
                        ProgressChanged(this, new ResizeConfig.ProgressChangedEventHandler(config));
                    }
                    return;
                }
            }

            filter.Process(img);
            byte[] finalBytes = img.ToByteArray();
            if (true)
            {
                string finalPks = Utilities.GeneratePrimaryKeyValuePairsString(Table.PrimaryKeys, pKs);
                string sqlUpdate = "UPDATE " + Table.Name + " SET " + Name + " = :pBlob" + " WHERE " + finalPks;
                OracleParameter param = new OracleParameter("pBlob", OracleDbType.Blob);
                param.Direction = ParameterDirection.Input;
                param.Value = finalBytes;

                OracleConnection tmpConnection = (OracleConnection)_dataSources.CreateTemporaryConnection();

                tmpConnection.Open();

                OracleTransaction transaction = tmpConnection.BeginTransaction();
                OracleCommand updateCommand = tmpConnection.CreateCommand();
                updateCommand.Transaction = transaction;
                updateCommand.CommandText = sqlUpdate;
                updateCommand.Parameters.Add(param);
                try
                {
                    updateCommand.ExecuteNonQuery();
                    transaction.Commit();
                    successMutex.WaitOne();
                    filter.SuccessNumber++;
                    successMutex.ReleaseMutex();
                    config.successNumber = filter.SuccessNumber;
                    spaceMutex.WaitOne();
                    filter.SpaceGain += ((originalImg.Width * originalImg.Height * originalImg.BitDepth()) - (img.Width * img.Height * img.BitDepth())) / 12;
                    spaceMutex.ReleaseMutex();
                    config.spaceGain = Utilities.GetFormatedSize(filter.SpaceGain);
                }
                catch
                {
                    transaction.Rollback();
                }
                finally
                {
                    transaction.Dispose();
                    tmpConnection.Close();
                    counterMutex.WaitOne();
                    filter.Counter++;
                    counterMutex.ReleaseMutex();
                    if (ProgressChanged != null)
                    {
                        config.progressPercentage = (int)(filter.Counter / config.totalCount * 100);
                        ProgressChanged(this, new ResizeConfig.ProgressChangedEventHandler(config));
                    }
                }
            }
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

            int n = Table.PrimaryKeys.Count();

            string sqlSelect = "SELECT " + String.Join(",", Table.PrimaryKeys) + ", " + Name + " FROM (SELECT ROWNUM RNUM, a.* FROM " + Table.Name + " a" + (to == null ? (finalSizeCondition != "" ? (" WHERE " + finalSizeCondition) : "") : (" WHERE ROWNUM<=" + to + (finalSizeCondition != "" ? (" AND " + finalSizeCondition) : ""))) + ")";
            sqlSelect += " WHERE RNUM>=" + finalFrom;
            OracleCommand selectCmd = new OracleCommand(sqlSelect, _connection);

            string sqlCount = "SELECT COUNT(*) FROM (SELECT ROWNUM RNUM, a.* FROM " + Table.Name + " a" + (to == null ? (finalSizeCondition != "" ? (" WHERE " + finalSizeCondition) : "") : (" WHERE ROWNUM<=" + to + (finalSizeCondition != "" ? (" AND " + finalSizeCondition) : ""))) + ")";
            sqlCount += " WHERE RNUM>=" + finalFrom;
            OracleCommand cmd = new OracleCommand(sqlCount, _connection);

            try
            {
                filter.TotalCount = (decimal)(cmd.ExecuteScalar());
                filter.Counter = 0;
                filter.SuccessNumber = 0;
                filter.SpaceGain = 0;
                OracleDataReader dr = selectCmd.ExecuteReader();
                while (dr.Read())
                {
                    if (bwAsync.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    List<string> pKs = new List<string>();
                    for (int i = 0; i < n; i++)
                    {
                        pKs.Add(dr.GetString(i));
                    }

                    OracleBlob blob = dr.GetOracleBlob(n);

                    ThreadPool.QueueUserWorkItem(ResizeImage, new object[] { blob, filter, pKs, sender, e, iqa, backupDestination });

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
