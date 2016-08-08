using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SPA_Angular_CRUD.Data;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace SPA_Angular_CRUD
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Initialize DB
            using (var ctx = new AppDbContext())
            {
                Database.SetInitializer(new AppDbContextInitializer());
                ctx.Database.Initialize(true);
            }
        }
    }
}
