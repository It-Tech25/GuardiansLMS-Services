using LMS.Components.ModelClasses;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.MasterDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BAL.Interfaces
{
    public interface IModuleMgmtService
    {
        #region Module and Sub Module
        List<ModuleDTO> GetModulesList(string search = "");
        List<ModuleDD> GetModulesDD();
        ModuleDTO GetModuleById(int id);
        GenericResponse AddEditModule(ModuleAddDTO req, int currentUserId);
        GenericResponse AddEditSubModule(SubModuleDTO req, int currentUserId);
        GenericResponse ActivateModule(int id, int currentUserId);
        GenericResponse DeActivateModule(int id, int currentUserId);
        GenericResponse DeleteModule(int id, int currentUserId);
        List<SubModuleListDTO> GetSubModulesList(int moduleId, string search = "");
        List<SubModuleDD> GetSubModulesDD(int moduleId);
        SubModuleDTO GetSubModuleById(int id);
        #endregion

        #region RoleMappping
        RoleMappingDTO GetUserRoleMappingData(int userTypeId);
        GenericResponse UpdateRoleMappping(RoleMappingDTO data, int currentUserId);
        #endregion
    }
}
