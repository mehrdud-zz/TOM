using System.Web.Mvc;

namespace ClaimsPoC.ClientHome
{
    public class ClientHomeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ClientHome";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ClientHome_default",
                "ClientHome/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
