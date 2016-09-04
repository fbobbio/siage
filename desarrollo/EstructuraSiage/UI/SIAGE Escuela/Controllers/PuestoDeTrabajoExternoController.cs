using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;

namespace SIAGE_Escuela.Controllers
{
    public class PuestoDeTrabajoExternoController : AjaxAbmcController<PuestoDeTrabajoExternoModel, IPuestoDeTrabajoRules>
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            Rule = ServiceLocator.Current.GetInstance<IPuestoDeTrabajoRules>();
            AbmcView = "PuestoDeTrabajoExternoEditor";
        }

        public override ActionResult Index()
        {
            CargarViewData(EstadoABMC.Consultar);
            return base.Index();
        }

        public override void  CargarViewData(EstadoABMC estado)
        {
            var entidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            var tiposCargo = ServiceLocator.Current.GetInstance<ITipoCargoRules>();
            string idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");

            ViewData.Add(ViewDataKey.ASIGNATURA.ToString(), entidades.GetAsignaturaAll());
            ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(), entidades.GetDepartamentoProvincialByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.DIRECCION_NIVEL.ToString(), entidades.GetDireccionDeNivelAll());
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), entidades.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), entidades.GetLocalidadByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), tiposCargo.FiltroBusquedaTipoCargo(null, null, null, null, null, EstadoTipoCargoEnum.VIGENTE));
            ViewData.Add(ViewDataKey.TIPO_INSTRUMENTO_LEGAL.ToString(), entidades.GetTipoInstrumentoLegalAll());
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), entidades.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.TITULO.ToString(), entidades.GetTituloAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), entidades.GetSituacionRevistaAll());
            ViewData.Add(ViewDataKey.SUCURSAL_BANCARIA.ToString(), entidades.GetSucursalBancariaAll());
            ViewData.Add(ViewDataKey.MOTIVO_BAJA.ToString(), entidades.GetMotivoBajaAgenteAll());
        }

        public override void RegistrarPost(PuestoDeTrabajoExternoModel model)
        {
            Rule.PuestoDeTrabajoExternoSave(model);
        }

        public override ActionResult Editar(int id)
        {
            ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Editar.ToString();
            ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Editar;
            CargarViewData(EstadoABMC.Editar);
            var model = Rule.GetPuestoDeTrabajoExternoById(id);
            return PartialView(AbmcView, model);
        }

        public override void EditarPost(PuestoDeTrabajoExternoModel model)
        {
            Rule.PuestoDeTrabajoExternoUpdate(model);
        }

        public override ActionResult Ver(int id)
        {
            AbmcView = "PuestoDeTrabajoExternoVer";
            return PartialView(AbmcView, id);
        }
        
        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows,
            int? fltEmpresa, int? fltAgente, EstadoPuestoDeTrabajoEnum? fltEstado, 
            DateTime? fltFechaInicioDesde, DateTime? fltFechaInicioHasta, 
            DateTime? fltFechaFinDesde, DateTime? fltFechaFinHasta)
        {
            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetPuestoExternoByFiltros( fltEmpresa, fltFechaInicioDesde,
                    fltFechaInicioHasta, fltFechaFinDesde,
                    fltFechaFinHasta, fltAgente, fltEstado);

            // Construyo la funcion de ordenamiento
            Func<PuestoDeTrabajoExternoModel, IComparable> funcOrden =
                sidx == "Empresa" ? x => x.EmpresaExternaNombre :
                sidx == "Cargo" ? x => x.TipoCargoNombre :
                sidx == "Agente" ? x => x.AgenteNombre :
                sidx == "FechaDesde" ? x => x.FechaInicio :
                sidx == "FechaHasta" ? x => x.FechaFin :
                (Func<PuestoDeTrabajoExternoModel, IComparable>)(x => x.Id);

            // Ordeno los registros);
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
                        a.EmpresaExternaNombre,
                        a.TipoCargoNombre,
                        a.AgenteNombre, 
                        a.FechaInicio.ToString(),
                        a.FechaFin.ToString()
                    }
                }
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPuestoTrabajoConsulta(int idPuesto)
        {
            var puestoModel = Rule.GetPuestoDeTrabajoById(idPuesto);
            var asignacion = Rule.GetAsignacionesByPuestoDeTrabajo(idPuesto);

            var puestoJson = new
            {
                // Datos de la empresa
                TipoEmpresa = puestoModel.Empresa.TipoEmpresa,
                CodigoEmpresa = puestoModel.Empresa.CodigoEmpresa,
                NombreEmpresa = puestoModel.Empresa.Nombre,
                EstadoEmpresa = puestoModel.Empresa.EstadoEmpresa,

                // Datos de la empresa externa
                NombreEmpresaExterna = puestoModel.EmpresaServicio.Nombre,
                EstadoEmpresaExterna = puestoModel.EmpresaServicio.Activo ? "ACTIVA" : "INACTIVA",

                // Datos del puesto de trabajo
                TipoCargo = puestoModel.TipoCargo.Nombre,
                HorasEfectivas = puestoModel.HorasEfectivas,

                // Datos de alta
                FechaAlta = puestoModel.FechaAlta.Value,
                AgenteAlta = puestoModel.UsuarioAlta,

                // Datos de modificación
                FechaModificacion = puestoModel.FechaAlta.Value,
                AgenteModificacion = puestoModel.UsuarioModificacion.NombreUsuario,

                // Datos de asignaciones
                Asignaciones = asignacion.Select(x => new {
                    NombreAgente = x.Agente.Persona.Nombre + " " + x.Agente.Persona.Apellido,
                    TipoDocumento = x.Agente.Persona.TipoDocumento,
                    NumeroDocumento = x.Agente.Persona.NumeroDocumento,
                    FechaInicio = x.FechaInicioEnPuesto,
                    FechaFin = x.FechaFinEnPuesto,
                    Estado = x.Estado.Valor
                })
            };

            var sw = new StringWriter();
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            serializer.Converters.Add(new StringEnumConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Serialize(sw, puestoJson);

            return Json(Convert.ToString(sw), JsonRequestBehavior.AllowGet);
        }
    }
}
