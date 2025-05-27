using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.CourseType;
using LMS.Components.ModelClasses.Login;
using LMS.Components.ModelClasses.MasterDTOs;
using LMS.Components.ModelClasses.UserDTOs;
using LMS.Components.Utilities;
using LMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.DAL.Repositories
{
	public class UserMgmtRepo : IUserMgmtRepo
	{
		private readonly MyDbContext context;
		public UserMgmtRepo(MyDbContext _context)
		{
			context = _context;
		}
        #region User LoginCheck

        public async Task<LoginResponse> getUserLoginCheck(string? emailId, string? pwd)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                var u = await context.userEntities.Where(a => a.MobileNumber == emailId || a.EmailId.ToLower().Trim() == emailId.ToLower().Trim() && a.IsDeleted == false).FirstOrDefaultAsync();
                if (u == null)
                {
                    response.statusCode = 0;
                    response.statusMessage = "Email/Mobile not registered / Inactive";
                }
                else if (u.IsDeleted == true)
                {
                    response.statusCode = 0;
                    response.statusMessage = "Email/Mobile is Inactive";
                }
                else if (u.IsActive == false)
                {
                    response.statusCode = 0;
                    response.statusMessage = "User is deactivated,reach out to Admin..!";
                }
                else
                {
                    string mPw = EncryptTool.Encrypt(pwd);
                    string oPw = EncryptTool.Decrypt(u.PWord);
                    string uPw = u.PWord;
                    if (!mPw.Equals(uPw))
                    {
                        response.statusCode = 0;
                        response.statusMessage = "Password Mismatch";
                    }
                    else
                    {
                        response.EmailId = u.EmailId;
                        response.UserId = u.UserId;
                        response.ImageUrl = u.ProfileUrl;
                        response.MobileNumber = u.MobileNumber;
                        response.UserName = u.UserName;
                        response.userTypeName = await context.userTypes.Where(a => a.UserTypeId == u.UserType).Select(b => b.UserTypeName).FirstOrDefaultAsync();
                        response.UserType = u.UserType;
                        response.statusCode = 1;
                        response.statusMessage = "login Sucess";
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Source = "getUserLoginCheck";
            }
            return response;
        }

        public UserEntity GetUserById(int id)
        {
            UserEntity u = new UserEntity();
            try
            {
                u = context.userEntities.Where(u => u.UserId == id && u.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex) { }
            return u;
        }

        public async Task<LoginResponse> getUserTokenbyClaim(string? uname)
        {
            try
            {
                LoginResponse response = new LoginResponse();
                var u = await context.userEntities.Where(a => a.UserName.Trim().Equals(uname.Trim()) && a.IsDeleted == false).
                 Select(b => new LoginResponse
                 {
                     UserId = b.UserId,
                     RefreshToken = b.RefreshToken,
                     RefTokenExpDate = b.RefTokenExpDate
                 }).FirstOrDefaultAsync();
                return u;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<LoginResponse> getUserTokenbyId(int id)
        {
            try
            {
                LoginResponse response = new LoginResponse();
                var u = await context.userEntities.Where(a => a.UserId == id && a.IsDeleted == false).
                 Select(b => new LoginResponse
                 {
                     UserId = b.UserId,
                     RefreshToken = b.RefreshToken,
                     RefTokenExpDate = b.RefTokenExpDate
                 }).FirstOrDefaultAsync();
                return u;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<(int, string)> updateRefreshToken(string refreshToken, long userId, DateTime refTokenexpDate)
        {
            try
            {
                var u = await context.userEntities.Where(a => a.UserId == userId && a.IsDeleted == false).FirstOrDefaultAsync();
                if (u == null) { return (0, "no user found"); }
                else
                {
                    u.RefreshToken = refreshToken;
                    u.RefTokenExpDate = refTokenexpDate;
                }
                int returnValue = context.SaveChanges();
                if (returnValue > 0)
                {
                    return (1, "refresh token updated sucessfully");
                }
                else
                {
                    return (0, "failed to update refresh token");
                }
            }
            catch (Exception ex)
            {
                return (0, ex.InnerException.Message);
            }
        }



        #endregion

      
        #region User Mgmt

        public List<UserListDTO> GetUsersList(string search = "")
        {
            List<UserListDTO> response = new List<UserListDTO>();
            try
            {
                response = (from m in context.userEntities
                            where m.IsDeleted == false && m.UserName.Contains(search)
                            select new UserListDTO
                            {
                                UserId = m.UserId,
                                UserName = m.UserName,
                                ProfileUrl = m.ProfileUrl,
                                EmailId = m.EmailId,
                                MobileNumber = m.MobileNumber,
                                UserType = m.UserType,
                              IsActive = m.IsActive,
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }

        public List<UsertDTO> GetUserTypDD()
        {
            List<UsertDTO> response = new List<UsertDTO>();
            try
            {
                response = (from m in context.userEntities
                            join u in context.userTypes on m.UserType equals u.UserTypeId
                            where m.IsDeleted == false && m.IsActive == true
                            select new UsertDTO
                            {
                                UserType = m.UserType, 
                                UserTypeName = u.UserTypeName, 
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }
          
        
        public List<UsertDTO> GetUserTypeDD()
        {
            List<UsertDTO> response = new List<UsertDTO>();
            try
            {
                response = (from m in context.userTypes
                            where m.IsDeleted == false && m.IsActive == true
                            select new UsertDTO
                            {
                                UserType = m.UserTypeId, 
                                UserTypeName = m.UserTypeName, 
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }


        public GenericResponse AddUser(UserEntity req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                context.userEntities.Add(req);
                context.SaveChanges();
                response.statusCode = 201;
                response.Message = "Created";
                response.CurrentId = req.UserId;
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.Message = ex.Message;
            }
            return response;
        }

        public GenericResponse UpdateUser(UserEntity req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                context.userEntities.Update(req);
                context.SaveChanges();
                response.statusCode = 200;
                response.Message = "Updated";
                response.CurrentId = req.UserId;
            }
            catch (Exception ex)
            {
                response.statusCode = 404;
                response.Message = ex.Message;
            }
            return response;
        }


        #endregion

       




    }
}
