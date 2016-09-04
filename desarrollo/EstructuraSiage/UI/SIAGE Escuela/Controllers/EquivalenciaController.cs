using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAGE_Escuela.Controllers
{
    public class EquivalenciaController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult PlanesDeEstudio(string sidx, string sord, int page, int rows)
        {
            var registros = new string[][] {
                new string[] { "50001" },
                new string[] { "50002" },
                new string[] { "50003" }
               
            };

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);

            // Construyo el json con los valores que se mostraran en la grilla
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRegistros,
                rows = from a in registros select new { cell = new string[] { a[0] } }
              
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AsignaturasPorPlanDeEstudio()
        {
            var registros = new string[][] {
                new string[] {"100", "Matematica" },
                new string[] {"101", "Fisica" },
                new string[] {"102", "Quimica" }
            };

            // Construyo el json con los valores que se mostraran en la grilla
            var jsonData = new
            {
                total = 1,
                page = 1,
                records = 2,
                rows = from a in registros select new { cell = new string[] { a[0], a[1] } }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //TODO (NOEL) No borrar esto, se usa para depurar
        public JsonResult pepe()
        {
            List<string> noel = new List<string> { "Hola", "Chau" };
            return Json(noel, JsonRequestBehavior.AllowGet);
        }

    }
}
