using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;

namespace SIAGE_Ministerio.Controllers
{
    public class CalendarioEscolarController : AjaxAbmcController<CicloLectivoModel, ICicloLectivoRules>
    {
        #region Atributos / Propiedades

        private int _idEmpresa;
        private IEntidadesGeneralesRules EntidadesGeneralesRule { set; get; }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            EntidadesGeneralesRule = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            AbmcView = "CalendarioEscolarEditor";
            Rule = ServiceLocator.Current.GetInstance<ICicloLectivoRules>();
            
        }

        public override ActionResult Index()
        {
            _idEmpresa = (int)Session[ConstantesSession.EMPRESA.ToString()];
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), ServiceLocator.Current.GetInstance<INivelEducativoRules>().GetNivelesEducativosByEmpresa(_idEmpresa));
            ViewData.Add(ViewDataKey.PERIODO_LECTIVO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetPeriodoLectivoAll());
            return View();
        }


        #endregion

        #region Get

        public override void CargarViewData(EstadoABMC estado)
        {
            GetAños();
            _idEmpresa = (int)Session[ConstantesSession.EMPRESA.ToString()];
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), ServiceLocator.Current.GetInstance<INivelEducativoRules>().GetNivelesEducativosByEmpresa(_idEmpresa));
            ViewData.Add(ViewDataKey.PERIODO_LECTIVO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetPeriodoLectivoAll());

            //INSTRUMENTO LEGAL

            var idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(), EntidadesGeneralesRule.GetDepartamentoProvincialByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), new List<LocalidadModel>());
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), EntidadesGeneralesRule.GetTipoDocumentoAll());
            //ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), EntidadesGeneralesRule.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.DIRECCION_NIVEL.ToString(), EntidadesGeneralesRule.GetDireccionDeNivelAll());
            ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), EntidadesGeneralesRule.GetTipoCargoAll());
            ViewData.Add(ViewDataKey.ASIGNATURA.ToString(), EntidadesGeneralesRule.GetAsignaturaAll());
            ViewData.Add(ViewDataKey.TITULO.ToString(), EntidadesGeneralesRule.GetTituloAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), EntidadesGeneralesRule.GetSituacionRevistaAll());
            ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(), EntidadesGeneralesRule.GetEstadoCivilAll());
            ViewData.Add(ViewDataKey.SEXO.ToString(), EntidadesGeneralesRule.GetSexoAll());
            ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(), EntidadesGeneralesRule.GetOrganismoEmisorDocumentoAll());
            ViewData.Add(ViewDataKey.PAIS.ToString(), EntidadesGeneralesRule.GetPaisAll());
            ViewData.Add(ViewDataKey.TIPO_INSTRUMENTO_LEGAL.ToString(), ServiceLocator.Current.GetInstance<ITipoInstrumentoLegalRules>().GetAll());
            ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(), EntidadesGeneralesRule.GetTipoCalleAll());
        }

        //public override ActionResult Registrar()
        //{
        //    return ProcesarAbmGet(null, EstadoABMC.Registrar);
        //}

        public override ActionResult Editar(int id)
        {
            CargarComboEditar(id);
            return base.Editar(id);
        }

        #endregion

        #region Post

        public override void RegistrarPost(CicloLectivoModel model)
        {
            model = Rule.CicloLectivoSave(model);
        }



        public override void EditarPost(CicloLectivoModel model)
        {
            model = Rule.CicloLectivoSave(model);
        }

        public override void EliminarPost(CicloLectivoModel model)
        {
            Rule.CicloLectivoDelete(model);
        }
        #endregion

        #region Procesamiento busquedas

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? FiltroAnio,
                                           int? FiltroPeriodo, int? FiltroNivelEducativo, DateTime? FiltroFechaDesde,
                                           DateTime? FiltroFechaHasta, int? FiltroProceso)
        {
            Func<CicloLectivoModel, IComparable> funcOrden =
                sidx == "FechaInicio"
                    ? x => x.FechaInicio
                    : (Func<CicloLectivoModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetCiclosLectivosByFiltros(FiltroAnio, FiltroPeriodo, FiltroNivelEducativo,
                                                            FiltroFechaDesde, FiltroFechaHasta, FiltroProceso);

            // Ordeno los registros
            registros = sord == "asc"
                            ? registros.OrderBy(funcOrden).ToList()
                            : registros.OrderByDescending(funcOrden).ToList();

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();
            registros = registros.Skip((page - 1) * rows).Take(rows).ToList();

            // Construyo el json con los valores que se mostraran en la grilla
            var jsonData = new
                               {
                                   total = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows),
                                   page = page,
                                   records = totalRegistros,
                                   rows = from a in registros
                                          select new
                                                     {
                                                         cell = new string[]
                                                                    {
                                                                        a.Id.ToString(),
                                                                        a.AñoCiclo.ToString(),
                                                                        a.NivelEducativoNombre,
                                                                        a.PeriodoLectivoNombre,
                                                                        a.FechaInicio.ToString(),
                                                                        a.FechaFin.ToString()
                                                                    }
                                                     }
                               };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarDetalle(string sidx, string sord, int page, int rows, int id)
        {
            var registros = Rule.GetCalendariosEscolaresByCicloLectivoId(id);
            registros = registros ?? new List<CalendarioEscolarModel>();

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();

            // Construyo el json con los valores que se mostraran en la grilla
            var jsonData = new
                               {
                                   total = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows),
                                   page = page,
                                   records = totalRegistros,
                                   rows = from a in registros
                                          select new
                                                     {
                                                         cell = new string[]
                                                                    {
                                                                        a.Id.ToString(),
                                                                        a.FechaInicio.ToString(),
                                                                        a.FechaFin.ToString(),
                                                                        a.ProcesoNombre.ToString(),
                                                                        a.Hora.ToString(),
                                                                        a.EsHabil ? "Si" : "No"
                                                                    }
                                                     }
                               };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Soporte

        public JsonResult GetProcesosByNivelEducativo(int idNivelEducativo)
        {
            var procesos =
                ServiceLocator.Current.GetInstance<IProcesoRules>().GetProcesosByNivelEducativo(idNivelEducativo).
                    OrderBy(x => x.Nombre).ToList();
            if (idNivelEducativo == (int)NivelEducativoNombreEnum.SUPERIOR)
            {
                foreach (var p in procesos)
                {
                    if (p.Nombre == ProcesoEnum.PRE_INSCRIPCION.ToString())
                    {
                        procesos.Remove(p);
                        break;
                    }
                }
            }

            return Json(new SelectList(procesos, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEtapasByNivelEducativo(int idNivelEducativo)
        {
            var etapas =
                ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetEtapasNivelByNivelEducativo(
                    idNivelEducativo).OrderBy(x => x.Nombre);
            return Json(new SelectList(etapas, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFechasByCicloLectivo(int ciclo)
        {
            var registros = Rule.GetCalendariosEscolaresByCicloLectivoId(ciclo);
            var fechas = new
                               {
                                   rows = from a in registros
                                          select new
                                                     {
                                                         cell = new string[]
                                                                    {
                                                                        a.Id.ToString(),
                                                                        a.FechaInicio.ToString(),
                                                                        a.FechaFin.ToString(),
                                                                        a.Hora.ToString(),
                                                                        a.EtapaNombre.ToString(),
                                                                        a.ProcesoNombre.ToString(),
                                                                        a.Concepto,
                                                                        a.EsHabil ? "Si" : "No"
                                                                    }
                                                     }
                               };
            return Json(fechas, JsonRequestBehavior.AllowGet);
        }

        private void CargarComboEditar(int id)
        {
            var ciclo = ServiceLocator.Current.GetInstance<ICicloLectivoRules>().GetCicloLectivoById(id);
            ViewData.Add(ViewDataKey.PROCESO.ToString(), ServiceLocator.Current.GetInstance<IProcesoRules>().GetProcesosByNivelEducativo(ciclo.NivelEducativoId));
            ViewData.Add(ViewDataKey.ETAPA.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetEtapasNivelByNivelEducativo(ciclo.NivelEducativoId));
        }

        private void GetAños()
        {
            List<ComboModel> lista = new List<ComboModel>(); 
            int añoInicial = 2011;
            int añoFin = añoInicial +51;
            for (int i = añoInicial; i < añoFin;i++ )
            {
                ComboModel item = new ComboModel();
                item.Id = i;
                item.Nombre = i.ToString();
                lista.Add(item);
            }
 
            ViewData.Add(ViewDataKey.ANIO.ToString(), lista);

        }

        
        #endregion
    }
}
    