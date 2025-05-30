using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.Instructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Interfaces
{
    public interface IInstructorRepo
    {
        GenericResponse AddInstructor(InstructorDto dto);
        GenericResponse UpdateInstructor(InstructorDto dto);
        GenericResponse DeleteInstructor(int id, int modifiedBy);
        List<InstructorDto> GetAllInstructors();
        InstructorDto GetInstructorById(int id);
        List<InstructorsDto> GetAllInstructorsDD();
        InstructorProfileDto GetInstructorProfile(int instructorId);

    }

}
