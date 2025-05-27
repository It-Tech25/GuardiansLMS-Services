using LMS.Components.ModelClasses.FeeCollection;
using LMS.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeeReceiptController : Controller
    {
        private readonly IFeeReceiptRepository repo;

        public FeeReceiptController(IFeeReceiptRepository _repo)
        {
            repo = _repo;
        }

        [HttpPost("Add")]
        public IActionResult Add(FeeReceiptDto dto)
        {
            var response = repo.AddReceipt(dto);
            return Ok(response);
        }

        [HttpPut("Update")]
        public IActionResult Update(FeeReceiptDto dto)
        {
            int userId = int.Parse(User.FindFirstValue("UserID"));
            var response = repo.UpdateReceipt(dto, userId);
            return Ok(response);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            int userId = int.Parse(User.FindFirstValue("UserID"));
            var response = repo.DeleteReceipt(id, userId);
            return Ok(response);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var response = repo.GetAllReceipts();
            return Ok(response);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var response = repo.GetReceiptById(id);
            return Ok(response);
        }
    }

}
