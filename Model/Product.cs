using System.Collections.Generic;

namespace ODataExample.Model
{
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
}