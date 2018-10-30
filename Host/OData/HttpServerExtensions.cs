using System;
using System.Web.Http;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;

namespace ODataExample.Host.OData
{
    internal static class HttpServerExtensions
    {
        public static void RegisterODataRoutes(this HttpServer httpServer, string modelName, IModelConfiguration modelConfiguration)
        {
            if (httpServer == null)
            {
                throw new ArgumentNullException(nameof(httpServer));
            }

            httpServer.Configuration.RegisterODataRoutes(modelName, modelConfiguration, new DefaultODataBatchHandler(httpServer));
        }
    }
}