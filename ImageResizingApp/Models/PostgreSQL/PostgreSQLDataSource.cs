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
        public IEnumerable<string> ConnectionParameters { get; }
        public List<ITable> Tables { get; set; }

        private NpgsqlConnection _connection;

        public PostgreSQLDataSource()
        {
            List<string> connectionParameters = new List<string>();
            connectionParameters.Add("Host");
            connectionParameters.Add("Username");
            connectionParameters.Add("Password");
            connectionParameters.Add("Database");
            ConnectionParameters = connectionParameters;

        }

        public IDataSource Clone()
        {
            return (IDataSource)this.MemberwiseClone();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public bool Open(Dictionary<string, string> connectionParametersMap)
        {
            try
            {
                string cs = ToConnectionString(connectionParametersMap);

                _connection = new NpgsqlConnection(cs);
                _connection.Open();

                var sql = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES ";

                using var cmd = new NpgsqlCommand(sql, _connection);

                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                Trace.WriteLine($"{rdr.GetName(0),-4}");

                Tables = new List<ITable>();

                while (rdr.Read())
                {
                    Tables.Add(new PostgreSQLTable()
                    {
                        Name = rdr.GetString(0),
                    });
                }

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

        public Task<IEnumerable<ITable>> getTables()
        {
            throw new NotImplementedException();
        }
    }
}
