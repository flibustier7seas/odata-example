using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;
using ODataExample.Model;

namespace ODataExample.OData.Controllers
{
    [EnableQuery(MaxExpansionDepth = 10)]
    public class ProductsController : ODataController
    {
        [EnableQuery(MaxExpansionDepth = 10)]
        public IQueryable<Product> Get()
        {
            return new List<Product>().AsQueryable();
        }
    }
}