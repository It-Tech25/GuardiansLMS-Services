using LMS.Components.Entities;
using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.Instructor;
using LMS.DAL.Interfaces;
using LMS.DAL.Repositories;
using LMS.Models.ModelClasses;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : Controller
    {
        private readonly IInstructorRepo _repo;
        private readonly MyDbContext context;

        public InstructorController(IInstructorRepo repo, MyDbContext _context)
        {
            _repo = repo;
            context = _context;
        }

        [HttpPost("Add")]
        public IActionResult AddInstructor([FromBody] InstructorDto dto)
        {
            var res = _repo.AddInstructor(dto);
            return Ok(res);
        }

        [HttpPut("Edit")]
        public IActionResult EditInstructor([FromBody] InstructorDto dto)
        {
            var res = _repo.UpdateInstructor(dto);
            return Ok(res);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteInstructor(int id, [FromQuery] int modifiedBy)
        {
            var res = _repo.DeleteInstructor(id, modifiedBy);
            return Ok(res);
        }

        [HttpGet("GetAllInstructors")]
        public IActionResult GetAll(string searchterm = "", int pagenumber = 0, int pagesize = 0)
        {
            var allInstructors = _repo.GetAllInstructors(); 
            var users = context.userEntities.ToDictionary(u => u.UserId, u => u.UserName);
            var courses = context.courseTypes.ToDictionary(c => c.CourseId, c => c.CourseName); 

            var enriched = allInstructors.Select(x => new
            {
                x.InstructorId,
                x.UserId,
                x.CourseId,
                UserName = users.ContainsKey(x.UserId) ? users[x.UserId] : "",
                CourseName = courses.ContainsKey(x.CourseId) ? courses[x.CourseId] : ""
            }).AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                var term = searchterm.Trim().ToLower();
                enriched = enriched.Where(x =>
                    (!string.IsNullOrEmpty(x.UserName) && x.UserName.ToLower().Contains(term)) ||
                    (!string.IsNullOrEmpty(x.CourseName) && x.CourseName.ToLower().Contains(term))
                );
            }

            int skip = (pagenumber - 1) * pagesize;
            var pagedResult = enriched.Skip(skip).Take(pagesize).ToList();

            var response = new
            {
                succeeded = true,
                totalRecords = enriched.Count(),
                data = pagedResult
            };

            return Ok(response);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _repo.GetInstructorById(id);
            if (data == null) return NotFound("Instructor not found");
            return Ok(data);
        }


        [HttpGet("GetInstructorProfile/{instructorId}")]
        public IActionResult GetInstructorProfile(int instructorId)
        {
            var result = _repo.GetInstructorProfile(instructorId);
            if (result == null)
                return NotFound(new { Message = "Instructor not found" });

            return Ok(result);
        }


    }

}
