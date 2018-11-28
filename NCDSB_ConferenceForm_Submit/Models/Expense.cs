using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.Models
{
    /// <summary>
    /// Model that will hold the estimated cost of attending the conference in addition
    /// to the the actual costs once a estimate has been approved
    /// </summary>
    public class Expense : Auditable
    {
        public int ID { get; set; }

        public Expense()
        {
            this.IsRemoved = false;
        }

        [Display(Name = "Conference Form")]
        public int ConferenceFormID { get; set; }

        public virtual ConferenceForm ConferenceForm { get; set; }

        [Required(ErrorMessage = "Expense Type is required.")]
        [Display(Name = "Expense Type")]
        public int ExpenseTypeID { get; set; }

        public virtual ExpenseType ExpenseType { get; set; }

        [Required(ErrorMessage = "Estimate Expense Amount is required.")]
        [Display(Name = "Estimated Cost")]
        [Range(1.0, 1000.0, ErrorMessage = "Expense cannot be $0.00")]
        public decimal ExpenseEstAmount { get; set; }

        [StringLength(250, ErrorMessage = "Reason for expense must not exceed 200 characters.")]
        public string Reason { get; set; }

        [Display(Name = "Actual Expense")]
        public decimal? ExpenseActAmount { get; set; }

        [Required(ErrorMessage = "Budget code is required.")]
        public int BudgetCodeID { get; set; }

        /// <summary>
        /// Uses to get and set the budget code that is used to pay the
        /// conference form
        /// </summary>
        public virtual BudgetCode BudgetCode { get; set; }

        public virtual Receipt Receipt { get; set; }

        /// <summary>
        /// Returns a string representation of the type of expense
        /// with the estimated amount
        /// </summary>
        public string ExpenseSummary
        {
            get
            {
                return ExpenseType.TypeOfExpense + " - " + ExpenseEstAmount.ToString("c2");
            }
        }
    }
}