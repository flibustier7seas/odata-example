using System.Collections.Generic;

namespace ODataExample.Model
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public enum UserRole
    {
        Administrator,
        Manager,
    }
}

