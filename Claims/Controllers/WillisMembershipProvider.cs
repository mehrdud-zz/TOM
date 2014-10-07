//http://bojanskr.blogspot.co.uk/2011/12/custom-role-provider.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;
using ClaimsPoC.Controllers;
using Factories;
using WebMatrix.WebData;

namespace ClaimsPoC.Providers
{
    public sealed class WillisMembershipProvider : SimpleMembershipProvider
    {
        private readonly IUserFactory userFactory;

        public WillisMembershipProvider()
            : this(new UserFactory())
        {
        }

        public WillisMembershipProvider(IUserFactory userFactory)
        {
            this.userFactory = userFactory;
        }

        /// <summary>
        /// Checks if user belongs to a given role.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool IsUserInRole(string username, string roleName)
        {            
            return
                userFactory.IsUserInRole(username, roleName);
        }
    }
}