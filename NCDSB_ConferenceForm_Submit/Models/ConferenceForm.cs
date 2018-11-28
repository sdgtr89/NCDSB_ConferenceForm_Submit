using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.Models
{
    /// <summary>
    /// Model that will hold the data for both an estimate and the actual cost 
    /// of attending the conference
    /// </summary>
    public class ConferenceForm : Auditable
    {
        public int ID { get; set; }

        /// <summary>
        /// Initializes a collection of expenses, mileage forms,
        /// sets the status to pending, and for now to a staff use - Must change this number
        /// </summary>
        public ConferenceForm()
        {
            this.Expenses = new HashSet<Expense>();            
            this.Mileage = new HashSet<Trip>();
            //sets the date of the conference form to the day they are 
            //filling it out
            this.Date = DateTime.Today;
            this.IsRemoved = false;
        }        

        /// <summary>
        /// Returns a summary of the Benefit of attending field
        /// </summary>
        [Display(Name = "Benefit")]
        public string BenefitSummary => string.Join(" ", BenefitOfAttending.Split(' ').Take(10)) + "...";

        /// <summary>
        /// Returns the date of the conference that the form references
        /// </summary>
        [Display(Name = "Conference Date")]
        public string ConferenceDate
            => Conference.StartDate.ToShortDateString() + " to " + Conference.EndDate.ToShortDateString();

        public string StaffSummary => Conference.ConferenceSummary + " for" + StaffMember.FullName; 

        [Required(ErrorMessage = "You must supply a conference date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "You must select a Conference to attend.")]
        [Display(Name = "Conference")]
        public int ConferenceID { get; set; }

        public virtual Conference Conference { get; set; }
        
        [Required(ErrorMessage = "Benefit of Attending is required.")]
        [Display(Name = "Benefit of Attending")]
        public string BenefitOfAttending { get; set; }

        [Required(ErrorMessage = "You must specify whether Replacement Staff is required.")]
        [Display(Name = "Replacement Staff Required?")]
        public bool ReqReplacementStaff { get; set; }

        /// <summary>
        /// Used by Admin to give a quick reason for the status change
        /// </summary>
        public string ReasonForStatusChange { get; set; }

        /// <summary>
        /// Used to add mileage to a conference form via 2 trips
        /// </summary>
        public virtual ICollection<Trip> Mileage { get; set; }       

        [Display(Name = "Form Status")]
        public int FormStatusID { get; set; }

        /// <summary>
        /// Allows for changing of the form status by an admin
        /// </summary>
        public virtual FormStatus FormStatus { get; set; }

        [Display(Name = "Staff Member")]
        public int StaffMemberID { get; set; }

        /// <summary>
        /// Connects the form to a staff member
        /// </summary>
        public virtual StaffMember StaffMember { get; set; }
        
        /// <summary>
        /// Supports a 1:M between expenses and conference forms
        /// </summary>
        public virtual ICollection<Expense> Expenses { get; set; }

        public string TempName { get; set; }

    }
}