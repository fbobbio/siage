using System;
using System.Linq;
using System.Web.Mvc;
using Siage.Base;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using SIAGE.UI_Common.Controllers;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class SeccionController : AjaxAbmcController<SeccionModel, ISeccionRules>
    {
        private int idEscuela;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "SeccionEditor";
            Rule = ServiceLocator.Current.GetInstance<ISeccionRules>();
            //escuela logueada
            idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
        }

        public override ActionResult Index()
        {
            if (Rule.ValidarEmpresaLogueada(idEscuela)) // validar si escuela es rural
            { return View(); }
            
            TempData[Constantes.ErrorVista] = "Opción válida unicamente para escuelas rurales que posean estructuras definitivas.";
            return RedirectToAction("Error", "Home");

        }
        
        public override void RegistrarPost(SeccionModel model)
        {
            Rule.SeccionSave(model, idEscuela);
        }

        public override void EditarPost(SeccionModel model)
        {
            Rule.SeccionUpdate(model, idEscuela);
        }

        public override void EliminarPost(SeccionModel model)
        {
            Rule.SeccionDelete(model, idEscuela);
        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, string filtroNombre)
        {
            Func<SeccionModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Nombre" ? x => x.Nombre :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<SeccionModel, IComparable>)(x => x.Id);
            //    // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetSeccionesByIdEscuela(idEscuela, filtroNombre);
            //    /******************************** FIN AREA EDITABLE *******************************/

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
                            //a.DiagramacionCurso.Carrera.Descripcion.ToString(),
                            a.Nombre
                           /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarBusquedaEstructura(string sidx, string sord, int page, int rows, int? id, int? filtroCarrera, int? filtroTurno, int? filtroGrado)
        {
            rows = 20;
            // Construyo la funcion de ordenamiento
            Func<DiagramacionCursoModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
               sidx == "Turno" ? x => x.TurnoNombre :
               sidx == "GradoAño" ? x => x.GradoAnio :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<DiagramacionCursoModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/

            var registros = Rule.GetDiagramacionesByEscuela(idEscuela);
            //var registros = Rule.GetEstructuraEscuelaByFiltrosSeccion(filtroTurno, filtroGrado);
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
                            a.TurnoNombre,
                            a.GradoAnioNombre,
                            a.Division.ToString()
                            /******************************** FIN AREA EDITABLE *******************************/
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProcesarBusquedaEstructuraCompleta(string sidx, string sord, int page, int rows, int id)
        {
            rows = 20;
            // Construyo la funcion de ordenamiento
            Func<DiagramacionCursoModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
               sidx == "Turno" ? x => x.TurnoNombre :
               sidx == "GradoAño" ? x => x.GradoAnio :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<DiagramacionCursoModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/

            var registros = Rule.GetDiagramacionesVigentesByEscuela(idEscuela, id);
            //var registros = Rule.GetEstructuraEscuelaByFiltrosSeccion(filtroTurno, filtroGrado);
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
                            a.TurnoNombre,
                            a.GradoAnioNombre,
                            a.Division.ToString()
                            /******************************** FIN AREA EDITABLE *******************************/
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIdsSeleccionados(int? idSeccion)
        {
            var jsonCurso = Rule.GetIdsDiagramacionesByIdSeccion(idSeccion);

            var ids = Json(jsonCurso, JsonRequestBehavior.AllowGet);
            return ids;
        }



        public JsonResult GetEstructuraEscuelaById(string sidx, string sord, int page, int rows, int id)
        {
            // Construyo la funcion de ordenamiento
            Func<DiagramacionCursoModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
               sidx == "Turno" ? x => x.TurnoNombre :
               sidx == "GradoAño" ? x => x.GradoAnio :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<DiagramacionCursoModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetDivisionesByIdSeccion(id);
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
                            a.TurnoNombre,
                            a.GradoAnioNombre,
                            a.Division.ToString()
                            /******************************** FIN AREA EDITABLE *******************************/
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

    }
}
