using System.Web.OData.Builder;
using Microsoft.OData.Edm;
using ODataExample.Model;

namespace ODataExample.OData
{
    public static class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder { Namespace = nameof(ODataExample) };

            RegisterEntitySets(builder);
            AdjustEntityTypes(builder);

            return builder.GetEdmModel();
        }

        private static void RegisterEntitySets(ODataModelBuilder builder)
        {
            builder.EntitySet<User>("Users");

            // It is necessary to work nested expand
            // http://stackoverflow.com/questions/28488639/odata-v4-web-api-2-2-deep-level-expand-not-working
            builder.EntitySet<Order>("Orders");

            builder.EntitySet<OrderPosition>("OrderPositions");
            builder.EntitySet<Product>("Products");

        }

        private static void AdjustEntityTypes(ODataModelBuilder builder)
        {
            builder.EntityType<User>()
                .Expand(10, nameof(User.Orders))
                // The property will be marked as containment navigation property
                // and expanding anything more than 3 levels the JSON output does not return information
                .ContainsMany(x => x.Orders)
                ;

            builder.EntityType<Order>()
                .Expand(4, nameof(Order.OrderPositions))
                .Page(maxTopValue: 100, pageSizeValue: 10)
                .Count();

            builder.EntityType<OrderPosition>()
                .Expand(nameof(OrderPosition.Products))
                .Page(maxTopValue: 100, pageSizeValue: 10);

            builder.EntityType<Product>()
                .Expand(nameof(Product.Parameters))
                .Page(maxTopValue: 100, pageSizeValue: 10);

            builder.EntityType<ProductParameters>()
                .Page(maxTopValue: 100, pageSizeValue: 10);

        }
    }
}