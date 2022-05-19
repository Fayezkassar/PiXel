using ImageResizingApp.Helpers;
using ImageResizingApp.Models.Interfaces;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using ImageMagick;
using static ImageResizingApp.Models.ResizingProgress;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    public class OracleImage : IImage
    {
        public IColumn Column { get; set; }
        public IEnumerable<string> PrimaryKeysValues { get; set; }

        public event EventHandler<ProgressChangedEventHandler> ProgressChanged;

        private readonly OracleConnection _connection;

        public OracleImage(IColumn column, OracleConnection connection, IEnumerable<string> primaryKeysValues)
        {
            _connection = connection;
            Column = column;
            PrimaryKeysValues = primaryKeysValues;
        }
        public BitmapImage GetBitmapImage()
        {
            string finalPks = Utilities.GeneratePrimaryKeyValuePairsString(Column.Table.PrimaryKeys, PrimaryKeysValues);
            string sql = "SELECT " + Column.Name + " FROM " + Column.Table.Name + " WHERE " + finalPks;
            OracleCommand cmd = new OracleCommand(sql, _connection);
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                OracleBlob blob = dr.GetOracleBlob(0);
                byte[] bytes = new byte[blob.Length];
                blob.Read(bytes, 0, (int)blob.Length);
                return Utilities.LoadImage(bytes);
            }
            return null;
        }
        public bool Resize(ImageResizeParameters irp)
        {
            bool imageResized = false;
            OracleTransaction transaction = _connection.BeginTransaction();
            OracleCommand updateCommand = _connection.CreateCommand();
            updateCommand.Transaction = transaction;

            string finalPks = Utilities.GeneratePrimaryKeyValuePairsString(Column.Table.PrimaryKeys, PrimaryKeysValues);
            string sqlSelect = "SELECT " + Column.Name + " FROM " + Column.Table.Name + "  WHERE " + finalPks;
            OracleCommand cmd = new OracleCommand(sqlSelect, _connection);
            OracleDataReader dr = cmd.ExecuteReader();

            ResizingProgress progress = new ResizingProgress(0, 0, 1, 0);
            try
            {
                while (dr.Read())
                {
                    OracleBlob blob = dr.GetOracleBlob(0);
                    byte[] bytes = new byte[blob.Length];
                    blob.Read(bytes, 0, (int)blob.Length);

                    MagickImage originalImg = new MagickImage(bytes);
                    MagickImage img = new MagickImage(bytes);

                    if (irp.BackupDestination != null && irp.BackupDestination.Length > 0)
                    {
                        try
                        {
                            img.Write(irp.BackupDestination + "\\" + string.Join("-", PrimaryKeysValues));
                        }
                        catch
                        {
                            break;
                        } 
                    }
                    try
                    {
                        irp.Filter.Process(img);
                        if (irp.IQA.Compare(originalImg, img))
                        {
                            byte[] finalBytes = img.ToByteArray();

                            string sqlUpdate = "UPDATE " + Column.Table.Name + " SET " + Column.Name + " = :pBlob" + " WHERE " + finalPks;
                            OracleParameter param = new OracleParameter("pBlob", OracleDbType.Blob);
                            param.Direction = ParameterDirection.Input;
                            param.Value = finalBytes;

                            updateCommand.CommandText = sqlUpdate;
                            updateCommand.Parameters.Add(param);

                            try
                            {
                                updateCommand.ExecuteNonQuery();
                                transaction.Commit();
                                progress.SpaceGain = Utilities.GetSpaceGain(originalImg, img);
                                progress.SuccessCount = 1;
                                imageResized = true;
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
                    catch
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                ++progress.ImageCount;
                if (ProgressChanged != null)
                {
                    ProgressChanged(this, new ProgressChangedEventHandler(progress));
                }
            }

            return imageResized;
        }
    }
}
