using ImageResizingApp.Helpers;
using ImageResizingApp.Models.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    internal class OracleTable : ITable
    {
        public string Name { get; set; }
        public string TableSize { get; set; }
        public string RecordsNumber { get; set; }
        public string RecordSize { get; set; }
        public OracleConnection _connection { get; set; }

        public OracleTable(OracleConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<IColumn> GetColumns()
        {
            List<IColumn> columns = new List<IColumn>();
            try
            {
                string sql = "SELECT COLUMN_NAME, DATA_TYPE FROM USER_TAB_COLUMNS WHERE TABLE_NAME = '" + Name + "'";

                using var cmd = new OracleCommand(sql, _connection);

                using OracleDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    columns.Add(new OracleColumn()
                    {
                        Name = rdr.GetString(0),
                        ColumnType = rdr.GetString(1),
                        Resizable = rdr.GetString(1) == "BLOB"
                    });
                }

            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return columns;
        }

        public void SetStats()
        {
            try
            {
                string sql = "SELECT SUM(S.BYTES)"
                                + " FROM USER_SEGMENTS S"
                                + " WHERE S.SEGMENT_NAME = '" + Name + "'"
                                + " OR S.SEGMENT_NAME IN("
                                + " ( SELECT L.SEGMENT_NAME FROM USER_LOBS L WHERE L.TABLE_NAME = '" + Name +"'))";

                OracleCommand cmd = new OracleCommand(sql, _connection);
                decimal tableSize = (decimal)cmd.ExecuteScalar();
                TableSize = Utilities.GetFormatedSize(tableSize);

                OracleCommand cmd1 = new OracleCommand("SELECT COUNT(*) FROM " + Name, _connection);
                decimal recordNumber = (decimal)cmd1.ExecuteScalar();
                RecordsNumber = recordNumber.ToString();

                if (recordNumber > 0)
                {
                    RecordSize = Utilities.GetFormatedSize(tableSize / recordNumber);
                }

            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public DataTable GetData()
        {
            DataTable dataTable = new DataTable();
            try
            {
                string sql = "SELECT * FROM " + Name + " WHERE ROWNUM <= 100";

                OracleCommand cmd = new OracleCommand(sql, _connection);

                OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);
                dataAdapter.Fill(dataTable);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dataTable;
        }
    }
}
