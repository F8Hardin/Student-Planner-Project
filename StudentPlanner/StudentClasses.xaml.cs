using System;
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
using System.Windows.Shapes;

namespace StudentPlanner
{
    /// <summary>
    /// Interaction logic for StudentClasses.xaml
    /// </summary>
    public partial class StudentClasses : Window
    {

        public List<Classinfo> MyClasses { get; set; } //list to store the classes entered by the user

        public StudentClasses()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Title = "Your classes";

            //testing adding to a listview
            MyClasses = new List<Classinfo>();
            Classinfo class1 = new Classinfo();
            class1.Classname = "ok";
            class1.Profname = "Dr. Ericson";
            MyClasses.Add(class1);
            DataContext = this;

        }

        private void addclass_Click(object sender, RoutedEventArgs e)//open a textbox where user can type in a class name and add it to the listview
        {
            //figure out how to get user input and store it in the list
        }
    }
}
