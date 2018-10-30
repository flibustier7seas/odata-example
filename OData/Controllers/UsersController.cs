using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.OData.Edm;
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

        [HttpGet]
        [ODataRoute("Users/FilterByName(name={name})")]
        public IQueryable<User> FilterUsersByName(string name)
        {
            return query.GetUsers().Where(x => x.Name.Contains(name));
        }

        [HttpGet]
        [ODataRoute("Users({key})/FilterOrdersByName(name={name})")]
        public IQueryable<Order> FilterOrdersByName(long key, string name)
        {
            // TODO: Bug: https://github.com/OData/WebApi/issues/255
            var path = Request.GetPathHandler().Parse("http://localhost/odata/", $"Users({key})/Orders", Request.GetRequestContainer());
            Request.ODataProperties().Path = path;

            return query.GetUsers()
                .Where(x => x.Id == key)
                .SelectMany(x => x.Orders)
                .Where(x => x.Name.Contains(name));
        }

        [HttpGet]
        [ODataRoute("Users({key})/Orders/FilterByName(name={name})")]
        public IQueryable<Order> FilterUserOrdersByName(long key, string name)
        {
            return query.GetUsers()
                .Where(x => x.Id == key)
                .SelectMany(x => x.Orders)
                .Where(x => x.Name.Contains(name));
        }
    }
}
