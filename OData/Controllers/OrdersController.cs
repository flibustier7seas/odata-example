using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;
using ODataExample.Model;

namespace ODataExample.OData.Controllers
{
    [EnableQuery(MaxExpansionDepth = 10)]
    public class OrdersController : ODataController
    {
        [EnableQuery(MaxExpansionDepth = 10)]
        public IQueryable<Order> Get()
        {
            return new List<Order>().AsQueryable();
        }
    }
}