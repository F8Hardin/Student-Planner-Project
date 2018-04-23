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
    public partial class MainWindow : Window
    {
        string todayDate = DateTime.Now.ToShortDateString();
        public List<Assignment> MyAssignments { get; set; } = new List<Assignment>();
        public List<Classinfo> MyClasses { get; set; } = new List<Classinfo>();
        private CalenderBackground background;

        public MainWindow()
        {
            System.IO.Directory.CreateDirectory("saves");
            create_files();

            InitializeComponent();
            this.Title = "Student Planner";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            background = new CalenderBackground(Calendar);
            background.AddOverlay("tjek", "../../tjek.png");
            background.AddOverlay("circle", "../../circle.png");
            background.AddOverlay("cross", "../../cross.png");
            background.AddOverlay("box", "../../box.png");

            calendar_dates();
            open_completed_file();
            open_classes_file();
            open_assignments_file();
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
            Monday.Items.Clear();
            Tuesday.Items.Clear();
            Wednesday.Items.Clear();
            Thursday.Items.Clear();
            Friday.Items.Clear();
            open_classes_file();
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
            due_today.Items.Clear();
            background.ClearDates();
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

                    string Month = homework.DueDate.Substring(0, 2);
                    int month = Convert.ToInt32(Month);

                    if (month > DateTime.Now.Month - 1)
                    {
                        viewCompletedAssignments.Items.Add(homework);
                    }
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

                    if(homework.DueDate == date)
                    {
                        due_today.Items.Add(homework);
                    }

                    MyAssignments.Add(homework);
                    date_check(homework, date);

                    Add_Calendar_Dates(homework, date);
                }
            }

            if (due_today.Items.Count == 0)
            {
                Assignment homework = new Assignment();
                homework.AssignName = "Nothing due today.";
                due_today.Items.Add(homework);
            }

            if(viewPastDue.Items.Count == 0)
            {
                Assignment homework = new Assignment();
                homework.AssignName = "No late assignments.";
                viewPastDue.Items.Add(homework);
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
                background.AddDate(new DateTime(dueYear, dueMonth, dueDay), "cross");
                Calendar.Background = background.GetBackground();
                Calendar.DisplayDateChanged += CalenderOnDisplayDateChanged;
                return;
            }
            if (thisMonth > dueMonth)
            {
                viewPastDue.Items.Add(homework);
                background.AddDate(new DateTime(dueYear, dueMonth, dueDay), "cross");
                Calendar.Background = background.GetBackground();
                Calendar.DisplayDateChanged += CalenderOnDisplayDateChanged;
                return;
            }
            if (thisDay > dueDay && thisMonth == dueMonth)
            {
                viewPastDue.Items.Add(homework);
                background.AddDate(new DateTime(dueYear, dueMonth, dueDay), "cross");
                Calendar.Background = background.GetBackground();
                Calendar.DisplayDateChanged += CalenderOnDisplayDateChanged;
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
                        Calendar.SelectedDates.Clear();
                    }
                }
                Calendar.SelectedDates.Clear();
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

        private void open_classes_file() //opens classes file to see the days the user has classes
        {
            using (var file = new System.IO.StreamReader(@"saves\StudentClassInfo.txt"))
            {
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    Classinfo course = new Classinfo();

                    course.Classname = line;
                    line = file.ReadLine();
                    course.Profname = line;
                    line = file.ReadLine();
                    course.Profemail = line;
                    line = file.ReadLine();
                    course.Time = line;
                    line = file.ReadLine();
                    course.Classdays = line;

                    if (course.Classdays.Contains("T"))
                    {
                        Tuesday.Items.Add(course);
                    }
                    if (course.Classdays.Contains("W"))
                    {
                        Wednesday.Items.Add(course);
                    }
                    if (course.Classdays.Contains("M"))
                    {
                        Monday.Items.Add(course);
                    }
                    if (course.Classdays.Contains("F"))
                    {
                        Friday.Items.Add(course);
                    }
                    if (course.Classdays.Contains("R"))
                    {
                        Thursday.Items.Add(course);
                    }
                }

                if(Monday.Items.Count == 0)
                {
                    Assignment homework = new Assignment();
                    homework.ClassName = "No classes";
                    Monday.Items.Add(homework);
                }
                if (Tuesday.Items.Count == 0)
                {
                    Assignment homework = new Assignment();
                    homework.ClassName = "No classes";
                    Tuesday.Items.Add(homework);
                }
                if (Wednesday.Items.Count == 0)
                {
                    Assignment homework = new Assignment();
                    homework.ClassName = "No classes";
                    Wednesday.Items.Add(homework);
                }
                if (Thursday.Items.Count == 0)
                {
                    Assignment homework = new Assignment();
                    homework.ClassName = "No classes";
                    Thursday.Items.Add(homework);
                }
                if (Friday.Items.Count == 0)
                {
                    Assignment homework = new Assignment();
                    homework.ClassName = "No classes";
                    Friday.Items.Add(homework);
                }

                file.Close();
            }
        }

        private void CalenderOnDisplayDateChanged(object sender, CalendarDateChangedEventArgs calendarDateChangedEventArgs) //this function comes is from Lars Pehrsson and his CalenderBackground class, see the class for details
        {
            Calendar.Background = background.GetBackground();
        }

        private void Add_Calendar_Dates(Assignment homework, string date) //puts the images on dates with assignments
        {
            string Month = homework.DueDate.Substring(0, 2);
            string Day = homework.DueDate.Substring(3, 2);
            string Year = homework.DueDate.Substring(6, 4);
            int dueyear = Convert.ToInt32(Year);
            int dueday = Convert.ToInt32(Day);
            int duemonth = Convert.ToInt32(Month);
            Month = date.Substring(0, 2);
            Day = date.Substring(3, 2);
            Year = date.Substring(6, 4);
            int thisYear = Convert.ToInt32(Year);
            int thisMonth = Convert.ToInt32(Month);
            int thisDay = Convert.ToInt32(Day);

            if (thisYear < dueyear)
            {
                background.AddDate(new DateTime(dueyear, duemonth, dueday), "circle");
            }
            if (thisMonth < duemonth)
            {
                background.AddDate(new DateTime(dueyear, duemonth, dueday), "circle");
            }
            if (thisDay < dueday)
            {
                background.AddDate(new DateTime(dueyear, duemonth, dueday), "circle");
            }
            Calendar.Background = background.GetBackground();
            Calendar.DisplayDateChanged += CalenderOnDisplayDateChanged;
        }
    }
}
