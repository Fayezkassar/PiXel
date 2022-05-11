﻿using ImageResizingApp.Models.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace ImageResizingApp.Models.DataSources.SQLServer
{
    public class SQLServerDataSource : DBDataSource
    {
        public SQLServerDataSource()
        {
            Tables = new List<ITable>();
            List<string> connectionParameters = new List<string>();
            connectionParameters.Add("Username");
            connectionParameters.Add("Password");
            connectionParameters.Add("Initial Catalog");
            connectionParameters.Add("Data Source");
            ConnectionParameters = connectionParameters;

        }

        public override IDataSource Clone()
        {
            return (IDataSource)MemberwiseClone();
        }

        protected override void SetConnection(Dictionary<string, string> connectionParametersMap)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = connectionParametersMap.GetValueOrDefault("Data Source");
            builder.InitialCatalog = connectionParametersMap.GetValueOrDefault("Initial Catalog");
            builder.UserID = connectionParametersMap.GetValueOrDefault("Username");
            builder.Password = connectionParametersMap.GetValueOrDefault("Password");
            _connection = new SqlConnection(builder.ConnectionString);
        }

        public override async Task SetTablesAsync()
        {
            try
            {
                SqlConnection connection = _connection as SqlConnection;
                string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
                List<ITable> tables = new List<ITable>();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tables.Add(new SQLServerTable(reader.GetString(0)));
                        }
                    }
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
