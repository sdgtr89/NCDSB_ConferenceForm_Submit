using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.Models
{
    /// <summary>
    /// Model that will hold the information on all the known board sites
    /// </summary>
    public class Address : Auditable
    {
        public int ID { get; set; }

        public Address()
        {
            this.Trips = new HashSet<Trip>();
            this.Conferences = new HashSet<Conference>();
            this.IsRemoved = false;
        }

        [Required(ErrorMessage = "Site Name is required.")]
        [Display(Name = "Site Name")]
        [Index("IX_Unique_SiteName", IsUnique = true)]
        [StringLength(50)]
        public string SiteName { get; set; }

        [Required(ErrorMessage = "Site Address is required.")]
        [Display(Name = "Address")]
        [StringLength(255)]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Site City is required.")]
        [Display(Name = "City")]
        public int CityID { get; set; }

        public virtual City City { get; set; }

        /// <summary>
        /// Added to support batches of mileage for each mileage form
        /// </summary>
        public virtual ICollection<Trip> Trips { get; set; }

        /// <summary>
        /// Added to relate a mileage form back to a conference form without
        /// making them dependent entities
        /// </summary>
        public virtual ICollection<Conference> Conferences { get; set; }

        public string FullAddress => SiteName + " - " + StreetAddress + ", " + City.Name;
    }
}