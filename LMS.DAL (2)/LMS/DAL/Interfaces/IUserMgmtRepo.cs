using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.Login;
using LMS.Components.ModelClasses.UserDTOs;

namespace LMS.DAL.Interfaces
{
	public interface IUserMgmtRepo
	{
		Task<LoginResponse> getUserLoginCheck(string? email, string? pwd);
		UserEntity GetUserById(int id);

        Task<LoginResponse> getUserTokenbyClaim(string? uname);
		Task<LoginResponse> getUserTokenbyId(int id);
		Task<(int, string)> updateRefreshToken(string refreshToken, long userId, DateTime refTokenexpDate);


        #region User Mgmt
        GenericResponse UpdateUser(UserEntity req);
        List<UsersDTO> GetUsersListDropDown();
        GenericResponse AddUser(UserEntity req);
        
        List<UsertDTO> GetUserTypDD();
        List<UserListDTO> GetUsersList(string search = "");
        List<UsertDTO> GetUserTypeDD();



        #endregion

    }
}
