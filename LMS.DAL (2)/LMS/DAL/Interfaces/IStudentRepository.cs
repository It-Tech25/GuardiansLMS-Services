using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Interfaces
{
    public interface IStudentRepository
    {
        GenericResponse AddStudent(StudentDto dto);
        GenericResponse UpdateStudent(StudentDto dto, int userId);
        GenericResponse DeleteStudent(int studentId, int userId);
        IEnumerable<StudentsDto> GetAllStudents();
        StudentsDto GetStudentById(int studentId);
    }

}
