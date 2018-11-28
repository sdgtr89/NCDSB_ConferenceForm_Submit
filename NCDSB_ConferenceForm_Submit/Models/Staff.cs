using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.Models
{
    /// <summary>
    /// Model that will hold all Staff Member information
    /// </summary>
    public class StaffMember
    {
        public int ID { get; set; }

        public StaffMember()
        {
            this.ConferenceForms = new HashSet<ConferenceForm>();
            this.MileageForms = new HashSet<MileageForm>();
        }

        /// <summary>
        /// Read-Only property that returns the full name (concatenated) of a staff member
        /// </summary>
        [Display(Name = "Staff Member")]
        public string FullName 
                    => FirstName + (string.IsNullOrEmpty(MiddleName) 
                    ? " " : (" " + (char?) MiddleName[0] + ". ").ToUpper())
                    + LastName;
        
        /// <summary>
        /// Read-Only property that returns the formal name (concatenated) of a staff member
        /// </summary>
        public string FormalName 
                    => LastName + ", " + FirstName 
                    + (string.IsNullOrEmpty(MiddleName) 
                    ? "" : (" " + (char?) MiddleName[0] + ".").ToUpper());

        /// <summary>
        /// Read-Only property that returns the full name and position (concatenated) of a staff member
        /// </summary>
        [Display(Name = "Staff Member")]
        public string StaffSummary => FullName + " - " + Position;        

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters in length.")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(30, ErrorMessage = "Middle name cannot exceed 30 characters in length.")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You cannot leave the last name blank.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters in length.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(50, ErrorMessage = "Position cannot exceed 50 characters in length.")]
        public string Position { get; set; }

        [Required(ErrorMessage = "You cannot leave the email blank.")]
        [DataType(DataType.EmailAddress)]
        [StringLength(256, ErrorMessage = "Email address is too long.")]
        [Index("IX_Unique_Email", IsUnique = true)]
        public string Email { get; set; }

        /// <summary>
        /// Relates each mileage form back to a user
        /// </summary>
        public virtual ICollection<MileageForm> MileageForms { get; set; }

        /// <summary>
        /// Relates each conference form back to a user
        /// </summary>
        public virtual ICollection<ConferenceForm> ConferenceForms { get; set; }
        
    }
}