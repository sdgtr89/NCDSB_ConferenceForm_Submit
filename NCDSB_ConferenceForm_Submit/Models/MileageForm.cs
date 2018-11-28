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
    /// Class serves as a Model for the data submitted in a single Mileage Form
    /// </summary>
    public class MileageForm : Auditable
    {
        
        public int ID { get; set; }

        public MileageForm()
        {
            this.Date = DateTime.Today;
            this.Trips = new HashSet<Trip>();
            this.IsRemoved = false;
        }

        [Required(ErrorMessage = "Mileage form date is required.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Used by admins to leave a quick message for the user on status change
        /// </summary>
        public string ReasonForStatusChange { get; set; }

        [Display(Name = "Status")]
        public int FormStatusID { get; set; }

        /// <summary>
        /// Relates the current mileage form to a form status
        /// </summary>
        public FormStatus FormStatus { get; set; }

        [Display(Name = "Staff Member")]
        public int StaffMemberID { get; set; }

        /// <summary>
        /// Relates the current mileage form back to a NCDSB staff member
        /// </summary>
        public virtual StaffMember StaffMember { get; set; }

        /// <summary>
        /// Parent entity with a collection of trips for batch mileage
        /// </summary>
        public virtual ICollection<Trip> Trips { get; set; }

        public int? ConferenceFormID { get; set; }

        /// <summary>
        /// Used to relate a mileage form back to a conference form
        /// </summary>
        public virtual ConferenceForm ConferenceForm { get; set; }

        /// <summary>
        /// Gives the staff member and the date to track a conference form quickly
        /// </summary>
        public string Summary => StaffMember.FullName + " - " + Date.ToShortDateString();


    }
}