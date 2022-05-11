using ImageResizingApp.Models.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Common;

namespace ImageResizingApp.Models.DataSources
{
    public abstract class DBDataSource : IDataSource
    {
        public IEnumerable<string> ConnectionParameters { get; set; }
        public IEnumerable<ITable> Tables { get; set; }

        protected DbConnection _connection;
        protected abstract void SetConnection(Dictionary<string, string> connectionParametersMap);

        public bool Open(Dictionary<string, string> connectionParametersMap)
        {
            try
            {
                SetConnection(connectionParametersMap);
                _connection.Open();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Close()
        {
            try
            {
                _connection?.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public abstract Task SetTablesAsync();

        public abstract IDataSource Clone();
    }
}
