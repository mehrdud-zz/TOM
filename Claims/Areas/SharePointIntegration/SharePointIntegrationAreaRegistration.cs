using System.Web.Mvc;

namespace ClaimsPoC.Areas.SharePointIntegration
{
    public class SharePointIntegrationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SharePointIntegration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SharePointIntegration_default",
                "SharePointIntegration/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
