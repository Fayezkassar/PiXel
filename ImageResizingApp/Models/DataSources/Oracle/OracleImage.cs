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
using static ImageResizingApp.Models.ResizeConfig;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    public class OracleImage : IImage
    {
        public IColumn Column { get; set; }

        private readonly OracleConnection _connection;
        public IEnumerable<string> PrimaryKeysValues { get; set; }

        public event EventHandler<ProgressChangedEventHandler> ProgressChanged;

        public OracleImage(IColumn column, OracleConnection connection, IEnumerable<string> primaryKeysValues)
        {
            _connection = connection;
            Column = column;
            PrimaryKeysValues = primaryKeysValues;
        }
        public BitmapImage GetBitmapImage()
        {
            string finalPks = Utilities.GeneratePrimaryKeyValuePairs(Column.Table.PrimaryKeys, PrimaryKeysValues);
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
        public bool Resize(IFilter filter, string backupDestination)
        {
            bool imageResized = false;
            OracleTransaction transaction = _connection.BeginTransaction();
            OracleCommand updateCommand = _connection.CreateCommand();
            updateCommand.Transaction = transaction;

            string finalPks = Utilities.GeneratePrimaryKeyValuePairs(Column.Table.PrimaryKeys, PrimaryKeysValues);
            string sqlSelect = "SELECT " + Column.Name + " FROM " + Column.Table.Name + "  WHERE " + finalPks;
            OracleCommand cmd = new OracleCommand(sqlSelect, _connection);
            OracleDataReader dr = cmd.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    OracleBlob blob = dr.GetOracleBlob(0);
                    MagickImage img;
                    byte[] bytes = new byte[blob.Length];
                    blob.Read(bytes, 0, (int)blob.Length);

                    img = new MagickImage(bytes);

                    if (backupDestination != null && backupDestination.Length > 0)
                    {
                        img.Write(backupDestination + "\\" + string.Join("-", PrimaryKeysValues));
                    }

                    filter.Process(img);
                    byte[] finalBytes = img.ToByteArray();

                    string sqlUpdate = "UPDATE " + Column.Table.Name + " SET " + Column.Name + " = :pBlob" + " WHERE " + finalPks;
                    OracleParameter param = new OracleParameter("pBlob", OracleDbType.Blob);
                    param.Direction = ParameterDirection.Input;
                    param.Value = finalBytes;

                    ResizeConfig config = new ResizeConfig();
                    config.totalCount = 1;
                    config.progressPercentage = 100;

                    updateCommand.CommandText = sqlUpdate;
                    updateCommand.Parameters.Add(param);
                    imageResized = true;
                    try
                    {
                        updateCommand.ExecuteNonQuery();
                        ransaction.Commit();
                        config.successNumber = 1;
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
                            ProgressChanged(this, new ProgressChangedEventHandler(config));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return imageResized;
        }
    }
}
