using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.FeeCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Interfaces
{
    public interface IFeeCollectionRepository
    {
        GenericResponse AddFee(FeeCollectionDto dto);
        GenericResponse UpdateFee(FeeCollectionDto dto, int userId);
        GenericResponse DeleteFee(int feeId, int userId);
        IEnumerable<FeeCollectionDto> GetAllFees();
        FeeCollectionDto GetFeeById(int feeId);
    }

}
