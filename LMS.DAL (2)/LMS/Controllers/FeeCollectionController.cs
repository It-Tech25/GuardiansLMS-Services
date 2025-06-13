using LMS.Components.ModelClasses.FeeCollection;
using LMS.DAL.Interfaces;
using LMS.Models.ModelClasses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeeCollectionController : Controller
    {
        private readonly IFeeCollectionRepository repo;

        public FeeCollectionController(IFeeCollectionRepository _repo)
        {
            repo = _repo;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddFee(FeeCollectionDto dto)
        {
            int userId = int.Parse(User.FindFirstValue("UserID"));

            if (dto.Receipt != null && dto.Receipt.Length > 0)
            {
                // Corrected this line (removed misuse of file.FileName)
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Receipt.FileName);

                var receiptFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "receipts");

                if (!Directory.Exists(receiptFolder))
                {
                    Directory.CreateDirectory(receiptFolder);
                }

                var filePath = Path.Combine(receiptFolder, fileName);

                // ✅ Await this async operation
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Receipt.CopyToAsync(stream);
                }

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                dto.ReceiptUrl = $"{baseUrl}/receipts/{fileName}";
            }

            var response = repo.AddFee(dto, userId);
            return Ok(response);
        }

        [HttpPut("Update")]
        public IActionResult UpdateFee(FeeCollectionDto dto)
        {
            int userId = int.Parse(User.FindFirstValue("UserID"));
            if (dto.Receipt != null && dto.Receipt.Length > 0)
            {
                var receiptFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "receipts");
                if (!Directory.Exists(receiptFolder))
                {
                    Directory.CreateDirectory(receiptFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Receipt.FileName);
                var filePath = Path.Combine(receiptFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    dto.Receipt.CopyToAsync(stream);
                }
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                dto.ReceiptUrl = $"{baseUrl}/receipts/{fileName}";
            }
            var response = repo.UpdateFee(dto, userId);
            return Ok(response);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteFee(int id)
        {
            int userId = int.Parse(User.FindFirstValue("UserID"));
            var response = repo.DeleteFee(id, userId);
            return Ok(response);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllFees(string searchterm = "", int pagenumber = 0, int pagesize = 0)
        {
            var allFees = repo.GetAllFees(); 

            var filtered = allFees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                var term = searchterm.Trim().ToLower();
                filtered = filtered.Where(x => x.StudentName.ToLower().Contains(term)); 
            }
            int skip = (pagenumber - 1) * pagesize;
            var pagedResult = filtered.Skip(skip).Take(pagesize).ToList();

            var response = new
            {
                succeeded = true,
                totalRecords = filtered.Count(),
                data = pagedResult
            };

            return Ok(response);
        }


        [HttpGet("GetById/{id}")]
        public IActionResult GetFeeById(int id)
        {
            var response = repo.GetFeeById(id);
            return Ok(response);
        }
    }

}
