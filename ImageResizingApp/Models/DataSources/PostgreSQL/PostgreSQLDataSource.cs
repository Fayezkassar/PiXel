using ImageResizingApp.Models.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ImageResizingApp.Models.DataSources.PostgreSQL
{
    public class PostgreSQLDataSource : DBDataSource
    {
        public PostgreSQLDataSource()
        {
            Tables = new List<ITable>();
            List<string> connectionParameters = new List<string>();
            connectionParameters.Add("Database");
            connectionParameters.Add("Host");
            connectionParameters.Add("Username");
            connectionParameters.Add("Password");
            ConnectionParameters = connectionParameters;

        }

        public override IDataSource Clone()
        {
            return (IDataSource)MemberwiseClone();
        }

        protected override void SetConnection(Dictionary<string, string> connectionParametersMap)
        {
            var items = from kvp in connectionParametersMap
                        select kvp.Key + "=" + kvp.Value;

            _connection = new NpgsqlConnection(string.Join(";", items));
        }

        public override async Task SetTablesAsync()
        {
            try
            {
                NpgsqlConnection connection = _connection as NpgsqlConnection;
                string sql = "SELECT table_name, table_schema FROM INFORMATION_SCHEMA.TABLES WHERE table_schema = 'public' AND table_type = 'BASE TABLE' ORDER BY table_name";

                using var cmd = new NpgsqlCommand(sql, connection);

                NpgsqlDataReader rdr = await cmd.ExecuteReaderAsync();

                List<ITable> tables = new List<ITable>();

                while (await rdr.ReadAsync())
                {
                    tables.Add(new PostgreSQLTable(connection)
                    {
                        Name = rdr.GetString(0),
                        SchemaName = rdr.GetString(1),
                    });
                }

                Tables = tables;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

        }
    }
}
