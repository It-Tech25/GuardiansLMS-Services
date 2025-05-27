using LMS.BAL.Interfaces;
using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.MasterDTOs;
using LMS.Components.ModelClasses.UserDTOs;
using LMS.Components.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace LMS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]    public class UserMgmtController : Controller
    {
        private readonly IUserMgmtService uService; 
        private readonly IConfiguration config;
        
        public UserMgmtController(IUserMgmtService userMgmtService, IConfiguration configuration )
        {
            uService = userMgmtService; 
            config = configuration;
        }


        [HttpGet("GetUsertypeDropDown")]
        public async Task<IActionResult> GetUsertypeDropDown()
        {
            List<UsertDTO> res = new List<UsertDTO>();
            res = uService.GetUserTypeDD();

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = res.Count();

            return Ok(finalResponse);
        }

        [HttpGet("GetUsersList")]
        public async Task<IActionResult> GetUsersList(string search = "")
        {
            List<UserListDTO> res = new List<UserListDTO>();
            res = uService.GetUsersList(search);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = res.Count();

            return Ok(finalResponse);
        }
        [HttpGet("GetUsersListDropDown")]
        public async Task<IActionResult> GetUsersListDD()
        {
            List<UsersDTO> res = new List<UsersDTO>();
            res = uService.GetUsersListDropDown();

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = res.Count();

            return Ok(finalResponse);
        }




        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(UserDTO req)
        {
            GenericResponse res = new GenericResponse();
            var userId = int.Parse(User.FindFirstValue("userId"));
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

            // Ensure the "UploadedFiles" directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Handle Profile Image Upload
            if (req.ProfileUrlUpload != null)
            {
                string livePath = "UploadedFiles/";
                string fileNameUploaded = Path.GetFileName(req.ProfileUrlUpload.FileName);
                if (!string.IsNullOrEmpty(fileNameUploaded))
                {
                    string filename = DateTime.UtcNow.ToString();
                    filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                    filename = Regex.Replace(filename, "[A-Za-z ]", "");
                    filename = "Img_" + filename + RandomGenerator.RandomString(4, false);
                    string extension = Path.GetExtension(fileNameUploaded);
                    filename += extension;

                    var filePath = Path.Combine(uploadsFolder, filename);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await req.ProfileUrlUpload.CopyToAsync(stream);
                    }

                    req.ProfileUrl = livePath + filename;
                }
                else
                {
                    req.ProfileUrl = livePath + "dummy.png";
                }
            }
            else
            {
                req.ProfileUrl = "UploadedFiles/dummy.png";
            }

            res = uService.AddUser(req, userId);
            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPost("EditUser")]
        public async Task<IActionResult> EditUser(UserEDTO req)
        {
            GenericResponse res = new GenericResponse();
            var userId = int.Parse(User.FindFirstValue("userId"));
          //  string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

            // Ensure the "UploadedFiles" directory exists
            //if (!Directory.Exists(uploadsFolder))
            //{
            //    Directory.CreateDirectory(uploadsFolder);
            //}

            //string? previousLogo = uService.GetUserById(Convert.ToInt32(req.UserId)).ProfileUrl;
            //if (!string.IsNullOrEmpty(previousLogo))
            //{
            //    string baseUrl = config.GetValue<string>("GlobalSettings:URLData");
            //    if (previousLogo.StartsWith(baseUrl))
            //    {
            //        previousLogo = previousLogo.Substring(baseUrl.Length);
            //    }
            //}

            //bool isLogoUploaded = false;
            //string? currentLogoUrl = string.Empty;

            //if (req.ProfileUrlUpload != null)
            //{
            //    string fileNameUploaded = Path.GetFileName(req.ProfileUrlUpload.FileName);
            //    if (!string.IsNullOrEmpty(fileNameUploaded))
            //    {
            //        string filename = DateTime.UtcNow.ToString();
            //        filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
            //        filename = Regex.Replace(filename, "[A-Za-z ]", "");
            //        filename = "Img_" + filename + RandomGenerator.RandomString(4, false);
            //        string extension = Path.GetExtension(fileNameUploaded);
            //        filename += extension;

            //        var filePath = Path.Combine(uploadsFolder, filename);
            //        using (var stream = new FileStream(filePath, FileMode.Create))
            //        {
            //            await req.ProfileUrlUpload.CopyToAsync(stream);
            //        }

            //        string livePath = "UploadedFiles/";
            //        currentLogoUrl = livePath + filename;
            //        isLogoUploaded = true;
            //    }
            //}

            //req.ProfileUrl = isLogoUploaded ? currentLogoUrl : previousLogo;
            res = uService.EditUser(req, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }


        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            UserEntity res = new UserEntity();
            res = uService.GetUserById(id);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }


        [HttpPut("ActivateUser")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = uService.ActivateUser(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("DeActivateUser")]
        public async Task<IActionResult> DeActivateUser(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = uService.DeActivateUser(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = uService.DeleteUser(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }



    }
}
