using System.Collections.Generic;

namespace ODataExample.Model
{
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
}