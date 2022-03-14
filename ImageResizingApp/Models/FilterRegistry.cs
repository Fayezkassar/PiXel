using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models
{
    public class FilterRegistry
    {
        private Dictionary<string, IFilter> _filters = new Dictionary<string, IFilter>();

        public IEnumerable<string> GetKeys()
        {
            return _filters.Keys;
        }

        public void AddFilter(string key, IFilter filter)
        {
            _filters.Add(key, filter);
        }

        public IFilter getFilterFromKey(string key)
        {
            IFilter filter;
            _filters.TryGetValue(key, out filter);
            return filter;
        }
    }
}
