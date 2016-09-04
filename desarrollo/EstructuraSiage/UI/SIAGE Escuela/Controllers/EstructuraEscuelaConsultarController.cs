using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using SIAGE_Escuela.Content.resources;


namespace SIAGE_Escuela.Controllers
{
    public class EstructuraEscuelaConsultarController : Controller
    {
       
        private IDiagramacionCursoRules Rule;
        private IEntidadesGeneralesRules entidadesGeneralesRules;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

           ViewData["CarreraList"] = new SelectList(ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetCarreraByEscuela(1), "Id", "Descripcion");
           ViewData["EscuelaList"] = new SelectList(ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetEscuelaAll(), "Id", "Nombre");
           ViewData["GradoAñoList"] = new SelectList(ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetTurnoPorEscuelaLogueada(), "Id", "Nombre");
           ViewData["TurnoList"] = new SelectList(ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetTurnoPorEscuelaLogueada(), "Id", "Nombre");
            Rule = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>();
            entidadesGeneralesRules = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
        }
        public ActionResult Index()
        {
            var SelectPredeterminado = new List<string> { Constantes.Seleccione };
            ViewData["ComboVacio"] = new SelectList(SelectPredeterminado);
            return View();
        }
       

        /// <summary>
        /// Metodo que recibe desde la vista todos los parametros necesarios para la obtención de los registros a mostrar, filtrarlos y paginados.
        /// A partir del parámetro id (sin incluirlo), los parámetros siguientes son opcionales y dependientes del caso de uso.
        /// </summary>
        /// <param name="sidx">Campo por el cual se ordenan los registros</param>
        /// <param name="sord">Dirección de ordenamiento (Ascendente/Descendente)</param>
        /// <param name="page">Número de página a mostrar</param>
        /// <param name="rows">Cantidad de registros por página</param>
        /// <param name="id">Valor de filtrado por ID</param>
        /// <returns>Objeto JSON que representa la matriz de datos a ser mostrados en la grilla</returns>
        public JsonResult ProcesarBusquedaBasico(string sidx, string sord, int page, int rows, int? id, int? filtroTurno, int? filtrGradoAño, DateTime? filtroFechaApertura, DateTime? filtroFechaCierre, int? filtroCarrera, int? filtroEscuela)
        {
            // Construyo la funcion de ordenamiento
            Func<DiagramacionCursoModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
               sidx == "Turno" ? x => x.Turno.Nombre  :
               sidx == "Turno" ? x => x.GradoAño:
               sidx == "Turno" ? x => x.FechaApertura :
               sidx == "Turno" ? x => x.FechaCierre :
               sidx == "Turno" ? x => x.Carrera.Id :
               sidx == "Turno" ? x => x.Escuela :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<DiagramacionCursoModel, IComparable>)(x => x.Id);
             
            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetEstructuraEscuelaByFiltros(filtroTurno, filtrGradoAño, filtroFechaApertura, filtroFechaCierre, filtroCarrera, filtroEscuela);
            /******************************** FIN AREA EDITABLE *******************************/

            // Ordeno los registros
            if (sord == "asc")
                registros = registros.OrderBy(funcOrden).ToList();
            else
                registros = registros.OrderByDescending(funcOrden).ToList();

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            registros = registros.Skip((page - 1) * rows).Take(rows).ToList();

            // Construyo el json con los valores que se mostraran en la grilla
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRegistros,
                rows = from a in registros
                       select new
                       {
                           cell = new string[] {
                            a.Id.ToString(), 
                            // Respetar el orden en que se mostrarán las columnas
                            /****************************** INICIO AREA EDITABLE ******************************/
                            a.Turno.ToString(),
                            a.GradoAño.ToString(),
                            a.Division.ToString()
                            /******************************** FIN AREA EDITABLE *******************************/
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //Cascada para carrera por Escuela
        public JsonResult CargarCarreraByEscuela(int escuela)
        {
            var json = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetCarreraByEscuela(escuela);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
