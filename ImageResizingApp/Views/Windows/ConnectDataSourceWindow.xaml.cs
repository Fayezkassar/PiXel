﻿using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageResizingApp.Views.Windows
{
    /// <summary>
    /// Interaction logic for ConnectDataSourceWindow.xaml
    /// </summary>
    public partial class ConnectDataSourceWindow : Window
    {
        public ConnectDataSourceWindow()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e) =>
            Close();
    }
}
