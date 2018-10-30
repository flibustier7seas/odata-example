using System.Web.Http;
using Microsoft.Owin;
using ODataExample.Host;
using ODataExample.Host.OData;
using ODataExample.OData;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace ODataExample.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var httpServer = new HttpServer(config);

            httpServer.RegisterODataRoutes(nameof(ODataExample), ODataExampleModelConfiguration.Configure());
            app.UseWebApi(httpServer);
        }
    }
}
