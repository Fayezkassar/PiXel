using ImageResizingApp.Models.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        protected override void SetConnection(Dictionary<string, string> connectionParametersMap)
        {
            string oradb = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = "
                + connectionParametersMap.GetValueOrDefault("Host") + ")(PORT = " + connectionParametersMap.GetValueOrDefault("Port")
                + "))(CONNECT_DATA = (SERVICE_NAME = " + connectionParametersMap.GetValueOrDefault("Service Name") + "))); User Id = "
                + connectionParametersMap.GetValueOrDefault("Username") + "; Password = " + connectionParametersMap.GetValueOrDefault("Password") + ";";

            _connection = new OracleConnection(oradb);
        }

        public override async Task SetTablesAsync()
        {
            try
            {
                OracleConnection connection = _connection as OracleConnection;
                OracleCommand cmd = new OracleCommand("select table_name from user_tables order by table_name", connection);
                OracleDataReader dr = cmd.ExecuteReader();

                List<ITable> tables = new List<ITable>();

                while (await dr.ReadAsync())
                {
                    tables.Add(new OracleTable(connection)
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
    }
}