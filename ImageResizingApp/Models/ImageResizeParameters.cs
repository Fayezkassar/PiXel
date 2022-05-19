using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models
{
    public class ImageResizeParameters
    {
        public IFilter Filter { get; set; }
        public IQualityAssessment IQA { get; set; }

        public string BackupDestination { get; set; }

        public ImageResizeParameters(IFilter filter, IQualityAssessment iqa, string backupDestination)
        {
            IQA = iqa;
            Filter = filter;
            BackupDestination = backupDestination;
        }
    }
}
