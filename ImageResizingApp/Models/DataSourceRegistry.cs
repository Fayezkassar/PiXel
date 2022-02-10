
using ImageResizingApp.Exceptions;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DataSourceRegistry
{
    private Dictionary<string, IDataSource> _dataSources = new Dictionary<string, IDataSource>();

    public IEnumerable<string> GetKeys()
    {
        return _dataSources.Keys;
    }

    public void AddDataSource(string key, IDataSource dataSource)
    {
        _dataSources.Add(key, dataSource);
    }

    public IDataSource getDataSourceFromKey(string key)
    {
        IDataSource dataSource;
        _dataSources.TryGetValue(key, out dataSource);
        return dataSource?.Clone();
    }
}