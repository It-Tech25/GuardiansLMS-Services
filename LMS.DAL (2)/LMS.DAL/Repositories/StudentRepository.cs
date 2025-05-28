using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.Student;
using LMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly MyDbContext context;

        public StudentRepository(MyDbContext _context)
        {
            context = _context;
        }

        public GenericResponse AddStudent(StudentDto dto)
        {
            GenericResponse res = new GenericResponse();
            try
            {
                var entity = new StudentEntity
                {
                    UserId = dto.UserId,
                    CourseId = dto.CourseId,
                    CreatedBy = dto.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false
                };
                context.Students.Add(entity);
                context.SaveChanges();

                res.statusCode = 200;
                res.Message = "Student added successfully";
                res.CurrentId = entity.StudentId;
            }
            catch (Exception ex)
            {
                res.statusCode = 500;
                res.Message = ex.Message;
            }
            return res;
        }

        public GenericResponse UpdateStudent(StudentDto dto, int userId)
        {
            GenericResponse res = new GenericResponse();
            try
            {
                var entity = context.Students.FirstOrDefault(s => s.StudentId == dto.StudentId && !s.IsDeleted);
                if (entity == null)
                {
                    res.statusCode = 404;
                    res.Message = "Student not found";
                    return res;
                }

                entity.UserId = dto.UserId;
                entity.CourseId = dto.CourseId;
                entity.ModifiedBy = userId;
                entity.ModifiedOn = DateTime.UtcNow;

                context.SaveChanges();

                res.statusCode = 200;
                res.Message = "Student updated successfully";
                res.CurrentId = entity.StudentId;
            }
            catch (Exception ex)
            {
                res.statusCode = 500;
                res.Message = ex.Message;
            }
            return res;
        }

        public GenericResponse DeleteStudent(int studentId, int userId)
        {
            GenericResponse res = new GenericResponse();
            try
            {
                var entity = context.Students.FirstOrDefault(s => s.StudentId == studentId && !s.IsDeleted);
                if (entity == null)
                {
                    res.statusCode = 404;
                    res.Message = "Student not found";
                    return res;
                }

                entity.IsDeleted = true;
                entity.ModifiedBy = userId;
                entity.ModifiedOn = DateTime.UtcNow;

                context.SaveChanges();

                res.statusCode = 200;
                res.Message = "Student deleted (soft delete)";
            }
            catch (Exception ex)
            {
                res.statusCode = 500;
                res.Message = ex.Message;
            }
            return res;
        }

        public IEnumerable<StudentDto> GetAllStudents()
        {
            var res = context.Students
                 .Where(s => !s.IsDeleted)
                 .Select(s => new StudentDto
                 {
                     StudentId = s.StudentId,
                     StudentName = context.userEntities.Where(a=>a.UserId==s.UserId).Select(a=>a.UserName).FirstOrDefault(),
                     CourseName = context.courseTypes.Where(a => a.CourseId == s.CourseId).Select(a => a.CourseName).FirstOrDefault(),
                     Created = context.userEntities.Where(a => a.UserId == s.CreatedBy).Select(a => a.UserName).FirstOrDefault()
                 }).ToList();
            return res;
        }

        public StudentDto GetStudentById(int studentId)
        {
            return context.Students
                .Where(s => s.StudentId == studentId && !s.IsDeleted)
                .Select(s => new StudentDto
                {
                    StudentId = s.StudentId,
                    UserId = s.UserId,
                    CourseId = s.CourseId,
                    CreatedBy = s.CreatedBy
                }).FirstOrDefault();
        }
    }

}
