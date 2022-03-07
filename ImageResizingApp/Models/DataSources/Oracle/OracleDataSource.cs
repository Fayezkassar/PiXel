using ImageResizingApp.Models.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    internal class OracleDataSource : IDataSource
    {
        public string Name { get; set; }

        public IEnumerable<string> ConnectionParameters { get; }

        public IEnumerable<ITable> Tables { get; set; }

        private OracleConnection _connection;

        public OracleDataSource()
        {
            List<string> connectionParameters = new List<string>();
            connectionParameters.Add("Host");
            connectionParameters.Add("Port");
            connectionParameters.Add("Service Name");
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

        private void SetConnection(Dictionary<string, string> connectionParametersMap)
        {
            string oradb = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = " 
                + connectionParametersMap.GetValueOrDefault("Host") + ")(PORT = " + connectionParametersMap.GetValueOrDefault("Port") 
                + "))(CONNECT_DATA = (SERVICE_NAME = " + connectionParametersMap.GetValueOrDefault("Service Name") + "))); User Id = " 
                + connectionParametersMap.GetValueOrDefault("Username") + "; Password = " + connectionParametersMap.GetValueOrDefault("Password") + ";";

            _connection = new OracleConnection(oradb);
        }

        public bool Open(Dictionary<string, string> connectionParametersMap)
        {
            try
            {
                SetConnection(connectionParametersMap);
                _connection.Open();

                OracleCommand cmd = new OracleCommand("select table_name from user_tables order by table_name", _connection);
                OracleDataReader dr = cmd.ExecuteReader();

                List<ITable> tables = new List<ITable>();

                while (dr.Read())
                {
                    tables.Add(new OracleTable(_connection)
                    {
                        Name = dr.GetString(0)
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
    }
}