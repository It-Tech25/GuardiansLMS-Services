using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.MasterDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Interfaces
{
    public interface IMasterMgmtRepo
    {

        #region User Type

        List<UserTypeDTO> GetUserTypeList(string search = "");
        List<UserTypeDD> GetUserTypesDD();
        UserTypeDTO GetUserTypeById(int id);
        UserTypeMasterEntity GetUserTypeDataById(int id);
        GenericResponse AddUserType(UserTypeMasterEntity req);
        GenericResponse UpdateUserType(UserTypeMasterEntity req);

        #endregion
    }
}
