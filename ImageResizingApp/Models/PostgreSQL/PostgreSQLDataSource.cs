using ImageResizingApp.Models.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizingApp.Models.PostgreSQL
{
    public class PostgreSQLDataSource : IDataSource
    {
        public string Name { get; set; }

        public IEnumerable<ITable> Tables { get; set; }

        public IEnumerable<string> ConnectionParameters { get; }

        private NpgsqlConnection _connection;

        public PostgreSQLDataSource()
        {
            List<string> connectionParameters = new List<string>();
            connectionParameters.Add("Database");
            connectionParameters.Add("Host");
            connectionParameters.Add("Username");
            connectionParameters.Add("Password");
            ConnectionParameters = connectionParameters;

        }

        public IDataSource Clone()
        {
            return (IDataSource)this.MemberwiseClone();
        }

        public bool Close()
        {
            try
            {
                _connection.Close();
                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool Open(Dictionary<string, string> connectionParametersMap)
        {
            try
            {
                string cs = ToConnectionString(connectionParametersMap);

                _connection = new NpgsqlConnection(cs);
                _connection.Open();

                string sql = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES WHERE table_schema = 'public' AND table_type = 'BASE TABLE' ORDER BY table_name";

                using var cmd = new NpgsqlCommand(sql, _connection);

                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                Trace.WriteLine($"{rdr.GetName(0),-4}");

                List<ITable> tables = new List<ITable>();

                while (rdr.Read())
                {
                    tables.Add(new PostgreSQLTable()
                    {
                        Name = rdr.GetString(0),
                    });

                }

                Tables = tables;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ToConnectionString(Dictionary<string, string> connectionParametersMap)
        {
            var items = from kvp in connectionParametersMap
                        select kvp.Key + "=" + kvp.Value;

            return string.Join(";", items);
        }
    }
}
