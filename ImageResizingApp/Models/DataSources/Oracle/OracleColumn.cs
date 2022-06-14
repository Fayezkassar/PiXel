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

        public event EventHandler<ResizingProgress.ProgressChangedEventHandler> ProgressChanged;

        private readonly OracleConnection _connection;
        private readonly OracleDataSource _dataSource;

        private static Mutex _imageCountMutex = new Mutex();
        private static Mutex _spaceGainCountMutex = new Mutex();
        private static Mutex _successCountMutex = new Mutex();

        public OracleColumn(OracleTable table, string name, string columnType)
        {
            Name = name;
            ColumnType = columnType;
            CanResize = columnType == "BLOB";
            Table = table;
            _dataSource = (OracleDataSource)table.DataSource;
            _connection = (OracleConnection)_dataSource.Connection;
        }

        public void ResizeImage(object obj)
        {
            object[] array = obj as object[];

            OracleBlob blob = (OracleBlob)array[0];
            ImageResizeParameters irp = (ImageResizeParameters)array[1];
            List<string> pKs = (List<string>)array[2];
            BackgroundWorker bwAsync = array[4] as BackgroundWorker;
            DoWorkEventArgs e = (DoWorkEventArgs)array[3];
            ResizingProgress progress = (ResizingProgress)array[5];

            if (bwAsync.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            _imageCountMutex.WaitOne();
            progress.ImageCount++;
            _imageCountMutex.ReleaseMutex();

            long blobSize = blob.Length;
            byte[] bytes = new byte[blobSize];
            blob.Read(bytes, 0, (int)blobSize);

            MagickImage img;
            MagickImage originalImg;
            try
            {
                img = new MagickImage(bytes);
                originalImg = new MagickImage(bytes);
            }
            catch
            {
                TriggerProgressChangedEvent(bwAsync, progress);
                return;
            }

            if (irp.BackupDestination != null && irp.BackupDestination.Length > 0)
            {
                try
                {
                    originalImg.Write(irp.BackupDestination + "\\" + string.Join("-", pKs));
                }
                catch
                {
                    TriggerProgressChangedEvent(bwAsync, progress);
                    return;
                }
            }

            irp.Filter.Process(img);
            byte[] finalBytes = img.ToByteArray();
            if (irp.IQA.Compare(originalImg, img))
            {
                string finalPks = Utilities.GeneratePrimaryKeyValuePairsString(Table.PrimaryKeys, pKs);
                string sqlUpdate = "UPDATE " + Table.Name + " SET " + Name + " = :pBlob" + " WHERE " + finalPks;
                OracleParameter param = new OracleParameter("pBlob", OracleDbType.Blob);
                param.Direction = ParameterDirection.Input;
                param.Value = finalBytes;

                OracleConnection tmpConnection = (OracleConnection)_dataSource.CreateConnection();

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

                    _successCountMutex.WaitOne();
                    progress.SuccessCount++;
                    _successCountMutex.ReleaseMutex();

                    _spaceGainCountMutex.WaitOne();
                    progress.SpaceGain += Utilities.GetSpaceGain(originalImg, img);
                    _spaceGainCountMutex.ReleaseMutex();

                }
                catch
                {
                    transaction.Rollback();
                }
                finally
                {
                    transaction.Dispose();
                    tmpConnection.Close();
                    TriggerProgressChangedEvent(bwAsync, progress);
                }
            }
        }

        private string GetSizeConditionString(int? minSize, int? maxSize)
        {
            string finalSizeCondition = "";
            string minSizeCondition = minSize != null ? ("dbms_lob.getlength(" + Name + ")>" + minSize) : "";
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
            return finalSizeCondition;
        }

        public void Resize(int? from, int? to, int? minSize, int? maxSize, IFilter filter, string backupDestination, object sender, DoWorkEventArgs e, IQualityAssessment iqa)
        {


            BackgroundWorker bwAsync = sender as BackgroundWorker;

            string finalSizeCondition = this.GetSizeConditionString(minSize, maxSize);
            var finalFrom = from ?? 0;
            string finalFromCondition = " WHERE RNUM>=" + finalFrom;

            string sqlSelectAllData = "SELECT ROWNUM RNUM, a.* FROM " + Table.Name + " a" + (to == null ? (finalSizeCondition != "" ? (" WHERE " + finalSizeCondition) : "") : (" WHERE ROWNUM<=" + to + (finalSizeCondition != "" ? (" AND " + finalSizeCondition) : ""))) + ")";

            string sqlSelectBlobAndPrimaryKeys = "SELECT " + String.Join(",", Table.PrimaryKeys) + ", " + Name + " FROM (" + sqlSelectAllData;
            sqlSelectBlobAndPrimaryKeys += finalFromCondition;
            OracleCommand selectCmd = new OracleCommand(sqlSelectBlobAndPrimaryKeys, _connection);

            string sqlSelectCount = "SELECT COUNT(*) FROM (" + sqlSelectAllData;
            sqlSelectCount += finalFromCondition;
            OracleCommand cmd = new OracleCommand(sqlSelectCount, _connection);

            int primaryKeysCount = Table.PrimaryKeys.Count();
            try
            {
                OracleDataReader dr = selectCmd.ExecuteReader();
                ResizingProgress progress = new ResizingProgress(0, 0, Convert.ToInt32((cmd.ExecuteScalar())), 0);
                TriggerProgressChangedEvent(bwAsync, progress);
                while (dr.Read())
                {
                    if (bwAsync.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    List<string> pKs = new List<string>();
                    for (int i = 0; i < primaryKeysCount; i++)
                    {
                        pKs.Add(dr.GetString(i));
                    }

                    OracleBlob blob = dr.GetOracleBlob(primaryKeysCount);

                    ThreadPool.QueueUserWorkItem(ResizeImage, new object[] { blob, new ImageResizeParameters(filter,iqa,backupDestination), pKs, e, bwAsync, progress });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void TriggerProgressChangedEvent(BackgroundWorker bw, ResizingProgress progress)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, new ResizingProgress.ProgressChangedEventHandler(new ResizingProgress(progress.ImageCount, progress.SuccessCount, progress.TotalImageCount, progress.SpaceGain)));
            }
        }

        public IImage GetImageForPrimaryKeysValues(IEnumerable<string> primaryValues)
        {
            return new OracleImage(this, _connection, primaryValues);
        }
    }
}
