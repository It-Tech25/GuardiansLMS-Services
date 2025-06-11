using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.ModelClasses.FeeCollection
{
    public class FeeCollectionDto
    {
        public int? FeeId { get; set; } // For update
        public int StudentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMode { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }

        public IFormFile? Receipt { get; set; }
        public String? ReceiptUrl { get; set; }
    }
    public class FeeCollectionsDto
    {
        public int? FeeId { get; set; } // For update
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMode { get; set; }
        public string Remarks { get; set; }
        public string StudentName { get; set; }
        public String? ReceiptUrl { get; set; }
        public int uid { get; set; }
    }

}
