﻿using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace ImageResizingApp.Models.DataSources.SQLServer
{
    public class SQLServerTable : ITable
    {
        public string Name { get; set; }
        public string TableSize { get; set; }
        public string RecordsNumber { get; set; }
        public string RecordSize { get; set; }

        public IEnumerable<IColumn> GetColumns()
        {
            throw new NotImplementedException();
        }

        public SQLServerTable(string name)
        {
            Name = name;
        }

        public void SetStats()
        {
            throw new NotImplementedException();
        }

        public DataTable GetData(int start, int itemCount)
        {
            throw new NotImplementedException();
        }
    }
}