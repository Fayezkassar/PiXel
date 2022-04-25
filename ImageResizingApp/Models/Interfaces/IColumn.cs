using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IColumn
    {
        public ITable Table { get; set; }
        public string Name { get; set; }
        public string ColumnType { get; set; }
        public bool Resizable { get;set; }
        public bool Resize(int? from, int? to, int? minSize, int? maxSize, IFilter filter, string backupDestination);
        public IImage GetImageWithPrimaryKeysValues(IEnumerable<string> primaryKeysValues);

        public event EventHandler<ProgressChangedEventHandler> ProgressChanged;
        public class ProgressChangedEventHandler : EventArgs
        {
            public int Progress
            {
                get;
                private set;
            }

            public ProgressChangedEventHandler(int progress)
            {
                Progress = progress;
            }
        }

    }
}
