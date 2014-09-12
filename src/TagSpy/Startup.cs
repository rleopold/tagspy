using Microsoft.Owin;
using Owin;
using TagSpy;

namespace TagSpy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}