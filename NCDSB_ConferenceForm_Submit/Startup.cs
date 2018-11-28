using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NCDSB_ConferenceForm_Submit.Startup))]
namespace NCDSB_ConferenceForm_Submit
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
