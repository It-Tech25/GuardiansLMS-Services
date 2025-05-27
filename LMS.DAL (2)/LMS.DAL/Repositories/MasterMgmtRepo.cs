using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.MasterDTOs;
using LMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repositories
{
    public class MasterMgmtRepo : IMasterMgmtRepo
    {
        private readonly MyDbContext context;
        public MasterMgmtRepo(MyDbContext _context)
        {
            context = _context;
        }

        #region User Type

        public List<UserTypeDTO> GetUserTypeList(string search = "")
        {
            List<UserTypeDTO> response = new List<UserTypeDTO>();
            try
            {
                response = (from m in context.userTypes
                            where m.IsDeleted == false && m.UserTypeName.Contains(search) && m.IsUsable == true //&& m.ParentId != null
                            select new UserTypeDTO
                            {
                                UserTypeId = m.UserTypeId,
                                UserTypeName = m.UserTypeName,
                                Description = m.Description
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }

        public List<UserTypeDD> GetUserTypesDD()
        {
            List<UserTypeDD> response = new List<UserTypeDD>();
            try
            {
                response = (from m in context.userTypes
                            where m.IsDeleted == false && m.IsUsable == true && m.IsActive == true
                            select new UserTypeDD
                            {
                                UserTypeId = m.UserTypeId,
                                UserTypeName = m.UserTypeName
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }

        public UserTypeDTO GetUserTypeById(int id)
        {
            UserTypeDTO response = new UserTypeDTO();
            try
            {
                response = (from m in context.userTypes
                            where m.IsDeleted == false && m.IsUsable == true && m.ParentId == null
                            select new UserTypeDTO
                            {
                                UserTypeId = m.UserTypeId,
                                UserTypeName = m.UserTypeName,
                                Description = m.Description
                            }).FirstOrDefault();
            }
            catch (Exception ex) { }
            return response;
        }

        public UserTypeMasterEntity GetUserTypeDataById(int id)
        {
            UserTypeMasterEntity response = new UserTypeMasterEntity();
            try
            {
                response = context.userTypes.Where(m => m.UserTypeId == id && m.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex) { }
            return response;
        }

        public GenericResponse AddUserType(UserTypeMasterEntity req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                context.userTypes.Add(req);
                context.SaveChanges();
                response.statusCode = 201;
                response.Message = "Created";
                response.CurrentId = req.UserTypeId;
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.Message = ex.Message;
            }
            return response;
        }

        public GenericResponse UpdateUserType(UserTypeMasterEntity req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                context.userTypes.Update(req);
                context.SaveChanges();
                response.statusCode = 200;
                response.Message = "Updated";
                response.CurrentId = req.UserTypeId;
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
