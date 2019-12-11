using System;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace ODataExample.Host.OData
{
    public static class HttpConfigurationExtensions
    {
        public static void SetupODataQueryDefaultSettings(this HttpConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configuration.EnableContinueOnErrorHeader();
            configuration.SetDefaultQuerySettings(
                new DefaultQuerySettings
                {
                    //MaxTop = null,
                    //EnableCount = true,
                    //EnableSelect = true,
                    //EnableExpand = true,
                }
            );
        }

        public static void MapODataServiceRoute(this HttpConfiguration configuration,
            string routeName,
            string routePrefix,
            IEdmModel model,
            ODataUriResolver uriResolver
            )
        {
            configuration.MapODataServiceRoute(routeName, routePrefix, builder =>
                builder
                    .AddService(ServiceLifetime.Singleton, sp => model)
                    .AddService(ServiceLifetime.Singleton, sp => uriResolver)
                    .AddService<IEnumerable<IODataRoutingConvention>>(ServiceLifetime.Singleton, sp =>
                        ODataRoutingConventions.CreateDefaultWithAttributeRouting(routeName, configuration)));
        }
    }
}