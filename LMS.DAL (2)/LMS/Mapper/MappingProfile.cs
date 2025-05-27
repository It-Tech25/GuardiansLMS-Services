using AutoMapper;
using LMS.Components.Entities;
using LMS.Components.ModelClasses.CourseBatch;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LMS.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClassScheduleEntity, ClassScheduleDto>().ReverseMap();
        }
    }

}
