using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.CourseBatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Interfaces
{
    public interface ICourseBatchRepo
    {
        GenericResponse AddBatch(CourseBatchDto dto);
        GenericResponse UpdateBatch(CourseBatchDto dto, int userId);
        GenericResponse DeleteBatch(int batchId, int userId);
        List<CourseBatchDto> GetAllBatches();
    }

}
