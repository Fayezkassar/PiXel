
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DataSourceRegistry
{

    public DataSourceRegistry()
    {
    }

    //private static DataSourceRegistry _instance;

    private Dictionary<string,IDataSource> _dataSources = new Dictionary<string,IDataSource>();

    //public static DataSourceRegistry GetInstance()
    //{
    //    if(_instance == null)
    //    {
    //        _instance = new DataSourceRegistry();
    //    }
    //    return _instance;
    //}

    public List<string> GetDataSourceNames()
    {

        return this._dataSources.Keys.ToList();
    }

    public void AddDataSource(string key, IDataSource dataSource)
    {
        this._dataSources.Add(key, dataSource);
    }

}