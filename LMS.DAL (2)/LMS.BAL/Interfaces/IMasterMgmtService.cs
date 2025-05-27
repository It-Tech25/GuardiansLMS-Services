using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.MasterDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BAL.Interfaces
{
    public interface IMasterMgmtService
    {
        #region User Type

        List<UserTypeDTO> GetUserTypeList(string search = "");
        List<UserTypeDD> GetUserTypesDD();
        UserTypeDTO GetUserTypeById(int id);
        GenericResponse AddEditUserType(UserTypeDTO req, int currentUserId);
        GenericResponse ActivateUserType(int id, int currentUserId);
        GenericResponse DeActivateUserType(int id, int currentUserId);
        GenericResponse DeleteUserType(int id, int currentUserId);

        #endregion
    }
}
