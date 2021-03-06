﻿using System.Text;
using System.Linq;
using System;
using System.Collections.Generic;
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
    public partial class StudentClasses : Window
    {
        public List<Classinfo> MyClasses { get; set; } = new List<Classinfo>(); //list to store the classes entered by the user, list should be saved to file so in the other xaml file it can be opened

        public StudentClasses() //opens up files, sets up boxes, and sets up the window
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Title = "Your classes";
            Box_Setup();
            open_from_file();
        }

        public void addclass_Click(object sender, RoutedEventArgs e) //reads what the user has typed into the textboxes and saves them to the list while adding them to the listview
        {
            Builder build = new Info_Builder();

            if (className.Text == "" || profName.Text == "")
            {
                MessageBox.Show("Required field left blank.");
                return;
            }

            if (profEmail.Text == "")
            {
                profEmail.Text = "-";
            }

            if (HourBox.Text == "")
            {
                HourBox.Text = "-";
                MinuteBox.Text = "--";
            }
            else if (MinuteBox.Text == "")
            {
                MinuteBox.Text = "00";
            }

            string time = HourBox.Text + ":" + MinuteBox.Text + " " + TimeBox.Text;
            string days = "";

            if (Monday.IsChecked ?? false)
                days += "M ";
            if (Tuesday.IsChecked ?? false)
                days += "T ";
            if (Wednesday.IsChecked ?? false)
                days += "W ";
            if (Thursday.IsChecked ?? false)
                days += "R ";
            if (Friday.IsChecked ?? false)
                days += "F ";

            Classinfo course = build.Class_build(className.Text, profName.Text, profEmail.Text, time, days);

            viewClassList.Items.Add(course);
            MyClasses.Add(course);

            className.Text = "";
            profName.Text = "";
            profEmail.Text = "";
            HourBox.Text = "";
            MinuteBox.Text = "";
            TimeBox.Text = "";
            addclass.Content = "Add Class";
            Monday.IsChecked = false;
            Tuesday.IsChecked = false;
            Wednesday.IsChecked = false;
            Thursday.IsChecked = false;
            Friday.IsChecked = false;
        }

        private void save_exit_Click(object sender, RoutedEventArgs e) //calls another funtion that loops through the list and reads it to the save file
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to keep these changes to your schedule?", "Important Message", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            save_to_file();
            this.Close();
        }

        private void save_to_file() //saves the data that is in the list to a file
        {
            MyClasses.Sort((x, y) => x.Time.CompareTo(y.Time));
            MyClasses.Sort((x, y) => x.Time.Substring(x.Time.Length - 2).CompareTo(y.Time.Substring(y.Time.Length - 2)));

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"saves\StudentClassInfo.txt"))
            {
                foreach (var Classinfo in MyClasses)
                {
                    file.WriteLine("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}", Classinfo.Classname, Classinfo.Profname, Classinfo.Profemail, Classinfo.Time, Classinfo.Classdays); //data seperated by new line in the file
                }
                file.Close();
            }
        }

        private void open_from_file() //reads any course info that is already entered into the list and listview
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

                    viewClassList.Items.Add(course);
                    MyClasses.Add(course);

                }
                file.Close();
            }
        }

        public void Box_Setup() //sets up the comboboxes for setting the time
        {
            TimeBox.Items.Add("AM");
            TimeBox.Items.Add("PM");

            for (int i = 1; i < 13; i++)
            {
                HourBox.Items.Add(i);
            }

            MinuteBox.Items.Add("00");
            for (int i = 1; i < 61; i++)
            {
                if (i < 10)
                    MinuteBox.Items.Add("0" + i);
                else
                    MinuteBox.Items.Add(i);
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e) //when clicked, deletes the class the user has selected
        {
            if (viewClassList.SelectedItems.Count == 0) //makes sure something is selected, avoiding null case
            {
                MessageBox.Show("Nothing Selected");
                return;
            }

            Classinfo course = (Classinfo)viewClassList.SelectedItems[0];

            MessageBoxResult result = MessageBox.Show("Are you sure you want to remove " + course.Classname + " from your schedule? (This will remove all assignments associated with this class once changes are saved.)", "Important Message", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            //after assignments have been fixed up, this function needs to loop through the file where assignments are saved and delete any assocaiated with the selected class
            MyClasses.Remove(course);
            viewClassList.Items.Remove(course);
        }

        private void edit_Click(object sender, RoutedEventArgs e) //allows the user to edit the course selected
        {
            if (viewClassList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Nothing Selected");
                return;
            }

            MessageBox.Show("Save changes after they have been made.");

            addclass.Content = "Save Changes";

            Classinfo course = (Classinfo)viewClassList.SelectedItems[0];

            className.Text = course.Classname;
            profName.Text = course.Profname;
            if (course.Profemail == "-")
            {
                profEmail.Text = "";
            }
            else
                profEmail.Text = course.Profemail;

            if (course.Time == "-:--")
            {
                HourBox.Text = "";
                MinuteBox.Text = "";
                TimeBox.Text = "";

                viewClassList.Items.Remove(course);
                MyClasses.Remove(course);

                return;
            }

            if (course.Classdays.Contains("M"))
                Monday.IsChecked = true;
            if (course.Classdays.Contains("T"))
                Tuesday.IsChecked = true;
            if (course.Classdays.Contains("W"))
                Wednesday.IsChecked = true;
            if (course.Classdays.Contains("R"))
                Thursday.IsChecked = true;
            if (course.Classdays.Contains("F"))
                Friday.IsChecked = true;

            int count = 0;
            string time = course.Time;
            string hour = "";
            string minute = "";

            foreach (char c in time)
            {
                count++;
                if (c != ':' && count < 3)
                {
                    hour += c;
                    time = time.Remove(0, 1);
                }
            }
            time = time.Remove(0, 1);
            minute = time.Substring(0, 2);
            time = time.Remove(0, 3);

            HourBox.Text = hour;
            MinuteBox.Text = minute;
            TimeBox.Text = time;

            viewClassList.Items.Remove(course);
            MyClasses.Remove(course);
        }

        private void save_Click(object sender, RoutedEventArgs e) //saves any changes made
        {
            save_to_file();
            MessageBox.Show("Changes saved.");
        }
    }
}