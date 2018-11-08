using System.Collections.Generic;

namespace ODataExample.Model
{
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
}
