using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Siage.Infraestructure.Configuration;
using SIAGE.UI_Common.Controllers;
using SIAGE_Escuela.ModelBinders;

namespace SIAGE_Escuela
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            // Service Locator
            GuyWire guyWire = new GuyWire();
            guyWire.Wire();

            // Model Binders
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime?), new LocalizedBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(decimal?), new LocalizedBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(float?), new LocalizedBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(double?), new LocalizedBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime), new LocalizedBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(decimal), new LocalizedBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(float), new LocalizedBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(double), new LocalizedBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(string), new StringBinder());
        }

        //protected void Application_Error()
        //{
        //    var exception = Server.GetLastError();
        //    Response.Clear();
        //    Server.ClearError();

        //    var routeData = new RouteData();
        //    routeData.Values["controller"] = "Error";
        //    routeData.Values["exception"] = exception;

        //    var httpException = exception as HttpException;
        //    if (httpException == null)
        //    {
        //        routeData.Values["action"] = "Aplicacion";
        //    }
        //    else
        //    {
        //        Response.StatusCode = httpException.GetHttpCode();
        //        switch (Response.StatusCode)
        //        {
        //            case 403:
        //                routeData.Values["action"] = "Forbidden";
        //                break;
        //            case 404:
        //                routeData.Values["action"] = "PageNotFound";
        //                break;
        //        }
        //    }

        //    Response.StatusCode = 500;

        //    IController errorsController = new ErrorController();
        //    var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
        //    errorsController.Execute(rc);
        //}
    }
}