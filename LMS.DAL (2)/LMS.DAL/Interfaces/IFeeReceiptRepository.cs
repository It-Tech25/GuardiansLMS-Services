using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.FeeCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Interfaces
{
    public interface IFeeReceiptRepository
    {
        GenericResponse AddReceipt(FeeReceiptDto dto);
        GenericResponse UpdateReceipt(FeeReceiptDto dto, int userId);
        GenericResponse DeleteReceipt(int receiptId, int userId);
        IEnumerable<FeeReceiptDto> GetAllReceipts();
        FeeReceiptDto GetReceiptById(int receiptId);
    }

}
