using System.Collections.Generic;

namespace ODataExample.Model
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}