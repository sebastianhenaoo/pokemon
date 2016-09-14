using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PokemonsGame.Startup))]
namespace PokemonsGame
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
