using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPlanner
{
    abstract class Builder
    {
        public abstract Assignment Assignment_build(string assignName, string className, string date, string notes);
        public abstract Classinfo Class_build(string className, string profName, string profEmail, string time, string classdays);
    }
}
