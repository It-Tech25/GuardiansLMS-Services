using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.Instructor;
using LMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repositories
{
    public class InstructorRepo : IInstructorRepo
    {
        private readonly MyDbContext context;

        public InstructorRepo(MyDbContext context)
        {
            this.context = context;
        }

        public GenericResponse AddInstructor(InstructorDto dto)
        {
            var response = new GenericResponse();
            try
            {
                var entity = new InstructorEntity
                {
                    CourseId = dto.CourseId,
                    UserId = dto.UserId,
                    CreatedBy = dto.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false
                };

                context.instructorEntity.Add(entity);
                context.SaveChanges();

                response.statusCode = 200;
                response.Message = "Instructor added successfully";
                response.CurrentId = entity.InstructorId;
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public GenericResponse UpdateInstructor(InstructorDto dto)
        {
            var response = new GenericResponse();
            try
            {
                var instructor = context.instructorEntity.Find(dto.InstructorId);
                if (instructor == null || instructor.IsDeleted)
                {
                    response.statusCode = 404;
                    response.Message = "Instructor not found";
                    return response;
                }

                instructor.CourseId = dto.CourseId;
                instructor.UserId = dto.UserId;
                instructor.ModifiedBy = dto.CreatedBy;
                instructor.ModifiedOn = DateTime.UtcNow;

                context.SaveChanges();

                response.statusCode = 200;
                response.Message = "Instructor updated successfully";
                response.CurrentId = instructor.InstructorId;
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public GenericResponse DeleteInstructor(int id, int modifiedBy)
        {
            var response = new GenericResponse();
            try
            {
                var instructor = context.instructorEntity.Find(id);
                if (instructor == null || instructor.IsDeleted)
                {
                    response.statusCode = 404;
                    response.Message = "Instructor not found";
                    return response;
                }

                instructor.IsDeleted = true;
                instructor.ModifiedBy = modifiedBy;
                instructor.ModifiedOn = DateTime.UtcNow;

                context.SaveChanges();

                response.statusCode = 200;
                response.Message = "Instructor deleted successfully";
                response.CurrentId = instructor.InstructorId;
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public List<InstructorDto> GetAllInstructors()
        {
            return context.instructorEntity
                .Where(x => !x.IsDeleted)
                .Select(x => new InstructorDto
                {
                    InstructorId = x.InstructorId,
                    CourseId = x.CourseId,
                    UserId = x.UserId,
                    CreatedBy = x.CreatedBy
                }).ToList();
        }

        public InstructorDto GetInstructorById(int id)
        {
            var instructor = context.instructorEntity
                .FirstOrDefault(x => x.InstructorId == id && !x.IsDeleted);

            if (instructor == null) return null;

            return new InstructorDto
            {
                InstructorId = instructor.InstructorId,
                CourseId = instructor.CourseId,
                UserId = instructor.UserId,
                CreatedBy = instructor.CreatedBy
            };
        }

        public InstructorProfileDto GetInstructorProfile(int instructorId)
        {
            var data = (from i in context.instructorEntity
                        join c in context.courseTypes on i.CourseId equals c.CourseId
                        join u in context.userEntities on i.UserId equals u.UserId
                        where i.InstructorId == instructorId && !i.IsDeleted
                        select new InstructorProfileDto
                        {
                            InstructorId = i.InstructorId,
                            CourseId = i.CourseId,
                            CourseName = c.CourseName,
                            UserId = i.UserId,
                            InstructorName = u.UserName, // or FirstName + LastName
                            CreatedOn = i.CreatedOn,
                            ModifiedOn = i.ModifiedOn
                        }).FirstOrDefault();

            return data;
        }


    }

}
