using ImageResizingApp.Models.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;

namespace ImageResizingApp.Models.DataSources.PostgreSQL
{
    public class PostgreSQLTable : ITable
    {
        public string Name { get; set; }

        public string SchemaName { get; set; }

        public NpgsqlConnection _connection { get; set; }

        public PostgreSQLTable(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<IColumn> getColumns()
        {
            string sql = "select column_name, data_type from INFORMATION_SCHEMA.COLUMNS where \"table_name\" = '"+ Name + "' AND \"table_schema\" = '" + SchemaName + "'";

            using var cmd = new NpgsqlCommand(sql, _connection);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();

            List<IColumn> coulmns = new List<IColumn>();

            while (rdr.Read())
            {
                coulmns.Add(new PostgresSQLColumn()
                {
                    Name = rdr.GetString(0),
                    ColumnType = rdr.GetString(1),
                });
            }

            rdr.Close();

            coulmns.ForEach(x =>
            {
                NpgsqlCommand cmd1 = new NpgsqlCommand("select pg_size_pretty(sum(pg_column_size(\"" + x.Name + "\"))) as total_size from "+ SchemaName +".\"" + Name + "\"", _connection);
                object totalSize = cmd1.ExecuteScalar();
                x.TotalSize = (totalSize != null && totalSize != DBNull.Value) ? (string)totalSize : "0 bytes";
            });

            return coulmns;
        }

        public TableStats GetStats()
        {
            TableStats tableStats = new TableStats();

            using NpgsqlDataReader rdr = new NpgsqlCommand("select pg_size_pretty(pg_total_relation_size('\"" + Name + "\"'))", _connection).ExecuteReader();
            while (rdr.Read())
            {
                tableStats.TableSize = rdr.GetString(0);
            }
            rdr.Close();
            NpgsqlCommand cmd = new NpgsqlCommand("select COUNT(*) FROM \"" + Name + "\"", _connection);
            long rowsNum = (long)cmd.ExecuteScalar();
            tableStats.RecordsNumber = rowsNum.ToString();

            return tableStats;
        }
    }
}
