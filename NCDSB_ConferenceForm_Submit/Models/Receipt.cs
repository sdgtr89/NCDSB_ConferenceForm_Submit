using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.Models
{
    /// <summary>
    /// Model that will hold the data for a single receipt file that is uploaded
    /// </summary>
    public class Receipt
    {
        [Key, ForeignKey("Expense")]
        public int ExpenseReceiptID { get; set; }

        [ScaffoldColumn(false)]
        public byte[] Content { get; set; }

        [StringLength(256)]
        [ScaffoldColumn(false)]
        public string MimeType { get; set; }

        public virtual Expense Expense { get; set; }
    }
}