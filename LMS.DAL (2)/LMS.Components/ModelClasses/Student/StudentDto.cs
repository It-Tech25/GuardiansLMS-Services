using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.ModelClasses.Student
{
    public class StudentDto
    {
        public int? StudentId { get; set; } // nullable for Add
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public int CreatedBy { get; set; }
    }

}
