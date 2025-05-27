using AutoMapper;
using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.CourseBatch;
using LMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LMS.DAL.Repositories
{
    public class ClassScheduleRepo : IClassScheduleRepo
    {
        private readonly MyDbContext context;
        private readonly IMapper mapper;

        public ClassScheduleRepo(MyDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public GenericResponse AddClassSchedule(ClassScheduleDto dto, int userId)
        {
            var entity = mapper.Map<ClassScheduleEntity>(dto);
            entity.CreatedBy = userId;
            entity.CreatedOn = DateTime.UtcNow;
            context.ClassSchedule.Add(entity);
            context.SaveChanges();

            return new GenericResponse { statusCode = 200, Message = "Class schedule created", CurrentId = entity.ScheduleId };
        }

        public GenericResponse UpdateClassSchedule(ClassScheduleDto dto, int userId)
        {
            var entity = context.ClassSchedule.Find(dto.ScheduleId);
            if (entity == null)
                return new GenericResponse { statusCode = 404, Message = "Schedule not found" };

            mapper.Map(dto, entity);
            entity.ModifiedBy = userId;
            entity.ModifiedOn = DateTime.UtcNow;

            context.SaveChanges();
            return new GenericResponse { statusCode = 200, Message = "Schedule updated", CurrentId = entity.ScheduleId };
        }

        public GenericResponse DeleteClassSchedule(int id, int userId)
        {
            var entity = context.ClassSchedule.Find(id);
            if (entity == null)
                return new GenericResponse { statusCode = 404, Message = "Schedule not found" };

            entity.IsDeleted = true;
            entity.ModifiedBy = userId;
            entity.ModifiedOn = DateTime.UtcNow;

            context.SaveChanges();
            return new GenericResponse { statusCode = 200, Message = "Schedule deleted" };
        }

        public IEnumerable<ClassScheduleDto> GetAllSchedules()
        {
            return mapper.Map<IEnumerable<ClassScheduleDto>>(
                context.ClassSchedule.Where(x => !x.IsDeleted).ToList()
            );
        }

        public ClassScheduleDto GetScheduleById(int id)
        {
            var entity = context.ClassSchedule.FirstOrDefault(x => x.ScheduleId == id && !x.IsDeleted);
            return mapper.Map<ClassScheduleDto>(entity);
        }
    }

}
