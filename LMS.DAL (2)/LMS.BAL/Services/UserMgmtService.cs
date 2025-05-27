using LMS.BAL.Interfaces;
using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.Login;
using LMS.Components.ModelClasses.UserDTOs;
using LMS.Components.Utilities;
using LMS.DAL.Interfaces;

namespace LMS.BAL.Services
{
    public class UserMgmtService : IUserMgmtService
	{
		private readonly IUserMgmtRepo uRepo;
		public UserMgmtService(IUserMgmtRepo userMgmtRepo)
		{
			this.uRepo = userMgmtRepo;
		}

		public async Task<LoginResponse> getUserLoginCheck(string? email, string? pwd)
		{
			return await uRepo.getUserLoginCheck(email, pwd);
		}

		public async Task<LoginResponse> getUserTokenbyClaim(string? uname)
		{
			return await uRepo.getUserTokenbyClaim(uname);
		}
		
		public async Task<LoginResponse> getUserTokenbyId(int id)
		{
			return await uRepo.getUserTokenbyId(id);
		}

		public async Task<(int, string)> updateRefreshToken(string refreshToken, long userId, DateTime refTokenexpDate)
		{
			return await uRepo.updateRefreshToken(refreshToken, userId, refTokenexpDate);
		}


       

        #region User Mgmt

        public GenericResponse AddUser(UserDTO req, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            UserEntity m = new UserEntity();

            m.UserName = req.UserName;
            m.UserType = req.UserType;
            m.EmailId = req.EmailId;
            m.PWord = EncryptTool.Encrypt(req.PWord);
            m.ProfileUrl = req.ProfileUrl;
            m.MobileNumber = req.MobileNumber;
            m.CreatedBy = currentUserId;
            m.CreatedOn = DateTime.Now;
            m.IsActive = true;
            m.IsDeleted = false;

            res = uRepo.AddUser(m);
            return res;
        }

        public GenericResponse EditUser(UserEDTO req, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            UserEntity m = uRepo.GetUserById(req.UserId);

            if (m == null || m.UserId == 0)
            {
                res.statusCode = 0;
                res.Message = "User ID does not exist";
                return res;
            }

            m.UserName = req.UserName;
            m.UserType = req.UserType;
            m.EmailId = req.EmailId;
           // m.ProfileUrl = req.ProfileUrl;
            m.MobileNumber = req.MobileNumber;

            //string oldPassword = EncryptTool.Decrypt(m.PWord);
            //if (!string.IsNullOrEmpty(req.PWord) && req.PWord != oldPassword)
            //{
            //    m.PWord = EncryptTool.Encrypt(req.PWord);
            //}

            res = uRepo.UpdateUser(m);
            return res;
        }



        public List<UserListDTO> GetUsersList(string search = "")
        {
            return uRepo.GetUsersList(search);
        }

        public List<UsertDTO> GetUserTypDD()
        {
            return uRepo.GetUserTypDD();
        }

        public List<UsertDTO> GetUserTypeDD()
        {
            return uRepo.GetUserTypeDD();
        } 
        
        public UserEntity GetUserById(int id)
        {
            return uRepo.GetUserById(id);
        }


        public GenericResponse DeleteUser(int id, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            UserEntity m = uRepo.GetUserById(id);
            if (m.UserId != 0)
            {
                m.IsDeleted = true;
                //m.ModifiedBy = currentUserId;
                //m.ModifiedOn = DateTime.Now;
                res = uRepo.UpdateUser(m);
            }
            else
            {
                res.statusCode = 0;
                res.Message = "Id does not exists";
            }
            return res;
        }
         
        public GenericResponse ActivateUser(int id, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            UserEntity m = uRepo.GetUserById(id);
            if (m.UserId != 0)
            {
                if (m.IsActive == true)
                {
                    res.statusCode = 0;
                    res.Message = "The user is already Active";
                }
                else
                {
                    m.IsActive = true;
                    m.ActivatedOn = DateTime.Now;
                    m.ActivatedBy = currentUserId;
                    res = uRepo.UpdateUser(m);
                }
            }
            else
            {
                res.statusCode = 0;
                res.Message = "Id does not exists";
            }
            return res;
        }

        public GenericResponse DeActivateUser(int id, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            UserEntity m = uRepo.GetUserById(id);
            if (m.UserId != 0)
            {
                if (m.IsActive == false)
                {
                    res.statusCode = 0;
                    res.Message = "The user is already De activated";
                }
                else
                {
                    m.IsActive = false;

                    res = uRepo.UpdateUser(m);
                }
            }
            else
            {
                res.statusCode = 0;
                res.Message = "Id does not exists";
            }
            return res;
        }



        #endregion






    }
}
