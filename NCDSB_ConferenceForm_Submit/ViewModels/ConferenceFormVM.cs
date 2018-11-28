using NCDSB_ConferenceForm_Submit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.ViewModels
{
    public class ConferenceFormVM
    {
        public ConferenceForm ConferenceForm;
        public IQueryable<Expense> Expenses;
        public IQueryable<Trip> Mileage;
    }
}