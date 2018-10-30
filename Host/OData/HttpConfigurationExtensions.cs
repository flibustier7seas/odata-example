
using System;
using System.Web.Http;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;

namespace ODataExample.Host.OData
{
    internal static class HttpConfigurationExtensions
    {
        public static void RegisterODataRoutes(this HttpConfiguration config, string modelName, IModelConfiguration modelConfiguration, ODataBatchHandler batchHandler)
        {
            config.SetDefaultQuerySettings(new DefaultQuerySettings());

            var modelBuilder = new VersionedODataModelBuilder(config)
            {
                ModelBuilderFactory = CreateModelBuilderFactory(config),
                ModelConfigurations = { modelConfiguration }
            };

            var models = modelBuilder.GetEdmModels();
            config.MapVersionedODataRoutes(
                routeName: $"{modelName}Route",
                routePrefix: "v{apiVersion}",
                models: models);
        }

        private static Func<ODataModelBuilder> CreateModelBuilderFactory(HttpConfiguration config)
        {
            return () =>
            {
                var builder = new ODataConventionModelBuilder(config)
                {
                    Namespace = nameof(ODataExample),
                };

                return builder;
            };
        }
    }
}
