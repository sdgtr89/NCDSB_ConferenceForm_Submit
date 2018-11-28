using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.Models
{
    /// <summary>
    /// Model to hold the possible budget reimbursement codes
    /// </summary>
    public class BudgetCode : Auditable
    {
        public int ID { get; set; }

        public BudgetCode()
        {
            this.Expenses = new HashSet<Expense>();
            this.IsRemoved = false;
        }

        [Required(ErrorMessage = "Budget Code Type is required.")]
        [Display(Name = "Budget Code")]
        [StringLength(100, ErrorMessage = "Code name cannot exceed 100 characters.")]
        [Index("IX_Unique_CodeType", IsUnique = true)]
        public string CodeType { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
    }
}