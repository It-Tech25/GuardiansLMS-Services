using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.ModelClasses.Instructor
{
    public class InstructorDto
    {
        public int InstructorId { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public int CreatedBy { get; set; }
    }
}
