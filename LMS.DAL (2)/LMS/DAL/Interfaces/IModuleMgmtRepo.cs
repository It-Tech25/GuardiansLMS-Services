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
    public interface IModuleMgmtRepo
    {
        #region Module
        List<ModuleDTO> GetModulesList(string search = "");
        List<ModuleDD> GetModulesDD();
        ModuleDTO GetModuleById(int id);
        ModuleMasterEntity GetModuleDataById(int id);
        ModuleMasterEntity GetModuleDataByName(string name);
        GenericResponse AddModule(ModuleMasterEntity req);
        GenericResponse UpdateModule(ModuleMasterEntity req);
        #endregion

        #region Sub Module
        List<SubModuleListDTO> GetSubModulesList(int moduleId, string search = "");
        List<SubModuleDD> GetSubModulesDD(int moduleId);
        SubModuleDTO GetSubModuleById(int id);
        ModuleMasterEntity GetSubModuleDataByName(string name, int moduleId);
        #endregion

        #region RoleMappping
        ModuleRoleMapping GetRoleMapppingDataById(int id);
        ModuleRoleMapping GetRoleMapppingByRoleAndModuleId(int usertypeId, int moduleId);
        GenericResponse AddRoleMappping(ModuleRoleMapping req);
        GenericResponse UpdateRoleMappping(ModuleRoleMapping req);
        #endregion
    }
}
