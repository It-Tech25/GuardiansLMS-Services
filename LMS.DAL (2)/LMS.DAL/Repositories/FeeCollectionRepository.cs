using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.FeeCollection;
using LMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repositories
{
    public class FeeCollectionRepository : IFeeCollectionRepository
    {
        private readonly MyDbContext context;

        public FeeCollectionRepository(MyDbContext _context)
        {
            context = _context;
        }

        public GenericResponse AddFee(FeeCollectionDto dto)
        {
            var res = new GenericResponse();
            try
            {
                var fee = new FeeCollection
                {
                    StudentId = dto.StudentId,
                    Amount = dto.Amount,
                    PaymentDate = dto.PaymentDate,
                    PaymentMode = dto.PaymentMode,
                    Remarks = dto.Remarks,
                    CreatedBy = dto.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false
                };

                context.FeeCollections.Add(fee);
                context.SaveChanges();

                res.statusCode = 200;
                res.Message = "Fee added successfully";
                res.CurrentId = fee.FeeId;
            }
            catch (Exception ex)
            {
                res.statusCode = 500;
                res.Message = ex.Message;
            }
            return res;
        }

        public GenericResponse UpdateFee(FeeCollectionDto dto, int userId)
        {
            var res = new GenericResponse();
            try
            {
                var fee = context.FeeCollections.FirstOrDefault(f => f.FeeId == dto.FeeId && !f.IsDeleted);
                if (fee == null)
                {
                    res.statusCode = 404;
                    res.Message = "Fee not found";
                    return res;
                }

                fee.Amount = dto.Amount;
                fee.PaymentDate = dto.PaymentDate;
                fee.PaymentMode = dto.PaymentMode;
                fee.Remarks = dto.Remarks;
                fee.ModifiedBy = userId;
                fee.ModifiedOn = DateTime.UtcNow;

                context.SaveChanges();

                res.statusCode = 200;
                res.Message = "Fee updated successfully";
                res.CurrentId = fee.FeeId;
            }
            catch (Exception ex)
            {
                res.statusCode = 500;
                res.Message = ex.Message;
            }
            return res;
        }

        public GenericResponse DeleteFee(int feeId, int userId)
        {
            var res = new GenericResponse();
            try
            {
                var fee = context.FeeCollections.FirstOrDefault(f => f.FeeId == feeId && !f.IsDeleted);
                if (fee == null)
                {
                    res.statusCode = 404;
                    res.Message = "Fee not found";
                    return res;
                }

                fee.IsDeleted = true;
                fee.ModifiedBy = userId;
                fee.ModifiedOn = DateTime.UtcNow;

                context.SaveChanges();

                res.statusCode = 200;
                res.Message = "Fee deleted (soft)";
            }
            catch (Exception ex)
            {
                res.statusCode = 500;
                res.Message = ex.Message;
            }
            return res;
        }

        public IEnumerable<FeeCollectionDto> GetAllFees()
        {
            return context.FeeCollections
                .Where(f => !f.IsDeleted)
                .Select(f => new FeeCollectionDto
                {
                    FeeId = f.FeeId,
                    StudentId = f.StudentId,
                    Amount = f.Amount,
                    PaymentDate = f.PaymentDate,
                    PaymentMode = f.PaymentMode,
                    Remarks = f.Remarks,
                    CreatedBy = f.CreatedBy
                }).ToList();
        }

        public FeeCollectionDto GetFeeById(int feeId)
        {
            return context.FeeCollections
                .Where(f => f.FeeId == feeId && !f.IsDeleted)
                .Select(f => new FeeCollectionDto
                {
                    FeeId = f.FeeId,
                    StudentId = f.StudentId,
                    Amount = f.Amount,
                    PaymentDate = f.PaymentDate,
                    PaymentMode = f.PaymentMode,
                    Remarks = f.Remarks,
                    CreatedBy = f.CreatedBy
                }).FirstOrDefault();
        }
    }

}
