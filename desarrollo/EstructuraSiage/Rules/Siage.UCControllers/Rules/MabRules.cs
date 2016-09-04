using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.DaoInterfaces;
using Siage.Core.Domain;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using AutoMapper;

namespace Siage.UCControllers.Rules
{
    /**
    * <summary> Clase de implementación de regla de negocio MabRules
    *
    * </summary>
    * <remarks>
    *		Autor: fede.bobbio
    *		Fecha: 6/13/2011 12:10:30 PM
    * </remarks>
    */
    public class MabRules : IMabRules
    {
        /** Región para la declaración de atributos de clase */
        #region Atributos
        private IDaoProvider _daoProvider;
        private IDaoMab _daoMab;
        private AsignacionInstrumentoLegalRules _reglaAsignacion;
        #endregion

        #region Propiedades

        private IDaoMab DaoMab
        {
            get
            {
                if (_daoMab == null)
                {
                    _daoMab = DaoProvider.GetDaoMab();
                }
                return _daoMab;
            }
        }

        private IDaoProvider DaoProvider
        {
            get
            {
                if (_daoProvider == null)
                {
                    _daoProvider = ServiceLocator.Current.GetInstance<IDaoProvider>();
                }
                return _daoProvider;
            }
        }

        private AsignacionInstrumentoLegalRules ReglaAsignacion
        {
            get
            {
                if (_reglaAsignacion == null)
                {
                    _reglaAsignacion = new AsignacionInstrumentoLegalRules();
                }
                return _reglaAsignacion;
            }
        }

        #endregion

        /** Región para la implementación de métodos definidos en las interfaces implementadas */
        #region IMabRules Members

        /// <summary>
        /// Busca un mabs a partir de su id
        /// </summary>
        /// <param name="id">id del mab</param>
        /// <returns> mab model</returns>
        public MabModel GetMabById(int id)
        {
            return Mapper.Map<Mab, MabModel>(DaoMab.GetById(id));
        }

        public MabModel MabSave(MabModel model)
        {
            var mab = new Mab();
            mab.UsuarioAlta = Usuario.CurrentDomain;

            //Cargo el tipo de novedad del MAB
            mab.TipoNovedad = CargarTipoNovedad(model.TipoNovedadId);

            //Método que valida si el model trae los datos requeridos suficientes
            ValidarDatosRequeridos(model);

            //Cargo las entidades de dominio
            var escuela = DaoProvider.GetDaoEmpresaBase().GetById(GetEmpresaUsuarioLogueado().Id);
            var agente = DaoProvider.GetDaoAgente().GetById(model.AsignacionAgente.Agente.Id);
            var puestoTrabajo = DaoProvider.GetDaoPuestoDeTrabajo().GetById(model.AsignacionAgente.PuestoDeTrabajo.Id);

            if (agente == null)
                throw new BaseException("No se ha encontrado el agente seleccionado en la base de datos");
            if(agente.FechaBaja != null)
                throw new ApplicationException("No se puede cargar un mab a un agente dado de baja.");
            if (puestoTrabajo == null)
                throw new BaseException("No se ha encontrado el puesto de trabajo seleccionado en la base de datos");

            //Asignaciones
            mab.AgenteMab = agente;
            if (GetTipoNovedadById(mab.TipoNovedad.Id).Id == (int)TipoNovedadEnum.ALTA)
            {
                mab.Asignacion = new Asignacion();
                mab.Asignacion.PuestoDeTrabajo = puestoTrabajo;
                mab.Asignacion.Agente = agente;
                mab.AsignacionAgenteOrigen = CargarAgenteAReemplazar(model);
            }
            //si no se de alta, verificar que el agente ya posea un mab de alta antes de hacerle otro tipo de mab
            else
            {
                bool tieneMabAlta = DaoMab.AgenteTieneMabAlta(agente.Id);
                if (tieneMabAlta && GetTipoNovedadById(mab.TipoNovedad.Id).Id == (int) TipoNovedadEnum.MOVIMIENTO)
                {
                    if (model.PuestoTrabajoActualMovimiento)
                        //Si se eligió puesto de trabajo actual busco la asignación correspondiente
                    {
                        mab.Asignacion = GetUltimaAsignacionByPuestoDeTrabajoAgente(agente.Id, puestoTrabajo.Id);
                        if (mab.Asignacion == null)
                            throw new BaseException("No se encontró la asignación del agente al puesto de trabajo");
                    }
                    else //Si eligió un puesto de trabajo nuevo
                    {
                        mab.Asignacion = new Asignacion();
                        mab.Asignacion.PuestoDeTrabajo = puestoTrabajo;
                        mab.Asignacion.Agente = agente;
                        mab.AsignacionAgenteOrigen = CargarAgenteAReemplazar(model);
                    }
                }

                if (tieneMabAlta && (GetTipoNovedadById(mab.TipoNovedad.Id).Id == (int)TipoNovedadEnum.AUSENTISMO
                    || GetTipoNovedadById(mab.TipoNovedad.Id).Id == (int) TipoNovedadEnum.BAJA))
                {
                    // si es de ausentismo o baja busco la asignación seleccionada
                    mab.Asignacion = GetUltimaAsignacionByPuestoDeTrabajoAgente(agente.Id, puestoTrabajo.Id);

                    if (mab.Asignacion == null)
                        throw new BaseException("No se encontró la asignación del agente al puesto de trabajo");
                }
            }
            //TODO: hacer la modificación de domicilio del agente

            //Valido El estado del puesto de trabajo seleccionado, para saber si puede ser asignado al agente
            ValidarEstadoPuestoDeTrabajo(mab, puestoTrabajo, model);
            //Valido que se haya cargado la modalidad si el puesto está relacionado con un plan
            mab.Modalidad = ValidarRelacionPuestoTrabajoConPlanEstudio(puestoTrabajo, model);
            //Valido la selección de situación de revista
            mab.SituacionDeRevista = ValidarSituacionDeRevistaSeleccionada(model.SituacionRevistaId);
            mab.Asignacion.SituacionDeRevista = mab.SituacionDeRevista;
            //Valido las fechas de novedad
            ValidarFechas(mab, model);
            //Cargo el instrumento legal si se seleccionó
            mab.ActoAdministrativo = CargarInstrumentoLegal(model, model.CargarInstrumentoLegalCheck);
            //Cargo el código de novedad
            mab.CodigoMovimiento = CargarCodigoMovimientoMab(model.CodigoDeNovedadId, mab);
            //Manejo la sucursal bancaria del agente
            AsociarSucursalBancariaAAgente(agente, model.SucursalBancaria);
            //Manejo datos sobre empresa y cargo anterior
            CargarDatosEmpresaYCargoAnterior(model, mab);
            //Genero el código de barras
            mab.CodigoBarra = GenerarCodigoBarraMab();
            mab.FechaAutorizacionCargo = DateTime.Today;

            //Traigo la ejecución mab para poder trabajar con los estados de la asignación
            var grupo = mab.CodigoMovimiento.GrupoMab;
            var ejecucionPT = DaoProvider.GetDaoEjecucionMab().GetById(grupo.Enpt.Id);
            
            //Trabajo las listas de detalles
            var listDetalles = ProcesarMab(mab, model,ejecucionPT);

            mab = EjecutarMab(mab,ejecucionPT);
            PersistirEntidades(mab, listDetalles);

            return Mapper.Map<Mab, MabModel>(mab);
        }

        public MabModel MabDelete(MabModel model)
        {
            if (model == null || model.Id <= 0)
                throw new BaseException("MAB inválido para borrar");

            var mab = DaoMab.GetById(model.Id);
            //TODO: Modificaciones para baja
            return Mapper.Map<Mab, MabModel>(DaoMab.SaveOrUpdate(mab));
        }

        public MabModel MabUpdate(MabModel model)
        {
            if (model == null || model.Id <= 0)
                throw new BaseException("MAB inválido para modificación");

            var mab = DaoMab.GetById(model.Id);

            if (mab == null)
                throw new BaseException("No se encontró MAB con Id " + model.Id);

            if (model.FechaNovedadDesde != null && mab.FechaDesde != model.FechaNovedadDesde.Value)
                throw new BaseException("La fecha desde de novedad no se puede modificar.");

            if (model.FechaNovedadHasta != null && mab.FechaHasta != model.FechaNovedadHasta.Value)
                throw new BaseException("La fecha hasta de novedad no se puede modificar.");

            var observacionesCargoAnterior = string.Empty;

            if (mab.Observaciones != null)
                observacionesCargoAnterior = mab.Observaciones.Split('|').Last();

            mab.Observaciones = model.ObservacionesMab + " | " + observacionesCargoAnterior;
            mab.FechaDesde = model.FechaNovedadDesde;
            mab.FechaHasta = model.FechaNovedadHasta;
            mab.ActoAdministrativo = CargarInstrumentoLegal(model, model.CargarInstrumentoLegalCheck);

            var listaDetalles = new List<DetalleAsignacion>();

            //Traigo la ejecución mab para poder trabajar con los estados de la asignación
            var grupo = mab.CodigoMovimiento.GrupoMab;
            var ejecucionPT = DaoProvider.GetDaoEjecucionMab().GetById(grupo.Enpt.Id);

            ValidacionFechaDesdeConAsignacion(listaDetalles, mab, ejecucionPT);

            return Mapper.Map<Mab, MabModel>(DaoMab.SaveOrUpdate(mab));
        }

        /// <summary>
        /// Buscar sucursal bancaria perteneciente a un agente
        /// </summary>
        /// <param name="idAgente">id del agente </param>
        /// <returns> Sucursal bancaria model </returns>
        public SucursalBancariaModel GetSucursalBancariaDeAgente(int idAgente)
        {
            var agente = GetAgenteDomainById(idAgente);
            return Mapper.Map<SucursalBanco, SucursalBancariaModel>(agente.Sucursal);
        }

        /// <summary>
        /// Buscar sucursal por Id
        /// </summary>
        /// <param name="id">id de la sucursal</param>
        /// <returns> Sucursal bancaria model </returns>
        public SucursalBancariaModel GetSucursalBancariaById(int id)
        {
            var sucursal = DaoProvider.GetDaoSucursalBancaria().GetById(id);
            return Mapper.Map<SucursalBanco, SucursalBancariaModel>(sucursal);
        }

        /// <summary>
        /// Buscar el agente reemplazado en base al puesto de trabajo
        /// </summary>
        /// <param name="idPuesto">id del puesto de trabajo</param>
        /// <returns> Agente model </returns>
        public AgenteModel GetAgenteReemplazado(int idPuesto)
        {
            //El agente a reemplazar sale de la última asignación, en estado inactiva que apunte al Puesto de trabajo actual
            var agenteReemplazado = DaoProvider.GetDaoPuestoDeTrabajo().GetAgenteUltimaAsignacionInactiva(idPuesto);
            return Mapper.Map<Agente, AgenteModel>(agenteReemplazado);
        }

        /// <summary>
        /// Busca un agente a partir de su id
        /// </summary>
        /// <param name="idAgente">id agente</param>
        /// <returns> Agente model </returns>
        public AgenteModel GetAgenteById(int idAgente)
        {
            return Mapper.Map<Agente, AgenteModel>(GetAgenteDomainById(idAgente));
        }

        /// <summary>
        /// Buscar un agente en base al puesto de trabajo y la empresa a la cual corresponde.
        /// </summary>
        /// <param name="idPuesto">id del puesto</param>
        /// <param name="idEmpresa">id de la empresa</param>
        /// <returns></returns>
        public AgenteModel GetAgentePuestoTrabajoSeleccionado(int idPuesto, int idEmpresa)
        {
            var agentePuestoSeleecionado = DaoProvider.GetDaoPuestoDeTrabajo().GetAgenteByPuestoDeTrabajo(idPuesto, idEmpresa);
            return Mapper.Map<Agente, AgenteModel>(agentePuestoSeleecionado);
        }

        /// <summary>
        /// Busca la empresa del usuario logueado
        /// </summary>
        /// <returns>empresa model</returns>
        public EmpresaModel GetEmpresaUsuarioLogueado()
        {
            var empresaRules = new EmpresaRules();
            return empresaRules.GetCurrentEmpresa();
        }

        /// <summary>
        /// Busca el tipo de novedad en base a su id
        /// </summary>
        /// <param name="idTipoNovedad">id del tipo de novedad</param>
        /// <returns>tipo de novedad model</returns>
        public TipoNovedadModel GetTipoNovedadById(int idTipoNovedad)
        {
            return Mapper.Map<TipoNovedad, TipoNovedadModel>(DaoProvider.GetDaoTipoNovedad().GetById(idTipoNovedad));
        }

        /// <summary>
        /// Busca puesto de trabajo a partir de su id
        /// </summary>
        /// <param name="idPuesto">id del puesto</param>
        /// <returns> puesto de trabajo model </returns>
        public PuestoDeTrabajoModel GetPuestoDeTrabajoById(int idPuesto)
        {
            return Mapper.Map<PuestoDeTrabajo, PuestoDeTrabajoModel>(GetPuestoDeTrabajoDomainById(idPuesto));
        }

        /// <summary>
        /// Busca una asignacion por id
        /// </summary>
        /// <param name="id">id asignacion</param>
        /// <returns>asignacion model</returns>
        public AsignacionModel GetAsignacionById(int id)
        {
            var asignacion = DaoProvider.GetDaoAsignacion().GetById(id);
            return Mapper.Map<Asignacion, AsignacionModel>(asignacion);
        }

        public SituacionDeRevistaModel GetUltimaSituacionRevistaByPuestoDeTrabajoAgenteModel(int idAgente, int idPuesto)
        {
            var asign = GetUltimaAsignacionByPuestoDeTrabajoAgente(idAgente, idPuesto);
            return Mapper.Map<SituacionDeRevista, SituacionDeRevistaModel>(asign.SituacionDeRevista);
        }

        /// <summary>
        /// Busca un mab a partir del tipo de novedad
        /// </summary>
        /// <param name="idTipoNovedad">id tipo de novedad</param>
        /// <returns>lista de mabs</returns>
        public List<MabModel> GetMabByTipoNovedad(int idTipoNovedad)
        {
            return Mapper.Map<List<Mab>, List<MabModel>>(DaoMab.GetByTipoNovedad(idTipoNovedad));
        }

        /// <summary>
        /// Busca un mab a partir de su codigo de barra
        /// </summary>
        /// <param name="codigo">codigo de barra</param>
        /// <returns>lista de mabs</returns>
        public List<MabModel> GetMabByFiltroCodigoBarra(string codigo)
        {
            var codigoPorAproximacion = string.IsNullOrEmpty(codigo) ? null : "%" + codigo + "%";
            var mab = DaoMab.GetByCodigoBarra(codigoPorAproximacion);

            return Mapper.Map<List<Mab>, List<MabModel>>(mab);
        }

        /// <summary>
        /// Busca un mab a partir de un agente y un rango de fechas
        /// </summary>
        /// <param name="idAgente">id del agente</param>
        /// <param name="fechaInicial">fecha inicial</param>
        /// <param name="fechaFinal">fecha final</param>
        /// <returns>lista de mabs</returns>
        public List<MabModel> GetMabByFiltroAgente(int idAgente, DateTime fechaInicial, DateTime fechaFinal)
        {
            return Mapper.Map<List<Mab>, List<MabModel>>(DaoMab.GetByAgente(idAgente, fechaInicial, fechaFinal));
        }

        /// <summary>
        /// Busca las sucursales bancarias
        /// </summary>
        /// <returns>lista de sucursales</returns>
        public List<SucursalBancariaModel> GetSucursalesBancarias()
        {
            return Mapper.Map<List<SucursalBanco>, List<SucursalBancariaModel>>(DaoProvider.GetDaoSucursalBancaria().GetAll());
        }

        /// <summary>
        /// Buscar las asignaciones en base al agente y al puesto de trabajo seleccionado
        /// </summary>
        /// <param name="idAgente">id del agente</param>
        /// <param name="idPuesto">id del puesto</param>
        /// <returns>asignacion</returns>
        public List<AsignacionModel> GetAsignacionesByPuestoYAgente(int idAgente, int idPuesto)
        {
            var asignacion = DaoProvider.GetDaoPuestoDeTrabajo().GetAsignacionesByPuestoDeTrabajoAgente(idAgente, idPuesto);
            return Mapper.Map<List<Asignacion>, List<AsignacionModel>>(asignacion);
        }

        public List<CodigoMovimientoMabModel> GetCodigosMovimientoByGrupoMabId(int idTipoNovedad)
        {
            TipoGrupoMabEnum grupoMab;

            switch (idTipoNovedad)
            {
                case (int)TipoNovedadEnum.ALTA:
                    grupoMab = TipoGrupoMabEnum.ALTA;
                    break;
                case (int)TipoNovedadEnum.MOVIMIENTO:
                    grupoMab = TipoGrupoMabEnum.MOVIMIENTO; //TODO: ver que este nombre debería cambiar
                    break;
                case (int)TipoNovedadEnum.AUSENTISMO:
                    grupoMab = TipoGrupoMabEnum.AUSENTISMO;
                    break;
                case (int)TipoNovedadEnum.BAJA:
                    grupoMab = TipoGrupoMabEnum.BAJA;
                    break;
                default:
                    grupoMab = 0;
                    break;
            }

            var codigoRules = ServiceLocator.Current.GetInstance<ICodigoMovimientoMabRules>();
            return codigoRules.GetCodigoMovimientoByTipoGrupoMab(grupoMab);
        }

        /// <summary>
        /// Verificar si el puesto de trabajo está relacionado con un plan de estudio
        /// </summary>
        /// <param name="idPuesto">id del puesto de trabajo</param>
        /// <returns> True si está relacionado con un Plan de estudio, False si no está relacionado</returns>
        public bool PuestoDeTrabajoHasPlanDeEstudioRelacionado(int idPuesto)
        {
            var puesto = GetPuestoDeTrabajoDomainById(idPuesto);
            return (!(puesto.UnidadesAcademicas == null || puesto.UnidadesAcademicas.Count == 0));
        }

        public bool GetValorParametroFechasMab()
        {
            return new EntidadesGeneralesRules().GetValorParametroBooleano(ParametroEnum.INGRESO_DE_FECHAS_EN_CONFECCIÓN_DE_MAB);
        }

        /// <summary>
        /// Cambia el estado de la asignacion del mab dependiendo de sus fechas.
        /// </summary>
        /// <param name="idMab">Id del mab a gestionar.</param>
        public void GestionarAsignacionPorMab(int idMab)
        {
            var mab = DaoMab.GetById(idMab);
            if (mab.FechaDesde.Value == DateTime.Now)
            {
                mab.Asignacion.Estado = DaoProvider.GetDaoEstadoAsignacion().GetByFilter(EstadoAsignacionEnum.INACTIVA);
                int cantidadDetalles = mab.Asignacion.DetalleAsignacion.Count(detalleAsignacion => detalleAsignacion.FechaDesde == mab.FechaDesde.Value && detalleAsignacion.FechaHasta == detalleAsignacion.FechaHasta.Value);
                //si no existen detalles creo uno nuevo y guardo la asignacion
                if (cantidadDetalles == 0)
                {
                    var detalleAsignacion = CrearDetalleAsignacion(mab, mab.Asignacion.Estado, TipoMovimientoDetalleAsignacionEnum.LICENCIA);
                    mab.Asignacion.DetalleAsignacion.Add(detalleAsignacion);
                    DaoProvider.GetDaoAsignacion().SaveOrUpdate(mab.Asignacion);
                }
            }
            else
            {
                if (mab.FechaHasta.Value == DateTime.Now)
                {
                    //si tiene una asignacion activa, cancelar la operación.
                    if (DaoProvider.GetDaoAsignacion().GetAsignacionesActivasByIdPuesto(mab.Asignacion.PuestoDeTrabajo.Id).Count > 0)
                        throw new ApplicationException("Ya hay un agente asignado para el pusto de trabajo seleccionado. Se deberá realizar un MAB de baja a ese agente para continuar con la operación.");
                    mab.Asignacion.Estado = DaoProvider.GetDaoEstadoAsignacion().GetByFilter(EstadoAsignacionEnum.ACTIVA);
                    var detalleAsignacion = CrearDetalleAsignacion(mab, mab.Asignacion.Estado, TipoMovimientoDetalleAsignacionEnum.RETORNO_A_ACTIVIDADES);
                    mab.Asignacion.DetalleAsignacion.Add(detalleAsignacion);
                    DaoProvider.GetDaoAsignacion().SaveOrUpdate(mab.Asignacion);
                }
            }
        }

        /// <summary>
        /// Crea un detalle de asignación, recibiendo el mab, el estado y el tipo de movimiento
        /// </summary>
        /// <param name="mab">entidad mab</param>
        /// <param name="estado">estado de la asignación</param>
        /// <param name="tipoMovimiento">tipo de movimiento del detalle</param>
        /// <returns></returns>
        private DetalleAsignacion CrearDetalleAsignacion(Mab mab, EstadoAsignacion estado, TipoMovimientoDetalleAsignacionEnum tipoMovimiento)
        {
            var detalleAsignacion = new DetalleAsignacion();
            detalleAsignacion.Estado = estado;
            detalleAsignacion.Asignacion = mab.Asignacion;
            detalleAsignacion.NroDetalle = DaoProvider.GetDaoDetalleAsignacion().GetNumeroDetalleCorrelativo(mab.Asignacion.Id);
            detalleAsignacion.TipoMovimiento = tipoMovimiento;
            detalleAsignacion.MAB = mab;
            detalleAsignacion.FechaDesde = estado.Valor == EstadoAsignacionEnum.ACTIVA ? DateTime.Now : mab.FechaDesde.Value;
            detalleAsignacion.FechaHasta = mab.FechaHasta;
            return detalleAsignacion;
        }

        /// <summary>
        /// Busca todas las asignaciones que pertenezcan a la empresa pasada por parámetro
        /// </summary>
        /// <param name="idEmpresa">id de la empresa</param>
        /// <returns>Lista de asignaciones</returns>
        public List<GestionAsignacionPorMabModel> GetAsignacionesByIdEmpresa(int idEmpresa)
        {
            var asignaciones = DaoProvider.GetDaoMab().GetByIdEmpresa(idEmpresa);
            return Mapper.Map<List<DtoGestionAsignacionPorMab>, List<GestionAsignacionPorMabModel>>(asignaciones);
        }

        #endregion
        /** Región para la declaración de métodos de validación y soporte en general */
        #region Soporte 

        private Asignacion GetUltimaAsignacionByPuestoDeTrabajoAgente(int idAgente, int idPuesto)
        {
            return DaoProvider.GetDaoPuestoDeTrabajo().GetUltimaAsignacionByPuestoDeTrabajoAgente(idAgente, idPuesto);
        }

        /** Método que carga el agente a reemplazar */
        private Asignacion CargarAgenteAReemplazar(MabModel model)
        {
            if (model.AsignacionAgenteReemplazado != null && model.AsignacionAgenteReemplazado.Id != null && model.AsignacionAgenteReemplazado.Id > 0)
            {
                //Traigo por método de regla
                var asig = GetAsignacionAgenteReemplazado(model.AsignacionAgente.PuestoDeTrabajo.Id);
                //Valido que sea el mismo que trae el model
                if (asig.Id == model.AsignacionAgenteReemplazado.Id)
                    return DaoProvider.GetDaoAsignacion().GetById(model.AsignacionAgenteReemplazado.Id);
                throw new BaseException("El agente a reemplazar seleccionado no es coincidente con el que actualmente se encuentra en la Base de Datos");
            }
            return null;
        }

        /** Método que verifica si se seleccionó una situación de revista y la devuelve, o informa el error */
        private SituacionDeRevista ValidarSituacionDeRevistaSeleccionada(int? idSituacion)
        {
            if (idSituacion == null || idSituacion <= 0)
                throw new BaseException("Debe seleccionarse la situación de revista del MAB");

            return DaoProvider.GetDaoSituacionDeRevista().GetById(idSituacion.Value);
        }

        /** Método que valida las fechas de novedad DESDE y HASTA del MAB, y si no se cargaron, asigna el agente que se encargará */
        private void ValidarFechas(Mab mab, MabModel model)
        {
            var empresaRules = new EmpresaRules();
            var calendario = new CalendarioRules();

            mab.FechaConfeccion = DateTime.Today;

            //Valido que la fecha hasta de novedad no sea menor a la fecha desde.
            if (model.FechaNovedadHasta < model.FechaNovedadDesde)
                throw new BaseException("La fecha hasta de novedad debe ser mayor o igual a la desde de novedad");

            //Valido que la fecha desde de novedad corresponda a un día habil
            if (model.FechaNovedadDesde != null)
            {
                var empresaUsuarioLogueado = empresaRules.GetCurrentEmpresa();

                if (empresaUsuarioLogueado.Domicilio == null || empresaUsuarioLogueado.Domicilio.IdLocalidad == null)
                    throw new BaseException("No se encontró domicilio para la empresa del usuario actual. Comuníquese con el administrador.");

                if (!(calendario.ValidarDiaHabil(model.FechaNovedadDesde.Value, empresaUsuarioLogueado.Domicilio.IdLocalidad)))
                    throw new BaseException("La fecha desde de novedad debe corresponder a un día hábil.");
            }

            mab.FechaDesde = model.FechaNovedadDesde;
            mab.FechaHasta = model.FechaNovedadHasta;
            // Valido que la fecha desde no sea mayor a la actual
            if (mab.FechaDesde != null && mab.FechaDesde > DateTime.Today)
                throw new BaseException("La fecha desde de novedad no debe ser mayor a la fecha actual");

            var tipoNovedad = GetTipoNovedadById(model.TipoNovedadId.Value);
            //De acuerdo al tipo de novedad, valido las fechas.
            switch (tipoNovedad.Tipo)
            {
                case "ALTA":
                    //Valido las fechas desde/hasta en relacion a la fecha desde/hasta del ultimo detalle de asignacion del agente reemplazado.
                    if (model.AsignacionAgenteReemplazado != null)
                    {
                        var idAgente = model.AsignacionAgenteReemplazado.Agente.Id;
                        var idPuestoTrabajo = model.AsignacionAgenteReemplazado.PuestoDeTrabajo.Id;
                        var fechaNovedadHasta = mab.FechaHasta;
                        var ultimoDetalle = DaoProvider.GetDaoAsignacion().GetUltimoDetalleAsignacion(idAgente, idPuestoTrabajo, fechaNovedadHasta);

                        if (ultimoDetalle != null)
                        {
                            //Valido que la fecha desde sea mayor a la fecha desde del último detalle de asignacion del agente reemplazado.
                            if (mab.FechaDesde < ultimoDetalle.FechaDesde)
                                throw new BaseException("La fecha desde de novedad debe ser una fecha mayor a " +
                                                               ultimoDetalle.FechaDesde);
                            //Valido que la fecha hasta sea mayor a la fecha hasta del último detalle de asignacion del agente reemplazado.
                            if (mab.FechaHasta != null && mab.FechaHasta > ultimoDetalle.FechaHasta)
                                throw new BaseException("La fecha hasta de novedad debe ser una fecha menor o igual a " + ultimoDetalle.FechaHasta);
                        }
                    }

                    if (mab.FechaDesde == null) // si la fechaDesde es null
                    {
                        if (GetValorParametroFechasMab())
                            throw new BaseException("Las fecha desde de novedad es obligatoria por parámetro de la aplicación");

                        if (model.DeseaImprimir)
                            throw new BaseException("No puede imprimirse el mab si no se cargó la fecha desde de novedad");
                    }
                    break;
                case "AUSENTISMO":
                case "MOVIMIENTO":
                    ValidacionesComunesDeFechasEntreMabs(mab, model);
                    break;
                case "BAJA":
                    ValidacionesComunesDeFechasEntreMabs(mab, model);
                    if (mab.FechaHasta.HasValue)
                        throw new BaseException("No se debe cargar la fecha hasta de novedad.");
                    break;
                default:
                    break;
            }
        }

        private void ValidacionesComunesDeFechasEntreMabs(Mab mab, MabModel model)
        {
            if (mab.FechaDesde != null)
            {
                if (model.AsignacionAgente != null && mab.FechaDesde < mab.Asignacion.FechaInicioEnPuesto)
                {
                    throw new BaseException("La fecha desde de novedad debe ser mayor o igual a " + mab.Asignacion.FechaInicioEnPuesto + " que es la fecha de inicio en puesto");
                }
            }
            else
            {
                throw new BaseException("La fecha desde de novedad es obligatoria");
            }
        }

        /** Método que carga el instrumento legal o acto administrativo */
        private AsignacionInstrumentoLegal CargarInstrumentoLegal(MabModel mab, bool cargarInstrumentoLegal)
        {
            AsignacionInstrumentoLegal asignacionIL = null;
            TipoMovimientoInstrumentoLegal tipoMovimientoInstrumento = null;
            TipoInstrumentoLegal tipoInstrumentoLegal = null;

            if (cargarInstrumentoLegal)
            {
                if (mab.AsignacionInstrumentoLegalAgente != null && mab.AsignacionInstrumentoLegalAgente.InstrumentoLegal != null)
                {
                    var tipoNovedad = (TipoNovedadEnum)mab.TipoNovedadId;
                    if (TipoMovimientoInstrumentoLegalPorTipoNovedad(tipoNovedad) != null)
                    {
                        tipoMovimientoInstrumento = TipoMovimientoInstrumentoLegalPorTipoNovedad(tipoNovedad);
                    }
                    asignacionIL =
                        Mapper.Map<AsignacionInstrumentoLegalModel, AsignacionInstrumentoLegal>(
                            mab.AsignacionInstrumentoLegalAgente);
                    asignacionIL.Empresa = Mapper.Map<EmpresaModel, EmpresaBase>(GetEmpresaUsuarioLogueado());
                    asignacionIL.TipoMovimientoInstrumentoLegal = tipoMovimientoInstrumento;
                    ReglaAsignacion.AsignacionInstrumentoLegalSave(asignacionIL, mab.AsignacionInstrumentoLegalAgente);
                }
            }

            return asignacionIL;
        }

        /// <summary>
        /// De acuerdo al tipo de novedad retorna el tipo de movimiento de instrumento legal.
        /// </summary>
        /// <param name="tipoNovedad">Tipo de novedad</param>
        /// <returns>TipoMovimientoInstrumentoLegal</returns>
        private TipoMovimientoInstrumentoLegal TipoMovimientoInstrumentoLegalPorTipoNovedad(TipoNovedadEnum tipoNovedad)
        {
            switch (tipoNovedad)
            {
                case TipoNovedadEnum.ALTA:
                    return
                        DaoProvider.GetDaoTipoMovimientoInstrumentoLegal().GetById(
                            (int) TipoMovimientoInstrumentoLegalEnum.MAB_ALTA);
                case TipoNovedadEnum.MOVIMIENTO:
                    return
                        DaoProvider.GetDaoTipoMovimientoInstrumentoLegal().GetById(
                            (int) TipoMovimientoInstrumentoLegalEnum.MAB_MOVIMIENTO);
                case TipoNovedadEnum.BAJA:
                    return
                        DaoProvider.GetDaoTipoMovimientoInstrumentoLegal().GetById(
                            (int) TipoMovimientoInstrumentoLegalEnum.MAB_BAJA);
                case TipoNovedadEnum.AUSENTISMO:
                    return
                        DaoProvider.GetDaoTipoMovimientoInstrumentoLegal().GetById(
                            (int) TipoMovimientoInstrumentoLegalEnum.MAB_AUSENTISMO);
                default:
                    return null;
            }
        }

        /** Método que carga el código de movimiento de MAB */
        private CodigoMovimientoMab CargarCodigoMovimientoMab(int? idCod, Mab mab)
        {
            if (idCod == null || idCod <= 0)
                throw new BaseException("Debe seleccionarse código de novedad del MAB");

            var cod = DaoProvider.GetDaoCodigoMovimientoMab().GetById(idCod.Value);
            //Valido que el estado del puesto de trabajo coincida con alguno de la ejecución MAB
            var estadoPuesto = DaoProvider.GetDaoEstadoPuestoTrabajo().GetByFilter(mab.PuestoDeTrabajo.Estado.Valor);

            if (cod.GrupoMab.Enpt.EstadosAnterioresPt != null && cod.GrupoMab.Enpt.EstadosAnterioresPt.Count > 0 && !cod.GrupoMab.Enpt.EstadosAnterioresPt.Contains(estadoPuesto))
                throw new BaseException("El estado del puesto de trabajo no coincide con el estado previo del puesto de trabajo del grupo de código de movimiento");

            return cod;
        }

        /** Método que asigna la sucursal bancaria al agente, si este no tiene */
        private void AsociarSucursalBancariaAAgente(Agente agente, SucursalBancariaModel sucModel)
        {
            // Si no posee sucursal bancaria
            if (agente.Sucursal == null)
            {
                if (sucModel == null || sucModel.Id <= 0)
                    throw new BaseException("Debe seleccionarse o ingresarse el código de una sucursal bancaria válida");

                var suc = DaoProvider.GetDaoSucursalBancaria().GetById(sucModel.Id);

                if (suc != null)
                    agente.Sucursal = suc;
                else
                    throw new BaseException("El código de una sucursal bancaria no es válido");
            }
        }

        /** Método que carga los datos de empresa y cargo anterior */
        private void CargarDatosEmpresaYCargoAnterior(MabModel model, Mab mab)
        {
            // Si se elige registrar datos de cargo anterior
            if (model.RegistrarCargoAnterior)
            {
                if (model.EsCargoDeEmpresaMinisterio) // Si es cargo del ministerio
                {
                    if (model.IdPuestoAnteriorDeMinisterio != null && model.IdPuestoAnteriorDeMinisterio > 0)
                    {
                        //Traigo la asignación del puesto de trabajo anterior más reciente
                        mab.PuestoDeTrabajoAnterior = DaoProvider.GetDaoPuestoDeTrabajo().GetAsignacionRecienteByPuestoDeTrabajoYAgente(model.IdPuestoAnteriorDeMinisterio.Value,model.AsignacionAgente.Agente.Id);
                        if (mab.PuestoDeTrabajoAnterior == null)
                            throw new BaseException("No se encontró asignación para el puesto de trabajo anterior para el agente seleccionado");
                    }
                    else
                    {
                        throw new BaseException("No se seleccionó un puesto de trabajo anterior válido");
                    }
                }
                else // Si no es cargo del ministerio cargo las observaciones y los datos de cargo anterior en las observaciones
                {
                    mab.Observaciones = model.ObservacionesMab + " | " + model.ObservacionesCargoAnterior;
                }
            }
        }

        /** Método que genera el código de barra para el MAB */
        private string GenerarCodigoBarraMab()
        {
            return "" + (DaoProvider.GetDaoMab().GetUltimoNumeroMab() + 1);
        }

        private int GetIdTipoMovimientoLegal(int idTipoNovedadMab)
        {
            var tipo = (TipoNovedadEnum)idTipoNovedadMab;
            TipoMovimientoInstrumentoLegalEnum functionValueReturn;

            switch (tipo)
            {
                case TipoNovedadEnum.ALTA:
                    functionValueReturn = TipoMovimientoInstrumentoLegalEnum.MAB_ALTA;
                    break;
                case TipoNovedadEnum.BAJA:
                    functionValueReturn = TipoMovimientoInstrumentoLegalEnum.MAB_BAJA;
                    break;
                case TipoNovedadEnum.MOVIMIENTO:
                    functionValueReturn = TipoMovimientoInstrumentoLegalEnum.MAB_MOVIMIENTO;
                    break;
                case TipoNovedadEnum.AUSENTISMO:
                    functionValueReturn = TipoMovimientoInstrumentoLegalEnum.MAB_AUSENTISMO;
                    break;
                default:
                    return 0;
            }

            return (int)functionValueReturn;
        }

        /** Método que se encarga de hacer las validaciones y asignaciones finales necesarias para el MAB*/
        private List<DetalleAsignacion> ProcesarMab(Mab mab, MabModel model,EjecucionMab ejecucionMab)
        {
            var tipo = (TipoNovedadEnum)mab.TipoNovedad.Id;
            List<DetalleAsignacion> lista = null;

            switch (tipo)
            {
                case TipoNovedadEnum.ALTA:
                    lista = ProcesarMabAlta(mab, model,ejecucionMab);
                    break;
                case TipoNovedadEnum.BAJA:
                    lista = ProcesarMabBaja(mab, model, ejecucionMab);
                    break;
                case TipoNovedadEnum.MOVIMIENTO:
                    lista = ProcesarMabMovimiento(mab, model, ejecucionMab);
                    break;
                case TipoNovedadEnum.AUSENTISMO:
                    lista = ProcesarMabAusentismo(mab, model, ejecucionMab);
                    break;
                default:
                    break;
            }
            return lista;
        }

        private List<DetalleAsignacion> ProcesarMabMovimiento(Mab mab, MabModel model, EjecucionMab ejecucionMab)
        {
            var listaDetalles = new List<DetalleAsignacion>();
            //TODO: Falta agregar lo de puesto de trabajo provisorio
            if (model.PuestoTrabajoActualMovimiento)
            {
                var ultimoDetalleAsignacion = DaoProvider.GetDaoDetalleAsignacion().GetUltimoDetalleAsignacion(mab.Asignacion.Id);
                
                if (ultimoDetalleAsignacion != null)
                {
                    ultimoDetalleAsignacion.FechaHasta = mab.FechaDesde;
                    listaDetalles.Add(ultimoDetalleAsignacion);

                    var detalleAsignacion = new DetalleAsignacion();
                    detalleAsignacion.Estado = ejecucionMab.EstadoAsignacion;
                    detalleAsignacion.MAB = mab;
                    detalleAsignacion.Asignacion = mab.Asignacion;
                    detalleAsignacion.NroDetalle = DaoProvider.GetDaoDetalleAsignacion().GetNumeroDetalleCorrelativo(mab.Asignacion.Id);
                    detalleAsignacion.FechaDesde = mab.FechaDesde.Value;
                    detalleAsignacion.FechaHasta = mab.FechaDesde;

                    listaDetalles.Add(detalleAsignacion);
                }
            }
            else // Si es nuevo puesto de trabajo
            {
                //Datos comunes de asignación
                mab.Asignacion.Codigo = DaoProvider.GetDaoAsignacion().GenerarCodigoAsignacion();
                mab.Asignacion.FechaInicioEnPuesto = mab.FechaDesde.Value;
                //TODO: revisar esta asignación
                mab.Asignacion.AsignacionRelacionada = mab.AsignacionAgenteOrigen;

                var detalleAsignacion = new DetalleAsignacion();
                detalleAsignacion.Estado = ejecucionMab.EstadoAsignacion;
                mab.Asignacion.SituacionDeRevista = mab.SituacionDeRevista;
                mab.Asignacion.Estado = DaoProvider.GetDaoEstadoAsignacion().GetByFilter(EstadoAsignacionEnum.ACTIVA);
                //TODO: generar este núm
                var ran2 = new Random(1);
                detalleAsignacion.NroDetalle = (int)(ran2.NextDouble() * 10000); ;
                detalleAsignacion.FechaDesde = mab.FechaDesde.Value;
                //TODO: validar esto
                if (mab.FechaHasta != null)
                {
                    detalleAsignacion.FechaHasta = mab.FechaHasta.Value;
                }
                //TODO: tipoMovimiento?? detalleAsignacion.
                //TODO: validar esta asignación
                detalleAsignacion.AsignacionProvisoriaOPrevia = GetUltimaAsignacionByPuestoDeTrabajoAgente(model.AsignacionAgente.Agente.Id, model.AsignacionAgente.PuestoDeTrabajo.Id);
                detalleAsignacion.MAB = mab;
                detalleAsignacion.Asignacion = mab.Asignacion;
                listaDetalles.Add(detalleAsignacion);
            }

            return listaDetalles;
        }

        private List<DetalleAsignacion> ProcesarMabBaja(Mab mab, MabModel model, EjecucionMab ejecucionMab)
        {
            var listaDetalles = new List<DetalleAsignacion>();

            if (mab.Asignacion.Estado.Valor == EstadoAsignacionEnum.INACTIVA)
            {
                throw new BaseException("La asignación del agente al puesto se encuentra INACTIVA, no se puede dar de baja");
            }
            
            mab.Asignacion.FechaFinEnPuesto = mab.FechaDesde;
            mab.Asignacion.Estado = DaoProvider.GetDaoEstadoAsignacion().GetByFilter(EstadoAsignacionEnum.CERRADA);
            mab.PuestoDeTrabajo.Estado = DaoProvider.GetDaoEstadoPuestoTrabajo().GetByFilter(EstadoPuestoDeTrabajoEnum.ACTIVO);
            var ultimoDetalleAsignacion = DaoProvider.GetDaoDetalleAsignacion().GetUltimoDetalleAsignacion(mab.Asignacion.Id);

            if (ultimoDetalleAsignacion != null)
            {
                ultimoDetalleAsignacion.FechaHasta = mab.FechaDesde;
                listaDetalles.Add(ultimoDetalleAsignacion);
            }
            
            var detalleAsignacion = new DetalleAsignacion();
            detalleAsignacion.Estado = ejecucionMab.EstadoAsignacion;
            detalleAsignacion.TipoMovimiento = TipoMovimientoDetalleAsignacionEnum.MAB;
            detalleAsignacion.MAB = mab;
            detalleAsignacion.Asignacion = mab.Asignacion;
            //TODO: generar este núm
            var ran = new Random(1);
            detalleAsignacion.NroDetalle = (int)(ran.NextDouble() * 10000);
            detalleAsignacion.FechaDesde = mab.FechaDesde.Value;
            detalleAsignacion.FechaHasta = mab.FechaDesde;

            listaDetalles.Add(detalleAsignacion);

            return listaDetalles;
        }

        private List<DetalleAsignacion> ProcesarMabAusentismo(Mab mab, MabModel model, EjecucionMab ejecucionMab)
        {
            var listaDetalles = new List<DetalleAsignacion>();
            if (mab.FechaDesde <= DateTime.Now) // Si la fecha es menor o igual a la actual modifico el estado de la asignación
            {
                mab.Asignacion.Estado = DaoProvider.GetDaoEstadoAsignacion().GetByFilter(EstadoAsignacionEnum.INACTIVA);
                var detalleAsignacion = new DetalleAsignacion();
                detalleAsignacion.Estado = DaoProvider.GetDaoEstadoAsignacion().GetByFilter(EstadoAsignacionEnum.INACTIVA);
                detalleAsignacion.Asignacion = mab.Asignacion;
                detalleAsignacion.NroDetalle = DaoProvider.GetDaoDetalleAsignacion().GetNumeroDetalleCorrelativo(mab.Asignacion.Id);
                detalleAsignacion.FechaDesde = mab.FechaDesde.Value;
                detalleAsignacion.FechaHasta = mab.FechaHasta;
                detalleAsignacion.TipoMovimiento = TipoMovimientoDetalleAsignacionEnum.LICENCIA;
                detalleAsignacion.MAB = mab;
                listaDetalles.Add(detalleAsignacion);
            }
            else // Si la fecha es mayor a la actual
            {
                //TODO falta implementar   
            }
            return listaDetalles;
        }

        //TODO validar si tiene o no tipo de retorno "private List<DetalleAsignacion> ValidacionFechaDesdeConAsignacion(List<DetalleAsignacion> listaDetalles, Mab mab)"
        private void ValidacionFechaDesdeConAsignacion(List<DetalleAsignacion> listaDetalles, Mab mab, EjecucionMab ejecucionMab)
        {
            //Si existe fecha_desde
            if (mab.FechaDesde != null)
            {
                //Datos comunes de asignación
                mab.Asignacion.Codigo = DaoProvider.GetDaoAsignacion().GenerarCodigoAsignacion(); ;
                mab.Asignacion.FechaInicioEnPuesto = mab.FechaDesde.Value;
                var detalleAsignacion = new DetalleAsignacion();
                detalleAsignacion.Estado = ejecucionMab.EstadoAsignacion;
                mab.Asignacion.SituacionDeRevista = mab.SituacionDeRevista;
                mab.Asignacion.Estado = DaoProvider.GetDaoEstadoAsignacion().GetByFilter(EstadoAsignacionEnum.ACTIVA);
                detalleAsignacion.NroDetalle = DaoProvider.GetDaoDetalleAsignacion().GetNumeroDetalleCorrelativo(mab.Asignacion.Id);
                detalleAsignacion.FechaDesde = mab.FechaDesde.Value;
                //TODO: Ver que estado setearle.
                detalleAsignacion.Estado = DaoProvider.GetDaoEstadoAsignacion().GetByFilter(EstadoAsignacionEnum.ACTIVA);
                detalleAsignacion.TipoMovimiento = TipoMovimientoDetalleAsignacionEnum.MAB;
                //TODO: Falta ver que tipo de movimiento asignarle al detalle de asignacion.

                // Y situación de revista = Titular o Interino
                if (mab.SituacionDeRevista.Nombre == "TITULAR" || mab.SituacionDeRevista.Nombre == "INTERINO")
                {
                    //Aparentemente esta validación no operaría fuera de los datos comunes
                }
                // Y situación de revista = Suplente o Precario
                if (mab.SituacionDeRevista.Nombre == "SUPLENTE" || mab.SituacionDeRevista.Nombre == "PRECARIO")
                {
                    mab.Asignacion.FechaFinEnPuesto = mab.FechaHasta;
                    mab.Asignacion.AsignacionRelacionada = mab.AsignacionAgenteOrigen;
                    detalleAsignacion.FechaHasta = mab.FechaHasta;
                }
                detalleAsignacion.MAB = mab;
                detalleAsignacion.Asignacion = mab.Asignacion;
                listaDetalles.Add(detalleAsignacion);
            }
        }

        /** Proceso el MAB de alta */
        private List<DetalleAsignacion> ProcesarMabAlta(Mab mab, MabModel model, EjecucionMab ejecucionMab)
        {
            var listaDetalles = new List<DetalleAsignacion>();

            if (PuestoTrabajoPoseeAsignacionPendienteDeMab(mab.Asignacion.PuestoDeTrabajo.Id))
            {
                //TODO: NO ENTRA EN FASE 2
            }
            else
            {
                ValidacionFechaDesdeConAsignacion(listaDetalles, mab, ejecucionMab);
                mab.PuestoDeTrabajo.Estado = DaoProvider.GetDaoEstadoPuestoTrabajo().GetByFilter(EstadoPuestoDeTrabajoEnum.ACTIVO);
                mab.PuestoDeTrabajo.Liquidado = true;
                //TODO: ver cómo conseguir esto - Viernes 8/7 Karina me dice que quizás lo sacan
                //mab.PuestoDeTrabajo.TipoPuesto = DaoProvider.GetDaoTipoPuesto().GetById((int)TipoPuestoEnum.ASIGNADO);
            }

            return listaDetalles;
        }

        /** Método que manda a persistir todas las entidades de MABs a base de datos */
        private void PersistirEntidades(Mab mab, List<DetalleAsignacion> listDetalles)
        {
            DaoProvider.GetDaoAgente().SaveOrUpdate(mab.AgenteMab);

            foreach (var detalleAsignacion in listDetalles)
            {
                detalleAsignacion.Asignacion = mab.Asignacion;
                detalleAsignacion.TipoMovimiento = TipoMovimientoDetalleAsignacionEnum.MAB;
            }
                    
            //INSTRUMENTO LEGAL, regla
            //if (mab.ActoAdministrativo != null)
            //    new AsignacionInstrumentoLegalRules().AsignacionInstrumentoLegalSave(mab.ActoAdministrativo, null);
            mab.Asignacion.DetalleAsignacion = listDetalles;
            DaoProvider.GetDaoAsignacion().SaveOrUpdate(mab.Asignacion);

            if (mab.AsignacionAgenteOrigen != null)
                DaoProvider.GetDaoAsignacion().SaveOrUpdate(mab.AsignacionAgenteOrigen);
            
            DaoProvider.GetDaoMab().SaveOrUpdate(mab);

            DaoProvider.GetDaoPuestoDeTrabajo().SaveOrUpdate(mab.PuestoDeTrabajo);
        }

        /** Método que verifica el estado de todas las asignaciones de un puesto de trabajo y devuelve true si es PENDIENTE DE MAB, false en otros casos */
        private bool PuestoTrabajoPoseeAsignacionPendienteDeMab(int idPuesto)
        {
            var ret = DaoProvider.GetDaoPuestoDeTrabajo().GetAsignacionesDePuestoTrabajoByEstado(idPuesto, EstadoAsignacionEnum.PENDIENTE_DE_MAB);
            return !(ret == null || ret.Count == 0);
        }
        
        /// <summary>
        /// Buscar la asignacion del agente reemplazado a partir del id del puesto de trabajo
        /// </summary>
        /// <param name="idPuesto">id del puesto de trabajo</param>
        /// <returns> Asignacion </returns>
        private Asignacion GetAsignacionAgenteReemplazado(int idPuesto)
        {
            //El agente a reemplazar sale de la última asignación, en estado inactiva que apunte al Puesto de trabajo actual
            return DaoProvider.GetDaoPuestoDeTrabajo().GetAsignacionAgenteUltimaAsignacionInactiva(idPuesto);
        }

        //TODO no se llama de ningun lado
        private InstrumentoLegalModel GetInstrumentoLegalAgenteReemplazado(int idPuesto)
        {
            var asignacion = DaoProvider.GetDaoPuestoDeTrabajo().GetAsignacionAgenteUltimaAsignacionInactiva(idPuesto);
            return null;
        }
               
        /// <summary>
        /// Método que realiza la ejecución del mab y setea los estados según corresponda su código y grupo
        /// </summary>
        /// <param name="mab">referencia al mab</param>
        /// <returns> mab </returns>
        private Mab EjecutarMab(Mab mab, EjecucionMab ejecucion)
        {
            var grupo = mab.CodigoMovimiento.GrupoMab;
            var ejecucionPT = ejecucion;
            var ejecucionPTAnterior = DaoProvider.GetDaoEjecucionMab().GetById(grupo.EnPTanterior.Id);

            if (ejecucionPT.EstadoPosteriorPt != null)
                mab.PuestoDeTrabajo.Estado = ejecucionPT.EstadoPosteriorPt;

            mab.PuestoDeTrabajo.Liquidado = ejecucionPT.Liquidacion;
            
            if (ejecucionPT.EstadoAsignacion != null)
                mab.Asignacion.Estado = ejecucionPT.EstadoAsignacion;
            //genera vacante
            //modifica situación de revista
            if (mab.PuestoDeTrabajoAnterior != null)
            {
                if (ejecucionPTAnterior.EstadoPosteriorPt != null)
                    mab.PuestoDeTrabajoAnterior.PuestoDeTrabajo.Estado = ejecucionPTAnterior.EstadoPosteriorPt;
            
                mab.PuestoDeTrabajoAnterior.PuestoDeTrabajo.Liquidado = ejecucionPTAnterior.Liquidacion;
                
                if (ejecucionPTAnterior.EstadoAsignacion != null)
                    mab.PuestoDeTrabajoAnterior.Estado = ejecucionPTAnterior.EstadoAsignacion;
                //genera vacante
                //modifica situación de revista
            }
            return mab;
        }

        /// <summary>
        /// Busca la entidad del agente a partir de su id
        /// </summary>
        /// <param name="idAgente">id agente</param>
        /// <returns>entidad agente</returns>
        private Agente GetAgenteDomainById(int idAgente)
        {
            return DaoProvider.GetDaoAgente().GetById(idAgente);
        }

        /// <summary>
        /// Buscar un puesto de trabajo a partir de su id
        /// </summary>
        /// <param name="idPuesto">id del puesto</param>
        /// <returns> entidad de puesto de trabajo </returns>
        private PuestoDeTrabajo GetPuestoDeTrabajoDomainById(int idPuesto)
        {
            return DaoProvider.GetDaoPuestoDeTrabajo().GetById(idPuesto);
        }

        /** Método que valida los datos requeridos de mab */
        private void ValidarDatosRequeridos(MabModel model)
        {
            //Valido la asignación
            if (model.AsignacionAgente == null)
                throw new BaseException("No se cargó correctamente la asignación");

            if (model.AsignacionAgente.Agente == null || model.AsignacionAgente.Agente.Id <= 0)
                throw new BaseException("No se seleccionó el agente al cual se desea realizar el MAB");

            if (model.AsignacionAgente.PuestoDeTrabajo == null || model.AsignacionAgente.PuestoDeTrabajo.Id <= 0)
                throw new BaseException("No se seleccionó el puesto de trabajo al cual se desea realizar el MAB");

            if (model.TipoNovedadId.Value == (int) TipoNovedadEnum.ALTA)
            {
                var situacion = DaoProvider.GetDaoSituacionDeRevista().GetById(model.SituacionRevistaId.Value).Nombre;
                switch (situacion)
                {
                    case "TITULAR":
                        if (model.FechaNovedadDesde == null)
                            throw new ApplicationException("La fecha desde de novedad es requerida.");
                        break;

                    case "SUPLENTE":
                        if (model.FechaNovedadDesde == null)
                            throw new ApplicationException("La fecha desde de novedad es requeridoa.");
                        if (model.FechaNovedadHasta == null)
                            throw new ApplicationException("La fecha hasta de novedad es requeridoa.");
                        break;

                    default:
                        break;
                }
            }
            if (model.TipoNovedadId.Value == (int)TipoNovedadEnum.AUSENTISMO)
            {
                var codigoMovimiento = DaoProvider.GetDaoCodigoMovimientoMab().GetById(model.CodigoDeNovedadId);
                if (codigoMovimiento != null && codigoMovimiento.Codigo == "99" && model.FechaNovedadHasta.HasValue)
                    throw new ApplicationException(
                        "No se puede cargar la fecha hasta de novedad para el codigo de novedad seleccionado");
            }
        }

        /** Método que valida el estado del puesto de trabajo seleccionado de acuerdo al tipo de novedad */
        private void ValidarEstadoPuestoDeTrabajo(Mab mab, PuestoDeTrabajo puesto, MabModel model)
        {
            if (puesto.Estado != null)
            {
                switch (mab.TipoNovedad.Tipo)
                {
                    case "ALTA":
                        if (puesto.Estado.Valor != EstadoPuestoDeTrabajoEnum.VACANTE)
                        {
                            if (puesto.Estado.Valor != EstadoPuestoDeTrabajoEnum.ACTIVO || puesto.Estado.Valor != EstadoPuestoDeTrabajoEnum.RETENIDO)
                                throw new BaseException("El puesto de trabajo no está libre para asignar al agente.");

                            var asignaciones =
                                DaoProvider.GetDaoAsignacion().GetAsignacionesByIdPuesto(
                                    model.AsignacionAgente.PuestoDeTrabajo.Id);
                            foreach (var asignacion in asignaciones)
                            {
                                if (asignacion.Estado.Valor == EstadoAsignacionEnum.ACTIVA)
                                    throw new ApplicationException(
                                        "El puesto posee una asignación activa y no puede asignarse el agente");
                                break;
                            }
                        }

                        mab.PuestoDeTrabajo = puesto;
                        break;
                    case "AUSENTISMO":
                        if (puesto.Estado.Valor != EstadoPuestoDeTrabajoEnum.ACTIVO)
                            throw new BaseException("El puesto de trabajo seleccionado no se encuentra en estado activo");
                        mab.PuestoDeTrabajo = puesto;
                        break;
                    case "BAJA":
                        //TODO: ver esto si hace falta alguna validación
                        mab.PuestoDeTrabajo = puesto;
                        break;
                    case "MOVIMIENTO":
                        if (!model.PuestoTrabajoActualMovimiento) //Si se eligió un puesto nuevo
                        {
                            if (!model.RegistrarCargoAnterior)
                                throw new BaseException("Para la opción nuevo puesto de trabajo se requieren los datos de cargo anterior");

                            if (puesto.Estado.Valor != EstadoPuestoDeTrabajoEnum.VACANTE && puesto.Estado.Valor != EstadoPuestoDeTrabajoEnum.ACTIVO &&
                                puesto.Estado.Valor != EstadoPuestoDeTrabajoEnum.RETENIDO)
                                throw new BaseException("El puesto de trabajo no está libre para asignar al agente.");

                            if (DaoProvider.GetDaoPuestoDeTrabajo().GetAsignacionesDePuestoTrabajoByEstado(puesto.Id,EstadoAsignacionEnum.ACTIVA).Count > 0)
                                throw new BaseException("El puesto de trabajo no está libre para asignar al agente.");
                        }
                        else if (puesto.Estado.Valor == EstadoPuestoDeTrabajoEnum.VACANTE) //Si se eligió puesto de trabajo actual                            
                            throw new BaseException("No puede seleccionarse un puesto de trabajo actual en estado vacante");

                        mab.PuestoDeTrabajo = puesto;
                        break;
                    default:
                        break;
                }
            }
            else
                throw new BaseException("El puesto de trabajo no posee un estado válido");
        }

        /** Método que carga el tipo de novedad del mab*/
        private TipoNovedad CargarTipoNovedad(int? idTipo)
        {
            if (idTipo == null || idTipo <= 0)
                throw new BaseException("Debe seleccionarse el tipo de novedad del MAB");

            return DaoProvider.GetDaoTipoNovedad().GetById(idTipo.Value);
        }

        private ModalidadMab ValidarRelacionPuestoTrabajoConPlanEstudio(PuestoDeTrabajo puesto, MabModel model)
        {
            //Si el puesto está relacionado
            if (PuestoDeTrabajoHasPlanDeEstudioRelacionado(puesto.Id) && (model.ModalidadId == null || model.ModalidadId <= 0))
                throw new BaseException("Debe seleccionarse la modalidad del MAB por estar relacionado el puesto de trabajo con un plan de estudio");

            if (model.ModalidadId != null && model.ModalidadId > 0)
                return DaoProvider.GetDaoModalidadMab().GetById(model.ModalidadId.Value);
            return null;
        }

        #endregion
    }
}