using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KafeCruisers.Startup))]
namespace KafeCruisers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
