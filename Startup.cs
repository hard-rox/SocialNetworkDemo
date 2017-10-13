using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocialNetDemo.Startup))]
namespace SocialNetDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
