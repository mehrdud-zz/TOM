using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClaimsPoC
{

    public class ListElement
    {
        public string Name;
        public string Value;
        public string Help;
    }



   


    public class Constants
    {
        public const string ADMIN_ROLE = "Admin";
        public const string USER_ROLE = "User";


        public const string CFT_SOURCE = "CFT";
        public const string ACTURIS_SOURCE = "Acturis";
        public const string ECLIPSE_SOURCE = "Eclipse";
        public const string ECLIPSE_POLICY_SOURCE = "Eclipse Policy";

         
    }
}