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
            connectionParameters.Add("Database");
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
            throw new NotImplementedException();
        }

        public bool Open(Dictionary<string, string> connectionParametersMap)
        {
            string oradb = "Data Source=(DESCRIPTION =" 
                + "(ADDRESS = (PROTOCOL = TCP)(HOST = "+ connectionParametersMap.GetValueOrDefault("Host") + ")(PORT = "+ connectionParametersMap.GetValueOrDefault("Port") + "))" 
                + "(CONNECT_DATA =" + "(SERVER = DEDICATED)" + "(SERVICE_NAME = "+ connectionParametersMap.GetValueOrDefault("Database") + ")));" 
                + "User Id="+ connectionParametersMap.GetValueOrDefault("Username") + ";Password=" + connectionParametersMap.GetValueOrDefault("Password") + ";";

            _connection = new OracleConnection(oradb);
            _connection.Open();
            string test = "Connected to Oracle" + _connection.ServerVersion;

            return true;

        }
    }
}