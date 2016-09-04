using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using SIAGE.UI_Common.Controllers;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Siage.Base;
using SIAGE.UI_Common.Content;

namespace SIAGE_Escuela.Controllers
{
    public class AccidenteLaboralController : AjaxAbmcController<AccidenteLaboralModel, IAccidenteLaboralRules>
    {
        //
        // GET: /AccidenteLaboral/
        private IEntidadesGeneralesRules entidades;
        private IEmpresaRules empresaRules;
        
        public ActionResult Index()
        {
            CargarViewData(EstadoABMC.Consultar);
            AbmcView = "AccidenteLaboralEditor";
            return View();

        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "AccidenteLaboralEditor";
            Rule = ServiceLocator.Current.GetInstance<IAccidenteLaboralRules>();
            entidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();

            
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            /** Cargo los ViewData del consultar agente */
            string idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(), entidades.GetDepartamentoProvincialByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), new List<LocalidadModel>());
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), entidades.GetTipoDocumentoAll());

            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), entidades.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.DIRECCION_NIVEL.ToString(), entidades.GetDireccionDeNivelAll());
            ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), entidades.GetTipoCargoAll());
            ViewData.Add(ViewDataKey.ASIGNATURA.ToString(), entidades.GetAsignaturaAll());
            ViewData.Add(ViewDataKey.TITULO.ToString(), entidades.GetTituloAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), entidades.GetSituacionRevistaAll());
            ViewData.Add(ViewDataKey.SUCURSAL_BANCARIA.ToString(), entidades.GetSucursalBancariaAll());
            ViewData.Add(ViewDataKey.MOTIVO_BAJA_AGENTE.ToString(), entidades.GetMotivoBajaAgenteAll());
            /********************************* View Data de persona ************************************/
            
            ViewData.Add(ViewDataKey.PAIS.ToString(), entidades.GetPaisAll());
            ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(), entidades.GetEstadoCivilAll());
            ViewData.Add(ViewDataKey.SEXO.ToString(), entidades.GetSexoAll());
            ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(), entidades.GetTipoCalleAll());
            ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(), entidades.GetOrganismoEmisorDocumentoAll());

            /** Cargo los ViewData del consultar puesto de trabajo */
            ViewData.Add(ViewDataKey.TURNO.ToString(), entidades.GetTurnoAll());
            ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), entidades.GetGradoAñoAll());
        }

        public override void RegistrarPost(AccidenteLaboralModel model)
        {
            Rule.AccidenteLaboralSave(model);
        }

        public override void EditarPost(AccidenteLaboralModel model)
        {
            Rule.AccidenteLaboralUpdate(model);
        }

        public override void EliminarPost(AccidenteLaboralModel model)
        {
            Rule.AccidenteLaboralDelete(model);
        }

        public JsonResult GetEmpresaByUsuarioLogueado()
        {

            var empresaActual = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();
            var localidad = new LocalidadModel();
            var nombreLocalidad = string.Empty;
            var provincia = string.Empty;
            var pais = string.Empty;
            var nombreCalle = string.Empty;
            var altura = string.Empty;
            var deptoProv = string.Empty;
            if(empresaActual.Domicilio != null)
            {
                localidad =
                    ServiceLocator.Current.GetInstance<ILocalidadRules>().GetLocalidadById(
                        empresaActual.Domicilio.IdLocalidad);
                nombreLocalidad = localidad.Nombre;
                provincia = localidad.DepartamentoProvincial.Provincia.Nombre;
                pais = localidad.DepartamentoProvincial.Provincia.Pais.Nombre;
                nombreCalle = empresaActual.Domicilio.NombreCalle;
                if (empresaActual.Domicilio.Altura != null)
                {
                    altura = empresaActual.Domicilio.Altura.Value.ToString();
                }
                deptoProv = empresaActual.Domicilio.Departamento;
            }
            var json = new
                           {
                               empresaActual.CodigoEmpresa,
                               NombreEmpresa = empresaActual.Nombre,
                               empresaActual.Telefono,
                               Calle = nombreCalle,
                               Numero = altura,
                               Localidad = nombreLocalidad,
                               Departamento = deptoProv,
                               Provincia = provincia,
                               IdEmpresa = empresaActual.Id,
                               Pais = pais
                           };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, 
                                            DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, string filtroApellido, 
                                            string filtroNombre, TipoSiniestroEnum? filtroTipoSiniestro, DateTime? filtroFechaSiniestro,
                                            DateTime? filtroFechaAnulacion, int? filtroIdAgente)
        {
            // Construyo la funcion de ordenamiento
            Func<AccidenteLaboralConsultaModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Nro Denuncia" ? x => x.NroDenuncia :
                /******************************** FIN AREA EDITABLE *******************************/
            (Func<AccidenteLaboralConsultaModel, IComparable>)(x => x.Id);


            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetAccidenteLaboralByFiltros(filtroFechaDesde, filtroFechaHasta, filtroApellido,
                                                         filtroNombre, filtroTipoSiniestro, filtroFechaSiniestro, filtroFechaAnulacion, filtroIdAgente);
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
                                                         cell = new string[]
                                                                    {
                                                                        a.Id.ToString(),
                                                                        // Respetar el orden en que se mostrarán las columnas
                                                                        /****************************** INICIO AREA EDITABLE ******************************/
                                                                        a.NroSiniestro.ToString(),
                                                                        a.NroDenuncia.ToString(),
                                                                        a.Apellido,
                                                                        a.Nombre,
                                                                        a.TipoSiniestro.ToString(),
                                                                        a.FechaAccidente.ToShortDateString(),
                                                                        a.FechaAnulacion.HasValue ? a.FechaAnulacion.Value.ToShortDateString() : string.Empty,
                                                                        a.HoraAccidente,
                                                                        a.LugarAccidente,
                                                                        /******************************** FIN AREA EDITABLE *******************************/
                                                                    }
                                                     }
                               };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca tipo y numero de documento del agente
        /// </summary>
        /// <param name="idAgente">id agente</param>
        /// <returns>Json con los datos del agente</returns>
        public JsonResult GetDniAgenteByIdAgente (int idAgente)
        {
            var datosAgente = Rule.GetDatosAgenteByIdAgente(idAgente);
            var json = new
            {
                NumeroDocumento = datosAgente.NumeroDocumento,
                TipoDocumento = datosAgente.TipoDocumento
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /// /// <summary>
        /// Busca los prestadores medicos que coinciden con los filtros de busqueda
        /// </summary>
        /// <param name="idEmpresa">id de la empresa logueada</param>
        /// <returns>Json con los prestadores medicos asociados a la empresa logueada</returns>
        public JsonResult ProcesarBusquedaPrestadoresMedicos(string sidx, string sord, int page, int rows)
        {
            // Construyo la funcion de ordenamiento
            Func<DtoEmpresaExternaConsultaModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Id" ? x => x.Id :
                sidx == "Nombre" ? x => x.Nombre :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<DtoEmpresaExternaConsultaModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetPrestadoresMedicos();
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
                    page,
                    records = totalRegistros,
                    rows = from a in registros
                        select new
                        {
                            cell = new string[]
                                    {
                                        a.Id.ToString(),
                                        // Respetar el orden en que se mostrarán las columnas
                                        /****************************** INICIO AREA EDITABLE ******************************/
                                        a.Nombre,
                                        (!string.IsNullOrEmpty(a.BarrioNuevo) ? "B° " + a.BarrioNuevo + " - " : string.Empty) +
                                        (!string.IsNullOrEmpty(a.NombreCalle) ? a.NombreCalle + " - " : string.Empty) +
                                        (a.Altura.HasValue ?  a.Altura.Value.ToString() : string.Empty)
                                        /******************************** FIN AREA EDITABLE *******************************/
                                    }
                        }
                };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busco un accidente laboral por su id
        /// </summary>
        /// <param name="idAccidenteLaboral">id accidente laboral</param>
        /// <returns>Json con los datos del accidente laboral encontrado</returns>
        public JsonResult GetAccidenteLaboralById (int idAccidenteLaboral)
        {
            var accidente = Rule.GetAccidenteLaboralById(idAccidenteLaboral);
            return Json(accidente, JsonRequestBehavior.AllowGet);
        }
    }
}
