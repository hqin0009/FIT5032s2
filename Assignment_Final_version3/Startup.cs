using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Assignment_Final_version3.Startup))]
namespace Assignment_Final_version3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
