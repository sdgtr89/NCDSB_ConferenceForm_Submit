using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.Models
{
    /// <summary>
    /// Model that will hold information about all upcoming conferences.
    /// </summary>
    public class Conference : Auditable
    {
        public int ID { get; set; }

        public Conference()
        {
            this.IsRemoved = false;
            this.ConferenceForms = new HashSet<ConferenceForm>();
        }        

        [Required(ErrorMessage = "Conference Name is required.")]
        [Display(Name = "Conference")]
        [Index("IX_Unique_Name_Date", 1, IsUnique = true)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Conference Location is required.")]
        [Display(Name = "Location")]
        public int AddressID { get; set; }

        public virtual Address Address { get; set; }

        [Required(ErrorMessage = "Conference Description is required.")]
        [StringLength(255)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Conference Start Date is required.")]
        [Index("IX_Unique_Name_Date", 2, IsUnique = true)]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Conference End Date is required.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Conference Cost is required.")]
        public decimal Cost { get; set; }

        /// <summary>
        /// Supports a 1:M between conference and conference forms
        /// </summary>
        public virtual ICollection<ConferenceForm> ConferenceForms { get; set; }

        /// <summary>
        /// Returns the name and address of the conference concatenated
        /// </summary>
        [Display(Name = "Conference")]
        public string ConferenceSummary => Name + " at " + Address.SiteName;

        /// <summary>
        /// Returns the conference start and end date unless it is a one day conference
        /// </summary>
        public string ConferenceDate
        {
            get
            {                
                if (StartDate != EndDate)
                {
                    return string.Format("{0:ddd, MMM d, yyyy}", StartDate) + " to "
                        + string.Format("{0:ddd, MMM d, yyyy}", EndDate);
                }
                else
                {
                    return string.Format("{0:ddd, MMM d, yyyy}", StartDate);
                }
            }
        }        
        
        /// <summary>
        /// Trims the description to give the user a snapshot
        /// </summary>
        [Display(Name = "Description")]
        public string Summary => string.Join(" ", Description.Split(' ').Take(6)) + "...";

    }
}