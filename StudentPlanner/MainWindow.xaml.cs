﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Student Planner";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //add function to read in file with class info
        }

        private void close_Click(object sender, RoutedEventArgs e)//closes the main window
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to quit the applicaiton?", "Warning", MessageBoxButton.OKCancel);
            if(result == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        private void classes_Click(object sender, RoutedEventArgs e)//opens the window to display classes
        {
            StudentClasses studentC = new StudentClasses();
            studentC.ShowDialog();
        }
    }
}
