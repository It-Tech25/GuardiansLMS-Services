using LMS.Components.ModelClasses.CourseBatch;
using LMS.DAL.Interfaces;
using LMS.DAL.Repositories;
using LMS.Models.ModelClasses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseBatchController : Controller
    {
        private readonly ICourseBatchRepo _repo;

        public CourseBatchController(ICourseBatchRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("Add")]
        public IActionResult AddBatch([FromBody] CourseBatchDto dto)
        {
            var res = _repo.AddBatch(dto);
            return Ok(res);
        }

        [HttpPut("Update")]
        public IActionResult UpdateBatch([FromBody] CourseBatchDto dto)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));
            var res = _repo.UpdateBatch(dto, userId);
            return Ok(res);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteBatch(int id)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));
            var res = _repo.DeleteBatch(id, userId);
            return Ok(res);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll(string searchterm = "", int pagenumber = 1, int pagesize = 10)
        {
            var all = _repo.GetAllBatches(); // returns all batches
                                             // Apply filtering & pagination here in controller:
            var filtered = all.AsQueryable();

            if (!string.IsNullOrWhiteSpace( searchterm))
            {
                var term =  searchterm.Trim().ToLower();
                filtered = filtered.Where(x => x.Course.ToLower().Contains(term));
            }

            int skip = (pagenumber - 1) * pagesize;
            filtered = filtered.Skip(skip).Take(pagesize);

            return Ok(filtered);
        }

    }

}
