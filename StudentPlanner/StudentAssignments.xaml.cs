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
        public List<Assignment> MyAssignments { get; set; } = new List<Assignment>();

        public StudentAssignments()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Title = "Your Assignments";
            open_from_file();
        }

        private void open_from_file() //reads the class names from a file, allowing the user to select a class from the combobox when creating an assignment
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

        private void add_click(object sender, RoutedEventArgs e)
        {
            Assignment homework = new Assignment();
            homework.ClassName = Classbox.Text;
            homework.DueDate = "ok";
            homework.AssignName = Assigntitle.Text;

            Classbox.Text = "";
            Assigntitle.Text = "";

            viewAssignmentList.Items.Add(homework);
            MyAssignments.Add(homework);
        }
    }
}
