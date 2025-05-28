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
    public class FeeReceiptRepository : IFeeReceiptRepository
    {
        private readonly MyDbContext context;

        public FeeReceiptRepository(MyDbContext _context)
        {
            context = _context;
        }

        public GenericResponse AddReceipt(FeeReceiptDto dto)
        {
            var res = new GenericResponse();
            try
            {
                var receipt = new FeeReceipt
                {
                    FeeId = dto.FeeId,
                    ReceiptNumber = GenerateReceiptNumber(), // Auto-generate
                    IssuedDate = dto.IssuedDate,
                    IssuedBy = dto.IssuedBy,
                    Remarks = dto.Remarks,
                    CreatedBy = dto.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false
                };

                context.FeeReceipts.Add(receipt);
                context.SaveChanges();

                res.statusCode = 200;
                res.Message = "Receipt created successfully";
                res.CurrentId = receipt.ReceiptId;
            }
            catch (Exception ex)
            {
                res.statusCode = 500;
                res.Message = ex.Message;
            }

            return res;
        }

        private string GenerateReceiptNumber()
        {
            var year = DateTime.UtcNow.Year;

            
            var lastReceipt = context.FeeReceipts
                .Where(r => r.IssuedDate.Year == year)
                .OrderByDescending(r => r.ReceiptId)
                .FirstOrDefault();

            int nextNumber = 1;

            if (lastReceipt != null)
            {
                var lastNumberPart = lastReceipt.ReceiptNumber?.Split('-').Last();
                if (int.TryParse(lastNumberPart, out int parsed))
                {
                    nextNumber = parsed + 1;
                }
            }

            return $"RCPT-{year}-{nextNumber.ToString("D4")}";
        }


        public GenericResponse UpdateReceipt(FeeReceiptDto dto, int userId)
        {
            var res = new GenericResponse();
            try
            {
                var receipt = context.FeeReceipts.FirstOrDefault(r => r.ReceiptId == dto.ReceiptId && !r.IsDeleted);
                if (receipt == null)
                {
                    res.statusCode = 404;
                    res.Message = "Receipt not found";
                    return res;
                }

                receipt.ReceiptNumber = dto.ReceiptNumber;
                receipt.IssuedDate = dto.IssuedDate;
                receipt.IssuedBy = dto.IssuedBy;
                receipt.Remarks = dto.Remarks;
                receipt.ModifiedBy = userId;
                receipt.ModifiedOn = DateTime.UtcNow;

                context.SaveChanges();

                res.statusCode = 200;
                res.Message = "Receipt updated successfully";
                res.CurrentId = receipt.ReceiptId;
            }
            catch (Exception ex)
            {
                res.statusCode = 500;
                res.Message = ex.Message;
            }

            return res;
        }

        public GenericResponse DeleteReceipt(int receiptId, int userId)
        {
            var res = new GenericResponse();
            try
            {
                var receipt = context.FeeReceipts.FirstOrDefault(r => r.ReceiptId == receiptId && !r.IsDeleted);
                if (receipt == null)
                {
                    res.statusCode = 404;
                    res.Message = "Receipt not found";
                    return res;
                }

                receipt.IsDeleted = true;
                receipt.ModifiedBy = userId;
                receipt.ModifiedOn = DateTime.UtcNow;

                context.SaveChanges();

                res.statusCode = 200;
                res.Message = "Receipt deleted successfully";
            }
            catch (Exception ex)
            {
                res.statusCode = 500;
                res.Message = ex.Message;
            }

            return res;
        }

        public IEnumerable<FeeReceiptDto> GetAllReceipts()
        {
            return context.FeeReceipts
                .Where(r => !r.IsDeleted)
                .Select(r => new FeeReceiptDto
                {
                    ReceiptId = r.ReceiptId,
                    FeeId = r.FeeId,
                    ReceiptNumber = r.ReceiptNumber,
                    IssuedDate = r.IssuedDate,
                    IssuedBy = r.IssuedBy,
                    Remarks = r.Remarks,
                    CreatedBy = r.CreatedBy
                }).ToList();
        }

        public FeeReceiptDto GetReceiptById(int receiptId)
        {
            return context.FeeReceipts
                .Where(r => r.ReceiptId == receiptId && !r.IsDeleted)
                .Select(r => new FeeReceiptDto
                {
                    ReceiptId = r.ReceiptId,
                    FeeId = r.FeeId,
                    ReceiptNumber = r.ReceiptNumber,
                    IssuedDate = r.IssuedDate,
                    IssuedBy = r.IssuedBy,
                    Remarks = r.Remarks,
                    CreatedBy = r.CreatedBy
                }).FirstOrDefault();
        }
    }

}
