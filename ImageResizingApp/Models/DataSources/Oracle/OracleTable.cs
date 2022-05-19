using ImageResizingApp.Helpers;
using ImageResizingApp.Models.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    internal class OracleTable : ITable
    {
        public string Name { get; set; }
        public string TableSize { get; set; }
        public string RecordsNumber { get; set; }
        public IEnumerable<string> PrimaryKeys { get; set; }
        public IEnumerable<IColumn> Columns { get; set; }

        private readonly OracleConnection _connection;

        private OracleDataSource _dataSource;

        public OracleTable(OracleDataSource oracleDataSource, OracleConnection connection)
        {
            Columns = new List<IColumn>();
            _connection = connection;
            _dataSource = oracleDataSource;
        }

        public async Task SetColumnsAsync()
        {
            try
            {
                string sql = "SELECT COLUMN_NAME, DATA_TYPE FROM USER_TAB_COLUMNS WHERE TABLE_NAME = '" + Name + "'";

                using var cmd = new OracleCommand(sql, _connection);

                using OracleDataReader rdr = cmd.ExecuteReader();

                List<IColumn> columns = new List<IColumn>();
                while (await rdr.ReadAsync())
                {
                    columns.Add(new OracleColumn(this, _connection, rdr.GetString(0), rdr.GetString(1), _dataSource));
                }
                Columns = columns;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SetPrimaryKeysAsync()
        {
            try
            {
                string sql = "SELECT cols.column_name FROM all_constraints cons, all_cons_columns cols WHERE cols.table_name = '" + Name + "' AND cons.constraint_type = 'P' AND cons.constraint_name = cols.constraint_name AND cons.owner = cols.owner ORDER BY cols.table_name, cols.position";
                OracleCommand cmd = new OracleCommand(sql, _connection);
                OracleDataReader dr = cmd.ExecuteReader();

                List<string> keys = new List<string>();

                while (await dr.ReadAsync())
                {
                    keys.Add(dr.GetString(0));
                }

                PrimaryKeys = keys;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SetStatsAsync()
        {
            try
            {
                string sql = "SELECT SUM(S.BYTES)"
                                + " FROM USER_SEGMENTS S"
                                + " WHERE S.SEGMENT_NAME = '" + Name + "'"
                                + " OR S.SEGMENT_NAME IN("
                                + " ( SELECT L.SEGMENT_NAME FROM USER_LOBS L WHERE L.TABLE_NAME = '" + Name + "'))";

                OracleCommand cmd = new OracleCommand(sql, _connection);
                decimal tableSize = (decimal)(await cmd.ExecuteScalarAsync());
                TableSize = Utilities.GetFormatedSize(tableSize);

                OracleCommand cmd1 = new OracleCommand("SELECT COUNT(*) FROM " + Name, _connection);
                decimal recordNumber = (decimal)(await cmd1.ExecuteScalarAsync());
                RecordsNumber = recordNumber.ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<DataTable> GetDataAsync(int start, int itemCount)
        {
            DataTable data = new DataTable();
            try
            {
                int end = start + itemCount;
                string sql = "SELECT * FROM ( select t.*, rownum r from " + Name + " t) where r >= " + start + " and r < " + end;

                OracleCommand cmd = new OracleCommand(sql, _connection);

                OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);

                await Task.Run(() => dataAdapter.Fill(data));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
