using System.Web.Mvc;
using System.Web.Routing;

namespace Pratica_Profissional
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BinderConfig.RegisterGlobalBinders(ModelBinders.Binders);
        }
    }
}
