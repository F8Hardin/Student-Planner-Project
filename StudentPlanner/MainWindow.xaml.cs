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
            open_completed_file();
            open_assignments_file();
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
            StudentClasses student = new StudentClasses();
            student.ShowDialog();
        }

        private void assignments_click(object sender, RoutedEventArgs e) //opens the window to display assignments
        {
            StudentAssignments student = new StudentAssignments();
            student.ShowDialog();
            viewCompletedAssignments.Items.Clear();
            open_completed_file();
            viewPastDue.Items.Clear();
            open_assignments_file();
        }

        private void open_completed_file() //opens completed assignment
        {
            using (var file = new System.IO.StreamReader(@"C:\Users\Fate\source\repos\StudentPlanner\saves\CompletedAssignments.txt"))
            {
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    Assignment homework = new Assignment();

                    homework.AssignName = line;
                    line = file.ReadLine();
                    homework.Notes = line;
                    line = file.ReadLine();
                    homework.ClassName = line;
                    line = file.ReadLine();
                    homework.DueDate = line;

                    viewCompletedAssignments.Items.Add(homework);
                }
            }
        }

        private void open_assignments_file() //opens assignments to be seen which are past due
        {
            using (var file = new System.IO.StreamReader(@"C:\Users\Fate\source\repos\StudentPlanner\saves\StudentAssignments.txt"))
            {
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    Assignment homework = new Assignment();

                    homework.AssignName = line;
                    line = file.ReadLine();
                    homework.Notes = line;
                    line = file.ReadLine();
                    homework.ClassName = line;
                    line = file.ReadLine();
                    homework.DueDate = line;

                    string date = DateTime.Now.ToShortDateString();
                    string result = "0";

                    if (date[2] == '/')
                        result += date;

                    if (homework.DueDate != result);
                        viewPastDue.Items.Add(homework);
                    MessageBox.Show(result);
                }
            }
        }
    }
}
