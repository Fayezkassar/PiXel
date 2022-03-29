using ImageResizingApp.Models.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media.Imaging;

namespace ImageResizingApp.Models.DataSources.PostgreSQL
{
    public class PostgreSQLTable : ITable
    {
        public string Name { get; set; }
        public string SchemaName { get; set; }
        public string TableSize { get; set; }
        public string RecordsNumber { get; set; }
        public string RecordSize { get; set; }
        public IEnumerable<string> PrimaryKeys { get; set; }

        private readonly NpgsqlConnection _connection;

        public PostgreSQLTable(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<IColumn> GetColumns()
        {
            List<IColumn> columns = new List<IColumn>();
            try
            {
                string sql = "select column_name, data_type from INFORMATION_SCHEMA.COLUMNS where \"table_name\" = '" + Name + "' and \"table_schema\" = '" + SchemaName + "'";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, _connection);

                using NpgsqlDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    columns.Add(new PostgresSQLColumn()
                    {
                        Name = rdr.GetString(0),
                        ColumnType = rdr.GetString(1),
                    });
                }
                rdr.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return columns;
        }


        public void SetStats()
        {
            try
            {
               // using NpgsqlDataReader rdr = new NpgsqlCommand("select pg_size_pretty(pg_total_relation_size('\"" + Name + "\"'))", _connection).ExecuteReader();
                //while (rdr.Read())
                //{
                //    tableStats.TableSize = rdr.GetString(0);
                //}
                //rdr.Close();
                NpgsqlCommand cmd = new NpgsqlCommand("select COUNT(*) FROM \"" + Name + "\"", _connection);

                long rowsNum = (long)cmd.ExecuteScalar();
                RecordsNumber = rowsNum.ToString();
                TableSize = "NOT IMPLEMENTED";
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public DataTable GetData(int start, int itemCount)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM " + SchemaName + ".\"" + Name + "\"";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, _connection);
                var dataAdapter = new NpgsqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public void SetPrimaryKeys()
        {
            throw new NotImplementedException();
        }

        public BitmapImage GetBitmapImage(DataRowView row)
        {
            throw new NotImplementedException();
        }
    }
}
