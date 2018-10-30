using System;
using System.Web.Http;
using Microsoft.AspNet.OData.Batch;
using Microsoft.OData.Edm;

namespace ODataExample.Host.OData
{
    internal static class HttpServerExtensions
    {
        public static void RegisterODataRoutes(this HttpServer httpServer, string modelName, IEdmModel model)
        {
            if (httpServer == null)
            {
                throw new ArgumentNullException(nameof(httpServer));
            }

            httpServer.Configuration.RegisterODataRoutes(modelName, model, new DefaultODataBatchHandler(httpServer));
        }
    }
}