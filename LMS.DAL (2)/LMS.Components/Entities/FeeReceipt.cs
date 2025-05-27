using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.Entities
{
    public class FeeReceipt
    {
        [Key]
        public int ReceiptId { get; set; }
        public int FeeId { get; set; }
        public string ReceiptNumber { get; set; }
        public DateTime IssuedDate { get; set; }
        public int IssuedBy { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
    }

}
