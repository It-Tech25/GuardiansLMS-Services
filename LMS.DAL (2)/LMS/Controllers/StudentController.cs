using LMS.Components.ModelClasses.Student;
using LMS.DAL.Interfaces;
using LMS.Models.ModelClasses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository repo;

        public StudentController(IStudentRepository _repo)
        {
            repo = _repo;
        }

        [HttpPost("Add")]
        public IActionResult AddStudent(StudentDto dto)
        {
            var response = repo.AddStudent(dto);
            return Ok(response);
        }

        [HttpPut("Update")]
        public IActionResult UpdateStudent(StudentDto dto)
        {
            int userId = int.Parse(User.FindFirstValue("UserID"));
            var response = repo.UpdateStudent(dto, userId);
            return Ok(response);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            int userId = int.Parse(User.FindFirstValue("UserID"));
            var response = repo.DeleteStudent(id, userId);
            return Ok(response);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllStudents(string searchterm = "", int pagenumber = 0, int pagesize = 0)
        {
            var students = repo.GetAllStudents(); 

            var filtered = students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                var term = searchterm.Trim().ToLower();
                filtered = filtered.Where(x =>
              (!string.IsNullOrEmpty(x.StudentName) && x.StudentName.ToLower().Contains(term)) ||
            (!string.IsNullOrEmpty(x.EmailId) && x.EmailId.ToLower().Contains(term)) ||
            (!string.IsNullOrEmpty(x.MobileNumber) && x.MobileNumber.ToLower().Contains(term))
                );
            }

            // Apply pagination
            int skip = (pagenumber - 1) * pagesize;
            var pagedResult = filtered.Skip(skip).Take(pagesize).ToList();

            // Return paged data with metadata
            var response = new
            {
                succeeded = true,
                totalRecords = filtered.Count(),
                data = pagedResult
            };

            return Ok(response);
        }


        [HttpGet("GetById/{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = repo.GetStudentById(id);
            return Ok(student);
        }
    }

}
