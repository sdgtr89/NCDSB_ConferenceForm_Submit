using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.Models
{
    /// <summary>
    /// Model that will hold the different types of expenses (Meals, Lodging etc.)
    /// </summary>
    public class ExpenseType : Auditable
    {
        public int ID { get; set; }

        public ExpenseType()
        {
            this.Expenses = new HashSet<Expense>();
            this.IsRemoved = false;
        }

        [Required(ErrorMessage = "Type of Expense is required.")]
        [Display(Name = "Expense Type")]
        [Index("IX_Unique_TypeOfExpense", IsUnique = true)]
        [StringLength(35)]
        public string TypeOfExpense { get; set; }

        /// <summary>
        /// Supports a 1:M between expense and expense type
        /// </summary>
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}