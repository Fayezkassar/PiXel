using ImageResizingApp.Models.Interfaces;
using Npgsql;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ImageResizingApp.Models.DataSources.PostgreSQL
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
            return (IDataSource)MemberwiseClone();
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
                SetConnection(connectionParametersMap);
                _connection.Open();

                string sql = "SELECT table_name, table_schema FROM INFORMATION_SCHEMA.TABLES WHERE table_schema = 'public' AND table_type = 'BASE TABLE' ORDER BY table_name";

                using var cmd = new NpgsqlCommand(sql, _connection);

                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                List<ITable> tables = new List<ITable>();

                while (rdr.Read())
                {
                    tables.Add(new PostgreSQLTable(_connection)
                    {
                        Name = rdr.GetString(0),
                        SchemaName = rdr.GetString(1),
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

        private void SetConnection(Dictionary<string, string> connectionParametersMap)
        {
            var items = from kvp in connectionParametersMap
                        select kvp.Key + "=" + kvp.Value;

            _connection = new NpgsqlConnection(string.Join(";", items));
        }
    }
}
