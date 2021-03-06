using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Registry
{
    private Dictionary<string, IFilter> _filters = new Dictionary<string, IFilter>();
    private Dictionary<string, IQualityAssessment> _imageQualityAssessments = new Dictionary<string, IQualityAssessment>();
    private Dictionary<string, IDataSource> _dataSources = new Dictionary<string, IDataSource>();

    public IEnumerable<string> GetDataSourceKeys()
    {
        return _dataSources.Keys;
    }

    public void AddDataSource(string key, IDataSource dataSource)
    {
        _dataSources.Add(key, dataSource);
    }

    public IDataSource GetDataSourceFromKey(string key)
    {
        IDataSource dataSource;
        _dataSources.TryGetValue(key, out dataSource);
        return dataSource?.Clone();
    }

    public IEnumerable<string> GetFilterKeys()
    {
        return _filters.Keys;
    }

    public void AddFilter(string key, IFilter filter)
    {
        _filters.Add(key, filter);
    }

    public IFilter GetFilterFromKey(string key)
    {
        IFilter filter;
        _filters.TryGetValue(key, out filter);
        return filter?.Clone();
    }

    public IEnumerable<string> GetIQAKeys()
    {
        return _imageQualityAssessments.Keys;
    }

    public void AddIQA(string key, IQualityAssessment iqa)
    {
        _imageQualityAssessments.Add(key, iqa);
    }

    public IQualityAssessment GetIQAFromKey(string key)
    {
        IQualityAssessment iqa;
        _imageQualityAssessments.TryGetValue(key, out iqa);
        return iqa;
    }
}