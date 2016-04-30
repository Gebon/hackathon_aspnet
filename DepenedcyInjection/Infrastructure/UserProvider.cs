using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace DepenedcyInjection.Infrastructure
{
    public interface IUserProvider
    {
        IPrincipal GetUser(Controller controller);
    }
    public class UserProvider : IUserProvider
    {
        public IPrincipal GetUser(Controller controller)
        {
            return controller.User;
        }
    }
}