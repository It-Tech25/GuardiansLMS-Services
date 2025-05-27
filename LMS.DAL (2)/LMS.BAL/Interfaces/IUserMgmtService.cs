using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.Login;
using LMS.Components.ModelClasses.UserDTOs;

namespace LMS.BAL.Interfaces
{
	public interface IUserMgmtService
	{
		Task<LoginResponse> getUserLoginCheck(string? email, string? pwd);
		Task<LoginResponse> getUserTokenbyClaim(string? uname);
		Task<LoginResponse> getUserTokenbyId(int id);
		Task<(int, string)> updateRefreshToken(string refreshToken, long userId, DateTime refTokenexpDate);



        #region User Mgmt
       // GenericResponse AddEditModule(UserDTO req, int currentUserId);
        List<UsertDTO> GetUserTypDD();
        List<UserListDTO> GetUsersList(string search = "");
        UserEntity GetUserById(int id);
        GenericResponse DeleteUser(int id, int currentUserId);

        GenericResponse DeActivateUser(int id, int currentUserId);
         GenericResponse ActivateUser(int id, int currentUserId);
        List<UsertDTO> GetUserTypeDD();
        GenericResponse AddUser(UserDTO req, int currentUserId);
        GenericResponse EditUser(UserEDTO req, int currentUserId);

        #endregion

    }
}
