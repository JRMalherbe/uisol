using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using System.Web.Routing;
using UISOL.Models;

namespace UISOL
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            /*
            RouteTable.Routes.Add(new ServiceRoute("ds.svc", new DataServiceHostFactory(), typeof(LabService)));

            var config = GlobalConfiguration.Configuration;
            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customer");
            builder.EntitySet<CustomerFile>("CustomerFile");
            config.Routes.MapODataServiceRoute("api", "api", builder.GetEdmModel());
            */
        }
    }
}
