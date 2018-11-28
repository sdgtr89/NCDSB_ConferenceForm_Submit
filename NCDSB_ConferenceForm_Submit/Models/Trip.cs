using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.Models
{
    /// <summary>
    /// Model to store trips which will be used to support batches of mileage
    /// for the mileage form model
    /// </summary>
    public class Trip : Auditable, IValidatableObject
    {
        public int ID { get; set; }

        public Trip()
        {
            this.IsRemoved = false;
        }

        [Display(Name = "Start")]
        [Required(ErrorMessage = "Start address is required.")]
        public int StartAddressID { get; set; }
        
        public virtual Address StartAddress { get; set; }

        [Display(Name = "End")]
        [Required(ErrorMessage = "End address is required.")]
        public int EndAddressID { get; set; }

        public virtual Address EndAddress { get; set; }

        [Required(ErrorMessage = "Distance is required.")]
        public decimal Distance { get; set; }

        [Required(ErrorMessage = "Trip date is required.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        public int? MileageFormID { get; set; }

        public virtual MileageForm MileageForm { get; set; }

        public int? ConferenceFormID { get; set; }

        /// <summary>
        /// Used so the user can select no more than 2 trips. One there, one back
        /// </summary>
        public ConferenceForm ConferenceForm { get; set; }

        [Display(Name = "Trip")]
        public string TripSummary => StartAddress + " - " + EndAddress;

        [Display(Name = "Date")]
        public string ShortDate => Date.ToShortDateString();

        /// <summary>
        /// Checks that the same address is not listed as the start and end address
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartAddressID == EndAddressID)
            {
                yield return new ValidationResult("Start and end address cannot be the same.", new[] { "StartAddressID" });
            };
        }
    }
}