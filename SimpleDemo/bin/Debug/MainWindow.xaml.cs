﻿// --------------------------------------------------------------------------------------------------------------------
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
    }

}