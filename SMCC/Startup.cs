using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SMCC.Startup))]
namespace SMCC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
