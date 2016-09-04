using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class CondicionEspecialInasistenciaController : AjaxAbmcController<CondicionEspecialInasistenciaModel, ICondicionEspecialInasistenciaRules>
    {
        private int _idEscuela;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            Rule = ServiceLocator.Current.GetInstance<ICondicionEspecialInasistenciaRules>();
            AbmcView = "CondicionEspecialInasistenciaEditor";
        }

        public override ActionResult Index()
        {
            // validar si estoy logueado como escuela
            _idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(_idEscuela))
            {
                _idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];

                ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoPorNivelEducativoDeEscuela(_idEscuela));
                ViewData.Add(ViewDataKey.TURNO.ToString(), ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnosByEscuela(_idEscuela));

                CargarViewData(EstadoABMC.Consultar);
                return View();
            }

            TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela.";
            return RedirectToAction("Error", "Home");
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            ViewData.Add(ViewDataKey.SEXO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetSexoAll());
        }

        public override void RegistrarPost(CondicionEspecialInasistenciaModel model)
        {
            model = Rule.CondicionEspecialInasistenciaSave(model);
        }

        public override void EditarPost(CondicionEspecialInasistenciaModel model)
        {
            model = Rule.CondicionEspecialInasistenciaSave(model);
        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, DateTime? fechaDesde, DateTime? fechaHasta, TipoCondicionInasistenciaEnum? condicion, string nroDocumento, int? sexo, int? turno, int? gradoAnio, DivisionEnum? division)
        {
            Func<InscripcionModel, IComparable> funcOrden =
                sidx == "Apellido" ? x => x.Estudiante.Persona.Apellido :
                sidx == "Nombre" ? x => x.Estudiante.Persona.Nombre :
                sidx == "TipoDocumento" ? x => x.Estudiante.Persona.TipoDocumento :
                sidx == "NumeroDocumento" ? x => x.Estudiante.Persona.NumeroDocumento :
                sidx == "GradoAnio" ? x => x.DiagramacionCurso.GradoAnioNombre :
                sidx == "Turno" ? x => x.DiagramacionCurso.TurnoNombre :
                sidx == "Division" ? x => x.DiagramacionCurso.Division:
                (Func<InscripcionModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetInscripcionesByFiltros(fechaDesde, fechaHasta, condicion, nroDocumento, sexo, turno, gradoAnio, division, (int)Session[ConstantesSession.EMPRESA.ToString()]);
           
            // Ordeno los registros
            registros = sord == "asc" ? registros.OrderBy(funcOrden).ToList() : registros.OrderByDescending(funcOrden).ToList();

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();
            registros = registros.Skip((page - 1) * rows).Take(rows).ToList();

            var estudianteRules = ServiceLocator.Current.GetInstance<IEstudianteRules>();

            // Construyo el json con los valores que se mostraran en la grilla
            var jsonData = new
            {
                total = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows),
                page = page,
                records = totalRegistros,
                rows = from a in registros select new {
                    cell = new string[] {
                        a.Id.ToString(), 
                        a.Estudiante.Persona.Apellido,
                        a.Estudiante.Persona.Nombre,
                        a.Estudiante.Persona.TipoDocumento,
                        a.Estudiante.Persona.NumeroDocumento,
                        a.Estudiante.Persona.SexoNombre.ToString(),
                        a.Estudiante.Persona.FechaNacimiento.ToString(),
                        estudianteRules.CalcularEdad(a.Estudiante.Persona.FechaNacimiento).ToString(), 
                        a.DiagramacionCurso.GradoAnioNombre,
                        a.DiagramacionCurso.TurnoNombre,
                        a.DiagramacionCurso.Division.ToString()
                    }
                }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarDetalle(string sidx, string sord, int page, int rows, int id)
        {
            var registros = Rule.GetCondicionEspecialInasistenciaByInscripcionId(id);
            registros = registros ?? new List<CondicionEspecialInasistenciaModel>();

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();

            // Construyo el json con los valores que se mostraran en la grilla
            var jsonData = new
            {
                total = (int)Math.Ceiling((decimal)totalRegistros / rows),
                page = page,
                records = totalRegistros,
                rows = from a in registros select new { 
                    cell = new [] {
                        a.Id.ToString(), 
                        a.TipoCondicionInasistencia.ToString(),
                        a.FechaDesde.ToString(),
                        a.FechaHasta.ToString(),
                        a.EntidadOtorgante
                    } 
                }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
