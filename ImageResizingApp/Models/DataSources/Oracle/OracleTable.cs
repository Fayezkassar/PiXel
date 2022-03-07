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

        public OracleConnection _connection { get; set; }

        public OracleTable(OracleConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<IColumn> getColumns()
        {
            string sql = "SELECT COLUMN_NAME, DATA_TYPE FROM USER_TAB_COLUMNS WHERE table_name = '" + Name + "'";

            using var cmd = new OracleCommand(sql, _connection);

            using OracleDataReader rdr = cmd.ExecuteReader();

            List<IColumn> coulmns = new List<IColumn>();

            while (rdr.Read())
            {
                coulmns.Add(new OracleColumn()
                {
                    Name = rdr.GetString(0),
                    ColumnType = rdr.GetString(1),
                });
            }

            return coulmns;
        }

        public TableStats GetStats()
        {
            try
            {
                TableStats tableStats = new TableStats();

                // !!!!!!!!!!!!!!!!!!!!!!!!!!       ONE QUERY ONLY            !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                string sql = "SELECT SUM(S.BYTES)"
                                + " FROM USER_SEGMENTS S"
                                + " WHERE S.SEGMENT_NAME = '" + Name + "'"
                                + " OR S.SEGMENT_NAME IN("
                                + " ( SELECT L.SEGMENT_NAME FROM USER_LOBS L WHERE L.TABLE_NAME = '" + Name +"'))";

                OracleCommand cmd = new OracleCommand(sql, _connection);
                decimal tableSize = (decimal)cmd.ExecuteScalar();
                tableStats.TableSize = SizeSuffix(tableSize);

                OracleCommand cmd1 = new OracleCommand("select COUNT(*) FROM " + Name, _connection);
                decimal recordNumber = (decimal)cmd1.ExecuteScalar();
                tableStats.RecordsNumber = recordNumber.ToString();

                if (recordNumber > 0)
                {
                    tableStats.RecordSize = SizeSuffix(tableSize / recordNumber);
                }

                return tableStats;
            } catch
            {
                return null;
            }
        }

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        static string SizeSuffix(decimal value, int decimalPlaces = 1)
        {
            int i = 0;
            while (Math.Round(value, decimalPlaces) >= 1000)
            {
                value /= 1024;
                i++;
            }
            return string.Format("{0:n" + decimalPlaces + "} {1}", value, SizeSuffixes[i]);
        }

        public DataTable getData()
        {
            try
            {
                string sql = "select * from " + Name + " WHERE ROWNUM <=100";

                OracleCommand cmd = new OracleCommand(sql, _connection);

                DataTable dt = new DataTable();
                OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);
                dataAdapter.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            
        }
    }
}
