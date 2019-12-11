using Microsoft.AspNet.OData.Builder;
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

            return builder.GetEdmModel();
        }

        private static void RegisterEntitySets(ODataModelBuilder builder)
        {
            builder.EntitySet<User>("Users");
            builder.EntitySet<Order>("Orders");
            builder.EntitySet<Product>("Products");
            
            builder.EntityType<User>()
                   .Expand(10, nameof(User.Orders))
                ;

            builder.EntityType<Order>()
                   .Expand(10, nameof(Order.OrderPositions))
                ;

            builder.EntityType<OrderPosition>()
                   .Expand(10, nameof(OrderPosition.Products))
                ;

            builder.EntityType<Product>()
                   .Expand(10, nameof(Product.Parameters))
                ;

        }
    }
}