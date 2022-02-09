using ImageResizingApp.Exceptions;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizingApp.Models.Oracle
{
    public class SQLServerDataSource : IDataSource
    {
        public string Name { get; set; }
        public IEnumerable<string> ConnectionParameters { get; }
        public List<ITable> Tables { get; set ; }

        public async Task<IEnumerable<ITable>> getTables()
        {
            throw new NotImplementedException();
        }
        public SQLServerDataSource()
        {
            List<string> connectionParameters = new List<string>();
            connectionParameters.Add("Username");
            connectionParameters.Add("Password");
            connectionParameters.Add("Initial Catalog");
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
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = Name;
                builder.UserID = connectionParametersMap.GetValueOrDefault("Username");
                builder.Password = connectionParametersMap.GetValueOrDefault("Password");
                builder.InitialCatalog = connectionParametersMap.GetValueOrDefault("Initial Catalog");
                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
