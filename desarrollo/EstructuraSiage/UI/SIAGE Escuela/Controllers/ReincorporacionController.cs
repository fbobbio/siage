using System;
using System.Collections.Generic;
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
    public class ReincorporacionController : AjaxAbmcController<ReincorporacionRegistrarModel, IReincorporacionRules>
    {
        /** Región para declarar la inicialización del controller */
        #region Inicialización
        IEntidadesGeneralesRules entidadesGeneralesRules;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "ReincorporacionEditor";
            Rule = ServiceLocator.Current.GetInstance<IReincorporacionRules>();
            entidadesGeneralesRules = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
        }

        public override ActionResult Index()
        {
            var idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(idEscuela) && ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetNivelEducativoByEscuela(idEscuela).Id == (int)NivelEducativoNombreEnum.MEDIO) // validar si estoy logueado como escuela de nivel medio
            {
                ViewData.Add(ViewDataKey.SEXO.ToString(), entidadesGeneralesRules.GetSexoAll());
                ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), entidadesGeneralesRules.GetTipoDocumentoAll());
                ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoPorNivelEducativoDeEscuela(idEscuela));
                return View();
            }
            TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela de nivel medio.";
            return RedirectToAction("Error", "Home");
         
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            ViewData.Add(ViewDataKey.SEXO.ToString(), entidadesGeneralesRules.GetSexoAll());
            ViewData.Add(ViewDataKey.MOTIVO_INCORPORACION.ToString(), entidadesGeneralesRules.GetMotivoIncorporacionAll());
            ViewData.Add(ViewDataKey.AGENTE.ToString(), new List<AgenteConsultaModel>());
        }
        #endregion

        /** Región para declarar los métodos POST (Agregar, Editar y Eliminar) */
        #region Post

        public override void RegistrarPost(ReincorporacionRegistrarModel model)
        {
            var modelo = Rule.ReincorporacionSave(model);
        }

        #endregion

        /** Región para declarar métodos de procesamiento que devuelvan JsonResults*/
        #region Procesamiento Busquedas

        public JsonResult GetDatosReincorporacion(int estudiante)
        {
            var model = Rule.getDatosReincorporaciones(estudiante);
            ViewData.Add(ViewDataKey.AGENTE.ToString(), model.Directivos);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, SexoEnum? filtroSexo, string filtroTipoDocumento, long? filtroNroDocumento, DateTime? filtroFechaDesde, 
                                            DateTime? filtroFechaHasta, int? filtroAnio, int? filtroTurno, DivisionEnum? filtroDivision, ReincorporacionEnum? filtroReincorporacion)
        {
            Func<InscripcionModel, IComparable> funcOrden = 
                sidx == "Apellido" ? x  => x.Estudiante.Persona.Apellido :
                sidx == "Nombre" ? x => x.Estudiante.Persona.Nombre :
                sidx == "TipodeDocumento" ? x => x.Estudiante.Persona.TipoDocumento :
                sidx == "NumdeDocumento" ? x => x.Estudiante.Persona.NumeroDocumento :
                (Func<InscripcionModel, IComparable>)(x => x.Id);

            //    Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetInscripcionConReincorporacionByFiltros(id, filtroSexo, filtroTipoDocumento, filtroNroDocumento, filtroFechaDesde, filtroFechaHasta,
                filtroAnio, filtroTurno, filtroDivision, filtroReincorporacion);

            // Ordeno los registros
            registros = sord == "asc" ? registros.OrderBy(funcOrden).ToList() : registros.OrderByDescending(funcOrden).ToList();

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
                rows = from a in registros select new {
                    cell = new string[] {
                        a.Id.ToString(), 
                        // Respetar el orden en que se mostrarán las columnas
                        a.Estudiante.Persona.Apellido,
                        a.Estudiante.Persona.Nombre,
                        a.Estudiante.Persona.TipoDocumento,
                        a.Estudiante.Persona.NumeroDocumento,
                    }
                }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);            
        }

        public JsonResult ProcesarDetalle(string sidx, string sord, int page, int rows, int id)
        {
            // puede incluir la funcion de ordenamiento

            var registros = Rule.getReincorporacionByInscripcionId(id);
            registros = registros ?? new List<ReincorporacionModel>();

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);

            // Construyo el json con los valores que se mostraran en la grilla
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRegistros,
                rows = from a in registros select new { cell = new string[] { a.Id.ToString(), a.Fecha.ToString(), a.Responsable.Persona.Nombre } }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}