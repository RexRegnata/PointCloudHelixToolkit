// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SimpleDemo
{
    using HelixToolkit.Wpf.SharpDX.Utilities;
    using System;
    using System.Windows;
    using HelixToolkit.Wpf.SharpDX;
    using SharpDX;
    using Microsoft.Win32;
    using System.IO;
    using DemoCore;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            this.InitializeComponent();
            Closed += (s, e) =>
            {
                if (DataContext is IDisposable)
                {
                    (DataContext as IDisposable).Dispose();

                }
            };
        }
        static NVOptimusEnabler nvEnabler = new NVOptimusEnabler();     


        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                mainViewModel.filepath = openFileDialog.FileName;
            txtFile.Text = openFileDialog.FileName;
            //mainViewModel.dataFromTxt(mainViewModel.filepath,mainViewModel.positionX, mainViewModel.positionY, mainViewModel.positionZ);
            
        }
        private void btnDisplayFile_Click(object sender, RoutedEventArgs e)
        {
                       
           

        }


    }
}