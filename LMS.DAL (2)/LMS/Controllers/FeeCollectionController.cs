using LMS.Components.ModelClasses.FeeCollection;
using LMS.DAL.Interfaces;
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
        public IActionResult AddFee(FeeCollectionDto dto)
        {
            var response = repo.AddFee(dto);
            return Ok(response);
        }

        [HttpPut("Update")]
        public IActionResult UpdateFee(FeeCollectionDto dto)
        {
            int userId = int.Parse(User.FindFirstValue("UserID"));
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
        public IActionResult GetAllFees()
        {
            var response = repo.GetAllFees();
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
