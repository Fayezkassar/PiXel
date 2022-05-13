using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models
{
    public class ResizeConfig
    {

        public int progressPercentage { get; set; }
        public int successNumber { get; set; }

        public decimal totalCount { get; set; }

        public double spaceGain { get; set; }

        public ResizeConfig(int progressPercentage, int successNumber, decimal totalCount, double spaceGain)
        {
            this.progressPercentage = progressPercentage;
            this.successNumber = successNumber;
            this.totalCount = totalCount;
            this.spaceGain = spaceGain;
        }

        public ResizeConfig()
        {
            
        }

        public class ProgressChangedEventHandler : EventArgs
        {
            public ResizeConfig Config
            {
                get;
                private set;
            }

            public ProgressChangedEventHandler(ResizeConfig config)
            {
                Config = config;
            }
        }
    }
}
