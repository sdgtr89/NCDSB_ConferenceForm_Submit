using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.Models.Extensions
{
    /// <summary>
    /// Utility Class that uses the user identity to return their name and position
    /// </summary>
    public static class IdentityExtension
    {
        public static string GetUserFirstName(this IIdentity identity)
        {
            var first = ((ClaimsIdentity)identity).FindFirst("FirstName");
            // Test for null to avoid issues during local testing
            return (first != null) ? first.Value : string.Empty;
        }

        public static string GetUserLastName(this IIdentity identity)
        {
            var last = ((ClaimsIdentity)identity).FindFirst("LastName");
            // Test for null to avoid issues during local testing
            return (last != null) ? last.Value : string.Empty;
        }

        public static string GetUserPosition(this IIdentity identity)
        {
            var position = ((ClaimsIdentity)identity).FindFirst("Position");
            // Test for null to avoid issues during local testing
            return (position != null) ? position.Value : string.Empty;
        }

        //uses the current user identity and returns the ID which is used for a 
        //relationship between the form and the current user
        public static string GetUserIdentity()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }

    }
}