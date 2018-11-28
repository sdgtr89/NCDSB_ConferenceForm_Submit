using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.Models
{
    /// <summary>
    /// Model that will hold all the possible statuses (Approved, Denied etc.)
    /// </summary>
    public class FormStatus : Auditable
    {

        public int ID { get; set; }

        public FormStatus()
        {
            this.ConferenceForms = new HashSet<ConferenceForm>();
            this.MileageForms = new HashSet<MileageForm>();
            this.IsRemoved = false;
        }

        [Required(ErrorMessage = "Form Status Type is required.")]
        [Display(Name = "Status")]
        [Index("IX_Unique_StatusType", IsUnique = true)]
        [StringLength(25)]
        public string StatusType { get; set; }

        /// <summary>
        /// Supports a 1:M between conference form and form status
        /// </summary>
        public virtual ICollection<ConferenceForm> ConferenceForms { get; set; }

        /// <summary>
        /// Supports a 1:M between mileage form and form status
        /// </summary>
        public virtual ICollection<MileageForm> MileageForms { get; set; }
    }
}