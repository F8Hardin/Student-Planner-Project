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
    /// Interaction logic for StudentAssignments.xaml
    /// </summary>
    public partial class StudentAssignments : Window
    {
        public List<Assignment> MyAssignments { get; set; } = new List<Assignment>(); //list to store assignments

        public StudentAssignments()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Title = "Your Assignments";
            open_classes_file();
            open_assignments_file();
            Box_Setup();
        }

        private void open_classes_file() //reads the class names from a file, allowing the user to select a class from the combobox when creating an assignment
        {
            using (var file = new System.IO.StreamReader(@"C:\Users\Fate\source\repos\StudentPlanner\saves\StudentClassInfo.txt"))
            {
                string line;
                string className;
                while((line = file.ReadLine()) != null)
                {
                    className = line;
                    line = file.ReadLine();
                    line = file.ReadLine();
                    line = file.ReadLine();
                    Classbox.Items.Add(className);
                }
            }
        }

        private void add_click(object sender, RoutedEventArgs e) //adds the assignments to the list and the listview
        {
            Assignment homework = new Assignment();
            homework.ClassName = Classbox.Text;

            string date = monthbox.Text + "/" + daybox.Text + "/" + yearbox.Text;

            homework.DueDate = date;
            homework.AssignName = Assigntitle.Text;
            homework.Notes = notes.Text;

            Classbox.Text = "";
            Assigntitle.Text = "";
            daybox.Text = "";
            monthbox.Text = "";
            yearbox.Text = "";
            notes.Text = "";

            viewAssignmentList.Items.Add(homework);
            MyAssignments.Add(homework);
        }

        private void Box_Setup() //sets up the comboboxes
        {
            for(int i = 1; i < 13; i++)
            {
                monthbox.Items.Add(i);
            }

            for(int i = 1; i < 32; i++)
            {
                daybox.Items.Add(i);
            }
        }

        private void save_to_file() //saves the assignments to a file
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Fate\source\repos\StudentPlanner\saves\StudentAssignments.txt"))
            {
                foreach (var Assignment in MyAssignments)
                {
                    file.WriteLine("{0}\r\n{1}\r\n{2}\r\n{3}", Assignment.AssignName, Assignment.Notes, Assignment.ClassName, Assignment.DueDate);
                }
                file.Close();
            }
        }

        private void save_exit_Click(object sender, RoutedEventArgs e) //calls the save function when clicked
        {
            save_to_file();

            MessageBoxResult result = MessageBox.Show("Assignments saved. Are you sure you want to exit?", "Important Message", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            this.Close();
        }

        private void view_Click(object sender, RoutedEventArgs e) //allows the user to see all the information related to an assignment
        {
            if(viewAssignmentList.SelectedItems.Count == 0)
            {
                MessageBox.Show("No assignment selected.");
                return;
            }

            Assignment homework = (Assignment)viewAssignmentList.SelectedItems[0];
            MessageBox.Show("Assignment Name: " + homework.AssignName + "\nCourse Title: " + homework.ClassName + "\nDue Date: " + homework.DueDate + "\nAdditional Notes: " + homework.Notes);
        }

        private void open_assignments_file()
        {
            using (var file = new System.IO.StreamReader(@"C:\Users\Fate\source\repos\StudentPlanner\saves\StudentAssignments.txt"))
            {
                string line;

                while((line = file.ReadLine()) != null)
                {
                    Assignment homework = new Assignment();

                    homework.AssignName = line;
                    line = file.ReadLine();
                    homework.Notes = line;
                    line = file.ReadLine();
                    homework.ClassName = line;
                    line = file.ReadLine();
                    homework.DueDate = line;

                    MyAssignments.Add(homework);
                    viewAssignmentList.Items.Add(homework);
                }
            }
        }
    }
}
