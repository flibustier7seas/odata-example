using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using ODataExample.Model;
using ODataExample.Storage;

namespace ODataExample.OData.Controllers
{
    [EnableQuery(MaxExpansionDepth = 10)]
    public class UsersController : ODataController
    {
        private readonly UserQuery query = new UserQuery();

        [EnableQuery(MaxExpansionDepth = 10)]
        public IQueryable<User> Get()
        {
            return query.GetUsers();
        }

        public SingleResult<User> Get(long key)
        {
            return SingleResult.Create(
                query.GetUsers().Where(x => x.Id == key)
            );
        }

        public IQueryable<Order> GetOrders(long key)
        {
            return query.GetUsers()
                    .Where(x => x.Id == key)
                    .SelectMany(x => x.Orders);
        }
    }
}
