using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using SIAGE.UI_Common.Controllers;

namespace SIAGE_Ministerio.Controllers
{
    public class SeccionPruebaController : AjaxAbmcController<SeccionModel, ISeccionRules>
    {
        private int idEscuela;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "SeccionPruebaEditor";            
            Rule = ServiceLocator.Current.GetInstance<ISeccionRules>();
            //escuela logueada
            idEscuela = (int)Session[ConstantesSession.EMPRESA_ID.ToString()];

        }

        public override ActionResult Index()
        {
            if (Rule.ValidarEmpresaRural(idEscuela)) // validar si escuela es rural
            { return View();}
            
             TempData["ErrorVista"] = "No ha iniciado sesión desde una escuela rural. Por favor vuelva a ingresar sus datos de acceso.";
             return RedirectToAction("Index", "Home");
          
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
           // var registros = Rule.GetDetalleSeccionByFiltrosSinEscuela(filtroNombre);
           //  var registros = Rule.GetDetalleSeccionByFiltros(idEscuela, filtroNombre);
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
            // Construyo la funcion de ordenamiento
            Func<DiagramacionCursoModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
               sidx == "Turno" ? x => x.TurnoNombre :
               sidx == "GradoAño" ? x => x.GradoAño :
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
                            a.GradoAñoNombre,
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

        

        public JsonResult GetEstructuraEscuelaById(string sidx, string sord, int page, int rows, int id )
        {
            // Construyo la funcion de ordenamiento
            Func<DiagramacionCursoModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
               sidx == "Turno" ? x => x.TurnoNombre :
               sidx == "GradoAño" ? x => x.GradoAño :
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
                            a.GradoAñoNombre,
                            a.Division.ToString()
                            /******************************** FIN AREA EDITABLE *******************************/
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


       
    }
}
