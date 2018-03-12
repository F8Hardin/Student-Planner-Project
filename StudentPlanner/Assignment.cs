using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPlanner
{
    public class Assignment
    {
        public string AssignName { get; set; } //stores the assignment name
        public string ClassName { get; set; } //stores the class name for the assignment
        public string DueDate { get; set; } //stores the due date
        public string Notes { get; set; } //stores any additional notes the user enters
    }
}
