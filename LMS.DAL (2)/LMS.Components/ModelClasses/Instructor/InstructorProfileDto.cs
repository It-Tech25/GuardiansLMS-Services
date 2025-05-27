using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.ModelClasses.Instructor
{
    public class InstructorProfileDto
    {
        public int InstructorId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int UserId { get; set; }
        public string InstructorName { get; set; } // from user table if applicable
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

}
