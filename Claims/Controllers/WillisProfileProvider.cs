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


    public sealed class WillisProfileProvider : System.Web.Providers.DefaultProfileProvider
    {
        private IUserFactory userFactory;

        public WillisProfileProvider() { 
        }


        public WillisProfileProvider(IUserFactory userFactory)
        {
            this.userFactory = userFactory;
        }

        public override void Initialize(string name, NameValueCollection config)
        {


            base.Initialize(name, config);


        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
       //     this.UserID = Convert.ToInt32(collection["UserID"]);
            base.SetPropertyValues(context, collection);
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {


            return base.GetPropertyValues(context, collection);
        }

        public override string Name
        {
            get
            {
                return base.Name;
            }
        }


        private int _UserId = 0;
        public int UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                _UserId = value;
            }
        }
    }
}