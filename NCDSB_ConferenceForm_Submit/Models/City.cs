using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.Models
{
    public class City : Auditable
    {
        public int ID { get; set; }

        public City()
        {
            this.Addresses = new HashSet<Address>();
            this.IsRemoved = false;
        }

        [Required(ErrorMessage = "City Name is required.")]
        [Display(Name = "City")]
        [Index("IX_Unique_City", IsUnique = true)]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters.")]
        public string Name { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}