using System.Web.Mvc;

namespace ClaimsPoC.Areas.WillisAssociate
{
    public class WillisAssociateAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WillisAssociate";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WillisAssociate_default",
                "WillisAssociate/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
