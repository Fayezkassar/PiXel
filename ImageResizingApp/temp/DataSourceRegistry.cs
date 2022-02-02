
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

    private static readonly DataSourceRegistry instance;

    private IEnumerable<IDataSource> dataSources;

    public static DataSourceRegistry getInstance()
    {
        return instance;
    }

    public IEnumerable<String> getDataSourceNames()
    {
        /* TO IMPLEMENT*/
        return null;
    }

    public IDataSource getDataSourceFor(String name)
    {
        /* TO IMPLEMENT*/
        return null;
    }

    public void addDataSource(IDataSource dataSource)
    {
        /* TO IMPLEMENT*/
    }

}