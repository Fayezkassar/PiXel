﻿using ImageResizingApp.Helpers;
using ImageResizingApp.Models.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    internal class OracleTable : ITable
    {
        public string Name { get; set; }
        public string TableSize { get; set; }
        public string RecordsNumber { get; set; }
        public string RecordSize { get; set; }
        public IEnumerable<string> PrimaryKeys { get; set; }
        private readonly OracleConnection _connection;
        public OracleTable(OracleConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<IColumn>> GetColumnsAsync()
        {
            List<IColumn> columns = new List<IColumn>();
            try
            {
                string sql = "SELECT COLUMN_NAME, DATA_TYPE FROM USER_TAB_COLUMNS WHERE TABLE_NAME = '" + Name + "'";

                using var cmd = new OracleCommand(sql, _connection);

                using OracleDataReader rdr = cmd.ExecuteReader();

                while (await rdr.ReadAsync())
                {
                    columns.Add(new OracleColumn(this, _connection)
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

        public async Task SetPrimaryKeysAsync() {
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
                                + " ( SELECT L.SEGMENT_NAME FROM USER_LOBS L WHERE L.TABLE_NAME = '" + Name +"'))";

                OracleCommand cmd = new OracleCommand(sql, _connection);
                decimal tableSize = (decimal) (await cmd.ExecuteScalarAsync());
                TableSize = Utilities.GetFormatedSize(tableSize);

                OracleCommand cmd1 = new OracleCommand("SELECT COUNT(*) FROM " + Name, _connection);
                decimal recordNumber = (decimal)(await cmd1.ExecuteScalarAsync());
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

        public async Task<DataTable> GetDataAsync(int start, int itemCount)
        {
            DataTable dataTable = new DataTable();
            try
            {
                int end  = start + itemCount;
                string sql = "SELECT * FROM ( select t.*, rownum r from " + Name + " t) where r >= " + start + " and r < " + end;

                OracleCommand cmd = new OracleCommand(sql, _connection);

                OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);

                await Task.Run(() => dataAdapter.Fill(dataTable));

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dataTable;
        }
    }
}
