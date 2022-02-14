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
            string oradb = "Data Source=localhost:1521/hdfTest;" 
                + "User Id=SYS as SYSDBA;password=Testtest123;";

            _connection = new OracleConnection(oradb);
            _connection.Open();
            string test = "Connected to Oracle" + _connection.ServerVersion;

            return true;

        }
    }
}