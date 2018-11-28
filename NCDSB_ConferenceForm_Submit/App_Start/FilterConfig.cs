using System.Web;
using System.Web.Mvc;

namespace NCDSB_ConferenceForm_Submit
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
