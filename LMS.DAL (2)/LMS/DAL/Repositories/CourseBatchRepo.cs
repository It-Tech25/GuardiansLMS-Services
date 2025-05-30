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
    public class CourseBatchRepo : ICourseBatchRepo
    {
        private readonly MyDbContext context;

        public CourseBatchRepo(MyDbContext ctx)
        {
            context = ctx;
        }

        public GenericResponse AddBatch(CourseBatchDto dto)
        {
            var res = new GenericResponse();
            try
            {
                var entity = new CourseBatchEntity
                {
                    CourseId = dto.CourseId,
                    InstructorId = dto.InstructorId,
                    StartDate = dto.StartDate,
                    Duration = dto.Duration,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    CreatedBy = dto.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false
                };

                context.CourseBatches.Add(entity);
                context.SaveChanges();

                res.statusCode = 200;
                res.Message = "Batch added";
                res.CurrentId = entity.BatchId;
            }
            catch (Exception ex)
            {
                res.statusCode = 500;
                res.Message = ex.Message;
            }
            return res;
        }

        public GenericResponse UpdateBatch(CourseBatchDto dto, int userId)
        {
            var res = new GenericResponse();
            try
            {
                var entity = context.CourseBatches.FirstOrDefault(x => x.BatchId == dto.BatchId && !x.IsDeleted);
                if (entity == null)
                {
                    res.statusCode = 404;
                    res.Message = "Batch not found";
                    return res;
                }

                entity.CourseId = dto.CourseId;
                entity.InstructorId = dto.InstructorId;
                entity.StartDate = dto.StartDate;
                entity.Duration = dto.Duration;
                entity.StartTime = dto.StartTime;
                entity.EndTime = dto.EndTime;
                entity.ModifiedBy = userId;
                entity.ModifiedOn = DateTime.UtcNow;

                context.SaveChanges();
                res.statusCode = 200;
                res.Message = "Batch updated";
            }
            catch (Exception ex)
            {
                res.statusCode = 500;
                res.Message = ex.Message;
            }
            return res;
        }

        public GenericResponse DeleteBatch(int batchId, int userId)
        {
            var res = new GenericResponse();
            try
            {
                var entity = context.CourseBatches.FirstOrDefault(x => x.BatchId == batchId && !x.IsDeleted);
                if (entity == null)
                {
                    res.statusCode = 404;
                    res.Message = "Batch not found";
                    return res;
                }

                entity.IsDeleted = true;
                entity.ModifiedBy = userId;
                entity.ModifiedOn = DateTime.UtcNow;

                context.SaveChanges();
                res.statusCode = 200;
                res.Message = "Batch deleted";
            }
            catch (Exception ex)
            {
                res.statusCode = 500;
                res.Message = ex.Message;
            }
            return res;
        }

        public List<CourseBatchDtos> GetAllBatches()
        {
             var result = context.CourseBatches
    .Where(x => !x.IsDeleted)
    .Select(x => new CourseBatchDtos
    {
        BatchId = x.BatchId,

        Course = context.courseTypes
                    .Where(a => a.CourseId == x.CourseId)
                    .Select(a => a.CourseName)
                    .FirstOrDefault(),

        Instructor = context.instructorEntity
                        .Where(i => i.InstructorId == x.InstructorId)
                        .Select(i => context.userEntities
                                    .Where(u => u.UserId == i.UserId)
                                    .Select(u => u.UserName)
                                    .FirstOrDefault())
                        .FirstOrDefault(),

        StartDate = x.StartDate,
        Duration = x.Duration,
        StartTime = x.StartTime,
        EndTime = x.EndTime
    }).ToList();
            return result;
        }
    }

}
