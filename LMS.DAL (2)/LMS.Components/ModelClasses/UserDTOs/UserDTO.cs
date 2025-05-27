using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.ModelClasses.UserDTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? PWord { get; set; }

        public string? EmailId { get; set; }

        public string? MobileNumber { get; set; }

        public string? ProfileUrl { get; set; }
        public IFormFile? ProfileUrlUpload { get; set; }

        //public string? UserCode { get; set; }

        public int? UserType { get; set; }

    }
  public class UserEDTO
    {
        public int UserId { get; set; }

        public string? UserName { get; set; }
         
        public string? EmailId { get; set; }

        public string? MobileNumber { get; set; }

         
        public int? UserType { get; set; }

    }


    public class UserListDTO
    {
        public int UserId { get; set; }

        public string? UserName { get; set; }

        public bool? IsActive { get; set; }
        public string? EmailId { get; set; }

        public string? ProfileUrl { get; set; }
        public string? MobileNumber { get; set; }
        public int? UserType { get; set; }



    }




    public class UsertDTO
    {
     
        public string? UserTypeName { get; set; } 
        public int? UserType { get; set; }
    }
}
