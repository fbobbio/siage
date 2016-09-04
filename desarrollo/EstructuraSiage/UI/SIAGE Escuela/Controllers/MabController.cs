using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Siage.Base;
using SIAGE.UI_Common.Controllers;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using System.Configuration;
using SIAGE.UI_Common.Content;

namespace SIAGE_Escuela.Controllers
{
    /** 
    * <summary> Clase de implementación del controlador MabController
    *	
    * </summary>
    * <remarks>
    *		Autor: fede.bobbio
    *		Fecha: 6/13/2011 12:43:45 PM
    * </remarks>
    */
    public class MabController : AjaxAbmcController<MabModel, IMabRules>
    {

        /** Región para la declaración de atributos de clase */
        #region Atributos
        private IEntidadesGeneralesRules _entidadesGeneralesRules;
        #endregion

        /** Región para declarar la inicialización del controller */
        #region Inicialización
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _entidadesGeneralesRules = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            AbmcView = "MabEditor";
            Rule = ServiceLocator.Current.GetInstance<IMabRules>();
        }

        #endregion

        /** Región para declarar los métodos POST (Agregar, Editar y Eliminar) */
        #region Post
        public override ActionResult Index()
        {
            CargarViewData(EstadoABMC.Consultar);
            return View();
        }

        public ActionResult GestionarAsignacionPorMab()
        {
            return View();
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            //MABS
            ViewData.Add(ViewDataKey.MODALIDAD_MAB.ToString(), _entidadesGeneralesRules.GetModalidadMabAll());
            ViewData.Add(ViewDataKey.TIPO_NOVEDAD_MAB.ToString(), _entidadesGeneralesRules.GetTipoNovedadAll());
            ViewData.Add(ViewDataKey.CODIGO_MOVIMIENTO_MAB.ToString(), _entidadesGeneralesRules.GetCodigoMovimientoMabAll());


            //PUESTO DE TRABAJO
            ViewData.Add(ViewDataKey.TURNO.ToString(), _entidadesGeneralesRules.GetTurnoAll());
            ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), _entidadesGeneralesRules.GetGradoAñoAll());

            //INSTRUMENTO LEGAL
            var idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(), _entidadesGeneralesRules.GetDepartamentoProvincialByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), new List<LocalidadModel>());
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), _entidadesGeneralesRules.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), _entidadesGeneralesRules.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.DIRECCION_NIVEL.ToString(), _entidadesGeneralesRules.GetDireccionDeNivelAll());
            ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), _entidadesGeneralesRules.GetTipoCargoAll());
            ViewData.Add(ViewDataKey.ASIGNATURA.ToString(), _entidadesGeneralesRules.GetAsignaturaAll());
            ViewData.Add(ViewDataKey.TITULO.ToString(), _entidadesGeneralesRules.GetTituloAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), _entidadesGeneralesRules.GetSituacionRevistaAll());
            ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(), _entidadesGeneralesRules.GetEstadoCivilAll());
            ViewData.Add(ViewDataKey.SEXO.ToString(), _entidadesGeneralesRules.GetSexoAll());
            ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(), _entidadesGeneralesRules.GetOrganismoEmisorDocumentoAll());
            ViewData.Add(ViewDataKey.PAIS.ToString(), _entidadesGeneralesRules.GetPaisAll());
            ViewData.Add(ViewDataKey.TIPO_INSTRUMENTO_LEGAL.ToString(), ServiceLocator.Current.GetInstance<ITipoInstrumentoLegalRules>().GetAll());
            ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(), _entidadesGeneralesRules.GetTipoCalleAll());
        }

        public override void RegistrarPost(MabModel model)
        {
            if (model.TipoNovedadId.Value == 4 && model.FechaNovedadDesde == null)
                throw new BaseException("La fecha desde de novedad es un dato requerido.");
            else
                Rule.MabSave(model);
        }

        public override void EditarPost(MabModel model)
        {
            Rule.MabUpdate(model);
        }

        public override void EliminarPost(MabModel model)
        {
            Rule.MabDelete(model);
        }

        public override ActionResult Ver(int id)
        {
            var mab = Rule.GetMabById(id);
            if(!string.IsNullOrEmpty(mab.ObservacionesCargoAnterior))
                mab.ObservacionesCargoAnterior = mab.ObservacionesCargoAnterior.Split('|').Last().Trim();
            if(!string.IsNullOrEmpty(mab.ObservacionesMab))
                mab.ObservacionesMab = mab.ObservacionesMab.Split('|').First().Trim();
            if (!string.IsNullOrEmpty(mab.ObservacionesCargoAnterior) || mab.IdPuestoAnteriorDeMinisterio != null) 
            {
                mab.RegistrarCargoAnterior = true;
                mab.EsCargoDeEmpresaMinisterio = string.IsNullOrEmpty(mab.ObservacionesCargoAnterior);
            }
            if(mab.AsignacionInstrumentoLegalAgente != null)
                mab.CargarInstrumentoLegalCheck = mab.AsignacionInstrumentoLegalAgente.Id.HasValue;

            CargarViewData(EstadoABMC.Consultar);
            
            return PartialView(AbmcView, mab);
        }
        #endregion

        /** Región para cuando se use ABMCMixto. Declaraciones de métodos POST de los detalles (Agregar, Editar y Eliminar) */
        #region Post Detalle
        #endregion

        /** Región para declarar métodos de procesamiento que devuelvan JsonResults*/
        #region Procesamiento Busquedas
        
        /// <summary>
        /// Metodo que recibe desde la vista todos los parametros necesarios para la obtención de los registros a mostrar, filtrarlos y paginados.
        /// A partir del parámetro id (sin incluirlo), los parámetros siguientes son opcionales y dependientes del caso de uso.
        /// </summary>
        /// <param name="sidx">Campo por el cual se ordenan los registros</param>
        /// <param name="sord">Dirección de ordenamiento (Ascendente/Descendente)</param>
        /// <param name="page">Número de página a mostrar</param>
        /// <param name="rows">Cantidad de registros por página</param>
        /// <param name="id">Valor de filtrado por ID</param>
        /// <returns>Objeto JSON que representa la matriz de datos a ser mostrados en la grilla</returns>
        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, string filtroCodigoBarra, int? filtroIdAgente, DateTime? filtroFechaInicial, DateTime? filtroFechaFinal)
        {
            // Construyo la funcion de ordenamiento
            Func<MabModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "FechaNovedadDesde" ? x => x.FechaNovedadDesde :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<MabModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = new List<MabModel>();
            if (!string.IsNullOrEmpty(filtroCodigoBarra))
                registros = Rule.GetMabByFiltroCodigoBarra(filtroCodigoBarra);
            else
            {
                if (filtroIdAgente == null || filtroFechaInicial == null || filtroFechaFinal == null)
                {
                    throw new ApplicationException("Debe ingresar al menos un filtro de búsqueda");
                }
                registros = Rule.GetMabByFiltroAgente(filtroIdAgente.Value, filtroFechaInicial.Value,
                                                      filtroFechaFinal.Value);
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
                            Rule.GetTipoNovedadById(a.TipoNovedadId.Value).Tipo != null ? Rule.GetTipoNovedadById(a.TipoNovedadId.Value).Tipo.ToString() : string.Empty,
                            a.AsignacionAgente.Agente.Persona.TipoDocumento != null ? a.AsignacionAgente.Agente.Persona.TipoDocumento.ToString() : string.Empty,
                            a.AsignacionAgente.Agente.Persona.NumeroDocumento != null ? a.AsignacionAgente.Agente.Persona.NumeroDocumento.ToString() : string.Empty,
                            a.AsignacionAgente.Agente.Persona.Apellido != null ? a.AsignacionAgente.Agente.Persona.Apellido.ToString() : string.Empty,
                            a.AsignacionAgente.Agente.Persona.Nombre != null ? a.AsignacionAgente.Agente.Persona.Nombre.ToString() : string.Empty,
                            a.FechaNovedadDesde != null ? a.FechaNovedadDesde.Value.ToString() : string.Empty,
                            a.FechaNovedadHasta != null ? a.FechaNovedadHasta.Value.ToString() : string.Empty,
                            a.CodigoDeNovedadId.ToString(),
                            a.EmpresaNombre != null ? a.EmpresaNombre.ToString() : string.Empty,
                            a.EmpresaCodigo != null ? a.EmpresaCodigo.ToString() : string.Empty,
                            a.CodigoCargo != null ? a.CodigoCargo.ToString() : string.Empty,
                            a.Horas.ToString(),
                            a.Materia != null ? a.Materia.ToString() : string.Empty,
                            a.Turno != null ? a.Turno.ToString() : string.Empty,
                            a.GradoAno != null ? a.GradoAno.ToString() : string.Empty,
                            a.SeccionDivision != null ? a.SeccionDivision.ToString() : string.Empty,
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarBusquedaSucursales(string sidx, string sord, int page, int rows)
        {
            // Construyo la funcion de ordenamiento
            Func<SucursalBancariaModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Id" ? x => x.Id :
                sidx == "Nombre" ? x => x.Nombre :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<SucursalBancariaModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetSucursalesBancarias();
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
                //total = totalPages,
                rows = from a in registros
                       select new
                       {
                           cell = new string[] {
                            a.Id.ToString(),
                            // Respetar el orden en que se mostrarán las columnas
                            /****************************** INICIO AREA EDITABLE ******************************/
                            a.Codigo.ToString(),
                            a.Nombre != null ? a.Nombre.ToString() : string.Empty,
                            a.Domicilio != null ? a.Domicilio.ToString() : string.Empty
                            /******************************** FIN AREA EDITABLE *******************************/
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busco y creo un json con todas las asignaciones en base al puesto y agente seleccionado
        /// </summary>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="idAgente">id del agente</param>
        /// <param name="idPuesto">id del puesto de trabajo</param>
        /// <returns>json con todos las asignaciones encontradas</returns>
        public JsonResult ProcesarBusquedaAsignacionesByPuestoYAgente(string sidx, string sord, int page, int rows, int idAgente, int idPuesto)
        {
            // Construyo la funcion de ordenamiento
            Func<AsignacionModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Id" ? x => x.Id :
                sidx == "Nombre" ? x => x.Agente.NombreCompleto :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<AsignacionModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetAsignacionesByPuestoYAgente(idAgente, idPuesto);
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
                //total = totalPages,
                rows = from a in registros
                       select new
                       {
                           cell = new string[] {
                            a.Id.ToString(),
                            // Respetar el orden en que se mostrarán las columnas
                            /****************************** INICIO AREA EDITABLE ******************************/
                            a.PuestoDeTrabajo.TipoCargo.Nombre != null ? a.PuestoDeTrabajo.TipoCargo.Nombre.ToString() :string.Empty,
                            a.Agente.Persona.Nombre != null ? a.Agente.Persona.Nombre.ToString() : string.Empty,
                            a.Agente.Persona.Apellido != null ? a.Agente.Persona.Apellido.ToString() : string.Empty,
                            a.Agente.Persona.NumeroDocumento != null ? a.Agente.Persona.NumeroDocumento .ToString() : string.Empty,
                            a.FechaInicioEnPuesto != null ? a.FechaInicioEnPuesto.ToString() : string.Empty,
                            a.FechaFinEnPuesto != null ? a.FechaFinEnPuesto.ToString() : string.Empty,
                            a.Estado.Valor != null ? a.Estado.Valor.ToString() : string.Empty
                            /******************************** FIN AREA EDITABLE *******************************/
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca los mabs de ausentismo para fecha desde = fecha actual
        /// </summary>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns>Lista con los mabs de ausentismo</returns>
        public JsonResult ProcesarBusquedaMabsAusentismoFechaDesde(string sidx, string sord, int page, int rows)
        {
            // Construyo la funcion de ordenamiento
            Func<GestionAsignacionPorMabModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Id" ? x => x.Id :
                sidx == "Nombre" ? x => x.NombreAgente :
                sidx == "FechaDesde" ? x => x.FechaDesde :
                sidx == "FechaHasta" ? x => x.FechaHasta :
                sidx == "TipoNovedad" ? x => x.TipoNovedad :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<GestionAsignacionPorMabModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var idEmpresa = GetIdEmpresaUsuarioLogueado();
            var registros = Rule.GetAsignacionesByIdEmpresa(idEmpresa).Where(x => x.FechaDesde == DateTime.Now);
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
                //total = totalPages,
                rows = from a in registros
                       select new
                       {
                           cell = new string[] {
                            a.Id.ToString(),
                            // Respetar el orden en que se mostrarán las columnas
                            /****************************** INICIO AREA EDITABLE ******************************/
                            a.TipoNovedad,
                            a.TipoDocumentoAgente,
                            a.NumeroDocumentoAgente,
                            a.ApellidoAgente,
                            a.NombreAgente,
                            a.FechaDesde.HasValue ? a.FechaDesde.ToString() : string.Empty,
                            a.FechaHasta.HasValue ? a.FechaHasta.ToString() : string.Empty,
                            a.CodigoMovimiento,
                            a.NombreEmpresa,
                            a.CodigoEmpresa,
                            a.Cargo
                            /******************************** FIN AREA EDITABLE *******************************/
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarBusquedaMabsAusentismoFechaHasta(string sidx, string sord, int page, int rows)
        {
            // Construyo la funcion de ordenamiento
            Func<GestionAsignacionPorMabModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Id" ? x => x.Id :
                sidx == "Nombre" ? x => x.NombreAgente :
                sidx == "FechaDesde" ? x => x.FechaDesde :
                sidx == "FechaHasta" ? x => x.FechaHasta :
                sidx == "TipoNovedad" ? x => x.TipoNovedad :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<GestionAsignacionPorMabModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var idEmpresa = GetIdEmpresaUsuarioLogueado();
            var registros = Rule.GetAsignacionesByIdEmpresa(idEmpresa).Where(x => x.FechaHasta == DateTime.Now);
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
                //total = totalPages,
                rows = from a in registros
                       select new
                       {
                           cell = new string[] {
                            a.Id.ToString(),
                            // Respetar el orden en que se mostrarán las columnas
                            /****************************** INICIO AREA EDITABLE ******************************/
                            a.TipoNovedad,
                            a.TipoDocumentoAgente,
                            a.NumeroDocumentoAgente,
                            a.ApellidoAgente,
                            a.NombreAgente,
                            a.FechaDesde.HasValue ? a.FechaDesde.Value.ToShortDateString() : string.Empty,
                            a.FechaHasta.HasValue ? a.FechaHasta.Value.ToShortDateString() : string.Empty,
                            a.CodigoMovimiento,
                            a.NombreEmpresa,
                            a.CodigoEmpresa,
                            a.Cargo
                            /******************************** FIN AREA EDITABLE *******************************/
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Devuelve el id de la empresa del usuario logueado
        /// </summary>
        /// <returns>id empresa usuario logueado</returns>
        private int GetIdEmpresaUsuarioLogueado()
        {
            //var empresaUsuarioLogueado = ServiceLocator.Current.GetInstance<IUsuarioRules>().GetCurrentUser().RolActual.EmpresaId;
            return (int)Session[ConstantesSession.EMPRESA.ToString()];
        }

        #endregion

        /** Región para la declaración de métodos de validación y soporte en general */
        #region Soporte

        /* Método que busca los datos de la empresa del usuario logueado */
        public JsonResult GetEmpresaUsuarioLogueado()
        {
            var empresa = Rule.GetEmpresaUsuarioLogueado();
            var resultado = new Object();
            if (empresa != null)
                resultado = new { idEmpresa = empresa.Id, codigo = empresa.CodigoEmpresa, nombre = empresa.Nombre };
            else
                resultado = null;
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        /** Método que checkea si un puesto de trabajo está relacionado con un plan de estudio o no */
        public JsonResult PuestoDeTrabajoHasPlanDeEstudioRelacionado(int idPuesto)
        {
            bool puestoTrabajoRelacionadoPlanEstudio = Rule.PuestoDeTrabajoHasPlanDeEstudioRelacionado(idPuesto);
            return Json(puestoTrabajoRelacionadoPlanEstudio, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSucursalBancaria (int? id)
        {
            if (id != null)
                return Json(Rule.GetSucursalBancariaById(id.Value), JsonRequestBehavior.AllowGet);
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Busca y devuelve una asignacion por su id
        /// </summary>
        /// <param name="id">id de la asignacion</param>
        /// <returns>json con la asignacion</returns>
        public JsonResult GetAsignacion(int? id)
        {
            if (id != null)
                return Json(Rule.GetAsignacionById(id.Value), JsonRequestBehavior.AllowGet);
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /** Método que devuelve la sucursal bancaria de un agente o null si este no tiene */
        public JsonResult GetSucursalBancariaAgente (int idAgente)
        {
            var sucursal = Rule.GetSucursalBancariaDeAgente(idAgente);
            return Json(sucursal, JsonRequestBehavior.AllowGet);
        }

        /** Método que devuelve el Id del agente reemplazado */
        public JsonResult GetAgenteReemplazado(int idPuesto)
        {
            var agenteReemplazado = Rule.GetAgenteReemplazado(idPuesto);
            var ret = agenteReemplazado == null ? -1 : agenteReemplazado.Id;
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInstrumentoLegalAgenteReemplazado(int Id)
        {
            //TODO: traer el instrumento legal de la asignación del agente a reemplazar
            return null;
        }

        public JsonResult GetValorParametroFechasMab()
        {
            return Json(Rule.GetValorParametroFechasMab(), JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Busca el agente en base a su id
        /// </summary>
        /// <param name="idAgente">id del agente</param>
        /// <returns>json con el numero de documento del agente</returns>
        public JsonResult GetDniAgenteById(int idAgente)
        {
            var agente = Rule.GetAgenteById(idAgente);
            var json = new
            {
                NumeroDocumento = agente.Persona.NumeroDocumento,
                TipoDocumento = agente.Persona.TipoDocumento
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        

        /** Método que devuelve el Id del agente que corresponde al puesto de trabajo seleccionado */
        public JsonResult GetAgentePuestoTrabajoSeleccionado(int idPuesto, int idEmpresa)
        {
            var agentePuestoTrabajoSeleccionado = Rule.GetAgentePuestoTrabajoSeleccionado(idPuesto, idEmpresa);
            var ret = agentePuestoTrabajoSeleccionado == null ? -1 : agentePuestoTrabajoSeleccionado.Id;
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCodigosNovedadByGrupo(int idTipoNovedad)
        {
            var codigosNovedad = Rule.GetCodigosMovimientoByGrupoMabId(idTipoNovedad).OrderBy(x => x.Id);
            var json = new
            {
                Codigos = (codigosNovedad.Select(d => new { Id = d.Id, Nombre = d.CodigoYDescripcion })).ToList()
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSituacionRevista(int idAgente,int idPuesto)
        {
            var sit = Rule.GetUltimaSituacionRevistaByPuestoDeTrabajoAgenteModel(idAgente, idPuesto);
            return Json(sit.Id, JsonRequestBehavior.AllowGet);
        }

        /** Método que devuelve el Id del agente correspondiente al mab seleccionado */
        public JsonResult GetMabById(int idMab)
        {
            var mab = Rule.GetMabById(idMab);
            var json = new Object();
            int idAgente = -1;
            int idAgenteReemplazado = -1;
            int idPuestoDeTrabajo = -1;
            int idSucursal = -1;
            string fechaNDesde = "";
            string fechaNHasta = "";
            string observacionesCargoAnterior = "";
            string observacionesMab = "";
            int idPuestoAnteriorDeMinisterio = -1;
            //var asignacionInstrumentoLegalAgente = new Object();
            var idAsignacionInstrumentoLegalAgente = -1;
            int codigoNovedad = -1;

            if (mab.AsignacionAgente != null)
            {
                idAgente = mab.AsignacionAgente.Agente.Id;
                idPuestoDeTrabajo = mab.AsignacionAgente.PuestoDeTrabajo.Id;
            }
            if (mab.AsignacionAgenteReemplazado != null)
            {
                idAgenteReemplazado = mab.AsignacionAgenteReemplazado.Agente.Id;
            }
            if (mab.SucursalBancaria != null)
            {
                idSucursal = mab.SucursalBancaria.Id;
            }
            if (mab.FechaNovedadDesde != null)
            {
                fechaNDesde = mab.FechaNovedadDesde.Value.ToShortDateString();
            }
            if (mab.FechaNovedadHasta != null)
            {
                fechaNHasta = mab.FechaNovedadHasta.Value.ToShortDateString();
            }
            if (!string.IsNullOrEmpty(mab.ObservacionesCargoAnterior))
            {
                observacionesCargoAnterior = mab.ObservacionesCargoAnterior.Split('|').Last();
            }
            if (!string.IsNullOrEmpty(mab.ObservacionesMab))
            {
                observacionesMab = mab.ObservacionesMab.Split('|').First();
            }
            if (mab.IdPuestoAnteriorDeMinisterio != null) 
            {
                idPuestoAnteriorDeMinisterio = mab.IdPuestoAnteriorDeMinisterio.Value;
            }
            if (mab.AsignacionInstrumentoLegalAgente != null)
            {
                idAsignacionInstrumentoLegalAgente = mab.AsignacionInstrumentoLegalAgente.Id.Value;
            }
            
            json = new
            {
                IdAgenteMab = idAgente,
                IdPuestoDeTrabajoMab = idPuestoDeTrabajo,
                IdAgenteReemplazadoMab = idAgenteReemplazado,
                IdSucursalMab = idSucursal,
                FechaNovedadDesde = fechaNDesde,
                FechaNovedadHasta = fechaNHasta,
                ObservacionesCargoAnterior = observacionesCargoAnterior,
                ObservacionesMab = observacionesMab,
                IdPuestoAnteriorDelMinisterio = idPuestoAnteriorDeMinisterio,
                IdAsignacionInstrumentoLegalAgente = idAsignacionInstrumentoLegalAgente,
                CodigoNovedadId = mab.CodigoDeNovedadId
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GestionarAsignacionPorMab(int idMab)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Rule.GestionarAsignacionPorMab(idMab);
                    return Json(new { status = true });
                }
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                    e = e.InnerException;

                ModelState.AddModelError(string.Empty, e.Message);
            }

            var errores = new List<string>();
            for (int i = 0; i < ModelState.Values.Count; i++)
            {
                var propiedad = ModelState.Values.ElementAt(i);
                if (propiedad.Errors.Count != 0)
                {
                    errores.AddRange(propiedad.Errors.Select(item => string.IsNullOrEmpty(item.ErrorMessage) ? item.Exception.Message : item.ErrorMessage));
                }
            }

            return Json(new { status = (ModelState.IsValid), details = errores.ToArray() });
        }

        #endregion
    }
}