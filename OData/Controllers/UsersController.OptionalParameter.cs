using System.Globalization;
using Microsoft.AspNet.OData.Routing;

namespace ODataExample.OData.Controllers
{
    public partial class UsersController
    {
        [ODataRoute("Users/GetFullName(firstName={firstName})")]
        public string GetFullName(string firstName)
        {
            return GetFullName(firstName, "Unknown");
        }

        [ODataRoute("Users/GetFullName(firstName={firstName},lastName={lastName})")]
        public string GetFullName(string firstName, string lastName)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", firstName, lastName);
        }

        [ODataRoute("Users/GetFirstName()")]
        public string GetFirstName()
        {
            return GetFirstName("Unknown");
        }

        [ODataRoute("Users/GetFirstName(firstName={firstName})")]
        public string GetFirstName(string firstName)
        {
            return firstName;
        }
    }
}
