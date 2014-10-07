using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelsLayer;
using Factories;

namespace ClaimsPoC.Clients.Controllers
{



    public class UserController : ClaimsPoC.Controllers.WillisController
    {
        private readonly IUserFactory userFactory;
        private readonly IClientFactory clientFactory;


        public UserController(IUserFactory userFactory)
        {
            this.userFactory = userFactory;
        }

        public UserController(IUserFactory userFactory, ClientFactory clientFactory)
        {
            this.userFactory = userFactory;
            this.clientFactory = clientFactory;
        }

        //
        // GET: /Contracts/ClientUser/

        public ActionResult Index()
        {
            List<User> clientusers = userFactory.GetUsers();
            return View(clientusers);
        }

        //
        // GET: /Contracts/ClientUser/Details/5

        public ActionResult Details(int id)
        {
            User clientuser = userFactory.GetUser(id);
            if (clientuser == null)
            {
                return HttpNotFound();
            }
            return View(clientuser);
        }

        //
        // GET: /Contracts/ClientUser/Create

        public ActionResult Create()
        {
            ViewBag.ClientID = new SelectList(clientFactory.GetClients(), "ClientID", "Name");
            return View();
        }

        //
        // POST: /Contracts/ClientUser/Create

        [HttpPost]
        public ActionResult Create(User clientuser, String UserRole)
        {
            if (ModelState.IsValid)
            {

                WebMatrix.WebData.WebSecurity.CreateUserAndAccount(
                     clientuser.UserName,
                     clientuser.Password,
                     new
                     {
                         Activated = clientuser.Activated,
                         RegistrationClientName = clientuser.ClientID,
                         DisplayName = String.Format("{0} {1}", clientuser.Firstname, clientuser.Lastname),
                         Firstname = clientuser.Firstname,
                         Lastname = clientuser.Lastname,
                         Email = clientuser.Email,
                         UserName = clientuser.UserName,
                         ClientID = clientuser.ClientID
                     },
                     false);

                         //        Activated = clientuser.Activated,
                         //DisplayName = String.Format("{0} {1}", clientuser.Firstname, clientuser.Lastname),
                         //ClientID = clientuser.ClientID,
                         //Firstname = clientuser.Firstname,
                         //Lastname = clientuser.Lastname,
                         //Email = clientuser.Email,
                if (User.IsInRole(ClaimsPoC.Properties.Settings.Default.SuperAdminRole))
                {
                    System.Web.Security.Roles.AddUserToRole(clientuser.UserName, UserRole);
                }
                else if (User.IsInRole(ClaimsPoC.Properties.Settings.Default.WillisEmployeeRole))
                {
                    System.Web.Security.Roles.AddUserToRole(clientuser.UserName, ClaimsPoC.Properties.Settings.Default.UserRole);
                }
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(clientFactory.GetClients(), "ClientID", "Name", clientuser.ClientID);
            return View(clientuser);
        }

        //
        // GET: /Contracts/ClientUser/Edit/5

        public ActionResult Edit(int id)
        {
            User clientuser = userFactory.GetUser(id);
            if (clientuser == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(clientFactory.GetClients(), "ClientID", "Name", clientuser.ClientID);
            return View(clientuser);
        }

        //
        // POST: /Contracts/ClientUser/Edit/5

        [HttpPost]
        public ActionResult Edit(User clientuser)
        {
            if (ModelState.IsValid)
            {
                userFactory.UpdateUser(clientuser);

                if (
                    (bool)clientuser.Activated &&
                    !WebMatrix.WebData.WebSecurity.IsConfirmed(clientuser.UserName))
                {



                    //WebMatrix.WebData.WebSecurity.ConfirmAccount(clientuser.Username, Guid.NewGuid().ToString());                 
                }

                if (!String.IsNullOrEmpty(clientuser.Password))
                {
                    string token = WebMatrix.WebData.WebSecurity.GeneratePasswordResetToken(clientuser.UserName);
                    WebMatrix.WebData.WebSecurity.ResetPassword(token, clientuser.Password);
                }

                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(clientFactory.GetClients(), "ClientID", "Name", clientuser.ClientID);
            return View(clientuser);
        }

        //
        // GET: /Contracts/ClientUser/Delete/5

        public ActionResult Delete(int id)
        {
            User clientuser = userFactory.GetUser(id);
            if (clientuser == null)
            {
                return HttpNotFound();
            }
            return View(clientuser);
        }

        //
        // POST: /Contracts/ClientUser/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User clientuser = userFactory.GetUser(id);
            userFactory.DeleteUser(clientuser);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            userFactory.Dispose(disposing);
            base.Dispose(disposing);
        }






    }
}