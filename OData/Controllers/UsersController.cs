using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.OData;
using ODataExample.Model;
using ODataExample.Storage;

namespace ODataExample.OData.Controllers
{
    [EnableQuery(MaxExpansionDepth = 10)]
    public class UsersController : ODataController
    {
        private readonly UserQuery _query = new UserQuery();

        [EnableQuery(MaxExpansionDepth = 10)]
        public IQueryable<User> Get()
        {
            return _query.GetUsers();
        }

        public SingleResult<User> Get(long key)
        {
            return SingleResult.Create(
                _query.GetUsers()
                .Where(x => x.Id == key)
            );
        }

        public IQueryable<Order> GetOrders(long key)
        {
            return _query.GetUsers()
                    .Where(x => x.Id == key)
                    .SelectMany(x => x.Orders);
        }
    }
}