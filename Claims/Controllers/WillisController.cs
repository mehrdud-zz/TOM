using ClaimsPoC.Filters;
using ClaimsPoC.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClaimsPoC.Controllers
{

    [InitializeSimpleMembership]
    public class WillisController : Controller
    {
        //
        // GET: /Willis/


        protected WillisMembershipProvider DefaultMembershipProvider;

        public WillisController() {
            DefaultMembershipProvider =
                new WillisMembershipProvider();
        }
    }
}
