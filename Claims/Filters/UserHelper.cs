using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class UserHelper
{
    private static ModelsLayer.ClaimsEntities db = new ModelsLayer.ClaimsEntities();
    public static ModelsLayer.User CurrentUser
    {
        get
        {
            string username = HttpContext.Current.User.Identity.Name;
            ModelsLayer.User user = db.Users.Single(m => m.UserName == username);
            return user;
        }
    }

}