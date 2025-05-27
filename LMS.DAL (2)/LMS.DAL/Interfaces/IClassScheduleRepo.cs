using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.CourseBatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Interfaces
{
    public interface IClassScheduleRepo
    {
        GenericResponse AddClassSchedule(ClassScheduleDto dto, int userId);
        GenericResponse UpdateClassSchedule(ClassScheduleDto dto, int userId);
        GenericResponse DeleteClassSchedule(int id, int userId);
        IEnumerable<ClassScheduleDto> GetAllSchedules();
        ClassScheduleDto GetScheduleById(int id);
    }

}
