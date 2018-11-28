using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.Models
{
    /// <summary>
    /// For use with Trip Entity Validation
    /// </summary>
    public class TripException : Exception
    {
        public override string Message
        {
            get
            {
                return "You can only add a round trip to a Conference Form," +
                    " which can contain only two trips.";
            }
        }
    }
}