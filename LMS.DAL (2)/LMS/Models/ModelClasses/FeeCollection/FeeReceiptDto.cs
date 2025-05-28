using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.ModelClasses.FeeCollection
{
    public class FeeReceiptDto
    {
        public int? ReceiptId { get; set; } // for update
        public int FeeId { get; set; }
        public string ReceiptNumber { get; set; }
        public DateTime IssuedDate { get; set; }
        public int IssuedBy { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
    }

}
