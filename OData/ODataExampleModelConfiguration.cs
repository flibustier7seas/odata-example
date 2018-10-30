﻿
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using ODataExample.Model;

namespace ODataExample.OData
{
    public static class ODataExampleModelConfiguration
    {
        public static IEdmModel Configure()
        {
            var builder = new ODataConventionModelBuilder();

            RegisterEntitySets(builder);
            RegisterEntityTypes(builder);

            return builder.GetEdmModel();
        }

        private static void RegisterEntitySets(ODataModelBuilder builder)
        {
            builder.EntitySet<User>("Users");
        }

        private static void RegisterEntityTypes(ODataModelBuilder builder)
        {
            // All expanded property should be marked as containment navigation property for deep expanded
            builder.EntityType<User>()
                .Expand(10, nameof(User.Orders))
                .ContainsMany(x => x.Orders);

            builder.EntityType<Order>()
                .Expand(10, nameof(Order.OrderPositions))
                .ContainsMany(x => x.OrderPositions);

            builder.EntityType<OrderPosition>()
                .Expand(nameof(OrderPosition.Products))
                .ContainsMany(x => x.Products);

            builder.EntityType<Product>()
                .Expand(nameof(Product.Parameters))
                .ContainsMany(x => x.Parameters);

            builder.EntityType<ProductParameters>();
        }
    }
}