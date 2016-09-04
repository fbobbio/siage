using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using SIAGE_Escuela.Content.resources;
using SIAGE_Escuela.Content;
using Siage.Base;

namespace SIAGE_Escuela.Controllers
{
    public class ActividadDefinicionInstitucionalController : Controller
    {
        private IEscuelaPlanRules Rule;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Rule = ServiceLocator.Current.GetInstance<IEscuelaPlanRules>();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult PlanesDeEstudio(string sidx, string sord, int page, int rows, int? id)
        {
            // Construyo la funcion de ordenamiento
            Func<PlanEstudioModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Codigo" ? x => x.CodigoPlan:
                sidx == "Titulo" ? x => x.Titulo :
                sidx == "Especialidad" ? x => x.Especialidad :
                sidx == "Orientacion" ? x => x.Orientacion :
                sidx == "SubOrientacion" ? x => x.SubOrientacion :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<PlanEstudioModel, IComparable>)(x => x.Id);


            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            //Devuelvo los planes de estudio que implementa una escuela para luego ver las materias del plan que seleccione
            var escuelasPlan = Rule.GetEscuelaPlanByEscuela(UsuarioLogueadoMock.Instancia.EscuelaId);
            var registros = new List<PlanEstudioModel>();
            foreach (var escuelaPlanModel in escuelasPlan)
            {
                registros.Add(escuelaPlanModel.PlanEstudio);
            }
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
                           a.CodigoPlan,
                           a.Titulo,
                           a.Especialidad,
                           a.Orientacion,
                           a.SubOrientacion
                           /******************************** FIN AREA EDITABLE *******************************/
                           }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //El parametro ID, es el ID del Plan de Estudio, al que hay que traer sus Asignaturas
        public JsonResult Asignaturas(string sidx, string sord, int page, int rows, int id)
        {
            // Construyo la funcion de ordenamiento
            Func<UnidadAcademicaModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                //sidx == "GradoAño" ? x => x.AsignaturaModel.GradoAño :
                //sidx == "Asignatura" ? x => x.AsignaturaModel.Nombre :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<UnidadAcademicaModel, IComparable>)(x => x.Id);


            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = ServiceLocator.Current.GetInstance<IEscuelaPlanRules>().GetUnidadesAcademicasByAsignaturaDefinicionInstitucional(id);
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
                           //a.AsignaturaModel.GradoAño.ToString(),
                           //a.AsignaturaModel.Nombre
                           /******************************** FIN AREA EDITABLE *******************************/
                           }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //El paremetro ID, es el ID de la Unidad Academica, al que hay que traer su respectivo Historial de Actividad de Definicion Institucional
        public JsonResult Historial(string sidx, string sord, int page, int rows, int id)
        {
            // Construyo la funcion de ordenamiento
            Func<ActividadDefinicionInstitucionalModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Nombre" ? x => x.Nombre :
                sidx == "FechaInicioVigencia" ? x => x.FechaInicioVigencia :
                sidx == "FechaFinVigencia" ? x => x.FechaFinVigencia :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<ActividadDefinicionInstitucionalModel, IComparable>)(x => x.Id);


            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            //ServiceLocator.Current.GetInstance<IUnidadAcademicaRules>().ActividadCurricularSave(model);
            var registros = ServiceLocator.Current.GetInstance<IUnidadAcademicaRules>().GetHitorialActividadDefinicionInstitucionalByUnidadAcademicaId(id);
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
                           a.Nombre.ToString(),
                           a.FechaInicioVigencia.ToString(),
                           a.FechaFinVigencia.ToString()
                           /******************************** FIN AREA EDITABLE *******************************/
                           }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //El parametr ID, es el ID de la Unidad Academica, al que hay que trar su ULTIMA: Actividad de Definicion Institucional
        public ActionResult BuscarEditor(int id)
        {
            ActividadDefinicionInstitucionalModel model;
            model = ServiceLocator.Current.GetInstance<IUnidadAcademicaRules>().GetUltimaActividadDefinicionInstitucionalByUnidadAcademica(id);

            return PartialView("ActividadCurricularEditor", model);
        }

        [HttpPost]
        public void Aceptar(ActividadDefinicionInstitucionalModel model)
        {
            ServiceLocator.Current.GetInstance<IActividadDefinicionInstitucionalRules>().ActividadDefinicionInstitucionalSave(model);
        }

    }
}