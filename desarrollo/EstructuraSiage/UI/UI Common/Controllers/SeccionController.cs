using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Base;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;



namespace SIAGE.UI_Common.Controllers
{
    public class SeccionController : AjaxAbmcMixtoController<SeccionModel, ISeccionRules, DetalleSeccionModel, IDetalleSeccionRules>
    {
        private IEmpresaRules empresaRules;
        private int idEscuela;
        private IEntidadesGeneralesRules entidadesGeneralesRules;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
          
            base.Initialize(requestContext);
            //ViewData["GradoAñoEscuelaList"] = new SelectList(ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetGradoAñoPorEscuelaLogueada(), "Id", "Nombre");
            //ViewData["TurnoEscuelaList"] = new SelectList(ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnoPorEscuelaLogueada(), "Id", "Nombre");

            entidadesGeneralesRules = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            Rule = ServiceLocator.Current.GetInstance<ISeccionRules>();
            AbmcView = "SeccionEditor";
            
            RuleDetalle = ServiceLocator.Current.GetInstance<IDetalleSeccionRules>();
            AbmcViewDetalle = "DetalleSeccionEditor";

            idEscuela = (int)Session[ConstantesSession.EMPRESA_ID.ToString()];

            

            empresaRules = ServiceLocator.Current.GetInstance<IEmpresaRules>();
            //Toma el valor de la provincia del Web.config
            string idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            ViewData["TipoEscuelaList"] = new SelectList(entidadesGeneralesRules.GetTipoEscuelaAll(), "Id", "Nombre");
            ViewData["PeriodoLectivoList"] = new SelectList(entidadesGeneralesRules.GetPeriodoLectivoAll(), "Id", "Nombre");
            ViewData["DepartamentoProvincialList"] = new SelectList(entidadesGeneralesRules.GetDepartamentoProvincialByProvincia(idProvincia), "Id", "Nombre");
            ViewData["LocalidadList"] = new SelectList(entidadesGeneralesRules.GetLocalidadByProvincia(String.Empty), "Id", "Nombre");
            ViewData["ZonaDesfavorableList"] = new SelectList(entidadesGeneralesRules.GetZonaDesfavorableAll(), "Id", "Nombre");
            ViewData["NivelEducativoList"] = new SelectList(entidadesGeneralesRules.GetNivelEducativoAll(), "Id", "Nombre");
            ViewData["ObraSocialList"] = new SelectList(entidadesGeneralesRules.GetObraSocialAll(), "Id", "Nombre");
            ViewData["TurnoList"] = new SelectList(entidadesGeneralesRules.GetTurnoAll(), "Id", "Nombre");
            ViewData["ModalidadJornadaList"] = new SelectList(entidadesGeneralesRules.GetModalidadJornadaAll(), "Id", "Nombre");
            ViewData["TiposInstrumentoLegal"] =
                new SelectList(ServiceLocator.Current.GetInstance<ITipoInstrumentoLegalRules>().GetAll(), "Id", "Nombre");
            ViewData["DomicilioList"] = new SelectList(entidadesGeneralesRules.GetDomicilioAll(), "Id", "Calle");

            // TODO Vicky ubicar la carga de SelectList en otro lado donde se ejecute solo en caso de ser necesario
            ViewData["ProgramaPresupuestarioList"] = new SelectList(entidadesGeneralesRules.GetProgramaPresupuestarioAll(), "Id", "Codigo");
            //idEscuela = (int)Session[ConstantesSession.EMPRESA_ID.ToString()];
          
          //  ViewData["TurnoList"] = new SelectList(entidadesGeneralesRules.GetTurnoAll(), "Id", "Nombre");
        }

        public override ActionResult Index()
        {
            // validacion
            if (Rule.ValidarEmpresaRural(idEscuela)) // validar
                return View();
            else
            {
                TempData["ErrorVista"] = "No ha iniciado sesión desde una escuela rural. Por favor vuelva a ingresar sus datos de acceso.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult ProcesarSeleccion(int? id)
        {
                    Session[ConstantesSession.EMPRESA_ID.ToString()] = id;
                    return PartialView("ConsultarSeccionEditor");
               
            
        }

        public override void RegistrarPost(SeccionModel model)
        {
           
            Rule.SeccionSave(model);
        }
        public override void EditarPost(SeccionModel model)
        {
            Rule.SeccionUpdate(model);
        }

        public override void EliminarPost(SeccionModel model)
        {
            Rule.SeccionDelete(model);
        }

        #region POST Detalle

        public override void RegistrarDetallePost(DetalleSeccionModel model, int idPadre)
        {
           
            Rule.DetalleSeccionSave(model, idPadre);
        }

        public override void EditarDetallePost(DetalleSeccionModel model, int idPadre)
        {
            Rule.DetalleSeccionUpdate(model, idPadre);
        }

        public override void EliminarDetallePost(DetalleSeccionModel model, int idPadre)
        {
            //En teoria no se puede eliminar
            model.FechaHasta = DateTime.Now;
            Rule.DetalleSeccionDelete(model, idPadre);
        }

        #endregion

        [HttpPost]
        public void GuardarSeccion(SeccionModel model)
        {
          
            Rule.SeccionSave(model);

        }

        [HttpPost]
        public ActionResult ValidarDetalle(int id)
        {
            var turno = ServiceLocator.Current.GetInstance<ISeccionRules>().GetSeccionById(id);
            return Json(turno.Detalle.Count != 0);
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
            var registros = Rule.GetDetalleSeccionByFiltros(filtroNombre);
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

        public JsonResult ProcesarDetalle(string sidx, string sord, int page, int rows, int id)
        {
            Func<DetalleSeccionModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Turno" ? x => x.Division.TurnoNombre :
                sidx == "GradoAño" ? x => x.Division.GradoAñoNombre :
                sidx == "Division" ? x => x.Division.Division :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<DetalleSeccionModel, IComparable>)(x => x.Id);
            //    // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = RuleDetalle.GetDetalleSeccionByIdSeccion(id);
            //    /******************************** FIN AREA EDITABLE *******************************/

            // Ordeno los registros)
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
                            a.Division.TurnoNombre,
                            a.Division.GradoAñoNombre,
                            a.Division.Division.ToString()
                          
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
            var registros = Rule.GetEstructuraEscuelaByFiltrosSeccion(filtroTurno, filtroGrado);
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

        public JsonResult GetEstructuraEscuelaById(int id)
        {
            DiagramacionCursoModel curso = Rule.GetEstructuraEscuelaById(id);
            var jsonCurso =
                 new
                 {
                     cursos = curso,
                     Division = curso.Division.ToString()
                 };
            var diagramacion = Json(jsonCurso, JsonRequestBehavior.AllowGet);
            return diagramacion;
        }

        //public JsonResult GetDivisionById(int id)
        //{
        //    DiagramacionCursoModel division = Rule.GetEstructuraEscuelaById(id);

        //    var diagramacion = Json(division, JsonRequestBehavior.AllowGet);
        //    return diagramacion;
        //}

       

    }       
}
