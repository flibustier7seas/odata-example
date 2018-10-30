
using Microsoft.AspNet.OData.Builder;
using Microsoft.Web.Http;

using ODataExample.Model;

namespace ODataExample.OData
{
    public class ODataExampleModelConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            RegisterEntitySets(builder);
            AdjustEntityTypes(builder);
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
