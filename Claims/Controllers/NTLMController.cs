using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClaimsPoC.Controllers
{
    public class NTLMController : Controller
    {
        //
        // GET: /NTLM/

        public ActionResult Index()
        {
            string username = User.Identity.Name;



            return View();
        }

    }
}
