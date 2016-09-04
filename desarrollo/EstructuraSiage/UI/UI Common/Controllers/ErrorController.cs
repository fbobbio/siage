using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAGE.UI_Common.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Aplicacion(Exception exception)
        {
            return View();
        }

        public ActionResult PageNotFound()
        {
            return View();
        }

        public ActionResult Forbidden()
        {
            return View();
        }
    }
}
