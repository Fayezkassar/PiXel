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

        public DbConnection Connection { get; set; }
        protected abstract void SetConnection();
        protected Dictionary<string, string> _connectionParametersMap;

        public bool Open(Dictionary<string, string> connectionParametersMap)
        {
            try
            {
                _connectionParametersMap = connectionParametersMap;
                SetConnection();
                Connection.Open();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public abstract DbConnection CreateTemporaryConnection();

        public bool Close()
        {
            try
            {
                Connection?.Close();
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
