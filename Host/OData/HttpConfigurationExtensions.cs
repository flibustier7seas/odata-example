using System.Web.Http;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.OData.Edm;

namespace ODataExample.Host.OData
{
    internal static class HttpConfigurationExtensions
    {
        public static void RegisterODataRoutes(this HttpConfiguration config, string modelName, IEdmModel model, ODataBatchHandler batchHandler)
        {
            config.SetDefaultQuerySettings(new DefaultQuerySettings());
            
            config.MapODataServiceRoute(
                routeName: $"{modelName}Route",
                routePrefix: "v1",
                model: model);
        }
    }
}
