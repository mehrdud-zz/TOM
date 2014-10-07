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


    public sealed class WillisRoleProvider : SimpleRoleProvider
    {


        private readonly IUserFactory userFactory;

        public WillisRoleProvider()
        {
            userFactory = new UserFactory();
        }

        public WillisRoleProvider(IUserFactory userFactory)
        {
            this.userFactory = userFactory;
        }





        ///// <summary>
        ///// Checks if user belongs to a given role.
        ///// </summary>
        ///// <param name="username"></param>
        ///// <param name="roleName"></param>
        ///// <returns></returns>
        //public override bool IsUserInRole(string username, string roleName)
        //{
        //    return
        //        userFactory.IsUserInRole(username, roleName);
        //}



        /// <summary>
        /// Get config value.
        /// </summary>
        /// <param name="configValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
            {
                return defaultValue;
            }

            return configValue;
        }


        public bool ValidateUser(string username, string password)
        {

            ModelsLayer.User user = null;

            try
            {
                user = userFactory.GetUser(username);
            }
            catch
            {
                user = null;
            }
            if (user != null)
                return true;
            else
                return false;
        }

    }
}