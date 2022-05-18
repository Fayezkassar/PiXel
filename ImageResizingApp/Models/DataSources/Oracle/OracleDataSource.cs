using ImageResizingApp.Models.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Common;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    public class OracleDataSource : DBDataSource
    {
        public OracleDataSource()
        {
            Tables = new List<ITable>();
            List<string> connectionParameters = new List<string>();
            connectionParameters.Add("Host");
            connectionParameters.Add("Port");
            connectionParameters.Add("Service Name");
            connectionParameters.Add("Username");
            connectionParameters.Add("Password");
            ConnectionParameters = connectionParameters;

        }

        public override IDataSource Clone()
        {
            return (IDataSource)MemberwiseClone();
        }

        protected override void SetConnection()
        {
            Connection = this.CreateTemporaryConnection();
        }

        public override async Task SetTablesAsync()
        {
            try
            {
                OracleConnection connection = Connection as OracleConnection;
                OracleCommand cmd = new OracleCommand("select table_name from user_tables order by table_name", connection);
                OracleDataReader dr = cmd.ExecuteReader();

                List<ITable> tables = new List<ITable>();

                while (await dr.ReadAsync())
                {
                    tables.Add(new OracleTable(this, connection)
                    {
                        Name = dr.GetString(0)
                    });
                }

                Tables = tables;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        public override DbConnection CreateTemporaryConnection()
        {
            string oradb = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = "
                + _connectionParametersMap.GetValueOrDefault("Host") + ")(PORT = " + _connectionParametersMap.GetValueOrDefault("Port")
                + "))(CONNECT_DATA = (SERVICE_NAME = " + _connectionParametersMap.GetValueOrDefault("Service Name") + "))); User Id = "
                + _connectionParametersMap.GetValueOrDefault("Username") + "; Password = " + _connectionParametersMap.GetValueOrDefault("Password") + ";";

             return new OracleConnection(oradb);
        }
    }
}