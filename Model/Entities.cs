using System.Collections.Generic;

namespace ODataExample.Model
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public class Order
    {
        public Order()
        {
            OrderPositions = new List<OrderPosition>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<OrderPosition> OrderPositions { get; set; }
    }

    public class OrderPosition
    {
        public OrderPosition()
        {
            Products = new List<Product>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }

    public class Product
    {
        public Product()
        {
            Parameters = new List<ProductParameters>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductParameters> Parameters { get; set; }
    }

    public class ProductParameters
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}