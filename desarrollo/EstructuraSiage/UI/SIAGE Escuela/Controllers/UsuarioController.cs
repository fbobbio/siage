using System;
using System.Linq;
using System.Web.Mvc;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class UsuarioController : Controller
    {
        private IUsuarioRules Rule;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            Rule = ServiceLocator.Current.GetInstance<IUsuarioRules>();
            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IniciarSesion(string txtNombreUsuario, string txtClave)
        {
            try
            {
                var usuarioActivo = Rule.IniciarSesion(txtNombreUsuario, txtClave);
                
                if (usuarioActivo != null)
                {
                    // Tiene más de un rol para esta aplicación
                    if (Rule.TieneMasDeUnRol())
                        return RedirectToAction("SeleccionarPerfil", "Home");

                    // Tiene un rol para esta aplicación
                    var rolActual = Rule.GetRolesByAplicacion().FirstOrDefault();
                    if (rolActual != null)
                        return RedirectToAction(SeleccionarPerfil(rolActual.Id), "Home");

                    // No tiene ningún rol para esta aplicación
                    TempData[Constantes.ErrorVista] = "El usuario no posee el rol adecuado para ingresar a la aplicación";
                }
            }
            catch (ApplicationException ex)
            {
                TempData[Constantes.ErrorVista] = ex.Message;
            }
            //si no permanece en la misma pagina
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public JsonResult GetPerfilesDisponibles()
        {
            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetRolesByAplicacion();

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();
            int totalPages = 1;

            // Construyo el json con los valores que se mostraran en la grilla
            var jsonData = new
            {
                total = totalPages,
                page = 1,
                records = totalRegistros,
                rows = from a in registros select new {
                    cell = new string[] {
                        a.Id.ToString(), 
                        a.TipoRol.ToString(),
                        a.EmpresaCodigo,
                        a.EmpresaNombre                          
                    }
                }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string SeleccionarPerfil(int idPerfil)
        {
            Rule.ConfigurarRolUsuarioActual(idPerfil);
            return "IndexEscuela";
        }

        [HttpGet]
        public ActionResult CerrarSesion()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}