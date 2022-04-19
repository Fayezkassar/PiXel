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

namespace ImageResizingApp.Models.DataSources.Oracle
{
    public class OracleImage : IImage
    {
        public IColumn Column { get; set; }

        private readonly OracleConnection _connection;
        public List<string> PrimaryValues { get; set; }

        public OracleImage(IColumn column, OracleConnection connection, List<string> primaryValues)
        {
            _connection = connection;
            Column = column;
            PrimaryValues = primaryValues;
        }
        public BitmapImage GetBitmapImage()
        {
            string finalPks = Utilities.GeneratePrimaryKeyValuePairs(Column.Table.PrimaryKeys, PrimaryValues);
            string sql = "SELECT "+ Column.Name + " FROM " + Column.Table.Name + " WHERE " + finalPks;
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
            //if (rowNumber != null)
            //{
            //    sqlSelect = "SELECT " + String.Join(",", Table.PrimaryKeys) + ", DOC FROM (SELECT ROWNUM RNUM, a.* FROM " + Table.Name + " a  WHERE NIDOC=" + rowNumber + ")"; // TO REPLACE NIDOC WITH ROWNNUM MAYBE and DOC WITH NAME PARAM
            //} else
            throw new NotImplementedException();
        }
    }
}
