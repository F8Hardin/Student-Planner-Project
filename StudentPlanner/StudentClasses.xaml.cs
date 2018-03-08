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

        public Classinfo course;
        public List<Classinfo> MyClasses { get; set; } = new List<Classinfo>(); //list to store the classes entered by the user, list should be saved to file so in the other xaml file it can be opened

        public StudentClasses()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Title = "Your classes";

            Box_Setup();
            open_from_file();
        }

        public void addclass_Click(object sender, RoutedEventArgs e) //reads what the user has typed into the textboxes and saves them to the list while adding them to the listview
        {
            course = new Classinfo();

            if (className.Text == "" || profName.Text == "")
            {
                MessageBox.Show("Required Field Left Empty.");
                return;
            }

            if (profEmail.Text == "")
            {
                profEmail.Text = "-";
            }

            string time = HourBox.Text + ":" +  SecondBox.Text + " " + TimeBox.Text;
            
            course.Classname = className.Text;
            course.Profname = profName.Text;
            course.Profemail = profEmail.Text;
            course.Time = time;

            viewClassList.Items.Add(course);
            MyClasses.Add(course);

            className.Text = "";
            profName.Text = "";
            profEmail.Text = "";
            HourBox.Text = "";
            SecondBox.Text = "";
            TimeBox.Text = "";
        }

        private void save_Click(object sender, RoutedEventArgs e) //calls another funtion that loops through the list and reads it to the save file
        {
            save_to_file();
            this.Close();
        }

        private void save_to_file() //saves the data that is in the list to a file
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Fate\Desktop\School\StudentPlanner\saves\StudentsClassInfo.txt"))
            {
                foreach (var Classinfo in MyClasses)
                {
                    file.WriteLine("{0}\r\n{1}\r\n{2}\r\n{3}", Classinfo.Classname, Classinfo.Profname, Classinfo.Profemail, Classinfo.Time); //data seperated by new line in the file
                }
                file.Close();
            }
        }

        private void open_from_file() //reads any course info that is already entered into the list and listview
        {
            using (var file = new System.IO.StreamReader(@"C:\Users\Fate\Desktop\School\StudentPlanner\saves\StudentsClassInfo.txt"))
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

            for(int i = 1; i < 13; i++)
            {
                HourBox.Items.Add(i);
            }
            
            SecondBox.Items.Add("00");
            for(int i = 1; i < 61; i++)
            {
                SecondBox.Items.Add(i);
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e) //when clicked, deletes the class the user has selected
        {
            Classinfo course = (Classinfo)viewClassList.SelectedItems[0];

            MyClasses.Remove(course);
            viewClassList.Items.Remove(course);
        }
    }
}
