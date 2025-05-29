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
    public class StudentsDto
    {
        public int? StudentId { get; set; } // nullable for Add
        public string? StudentName { get; set; }
        public string? CourseName { get; set; }
        public string? EmailId { get; set; }
        public string? MobileNumber { get; set; }
        public string? Created { get; set; }
    }
    public class StudentsDD
    {
        public int? StudentId { get; set; } // nullable for Add
        public string? StudentName { get; set; }
    }

}
