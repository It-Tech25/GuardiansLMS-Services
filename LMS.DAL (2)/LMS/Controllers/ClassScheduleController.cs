using AutoMapper;
using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.CourseBatch;
using LMS.DAL.Interfaces;
using LMS.Models.ModelClasses;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LMS.Controllers
{
    public class ClassScheduleController : Controller
    {
        private readonly MyDbContext context;
        private readonly IMapper mapper;

        public ClassScheduleController(MyDbContext context, IMapper mapper)
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

        public IEnumerable<ClassScheduleDto> GetAllSchedules(string searchterm="",int pagenumber=0,int pagesize=0)
        {
            var query = context.ClassSchedule.AsQueryable();

            query = query.Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                var term = searchterm.Trim().ToLower();
                query = query.Where(x => x.ClassDate.ToLongDateString().Contains(term));
            }

            int skip = (pagenumber - 1) * pagesize;
            query = query.Skip(skip).Take(pagesize);

            var list = query.ToList();
            return mapper.Map<IEnumerable<ClassScheduleDto>>(list);
        }


        public ClassScheduleDto GetScheduleById(int id)
        {
            var entity = context.ClassSchedule.FirstOrDefault(x => x.ScheduleId == id && !x.IsDeleted);
            return mapper.Map<ClassScheduleDto>(entity);
        }
    }

}
