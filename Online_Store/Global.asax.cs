using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Online_Store.Classes;

namespace Online_Store
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            //AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Default stuff
            AreaRegistration.RegisterAllAreas();

            // Manually installed WebAPI 2.2 after making an MVC project.
            GlobalConfiguration.Configure(WebApiConfig.Register); // NEW way
            //WebApiConfig.Register(GlobalConfiguration.Configuration); // DEPRECATED

            // Default stuff
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

          //  AreaRegistration.RegisterAllAreas();
          //  //  WebApiConfig.Register(GlobalConfiguration.Configuration);
          //  FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
          //  RouteConfig.RegisterRoutes(RouteTable.Routes);
          //  BundleConfig.RegisterBundles(BundleTable.Bundles);
          ////  //ModelBinders.Binders.Add(typeof(decimal), new ModelBinder.DecimalModelBinder());
          ////  //ModelBinders.Binders.Add(typeof(decimal?), new ModelBinder.DecimalModelBinder());
        }
    }
}
