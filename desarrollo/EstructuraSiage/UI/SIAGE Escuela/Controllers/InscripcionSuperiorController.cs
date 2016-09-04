using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using Siage.UCControllers;
using Siage.Base;
using SIAGE.UI_Common.Controllers;
using SIAGE.UI_Common.Content;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class InscripcionSuperiorController : AjaxAbmcController<InscripcionSuperiorModel, IInscripcionRules>
    {
        #region Atributos / Propiedades
        
        private int idEscuela;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "InscripcionSuperiorEditor";
            idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
            Rule = ServiceLocator.Current.GetInstance<IInscripcionRules>();
        }

        public override ActionResult Index()
        {
            if (Rule.EstaEnPeriodoInscripcionSuperior())
            {
                ViewData.Add(ViewDataKey.SEXO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetSexoAll());
                return View();
            }
            TempData[Constantes.ErrorVista] = "No se puede ingresar a esta opción ya que se encuentra fuera del período de inscripción definido por calendario escolar.";
            return RedirectToAction("Error", "Home");
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            ViewData.Add(ViewDataKey.SEXO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetSexoAll());
        }

        #endregion

        #region POST EstructuraEscuela

        public override void RegistrarPost(InscripcionSuperiorModel model)
        {
            model.EscuelaLogueadaId = idEscuela;
            Rule.InscripcionSuperiorSave(model);
        }

        public override void EliminarPost(InscripcionSuperiorModel model)
        {
            Rule.InscripcionSuperiorDelete(model.Id);
        }

        #endregion

        #region Procesamiento Busquedas

        #endregion

        #region Soporte

        public JsonResult CargarComboCarreras(int idEstudiante)
        {
            var lista = ServiceLocator.Current.GetInstance<ICarreraRules>().GetCarrerasByEstudianteMatriculado(idEstudiante);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMatriculaByEstudianteYCarrera(int idCarrera, int idEstudiante)
        {
            var matricula = ServiceLocator.Current.GetInstance<IMatriculaRules>().GetNumeroMatriculaByEstudianteYCarrera(idEstudiante, idCarrera);
            return Json(matricula, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAsignaturasHabilitadasParaCursarEnCarreraByEstudiante(int idCarrera, int idEstudiante)
        {
            var rows = from DetalleAsignaturaModel e in Rule.GetAsignaturasHabilitadasParaCursarEnCarreraEscuelaByEstudiante(idCarrera, idEstudiante, idEscuela)
                       select new { cell = new string[] { e.Id.ToString(), e.GradoAnioNombre, e.Asignatura.AsignaturaNombre } };
            return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDivisionesPorEscuelaCarreraDetalleAsignatura(int idCarrera, int idDetalleAsignatura)
        {
            var rows = from InscripcionDivisionesConsultaModel e in Rule.GetDivisionesPorEscuelaCarreraDetalleAsignatura(idCarrera, idEscuela, idDetalleAsignatura)
                       select new { cell = new string[] { e.IdUnidadAcademica.ToString(), e.Division, e.Turno, e.Cupo, e.CantidadInscriptos.ToString(), e.DescripcionHorario} };
            return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarGrillaMateriasInscriptas(int idCarrera, int idEstudiante)
        {
            var rows = from  e in Rule.GetInscripcionesEstudianteByEscuelaCarrera(idCarrera, idEscuela, idEstudiante)
                       select new { cell = new string[] { e.Id.ToString(), e.GradoAnioNombre, e.AsignaturaNombre } };
            return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
