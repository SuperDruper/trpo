using BsuirHealthProjectServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BsuirHealthProjectServer.Shared;
using Newtonsoft.Json;

namespace BsuirHealthProjectServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            ModelBinders.Binders.Add(typeof(float), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(float?), new DecimalModelBinder());

            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());

            // Prevents xml serializing, forbids serialization of object referencies
            var config = GlobalConfiguration.Configuration;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
