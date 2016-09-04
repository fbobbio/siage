using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class InscripcionController : AjaxAbmcController<InscripcionRegistrarModel, IInscripcionRules>
    {
        #region Atributos / Propiedades

        private int idEscuela;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "InscripcionEditor";
            Rule = ServiceLocator.Current.GetInstance<IInscripcionRules>();
            idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
        }

        public override ActionResult Index()
        {
            var nivel = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetNivelEducativoByEscuela(idEscuela).Id;
            if (nivel != (int)NivelEducativoNombreEnum.SUPERIOR) // validar si estoy logueado como escuela de nivel superior
            {
                if (ServiceLocator.Current.GetInstance<IEscuelaPlanRules>().TieneEscuelaPlan(idEscuela))
                {
                    ViewData.Add(ViewDataKey.SEXO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetSexoAll());
                    ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoPorNivelEducativoDeEscuela(idEscuela));
                    return View();
                }
                TempData[Constantes.ErrorVista] = "No se pueden realizar inscripciones ya que la escuela no tiene asignado un plan de estudio.";
            }
            else
                TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela de nivel inicial, primario o medio.";
            return RedirectToAction("Error", "Home");   
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            ViewData.Add(ViewDataKey.SEXO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetSexoAll());
            ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoPorNivelEducativoDeEscuela(idEscuela));
        }

        #endregion

        #region POST EstructuraEscuela

        public override void RegistrarPost(InscripcionRegistrarModel model)
        {
            var nivel = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetNivelEducativoByEscuela(idEscuela).Id;
            model.EscuelaId = idEscuela;
            model.NivelEducativoId = nivel;
            Rule.InscripcionSave(model);
        }

        public override void EditarPost(InscripcionRegistrarModel model)
        {
            model.EscuelaId = idEscuela;
            Rule.InscripcionUpdate(model);
        }

        public override void EliminarPost(InscripcionRegistrarModel model)
        {
            model.EscuelaId = idEscuela;
            Rule.InscripcionDelete(model);
        }

        #endregion

        #region Procesamiento Busquedas

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, 
            string filtroNroDocumentoEstudiante, string filtroSexoEstudiante, int? filtroGradoAnio,
            int? filtroTurno, DivisionEnum? filtroDivision, int? filtroEspecialidad, 
            DateTime? filtroPeriodoInscripcionDesde, DateTime? filtroPeriodoInscripcionHasta, int? filtroCicloLectivo)
        {
            Func<InscripcionConsultaModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Turno" ? x => x.Turno :
                sidx == "GradoAnio" ? x => x.GradoAnio :
                sidx == "Division" ? x => x.Division.ToString() :
                sidx == "TipoDocumento" ? x => x.TipoDocumento :
                sidx == "NroDocumento" ? x => x.NroDocumento :
                sidx == "Sexo" ? x => x.Sexo :
                sidx == "Nombre" ? x => x.Nombre :
                sidx == "Apellido" ? x => x.Apellido :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<InscripcionConsultaModel, IComparable>)(x => x.Id);
            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetByFiltros(idEscuela, filtroGradoAnio, filtroNroDocumentoEstudiante, filtroSexoEstudiante,filtroEspecialidad,filtroDivision,filtroTurno,
                filtroPeriodoInscripcionDesde, filtroPeriodoInscripcionHasta, filtroCicloLectivo );
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
                            a.Turno,
                            a.GradoAnio,
                            a.GradoAnioId.ToString(),
                            a.Division.ToString(),
                            a.EstudianteId.ToString(),
                            a.TipoDocumento,
                            a.NroDocumento,
                            a.Sexo,
                            a.Nombre,
                            a.Apellido,
                            a.FechaNacimiento.ToString()
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
       
        #endregion

        #region Soporte

        public JsonResult CargarDivisiones(int gradoAnio)
        {
            var values = from DiagramacionCursoModel e in ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetDiagramacionCursoByFiltros(null, gradoAnio, null, null, idEscuela)
                         orderby e.Division
                         select new { Id = (int)e.Division, Division = e.Division.ToString() };
            return Json(new SelectList(values, "Id", "Division"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDiagramacionesCursoByGradoAnio(int gradoAnio)
        {
            var rows = from DiagramacionCursoModel e in ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetDiagramacionPerteneceAUnidadAcademicaByEscuelaGradoAño(idEscuela, gradoAnio)
                         orderby e.Turno
                         select new { cell = new string[] {e.Id.ToString(), e.Division.ToString(), e.TurnoNombre, e.Cupo.ToString(),  
                             (from InscripcionModel ins in e.Inscripciones
                              where ins.FechaBaja == null
                              select ins).Count().ToString()}};
            return Json(new {rows = rows}, JsonRequestBehavior.AllowGet);
        }

       public JsonResult VerificarEspecialidad(int idDiagramacion)
       {
           var especialidad = ServiceLocator.Current.GetInstance<IEspecialidadRules>().GetEspecialidadByDiagramacionCurso(idDiagramacion);
           return Json(especialidad.Nombre, JsonRequestBehavior.AllowGet);
       }

       public JsonResult GetMateriasAdeudadasByEstudiante(int idEstudiante)
       {
           var rows = from HistorialAdeudadasConsultaModel e in Rule.GetHistorialAdeudadasByEstudiante(idEstudiante)
                      select new { cell = new string[] { e.Id.ToString(), e.GradoAnioNombre, e.CodigoAsignaturaNombre, e.Condicion.ToString(), e.Observaciones, e.PlanId.ToString(), e.GradoAnioId.ToString(), e.CodigoAsignaturaId.ToString(), ((int)e.Condicion).ToString() } };
           return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
       }

       public JsonResult ObtenerDocumentosRequeridoPorProcesoGradoAnio(ProcesoEnum proceso, int gradoAnioId)
       {
           var rows = from DocumentoRequeridoModel e in ServiceLocator.Current.GetInstance<IDocumentoRequeridoRules>().DocumentoRequeridoByFiltros(proceso, gradoAnioId, null)
                      select new { cell = new string[] { e.Id.ToString(), e.GradoAnio, e.Proceso.ToString(), e.Documento.Nombre } };
           return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
       }

        public JsonResult ObtenerDocumentosRequeridoPorProcesoMenosPresentadosEstudiante(ProcesoEnum proceso, int idEstudiante, int idGradoAnio)
        {
            var rows = from DocumentoRequeridoModel e in ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetDocumentosRequeridoPorProcesoMenosPresentadosEstudiante(proceso, idEstudiante, idEscuela, idGradoAnio, null)
                       select new { cell = new string[] { e.Id.ToString(), e.GradoAnio, e.Proceso.ToString(), e.Documento.Nombre } };
            return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerDocumentosRequeridoPorProcesoPresentadosEstudiante(ProcesoEnum proceso, int idEstudiante, int idGradoAnio)
        {
            var rows = from DocumentoRequeridoModel e in ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetDocumentosRequeridoPorProcesoPresentadosEstudiante(proceso, idEstudiante, idEscuela, idGradoAnio, null)
                       select new { cell = new string[] { e.Id.ToString(), e.GradoAnio, e.Proceso.ToString(), e.Documento.Nombre } };
            return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ObtenerHistorialDocumentosRequeridoPorEstudiante(int idEstudiante)
        {
            return Json(new {rows = ""}, JsonRequestBehavior.AllowGet);
        }

       public JsonResult TieneHistorial(int alumnoId)
       {
           bool var = Rule.TieneHistorial(alumnoId, idEscuela);
           return Json(var, JsonRequestBehavior.AllowGet);
       }

       public JsonResult GradosAnioAnteriores(int idGradoAnio)
       {
           var grados = Rule.GetGradoAñoAnteriores(idGradoAnio, idEscuela);
           return Json(new SelectList(grados, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
       }

       public JsonResult GetAsignaturasByEscuelaGrado(int idGradoAnio, int idPlanEstudio)
       {
           var asignaturas = Rule.GetAsignaturasByEscuelaGrado(idEscuela, idGradoAnio,idPlanEstudio);
           return Json(new SelectList(asignaturas, "Id", "AsignaturaNombre"), JsonRequestBehavior.AllowGet);
       }

       public JsonResult GetPlanesByEscuelaGrado(int idGradoAnio)
       {
           var planes = Rule.GetPlanesByEscuelaGrado(idEscuela, idGradoAnio);
           return Json(new SelectList(planes, "Id", "CodigoPlan"), JsonRequestBehavior.AllowGet);
       }

       public JsonResult GetEstudianteByInscripcion(int idInscripcion)
       {
           var estudiante = Rule.GetEstudianteByInscripcion(idInscripcion);
           var edad = DateTime.Now - estudiante.FechaNacimiento;
           var edadRetorno = Math.Truncate(edad.Days / 365f);
           return Json(estudiante, JsonRequestBehavior.AllowGet);
       }

        public JsonResult GetDiagramacionById(int idDiagramacion)
        {
            var diagramacion = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetDiagramacionCursoById(idDiagramacion);
            return Json(new {Id = diagramacion.Id.ToString(), Division = diagramacion.Division.ToString(), Turno = diagramacion.TurnoNombre, Cupo = diagramacion.Cupo.ToString(),  Inscriptos = diagramacion.Inscripciones.Count().ToString()}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTurnosByGradoAnio(int idGradoAnio)
        {
            var turnos = Rule.GetTurnosByGradoAnio(idGradoAnio, idEscuela).OrderBy(x => x.Nombre);
            return Json(new SelectList(turnos, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEspecialidadesEscuelaAsociadasAGradoAnio(int idGradoAnio)
        {
            var especialidades = Rule.GetEspecialidadByGradoAño(idGradoAnio, idEscuela).OrderBy(x => x.Nombre);
            return Json(new SelectList(especialidades, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDivisionesEscuelaByGradoAnioTurno(int idGradoAnio, int idTurno)
        {
            List<DivisionEnum> divisiones = Rule.GetDivisionesEscuelaByGradoAnioTurno(idGradoAnio, idTurno, idEscuela);
            var items = new SelectList(
                (divisiones.Cast<object>().Select(
                    item => new {Text = item.ToString(), Value = item.ToString()})), "Value", "Text");
            
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerificarNivel()
        {
            var nivel = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetNivelEducativoByEscuela(idEscuela).Id;
            return Json(nivel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EsInscripcionAnulada(int idInscripcion)
        {
            return Json(Rule.GetEstadoInscripcionById(idInscripcion) == EstadoInscripcionEnum.ANULADA ? true : false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MostrarLibroFolioInscripcionAnterior(int alumnoId)
        {
            var insc = Rule.GetLibroFolioUltimaInscripcion(alumnoId);
            return Json(new {insc.LibroMatriz, insc.Folio}, JsonRequestBehavior.AllowGet);
        }
        
        #endregion
    }
}
