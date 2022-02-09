﻿using ImageResizingApp.Exceptions;
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

        private SqlConnection _connection;

        public IEnumerable<ITable> Tables { get; set; }

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
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = Name;
                builder.UserID = connectionParametersMap.GetValueOrDefault("Username");
                builder.Password = connectionParametersMap.GetValueOrDefault("Password");
                builder.InitialCatalog = connectionParametersMap.GetValueOrDefault("Initial Catalog");
                _connection = new SqlConnection(builder.ConnectionString);
                _connection.Open();
                string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                List<ITable> tables = new List<ITable>();
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(new SQLServerTable(reader.GetString(0)));
                        }
                    }
                }
                Tables = tables;
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
