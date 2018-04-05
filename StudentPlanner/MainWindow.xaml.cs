using System;
using System.IO;
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
        string todayDate = DateTime.Now.ToShortDateString();
        public List<Assignment> MyAssignments { get; set; } = new List<Assignment>();

        public MainWindow()
        {
            System.IO.Directory.CreateDirectory("saves");
            create_files();
            InitializeComponent();
            this.Title = "Student Planner";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            open_completed_file();
            open_assignments_file();
            calendar_dates();
        }

        private void close_Click(object sender, RoutedEventArgs e)//closes the main window
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to quit the applicaiton?", "Warning", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
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
            string date = "";
            StudentAssignments student = new StudentAssignments(date);
            student.ShowDialog();
            viewCompletedAssignments.Items.Clear();
            open_completed_file();
            viewPastDue.Items.Clear();
            MyAssignments.Clear();
            open_assignments_file();            
        }

        private void open_completed_file() //opens completed assignment
        {
            using (var file = new System.IO.StreamReader(@"saves\CompletedAssignments.txt"))
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
            MyAssignments = new List<Assignment>();

            using (var file = new System.IO.StreamReader(@"saves\StudentAssignments.txt"))
            {
                string line;
                string date = check_today(todayDate);

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

                    MyAssignments.Add(homework);
                    date_check(homework, date);
                }
            }
        }

        private void date_check(Assignment homework, string date) //checks the dates of assignments to see if past due
        {
            string Month = homework.DueDate.Substring(0, 2);
            string Day = homework.DueDate.Substring(3, 2);
            string Year = homework.DueDate.Substring(6, 4);

            int dueYear = Convert.ToInt32(Year);
            int dueMonth = Convert.ToInt32(Month);
            int dueDay = Convert.ToInt32(Day);

            Month = date.Substring(0, 2);
            Day = date.Substring(3, 2);
            Year = date.Substring(6, 4);

            int thisYear = Convert.ToInt32(Year);
            int thisMonth = Convert.ToInt32(Month);
            int thisDay = Convert.ToInt32(Day);

            if (thisYear > dueYear)
            {
                viewPastDue.Items.Add(homework);
                return;
            }
            if (thisMonth > dueMonth)
            {
                viewPastDue.Items.Add(homework);
                return;
            }
            if (thisDay > dueDay && thisMonth == dueMonth)
            {
                viewPastDue.Items.Add(homework);
            }
        }

        private string check_today(string date) //sets up the date read in so it can be treated the same as the due dates of assignments
        {
            if (date[1] == '/')
                date = date.Insert(0, "0");
            if (date[4] == '/')
                date = date.Insert(3, "0");

            return date;
        }

        private void calendar_dates() //crosses out the dates that have already passed
        {
            Calendar.DisplayDateStart = DateTime.Today;
            CalendarDateRange range = new CalendarDateRange(DateTime.MinValue, DateTime.Today);
            Calendar.BlackoutDates.Add(range);
        }

        private void date_clicked(object sender, SelectionChangedEventArgs e) //
        {
            var calendar = sender as Calendar;
            if (calendar.SelectedDate.HasValue)
            {
                DateTime date = calendar.SelectedDate.Value;
                List<Assignment> todays_assignments = check_date_due(date);
                string title = check_today(date.ToShortDateString());

                if (todays_assignments.Count != 0)
                {
                    string show = "Due Today: \n \n";

                    foreach (var Assignment in todays_assignments)
                    {
                        show += Assignment.ClassName;
                        show += ": \n";
                        show += Assignment.AssignName;
                        show += "\n -";

                        if (Assignment.Notes != "")
                        {
                            show += Assignment.Notes;
                            show += "\n";
                        }
                        else
                            show += "No Notes \n";
                        show += "\n";
                    }
                    MessageBox.Show(show);
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("There is no assignment for the selected date, would you like to add one?", title, MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        StudentAssignments student = new StudentAssignments(title);
                        student.ShowDialog();
                        viewCompletedAssignments.Items.Clear();
                        open_completed_file();
                        viewPastDue.Items.Clear();
                        open_assignments_file();
                    }
                }
            }
        }

        private List<Assignment> check_date_due(DateTime date) //checks to see if the date selected has any assignments due
        {
            string selected = date.ToShortDateString();
            selected = check_today(selected);
            List<Assignment> todays_assignments = new List<Assignment>();

            foreach (var Assignment in MyAssignments)
            {
                if (selected == Assignment.DueDate && !todays_assignments.Contains(Assignment))
                {
                    todays_assignments.Add(Assignment);
                }
            }

            return todays_assignments;
        }

        private void create_files() //creates the files needed to store assignments if they do not already exist
        {
            if (!File.Exists(@"saves\CompletedAssignments.txt"))
                File.Create(@"saves\CompletedAssignments.txt").Close();
            if (!File.Exists(@"saves\StudentAssignments.txt"))
                File.Create(@"saves\StudentAssignments.txt").Close();
            if (!File.Exists(@"saves\StudentClassInfo.txt"))
                File.Create(@"saves\StudentClassInfo.txt").Close();
        }
    }
}
