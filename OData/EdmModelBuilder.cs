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
        }

        private static void AdjustEntityTypes(ODataModelBuilder builder)
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
