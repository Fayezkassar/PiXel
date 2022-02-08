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
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<string> ConnectionParameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<ITable> Tables { get; set; }

        IEnumerable<string> IDataSource.ConnectionParameters => throw new NotImplementedException();

        public IDataSource Clone()
        {
            throw new NotImplementedException();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public bool Open(Dictionary<string, string> connectionParametersMap)
        {
            string cs = ToConnectionString(connectionParametersMap);

            using var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES ";

            using var cmd = new NpgsqlCommand(sql, con);

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
