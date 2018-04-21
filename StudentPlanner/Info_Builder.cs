using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPlanner
{
    class Info_Builder : Builder
    {
        public override Assignment Assignment_build(string assignName, string className, string date, string notes)
        {
            Assignment homework = new Assignment();
            homework.AssignName = assignName;
            homework.ClassName = className;
            homework.DueDate = date;
            homework.Notes = notes;

            return homework;
        }

        public override Classinfo Class_build(string className, string profName, string profEmail, string time, string classDays)
        {
            Classinfo course = new Classinfo();
            course.Classname = className;
            course.Profname = profName;
            course.Profemail = profEmail;
            course.Time = time;
            course.Classdays = classDays;

            return course;
        }
    }
}
