using System.Web.Http;
using Microsoft.OData.UriParser;
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
            var modelName = nameof(ODataExample);
            var edmModel = EdmModelBuilder.GetEdmModel();

            var config = new HttpConfiguration();
            config.SetupODataQueryDefaultSettings();

            config.MapODataServiceRoute(
                routeName: $"{modelName}Route",
                routePrefix: modelName.ToLowerInvariant(),
                model: edmModel,
                uriResolver: new UnqualifiedODataUriResolver());


            var httpServer = new HttpServer(config);
            app.UseWebApi(httpServer);
        }
    }
}
