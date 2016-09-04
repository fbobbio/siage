using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using Siage.UCControllers;
using Siage.Base;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;
using SIAGE_Escuela.Content.resources;


namespace SIAGE_Escuela.Controllers
{
    public class MatriculaController : AjaxAbmcController<MatriculaModel, IMatriculaRules>
    {
        #region Atributos / Propiedades

        private int idEscuela;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Rule = ServiceLocator.Current.GetInstance<IMatriculaRules>();
            AbmcView = "MatriculaEditor";
            idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
        }

        public override ActionResult Index()
        {
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(idEscuela)) // validar si estoy logueado como escuela
            {
                if (
                    ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetNivelEducativoByEscuela(idEscuela)
                        .Id == (int) NivelEducativoNombreEnum.SUPERIOR)
                    // validar si estoy logueado como escuela de nivel superior
                {
                    ViewData.Add(ViewDataKey.CARRERA.ToString(),
                                 ServiceLocator.Current.GetInstance<ICarreraRules>().GetCarrerasPorEscuelaLogueada());
                    ViewData.Add(ViewDataKey.SEXO.ToString(),
                                 ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetSexoAll());
                    return View();
                }

                TempData[Constantes.ErrorVista] =
                    "Opción válida unicamente para usuarios logueados como escuela de nivel superior.";
                return RedirectToAction("Error", "Home");
            }
            TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela.";
            return RedirectToAction("Error", "Home");

        }

        public override void CargarViewData(EstadoABMC estado)
        {
            ViewData.Add(ViewDataKey.SEXO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetSexoAll());
            ViewData.Add(ViewDataKey.CARRERA.ToString(), ServiceLocator.Current.GetInstance<ICarreraRules>().GetCarrerasByEscuela(idEscuela));
        }

        #endregion

        #region POST Matricula

        public override void RegistrarPost(MatriculaModel model)
        {
            model.Escuela = idEscuela;
            Rule.MatriculaSave(model);
        }

        public override void EditarPost(MatriculaModel model)
        {
            model.Escuela = idEscuela;
            Rule.MatriculaUpdate(model);
        }

        public override void EliminarPost(MatriculaModel model)
        {
            Rule.MatriculaDelete(model);
        }

        #endregion

        #region Procesamiento Busquedas

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, int? filtroNroDocumento, int? filtroCarrera, string filtroSexo, EstadoMatriculaEnum? filtroEstadoMatricula)
        {
            Func<PreinscripcionModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "NroDocumento" ? x => x.Estudiante.Persona.NumeroDocumento :
                sidx == "TipoDocumento" ? x => x.Estudiante.Persona.TipoDocumento :
                sidx == "Sexo" ? x => x.Estudiante.Persona.SexoNombre.ToString() :
                sidx == "Nombre" ? x => x.Estudiante.Persona.Nombre :
                sidx == "Apellido" ? x => x.Estudiante.Persona.Apellido :
                sidx == "Carrera" ? x => x.Matricula.CarreraNombre :
                sidx == "NroMatricula" ? x => x.Matricula.NroMatricula :
                sidx == "Estado" ? x => x.Matricula.Estado.ToString() :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<PreinscripcionModel, IComparable>)(x => x.Matricula.Id);
            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetPreinscripcionByFiltros(filtroCarrera, filtroNroDocumento, filtroSexo, filtroEstadoMatricula, idEscuela);
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
                            a.Matricula.Id.ToString(),
                            a.Estudiante.Id.ToString(),
                            a.Estudiante.Persona.TipoDocumento,
                            a.Estudiante.Persona.NumeroDocumento,
                            a.Estudiante.Persona.SexoNombre.ToString(),
                            a.Estudiante.Persona.Nombre,
                            a.Estudiante.Persona.Apellido,
                            a.Matricula.CarreraNombre,
                            a.Matricula.NroMatricula.ToString(),
                            a.Matricula.Estado.ToString()
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Soporte

        public JsonResult ExisteMatriculaEnCarrera(int alumnoId, int carreraId)
        {
            return Json(Rule.ExisteMatriculaEnCarrera(alumnoId, carreraId, idEscuela), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExisteMatriculaEnCarrerasDeEscuela(int alumnoId, int carreraId)
        {
            return Json(Rule.ExisteMatriculaEnCarrerasDeEscuela(alumnoId, carreraId, idEscuela), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerDocumentosRequeridoPorProcesoPorCarrera(ProcesoEnum proceso, int idCarrera)
        {
            var rows = from DocumentoRequeridoModel e in ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetDocumentosRequeridoPorProcesoYCarrera(proceso, idCarrera)
                       select new { cell = new string[] { e.Id.ToString(), e.GradoAnio, e.Proceso.ToString(), e.Documento.Nombre, e.Observaciones, /*Aca va el dato de presentado*/ "" } };
            return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerificaPreinscripcion()
        {
            return Json(bool.Parse(Rule.RequierePreinscripcion(idEscuela).Valor), JsonRequestBehavior.AllowGet);
        }


        public JsonResult ObtenerDocumentosRequeridoPorProcesoPorCarreraMenosPresentadosEstudiante(ProcesoEnum proceso, int idCarrera, int idEstudiante)
        {
            var rows = from DocumentoRequeridoModel e in ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetDocumentosRequeridoPorProcesoMenosPresentadosEstudiante(proceso, idEstudiante, idEscuela, null, idCarrera)
                       select new { cell = new string[] { e.Id.ToString(), e.GradoAnio, e.Proceso.ToString(), e.Documento.Nombre, e.EsObligatorio ? "Si" : "No" } };
            return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerDocumentosRequeridoPorProcesoPorCarreraPresentadosEstudiante(ProcesoEnum proceso, int idCarrera, int idEstudiante)
        {
            var rows = from DocumentoRequeridoModel e in ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetDocumentosRequeridoPorProcesoPresentadosEstudiante(proceso, idEstudiante, idEscuela, null, idCarrera)
                       select new { cell = new string[] { e.Id.ToString(), e.GradoAnio, e.Proceso.ToString(), e.Documento.Nombre } };
            return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEstadoMatriculaById(int id)
        {
            return Json(Rule.GetEstadoMatriculaById(id).ToString(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }       
}
