using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModelsLayer;
using System.Data;

namespace Factories
{
    public interface IUserFactory
    {
        void Initialize();
        User GetUser(string username);
        User GetUser(int userId);
        List<User> GetUsers();
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        void Dispose(bool disposing);
        bool IsUserInRole(string username, string role);
        bool ActivateUser(User usertoLogin);
        bool IsUserActive(string username);
        User GetCurrentUser();

        Guid GetApplicationId();
    }

    public class UserFactory : IUserFactory
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();

        public void Initialize()
        {

        }


        public User GetCurrentUser()
        {
            string username = HttpContext.Current.User.Identity.Name;

            User clientuser = GetUser(username);

            return clientuser;
        }

        public User GetUser(string username)
        {
            var clientusers = _db.Users.Where(user => user.UserName == username);
            if (clientusers.Count() == 1)
            {
                var clientUser = clientusers.ToList()[0];
                //clientUser.Membership = db.aspnet_Membership.Single(membership => membership.UserId == clientUser.UserId);
                return clientUser;
            }

            return null;
        }



        public User GetUser(int userId)
        {
            // Include(c => c.ClientID).
            var clientuser = _db.Users.Where(m => m.UserId == userId);
            if (clientuser.Count() == 1)
            {
                return clientuser.ToList()[0];
            }
            return null;
        }

        public List<User> GetUsers()
        {
            var usersList = _db.Users.ToList();
            // .Include(c => c.Client); 
            //foreach (var clientuser in usersList)
            //{
            //    try
            //    {
            //        // clientuser.Membership = db.aspnet_Membership.Single(membership => membership.UserId == clientuser.UserID);
            //    }
            //    catch { }
            //}
            return usersList;
        }

        public bool CreateUser(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();

            return true;
        }

        public bool UpdateUser(User user)
        {
            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteUser(User user)
        {
            _db.Users.Remove(user);
            _db.SaveChanges();
            return true;
        }

        public void Dispose(bool disposing)
        {
            _db.Dispose();
            //base.Dispose(Disposing);
        }

        public bool IsUserInRole(string username, string role)
        {

            bool result = false;

            var usreRoles =
                from users in _db.aspnet_Users
                from roles in _db.aspnet_Roles
                where users.UserName == username &&
                roles.RoleName == role &&
                users.aspnet_Roles.Contains(roles)
                select users;

            if (usreRoles.Count() == 1)
                result = true;

            return result;
        }

        public bool ActivateUser(User usertoLogin)
        {
            if (usertoLogin != null && usertoLogin.Activated && usertoLogin.ClientID != null)
            {
                usertoLogin.Activated = true;
                _db.Entry(usertoLogin).State = EntityState.Modified;
                _db.SaveChanges();
                return true;
            }

            return false;
        }

        public bool IsUserActive(string username)
        {
            var user = GetUser(username);
            if (user != null && user.Activated && user.ClientID != null)
                return true;

            return false;
        }


        public Guid GetApplicationId()
        {
            return _db.aspnet_Applications.Single().ApplicationId;
        }
    }

}