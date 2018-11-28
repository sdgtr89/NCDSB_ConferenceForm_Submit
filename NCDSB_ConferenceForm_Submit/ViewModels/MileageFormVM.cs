using NCDSB_ConferenceForm_Submit.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.ViewModels
{
    public class MileageFormVM
    {        
        //Address
        [Required(ErrorMessage = "Start Address is required")]
        public string StartAddress { get; set; }
        [Required(ErrorMessage = "End Address is required")]
        public string EndAddress { get; set; }

        //Trip Distance
        public decimal Distance { get; set; }
        [Required(ErrorMessage = "Trip Date is required")]
        public DateTime Date { get; set; }
    }
}