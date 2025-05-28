using LMS.BAL.Interfaces;
using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.MasterDTOs;
using LMS.DAL.Interfaces;

namespace LMS.BAL.Services
{
    public class MasterMgmtService : IMasterMgmtService
    {
        private readonly IMasterMgmtRepo mRepo;

        public MasterMgmtService(IMasterMgmtRepo mRepo)
        {
            this.mRepo = mRepo;
        }
        #region User Type

        public List<UserTypeDTO> GetUserTypeList(string search = "")
        {
            return mRepo.GetUserTypeList(search);
        }
        public List<UserTypeDD> GetUserTypesDD()
        {
            return mRepo.GetUserTypesDD();
        }
        public UserTypeDTO GetUserTypeById(int id)
        {
            return mRepo.GetUserTypeById(id);
        }
        public GenericResponse AddEditUserType(UserTypeDTO req, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            UserTypeMasterEntity m = new UserTypeMasterEntity();

            if (req.UserTypeId == 0)
            {
                m.UserTypeName = req.UserTypeName;
                m.ParentId = null;
                m.Description = req.Description;
                m.CreatedBy = currentUserId;
                m.CreatedOn = DateTime.Now;
                m.IsActive = true;
                m.IsDeleted = false;
                res = mRepo.AddUserType(m);
            }
            else
            {
                m = mRepo.GetUserTypeDataById(req.UserTypeId);
                if (m.UserTypeId == 0)
                {
                    res.statusCode = 0;
                    res.Message = "Id does not exists";
                }
                else
                {
                    m.UserTypeName = req.UserTypeName;
                    m.Description = req.Description;
                    m.ModifiedBy = currentUserId;
                    m.ModifiedOn = DateTime.Now;
                    res = mRepo.UpdateUserType(m);
                }
            }
            return res;
        }
        public GenericResponse ActivateUserType(int id, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            UserTypeMasterEntity m = mRepo.GetUserTypeDataById(id);
            if (m.UserTypeId != 0)
            {
                if (m.IsActive == true)
                {
                    res.statusCode = 0;
                    res.Message = "The Role is already Active";
                }
                else
                {
                    m.IsActive = true;
                    res = mRepo.UpdateUserType(m);
                }
            }
            else
            {
                res.statusCode = 0;
                res.Message = "Id does not exists";
            }
            return res;
        }
        public GenericResponse DeActivateUserType(int id, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            UserTypeMasterEntity m = mRepo.GetUserTypeDataById(id);
            if (m.UserTypeId != 0)
            {
                if (m.IsActive == false)
                {
                    res.statusCode = 0;
                    res.Message = "The Role is already De activated";
                }
                else
                {
                    m.IsActive = false;
                    m.DeactivatedBy = currentUserId;
                    m.DeactivatedOn = DateTime.Now;
                    res = mRepo.UpdateUserType(m);
                }
            }
            else
            {
                res.statusCode = 0;
                res.Message = "Id does not exists";
            }
            return res;
        }
        public GenericResponse DeleteUserType(int id, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            UserTypeMasterEntity m = mRepo.GetUserTypeDataById(id);
            if (m.UserTypeId != 0)
            {
                m.IsDeleted = true;
                m.ModifiedBy = currentUserId;
                m.ModifiedOn = DateTime.Now;
                res = mRepo.UpdateUserType(m);
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
