using System.Web.Mvc;
using Siage.Base;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "SIAGE";
           
            if (TempData[Constantes.ErrorVista] != null)
                ViewData[Constantes.ErrorVista] = TempData[Constantes.ErrorVista];
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult IndexEscuela()
        {
            ViewData["Message"] = "SIAGE";
            return View();
        }

        [HttpGet]
        public ActionResult SeleccionarPerfil()
        {
            ViewData["Message"] = "SIAGE";
            return View();
        }

        public ActionResult Error()
        {
            TempData["Message"] = "No ha iniciado sesión desde una escuela rural. Por favor vuelva a ingresar sus datos de acceso.";
            return View();
        }
    }
}
