using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using LMS.Components.ModelClasses.Instructor;
using LMS.Components.Entities;
using LMS.DAL.Interfaces;
using LMS.DAL.Repositories;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : Controller
    {
        private readonly IInstructorRepo _repo;

        public InstructorController(IInstructorRepo repo)
        {
            _repo = repo;
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

        [HttpGet("All")]
        public IActionResult GetAll()
        {
            var list = _repo.GetAllInstructors();
            return Ok(list);
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
