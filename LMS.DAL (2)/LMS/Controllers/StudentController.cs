using LMS.Components.ModelClasses.Student;
using LMS.DAL.Interfaces;
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
        public IActionResult GetAllStudents()
        {
            var students = repo.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = repo.GetStudentById(id);
            return Ok(student);
        }
    }

}
