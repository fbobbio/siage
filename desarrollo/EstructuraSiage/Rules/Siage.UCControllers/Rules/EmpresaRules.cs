using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Siage.Core.DaoInterfaces;
using Siage.Core.Domain;
using AutoMapper;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Siage.Base;
using Siage.Base.Dto;

namespace Siage.UCControllers.Rules
{
    public class EmpresaRules : IEmpresaRules
    {
        #region Atributos

        private IDaoProvider _daoProvider;
        private IDaoEmpresaBase _daoEmpresa;
        private IDaoEscuela _daoEscuela;
        private IDaoEscuelaAnexo _daoEscuelaAnexo;
        private IDaoInspeccion _daoInspeccion;
        private IDaoDireccionDeNivel _daoDireccionDeNivel;
        private IDaoLocalidad _daoLocalidad;
        private AsignacionInstrumentoLegalRules _reglaAsignacion;
        private InstrumentoLegalRules _reglaInstrumento;
        private IDaoPersonaFisica _daoPersonaFisica;

        #endregion

        #region Propiedades

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

        private InstrumentoLegalRules ReglaInstrumentoLegal
        {
            get
            {
                if(_reglaInstrumento == null)
                {
                    _reglaInstrumento = new InstrumentoLegalRules();
                }
                return _reglaInstrumento;
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

        private IDaoEmpresaBase DaoEmpresa
        {
            get
            {
                if (_daoEmpresa == null)
                {
                    _daoEmpresa = DaoProvider.GetDaoEmpresaBase();
                }
                return _daoEmpresa;
            }
        }

        public IDaoEscuela DaoEscuela
        {
            get
            {
                if (_daoEscuela == null)
                {
                    _daoEscuela = DaoProvider.GetDaoEscuela();
                }
                return _daoEscuela;
            }
        }

        public IDaoEscuelaAnexo DaoEscuelaAnexo
        {
            get
            {
                if (_daoEscuelaAnexo == null)
                {
                    _daoEscuelaAnexo = DaoProvider.GetDaoEscuelaAnexo();
                }
                return _daoEscuelaAnexo;
            }
        }

        public IDaoInspeccion DaoInspeccion
        {
            get
            {
                if (_daoInspeccion == null)
                {
                    _daoInspeccion = DaoProvider.GetDaoInspeccion();
                }
                return _daoInspeccion;
            }
        }

        public IDaoDireccionDeNivel DaoDireccionDeNivel
        {
            get
            {
                if (_daoDireccionDeNivel == null)
                {
                    _daoDireccionDeNivel = DaoProvider.GetDaoDireccionDeNivel();
                }
                return _daoDireccionDeNivel;
            }
        }

        private IDaoLocalidad DaoLocalidad
        {
            get
            {
                if (_daoLocalidad == null)
                {
                    _daoLocalidad = DaoProvider.GetDaoLocalidad();
                }
                return _daoLocalidad;
            }
        }
        private IDaoPersonaFisica DaoPersonaFisica
        {
            get
            {
                if (_daoPersonaFisica == null)
                {
                    _daoPersonaFisica = DaoProvider.GetDaoPersonaFisica();
                }
                return _daoPersonaFisica;
            }
        }

        #endregion

        #region IEmpresaRules members

        public string GetCodigoEmpresaById(int id)
        {
            var empresa = DaoEmpresa.GetById(id);
            return empresa.CodigoEmpresa;
        }

        public CallesPredioNombresModel GetCallesPredioByEdificioId(int idEdificio)
        {
            return
                Mapper.Map<DtoCallesPredioNombres, CallesPredioNombresModel>(
                    DaoProvider.GetDaoPredio().GetCallesPredioByEdificioId(idEdificio));
        }


        public List<EmpresaInspeccionadaPorInspeccionModel> GetEmpresasInspeccionadasPorInspeccionId(int idInspeccion)
        {
            return Mapper.Map<List<DtoEmpresaInspeccionadaPorInspeccion>,List<EmpresaInspeccionadaPorInspeccionModel>>(DaoEmpresa.GetEmpresasInspeccionadasPorInspeccionId(idInspeccion));
        }

        public EmpresaConsultarModel GetEmpresaConsultaById(int id)
        {
            var empresa = DaoEmpresa.GetById(id);
            switch (empresa.TipoEmpresa)
            {
                case TipoEmpresaEnum.MINISTERIO:
                case TipoEmpresaEnum.DIRECCION_DE_INFRAESTRUCTURA:
                case TipoEmpresaEnum.DIRECCION_DE_RECURSOS_HUMANOS:
                case TipoEmpresaEnum.DIRECCION_DE_SISTEMAS:
                case TipoEmpresaEnum.DIRECCION_DE_TESORERIA:
                case TipoEmpresaEnum.SECRETARIA:
                case TipoEmpresaEnum.SUBSECRETARIA:
                case TipoEmpresaEnum.APOYO_ADMINISTRATIVO:
                    return Mapper.Map<EmpresaBase, EmpresaConsultarModel>(empresa);
                case TipoEmpresaEnum.DIRECCION_DE_NIVEL:
                    return Mapper.Map<DireccionDeNivel, EmpresaConsultarModel>((DireccionDeNivel)empresa);
                case TipoEmpresaEnum.INSPECCION:
                    return Mapper.Map<Inspeccion, EmpresaConsultarModel>((Inspeccion)empresa);
                case TipoEmpresaEnum.ESCUELA_MADRE:
                    return Mapper.Map<Escuela, EmpresaConsultarModel>((Escuela)empresa);
                case TipoEmpresaEnum.ESCUELA_ANEXO:
                    return Mapper.Map<EscuelaAnexo, EmpresaConsultarModel>((EscuelaAnexo)empresa);
                default:
                    return new EmpresaConsultarModel();
            }

        }

        /// <summary>
        /// Obtiene empresa dado su Id, mapeando desde la entidad correcta dado el tipo de empresa del que se trate
        /// </summary>
        /// <param name="id">identificador de empresa</param>
        /// <returns>Modelo empresa registrar de la empresa con id dado</returns>
        public EmpresaRegistrarModel GetEmpresaById(int empresaId)
        {
            EmpresaRegistrarModel modelo;
            var temporal = DaoEmpresa.GetById(empresaId);

            switch (temporal.TipoEmpresa)
            {
                case TipoEmpresaEnum.MINISTERIO:
                case TipoEmpresaEnum.DIRECCION_DE_INFRAESTRUCTURA:
                case TipoEmpresaEnum.DIRECCION_DE_RECURSOS_HUMANOS:
                case TipoEmpresaEnum.DIRECCION_DE_SISTEMAS:
                case TipoEmpresaEnum.DIRECCION_DE_TESORERIA:
                case TipoEmpresaEnum.SECRETARIA:
                case TipoEmpresaEnum.SUBSECRETARIA:
                case TipoEmpresaEnum.APOYO_ADMINISTRATIVO:
                    modelo = Mapper.Map<EmpresaBase, EmpresaRegistrarModel>(temporal);
                    break;
                case TipoEmpresaEnum.DIRECCION_DE_NIVEL:
                    modelo = Mapper.Map<DireccionDeNivel, EmpresaRegistrarModel>((DireccionDeNivel) temporal);
                    break;
                case TipoEmpresaEnum.INSPECCION:
                    modelo = Mapper.Map<Inspeccion, EmpresaRegistrarModel>((Inspeccion) temporal);
                    if (((Inspeccion) temporal).EmpresaDeInspeccionSuperior!=null)
                    modelo.EmpresaInspeccionSupervisoraId = ((Inspeccion) temporal).EmpresaDeInspeccionSuperior.Id;
                    break;
                case TipoEmpresaEnum.ESCUELA_MADRE:
                    var madre = (Escuela)temporal;
                    var asignacionInspeccion = DaoProvider.GetDaoAsignacionInspeccionEscuela().GetAsignacionByEscuela(temporal.Id);;
                    modelo = Mapper.Map<Escuela, EmpresaRegistrarModel>(madre);

                    modelo.EmpresaInspeccionId = asignacionInspeccion.Inspeccion.Id;
                    modelo.EmpresaInspeccionNombre = asignacionInspeccion.Inspeccion.Nombre;
                    modelo.EmpresaInspeccionCod = asignacionInspeccion.Inspeccion.CodigoEmpresa;
                        
                    if (modelo.Privado)
                    {
                        modelo.Director = Mapper.Map<PersonaFisica, PersonaFisicaModel>(madre.EscuelaPrivada.Director);
                        modelo.RepresentanteLegal = Mapper.Map<PersonaFisica, PersonaFisicaModel>(madre.EscuelaPrivada.RepresentanteLegal);
                    }
                    break;
                case TipoEmpresaEnum.ESCUELA_ANEXO:
                    var escue = (EscuelaAnexo) temporal;
                    modelo = Mapper.Map<EscuelaAnexo, EmpresaRegistrarModel>(escue);
                    
                      var asignacionInspeccionEscAnexo = DaoProvider.GetDaoAsignacionInspeccionEscuela().GetAsignacionByEscuela(temporal.Id);
                      modelo.EmpresaInspeccionId = asignacionInspeccionEscAnexo.Inspeccion.Id;
                      modelo.EmpresaInspeccionNombre = asignacionInspeccionEscAnexo.Inspeccion.Nombre;
                      modelo.EmpresaInspeccionCod = asignacionInspeccionEscAnexo.Inspeccion.CodigoEmpresa;
                    if (modelo.Privado)
                    {
                        modelo.Director = Mapper.Map<PersonaFisica, PersonaFisicaModel>(escue.EscuelaPrivada.Director);
                        modelo.RepresentanteLegal = Mapper.Map<PersonaFisica, PersonaFisicaModel>(escue.EscuelaPrivada.RepresentanteLegal);
                    }
                    break;
                default:
                    modelo = new EmpresaRegistrarModel();
                    break;
            }
            return modelo;
        }

        public EmpresaRegistrarModel GetEmpresaRegistrarById(int id)
        {
            return GetEmpresaById(id);
        }

        /// Obtiene model de cierre de empresa dado su Id, mapeando desde la entidad correcta dado el tipo de empresa del que se trate
        /// </summary>
        /// <param name="id">identificador de empresa</param>
        /// <returns>Modelo empresaCierre de la empresa con id dado</returns>
        public EmpresaCierreModel GetEmpresaCierreModelById(int id)
        {
            var temporal = DaoEmpresa.GetById(id);

            if (temporal == null)
                throw new BaseException(Resources.Empresa.EmpresaNoExistente);

            return Mapper.Map<EmpresaBase, EmpresaCierreModel>(temporal);
        }

        /// <summary>
        /// Obtiene model de reactivación de empresa
        /// </summary>
        /// <param name="id">Id de la empresa a reactivar</param>
        /// <returns>El model de reactivación de la empresa</returns>
        public EmpresaReactivacionModel GetEmpresaReactivacionById(int id)
        {
            var empresa = DaoEmpresa.GetById(id);
            return Mapper.Map<EmpresaBase, EmpresaReactivacionModel>(empresa);
        }

        /// <summary>
        /// Obtiene model de los domicilios de las vinculaciones a edificios activas que tiene la empresa pasada por parámetro
        /// </summary>
        /// <param name="idEmpresa">Id de la empresa a buscar los edificios vinculados</param>
        /// <returns>los domicilios de las vinculaciones a edificios activas que tiene la empresa</returns>
        public IList<DomicilioEdificioModel> GetDomiciliosDeEdificiosVinculadosAEmpresa(int idEmpresa)
        {
            EmpresaBase empresa = DaoEmpresa.GetById(idEmpresa);
            IList<VinculoEmpresaEdificio> vinculos = DaoEmpresa.GetVinculosCompletos(idEmpresa);// empresa.VinculoEmpresaEdificio;
            /*var domiciliosActivos = new List<DomicilioEdificioModel>();
            var d1 = new DomicilioEdificioModel
                         {
                             Altura = "44",
                             Barrio = "asdasd",
                             Calle = "aagggg",
                             Id = 1,
                             Localidad = 2
                         };
            domiciliosActivos.Add(d1);
            return domiciliosActivos;*/
            return (from vinc in vinculos
                    select Mapper.Map<Domicilio, DomicilioEdificioModel>(vinc.Edificio.Domicilio)).ToList();
        }

        /// <summary>
        /// Verifica si una Empresa posee vínculos a edificios activos a partir de su id
        /// </summary>
        /// <param name="idEmpresa">Id de la empresa</param>
        /// <returns>true si la empresa tiene vínculos activos, false si no posee</returns>
        public bool EmpresaHasVinculosActivos(int idEmpresa)
        {
            var empresa = DaoEmpresa.GetById(idEmpresa);
            return empresa.VinculoEmpresaEdificio.Any(vinculoEmpresaEdificio => vinculoEmpresaEdificio.Estado == EstadoVinculoEmpresaEdificioEnum.ACTIVO);
        }

        /// <summary>
        /// Verifica si una Empresa es escuela
        /// </summary>
        /// <param name="idEmpresa">Id de la empresa</param>
        /// <returns>true es escuela, false si no</returns>
        public bool EmpresaEsEscuela(int idEmpresa)
        {
            var empresa = DaoEmpresa.GetById(idEmpresa);
            return (empresa is Escuela || empresa is EscuelaAnexo);
        }

        /// <summary>
        /// Verifica si una Escuela es de nivel superior
        /// </summary>
        /// <param name="idEmpresa">Id de la empresa</param>
        /// <returns>true es de nivel superior, false si no</returns>
        public bool IsEscuelaNivelSuperior(int idEmpresa)
        {
            var esc = DaoEscuela.GetById(idEmpresa);
            if (esc == null) // Si no se encontró como escuela madre busco en el dao de escuela anexo TODO: FIXEAR ESTO EN ALGÚN MOMENTO 07/09/2011
            {
                var escAnex = DaoProvider.GetDaoEscuelaAnexo().GetById(idEmpresa);
                return (escAnex.NivelEducativo != null && escAnex.NivelEducativo.Id == (int)NivelEducativoNombreEnum.SUPERIOR);
            }
            return (esc.NivelEducativo != null && esc.NivelEducativo.Id == (int)NivelEducativoNombreEnum.SUPERIOR);
        }

        /// <summary>
        /// Verifica si una Empresa es madre
        /// </summary>
        /// <param name="idEmpresa">Id de la empresa</param>
        /// <returns>true si es escuela madre, false si no</returns>
        public bool EmpresaEsMadre(int idEmpresa)
        {
            var empresa = DaoEmpresa.GetById(idEmpresa);
            return (empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE);
        }

        /// <summary>
        /// Devuelve si la escuela madre de la escuela pasada por parámetro está cerrada
        /// </summary>
        /// <param name="idEmpresa">Id de la escuela</param>
        /// <returns>True si La escuela madre de la escuela pasada por parámetro si está cerrada, false si no está cerrada</returns>
        public bool GetEscuelaMadreCerrada(int idEmpresa)
        {
            var escuelaHija = DaoEscuela.GetById(idEmpresa);
            if (escuelaHija == null) // Si no se encontró como escuela madre busco en el dao de escuela anexo TODO: FIXEAR ESTO EN ALGÚN MOMENTO 07/09/2011
            {
                var escAnex = DaoProvider.GetDaoEscuelaAnexo().GetById(idEmpresa);
                return escAnex.EscuelaMadre.EstadoEmpresa == EstadoEmpresaEnum.CERRADA;
            }
            return escuelaHija.EscuelaMadre.EstadoEmpresa == EstadoEmpresaEnum.CERRADA;
        }

        /// <summary>
        /// Obtiene el string de la localidad a partir de su id
        /// </summary>
        /// <param name="id">Id de la localidad
        /// <returns>string de la localidad a partir de su id</returns>
        public string GetLocalidadToStringById(int idLocalidad)
        {
            var localidad = DaoLocalidad.GetById(idLocalidad);
            return localidad != null ? localidad.Nombre : String.Empty;
        }

        public List<NivelEducativoPorTipoEducacionModel> GetNivelesEducativosPorTipoEducacion(TipoEducacionEnum idTipoEducacion)
        {
            var nivelesEducativos =
                DaoProvider.GetDaoNivelEducativoPorTipoEducacion().GetByTipoEducacionId(idTipoEducacion);
          return  Mapper.Map<List<NivelEducativoPorTipoEducacion>,List<NivelEducativoPorTipoEducacionModel>>(nivelesEducativos) ;
        }

        /// <summary>
        /// Obtiene Modelo Visar Empresa dado el id de empresa
        /// </summary>
        /// <param name="id">número identificatorio de empresa</param>
        /// <returns>Modelo de visar empresa con la empresa de id dado</returns>
        public EmpresaVisarModel GetEmpresaVisarById(int id)
        {
            var temporal = DaoEmpresa.GetById(id);

            if (temporal == null)
                throw new BaseException(Resources.Empresa.EmpresaNoExistente);

            return Mapper.Map<EmpresaBase, EmpresaVisarModel>(DaoEmpresa.GetById(id));
        }

        /// <summary>
        /// Obtiene Modelo Activacion Codigo Empresa dado el id de empresa
        /// </summary>
        /// <param name="id">número identificatorio de empresa</param>
        /// <returns>Modelo de activacion codigo empresa con la empresa de id dado</returns>
        public ActivacionCodigoEmpresaModel GetEmpresaActivarCodigoById(int id)
        {
            var empresaAActivar = DaoEmpresa.GetById(id);

            if (empresaAActivar == null)
                throw new BaseException(Resources.Empresa.EmpresaNoExistente);
            //if (
            //    !(empresaAActivar.EstadoEmpresa == EstadoEmpresaEnum.GENERADA ||
            //      empresaAActivar.EstadoEmpresa == EstadoEmpresaEnum.EN_PROCESO_DE_REACTIVACION))
            //    throw new BaseException(Resources.Empresa.EstadoIncorrecto);
            if (empresaAActivar.EstadoEmpresa != EstadoEmpresaEnum.ACTIVA)
                throw new BaseException(Resources.Empresa.EstadoIncorrecto);

            return Mapper.Map<EmpresaBase, ActivacionCodigoEmpresaModel>(empresaAActivar);
        }

        /// <summary>
        /// Modifica el tipo de empresa de una escuela
        /// </summary>
        /// <param name="model">Modelo de la escuela modificada</param>
        public EscuelaModificarTipoEmpresaModel ModificarTipoEmpresa(EscuelaModificarTipoEmpresaModel model)
        {
            var seteador = new StringBuilder();
            var entidad = DaoEmpresa.GetById(model.Id);
            var valorParametroJerarquiInspeccionIgualOrganigrama =
                new EntidadesGeneralesRules().GetValorParametroBooleano(
                    ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA);
            if (entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
            {
                var escuelaMadre = DaoProvider.GetDaoEscuela().GetById(entidad.Id);

                if (!TieneDependencias(entidad.Id))
                {
                    seteador.Append("RAIZ = " + (model.EsRaiz ? "Y" : "N") + ", ");
                    if (!model.EsRaiz)
                    {
                        var escuelaRaiz = DaoEscuela.GetById(model.IdEscuelaRaiz.Value);
                        if (escuelaRaiz != null)
                        {
                            seteador.Append("ID_ESCUELA_RAIZ = " + model.IdEscuelaRaiz.Value.ToString() + ", ");
                            seteador.Append("NUMERO_ESCUELA = " + escuelaRaiz.NumeroEscuela.ToString() + ", ");
                        }
                    }
                    else
                        seteador.Append("NUMERO_ESCUELA = " + (DaoEscuela.GetMaximoNumeroEscuela() + 1).ToString() + ", ");
                    if(!valorParametroJerarquiInspeccionIgualOrganigrama)
                        seteador.Append("ID_SEQ_EMPRESA_PADRE = " + escuelaMadre.EmpresaPadreOrganigrama.Id.ToString() + ", ");
                }
            }
            else//entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE
            {
                seteador.Append("NUMERO_ANEXO = " + model.NumeroAnexo.Value.ToString() + ", ");
                var escuelaMadre = DaoProvider.GetDaoEscuela().GetById(model.IdEscuelaMadre.Value);
                if (escuelaMadre != null)
                {
                    seteador.Append("ID_ESCUELA_MADRE = " + escuelaMadre.Id.ToString() + ", ");
                    seteador.Append("NUMERO_ESCUELA = " + escuelaMadre.NumeroEscuela.ToString() + ", ");
                }
                if (!valorParametroJerarquiInspeccionIgualOrganigrama)
                    seteador.Append("ID_SEQ_EMPRESA_PADRE = " + escuelaMadre.TipoDireccionNivel.Id.ToString() + ",");
            }
            seteador.Append("N_EMPRESA = '" + model.Nombre.ToUpper() + "'");
            DaoProvider.GetDaoEmpresaBase().CambiarTipoEmpresa(entidad.Id, model.TipoEmpresa, seteador.ToString());
            return null;
        }

        /// <summary>
        /// Verifica si el nuevo tipo de empresa es Escuela Anexo
        /// </summary>
        /// <param name="entidad">Entidad de dominio a verificar</param>
        /// <returns>true si se cambia a escuela anexo, false si lo hace a escuela madre</returns>
        public bool CambiarAEscuelaAnexo(Escuela entidad)
        {
            return entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO;
        }

        /// <summary>
        /// Verifica si la entidad es Raiz de otras escuelas
        /// </summary>
        /// <param name="entidad">Entidad a verificar</param>
        /// <returns>True si tiene dependencias, false si no tiene dependencias</returns>
        public bool TieneDependencias(int entidadId)
        {
            return !DaoProvider.GetDaoEscuela().ComprobarRaizEscuelas(entidadId);
        }

        /// <summary>
        /// Persiste empresa en base de datos
        /// </summary>
        /// <param name="model">Modelo empresa registrar con la empresa a persistir</param>
        /// <returns>Modelo empresa registrar con la empresa persistida</returns>
        public EmpresaRegistrarModel EmpresaSave(EmpresaRegistrarModel model)
        {
            // 1. convertir modelo en el dominio que le corresponde
            EmpresaBase entidad = null;
            if (model.Id > 0)
            {
                entidad = DaoEmpresa.GetById(model.Id);
            }
            AsignacionInspeccionEscuela asignacionInspeccionEscuelaEnCasoDeAltaDeEscuela = null;

            switch (model.TipoEmpresa)
            {
                case TipoEmpresaEnum.ESCUELA_MADRE:
                    entidad = GenerarEntidadEscuela(model, (Escuela) entidad);
                    break;
                case TipoEmpresaEnum.ESCUELA_ANEXO:
                    if (model.EscuelaMadreId.HasValue)
                        entidad = GenerarEntidadEscuelaAnexo(model, (EscuelaAnexo) entidad);
                    else throw new BaseException("No se ha seleccionado una escuela madre");
                    break;
                case TipoEmpresaEnum.INSPECCION:
                    entidad = GenerarEntidadInspeccion(model, (Inspeccion) entidad);
                    break;
                case TipoEmpresaEnum.DIRECCION_DE_NIVEL:
                    entidad = GenerarEntidadDireccionDeNivel(model, (DireccionDeNivel) entidad);
                    break;
                case TipoEmpresaEnum.MINISTERIO:
                    if (entidad == null)
                        entidad = new Ministerio();
                    break;
                case TipoEmpresaEnum.SECRETARIA:
                    if (entidad == null)
                        entidad = new Secretaria();
                    break;
                case TipoEmpresaEnum.SUBSECRETARIA:
                    if (entidad == null)
                        entidad = new SubSecretaria();
                    break;
                case TipoEmpresaEnum.APOYO_ADMINISTRATIVO:
                    if (entidad == null)
                        entidad = new ApoyoAdministrativo();
                    break;
                case TipoEmpresaEnum.DIRECCION_DE_INFRAESTRUCTURA:
                    if (entidad == null)
                        entidad = new DireccionDeInfraestructura();
                    break;
                case TipoEmpresaEnum.DIRECCION_DE_RECURSOS_HUMANOS:
                    if (entidad == null)
                        entidad = new DireccionDeRRHH();
                    break;
                case TipoEmpresaEnum.DIRECCION_DE_SISTEMAS:
                    if (entidad == null)
                        entidad = new DireccionDeSistemas();
                    break;
                case TipoEmpresaEnum.DIRECCION_DE_TESORERIA:
                    if (entidad == null)
                        entidad = new DireccionDeTesoreria();
                    break;
                default:
                    //entidad = Mapper.Map<EmpresaRegistrarModel, Empresa>(model);
                    if (!string.IsNullOrEmpty(model.Telefono))
                        model.Telefono = "";
                    entidad = TransformarModelo(model);
                    break;
            }

            //Inicio transformar empresa
            var currentUsuario = Usuario.CurrentDomain;
            //aqui se obtiene el usuario del dominio
            entidad.UsuarioModificacion = currentUsuario;
            entidad.FechaUltimaModificacion = DateTime.Now;

            //estoy registrando
            if (model.Id <= 0)
            {
                entidad.UsuarioAlta = currentUsuario;

                if (currentUsuario.RolActual != null)
                {
                    //aqui se obtiene la empresa del usuario del DTO que es el que se guarda en sesion
                    entidad.EmpresaRegistro = DaoEmpresa.GetById(Usuario.Current.RolActual.EmpresaId);
                }
               
                entidad.FechaAlta = DateTime.Now;
                entidad.Estado = EstadoEmpresaEnum.GENERADA;
                entidad.HistorialEstados.Add(NuevoEstado(EstadoEmpresaEnum.GENERADA, entidad));
                entidad.TipoEmpresa = model.TipoEmpresa;
                //asignacion de inspeccion escuela en caso que sea escuela
                if (entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE ||
                    entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
                {
                    if (Usuario.Current.RolActual.AgenteId.HasValue && model.EmpresaInspeccionId.HasValue)
                        asignacionInspeccionEscuelaEnCasoDeAltaDeEscuela = new AsignacionInspeccionEscuela()
                                                                               {
                                                                                   FechaAltaAsignacion = DateTime.Today,
                                                                                   Estado =
                                                                                       EstadoAsignacionInspeccionEscuelaEnum
                                                                                       .PROVISORIO,
                                                                                   Escuela = entidad,
                                                                                   UsuarioAlta = Usuario.CurrentDomain,
                                                                                   Inspeccion =
                                                                                       DaoProvider.GetDaoInspeccion().
                                                                                       GetById(
                                                                                           model.EmpresaInspeccionId.
                                                                                               Value)
                                                                               };
                }
            }

            entidad.FechaNotificacion = model.FechaNotificacion;
            if (model.OrdenDePagoId.HasValue)
            {
                entidad.OrdenDePago = DaoProvider.GetDaoOrdenDePago().GetById(model.OrdenDePagoId.Value);
            }
            else
            {
                throw new BaseException("La orden de pago es obligatoria para la empresa");
            }
            if (model.ProgramaPresupuestarioId.HasValue)
            {
                entidad.ProgramaPresupuestario =
                    DaoProvider.GetDaoProgramaPresupuestario().GetById(model.ProgramaPresupuestarioId.Value);
            }
            else
            {
                throw new BaseException("El programa presupuestario es obligatorio para la empresa");
            }

            //TODO definir algoritmo para crear codigo de empresa
            //el codigo de empresa surge de un algoritmo que no ha sido definido
            if (!string.IsNullOrEmpty(model.CodigoEmpresa))
                entidad.CodigoEmpresa = model.CodigoEmpresa.ToUpper();
            if (!string.IsNullOrEmpty(model.Nombre))
                entidad.Nombre = model.Nombre.ToUpper();
            entidad.FechaInicioActividad = model.FechaInicioActividades;
            if (model.Id > 0) //Si estoy editando valido la fecha de inicio actividades
            {
                ValidarFechaInicioActividades(entidad);
            }
            if (!string.IsNullOrEmpty(model.Observaciones))
                entidad.Observaciones = model.Observaciones;
            if (entidad.TipoEmpresa != TipoEmpresaEnum.ESCUELA_ANEXO &&
                entidad.TipoEmpresa != TipoEmpresaEnum.ESCUELA_MADRE && entidad.TipoEmpresa!=TipoEmpresaEnum.INSPECCION)
                entidad.EmpresaPadreOrganigrama = model.EmpresaPadreOrganigramaId == 0
                                                      ? null
                                                      : DaoEmpresa.GetById(model.EmpresaPadreOrganigramaId);
            //Valido la selección de empresa padre
            if (entidad.EmpresaPadreOrganigrama != null)
                ValidarEmpresaPadre(entidad.TipoEmpresa, entidad.EmpresaPadreOrganigrama.TipoEmpresa);

            Domicilio domicilio = null;
            if (model.DomicilioId.HasValue)
            {
                domicilio = DaoProvider.GetDaoDomicilio().GetById(model.DomicilioId.Value);
                entidad.Domicilio = domicilio;
            }
            else throw new ApplicationException("Se debe seleccionar domicilio en Edificios.");

            //VINCULO EMPRESA EDIFICIO sólo los guardo cuando estemos registrando, en la modificación no se pueden tocar.
            if (model.VinculoEmpresaEdificio != null && model.Id <= 0)
            {

                VinculoEmpresaEdificio vinculoNuevo = null;
                foreach (var vee in model.VinculoEmpresaEdificio)
                {
                    vinculoNuevo = new VinculoEmpresaEdificio();
                    vinculoNuevo.Edificio = DaoProvider.GetDaoEdificio().GetById(vee.EdificioId.Value);
                    vinculoNuevo.Estado = EstadoVinculoEmpresaEdificioEnum.PROVISORIO;
                    vinculoNuevo.FechaDesde = vee.FechaDesde;
                    vinculoNuevo.Observacion = vee.Observacion;
                    int idEntero = 0;
                    if (int.TryParse(domicilio.EntidadId, out idEntero))
                        vinculoNuevo.DeterminaDomicilio = idEntero == vee.EdificioId.Value;
                    entidad.AddVinculoEdificio(vinculoNuevo);
                }
            }
            else 
            {
                if(model.VinculoEmpresaEdificio == null)
                    throw new ApplicationException("Se debe seleccionar un Edificio.");
                foreach (var vee in entidad.VinculoEmpresaEdificio)
                {
                    int idEntero = 0;
                    if (int.TryParse(domicilio.EntidadId, out idEntero))
                        vee.DeterminaDomicilio = idEntero == vee.Edificio.Id;
                }

            }

            //Fin transformarEmpresa
            ValidarEmpresa(entidad, model.DomicilioId.HasValue);

            //persisto en base
            entidad = DaoEmpresa.SaveOrUpdate(entidad);

            if (!string.IsNullOrEmpty(model.Telefono))
            {
                ValidarTelefono(model.Telefono);
                ComunicacionSave(model, entidad.Id);
            }

            TipoMovimientoInstrumentoLegal tipoMovimientoInstrumento = null;
            AsignacionInstrumentoLegal asignacionInstrumentoLegal = null;
            TipoInstrumentoLegal tipoInstrumentoLegal = null;
            if (model.Id > 0)
            {
                if (model.InstrumentosLegales != null)
                {
                    tipoMovimientoInstrumento =
                        DaoProvider.GetDaoTipoMovimientoInstrumentoLegal().GetById(
                            (int) TipoMovimientoInstrumentoLegalEnum.MODIFICACION_EMPRESA);
                    asignacionInstrumentoLegal =
                        Mapper.Map<AsignacionInstrumentoLegalModel, AsignacionInstrumentoLegal>(
                            model.InstrumentosLegales);
                    asignacionInstrumentoLegal.Empresa = entidad;
                    asignacionInstrumentoLegal.TipoMovimientoInstrumentoLegal = tipoMovimientoInstrumento;
                    asignacionInstrumentoLegal.TipoMovimientoInstrumentoLegal = tipoMovimientoInstrumento;
                    asignacionInstrumentoLegal.FecNotificacion = model.FecNotificacionAsignacionIL;
                    ReglaAsignacion.AsignacionInstrumentoLegalSave(asignacionInstrumentoLegal, model.InstrumentosLegales);
                }
            }
            else
            {
                if (model.InstrumentosLegales != null)
                {
                    tipoMovimientoInstrumento =
                        DaoProvider.GetDaoTipoMovimientoInstrumentoLegal().GetById(
                            (int) TipoMovimientoInstrumentoLegalEnum.ALTA_EMPRESA);
                    
                    asignacionInstrumentoLegal =
                        Mapper.Map<AsignacionInstrumentoLegalModel, AsignacionInstrumentoLegal>(
                            model.InstrumentosLegales);

                    if (model.InstrumentosLegales.InstrumentoLegal != null && model.InstrumentosLegales.InstrumentoLegal.Id.HasValue && model.InstrumentosLegales.InstrumentoLegal.Id > 0)
                    {
                        asignacionInstrumentoLegal.InstrumentoLegal =
                            DaoProvider.GetDaoInstrumentoLegal().GetById(model.InstrumentosLegales.InstrumentoLegal.Id.Value);
                    }
                    asignacionInstrumentoLegal.Empresa = entidad;
                    asignacionInstrumentoLegal.TipoMovimientoInstrumentoLegal = tipoMovimientoInstrumento;
                    asignacionInstrumentoLegal.FecNotificacion = model.FecNotificacionAsignacionIL;
                    ReglaAsignacion.AsignacionInstrumentoLegalSave(asignacionInstrumentoLegal, model.InstrumentosLegales);
                }
            }
            if (entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE ||
                entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
            {
                if (model.Id == 0 &&
                    new EntidadesGeneralesRules().GetValorParametroBooleano(
                        ParametroEnum.ESTRUCTURA_ESCOLAR_EN_CREACIÓN_EMPRESA) &&
                    (model.EstructuraEscolar == null || model.EstructuraEscolar.Count == 0))
                    throw new BaseException(Resources.Empresa.EstructuraEscolarRequerida);
                if (model.EstructuraEscolar != null && model.EstructuraEscolar.Count > 0)
                {
                    var diagCursoRules = new DiagramacionCursoRules();
                    foreach (var diagramacionCurso in model.EstructuraEscolar)
                    {
                        if (diagramacionCurso.Id <= 0) //Si son nuevas, guardo
                        {
                            diagramacionCurso.Id = 0;
                            diagramacionCurso.Escuela = entidad.Id;
                            diagramacionCurso.Estado = EstadoDiagramacionCursoEnum.HABILITADA;
                            diagCursoRules.DiagramacionCursoSave(diagramacionCurso);
                        }
                        else // Si ya están en la db actualizo
                            diagCursoRules.DiagramacionCursoUpdate(diagramacionCurso);
                    }
                }
                if (asignacionInspeccionEscuelaEnCasoDeAltaDeEscuela != null)
                    DaoProvider.GetDaoAsignacionInspeccionEscuela().Save(
                        asignacionInspeccionEscuelaEnCasoDeAltaDeEscuela);

                if (model.ZonaDesfavorableId.Value != (int) ZonaDesfavorableEnum.A)
                    InstrumentoLegalParaZonaDesfavorable(model, entidad);
            }

            // Asigno valores a los 3 parametros (reglas de negocio) que corresponden a una escuela
            if (entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO ||
                entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
            {
                var parametroRules = ServiceLocator.Current.GetInstance<IParametroRules>();
                var parametrosEscuela = parametroRules.GetParametroByPaquete(PaqueteParametroEnum.REGLAS_DE_NEGOCIO);
                foreach (var p in parametrosEscuela)
                {
                    var valor = new ValorParametroModel();
                    valor.Valor = "N";
                    valor.Escuela = entidad.Id;
                    valor.Parametro = p;
                    valor.FechaVigencia = DateTime.Today;
                    valor = parametroRules.SaveParametroEscuela(valor);
                }
            }
            
            var idDomicilioAuxiliar = model.DomicilioId;
            var telefonoAuxiliar = model.Telefono;
            var calleNuevaDomicilioAuxiliar = model.CalleNuevoDomicilio;
            var alturaNuevaDomicilioAuxiliar = model.AlturaNuevoDomicilio;

            //entidad.Domicilio.Calle = DomicilioSave(model.DomicilioId.Value, model);

            var modeloGuardado = Mapper.Map<EmpresaBase, EmpresaRegistrarModel>(entidad);
            modeloGuardado.Id = entidad.Id;
            modeloGuardado.DomicilioId = idDomicilioAuxiliar;
            modeloGuardado.Telefono = telefonoAuxiliar;
            modeloGuardado.CalleNuevoDomicilio = calleNuevaDomicilioAuxiliar;
            modeloGuardado.AlturaNuevoDomicilio = alturaNuevaDomicilioAuxiliar;
            return modeloGuardado;
        }

        public void ValidarTelefono(string telefono)
        {
            try
            {
                Convert.ToInt64(telefono);
            }
            catch (Exception)
            {
                throw new ApplicationException("Ingresar sólo números en el campo Teléfono");
            }
        }

        private void ValidarFechaInicioActividades(EmpresaBase empresa)
        {
            if (!DaoEmpresa.FechaInicioActividadesValidaPuestoTrabajo(empresa))
            {
                throw new BaseException("Existen puestos de trabajo con fecha de inicio anteriores a la fecha que se intenta modificar de la empresa " + empresa.FechaInicioActividad.ToShortDateString());
            }
            if ((empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE || empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO) && !DaoEmpresa.FechaInicioActividadesValidaPlanDeEstudio(empresa))
            {
                throw new BaseException("Existen asignaciones de plan de estudio con fechas anteriores a la fecha que se intenta modificar de la empresa " + empresa.FechaInicioActividad.ToShortDateString());
            }
        }

        /// <summary>
        /// Registro un instrumento legal para zona desfavorable != A
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entidad"></param>
        public void InstrumentoLegalParaZonaDesfavorable(EmpresaRegistrarModel model, EmpresaBase entidad)
        {
            if (model.AsignacionInstrumentoLegalZonaDesfavorable != null)
            {
                if (model.InstrumentosLegales != null && model.InstrumentosLegales.InstrumentoLegal != null)
                {
                        AsignarInstrumentoLegalParaZonaDesfavorable(model, entidad);
                }
                else
                {
                    model.AsignacionInstrumentoLegalZonaDesfavorable.Id = 0;
                    AsignarInstrumentoLegalParaZonaDesfavorable(model, entidad);
                }
            }
        }

        //Método que valida que si estamos ingresando nuevos instrumentos legales no posean números repetidos
        private void ValidarInstrumentosLegalesConNumerosRepetidos(EmpresaRegistrarModel model)
        {
            //Si el instrumento legal general tiene el mismo número que el de zona desfavorable y además alguno de los dos tiene id = 0, tiramos un error
            if ((model.AsignacionInstrumentoLegalZonaDesfavorable.InstrumentoLegal.Id == 0 || model.InstrumentosLegales.InstrumentoLegal.Id == 0))
            {
                throw new BaseException("Se están intentando crear instrumentos legales con números repetidos, valide el instrumento legal general con el de zona desfavorable");
            }
        }

        public void AsignarInstrumentoLegalParaZonaDesfavorable(EmpresaRegistrarModel model, EmpresaBase entidad)
        {
            if (model.AsignacionInstrumentoLegalZonaDesfavorable.InstrumentoLegal.NroInstrumentoLegal == model.InstrumentosLegales.InstrumentoLegal.NroInstrumentoLegal)
            {
                ValidarInstrumentosLegalesConNumerosRepetidos(model);
                model.AsignacionInstrumentoLegalZonaDesfavorable.InstrumentoLegal =
                            model.InstrumentosLegales.InstrumentoLegal;
            }

            var tipoMovimientoInstrumento =
                            DaoProvider.GetDaoTipoMovimientoInstrumentoLegal().GetById(
                                (int)TipoMovimientoInstrumentoLegalEnum.ESCUELA_ZONA_DESFAVORABLE);
           


            var asignacionInstrumentoLegal =
                Mapper.Map<AsignacionInstrumentoLegalModel, AsignacionInstrumentoLegal>(
                    model.AsignacionInstrumentoLegalZonaDesfavorable);

            if (model.AsignacionInstrumentoLegalZonaDesfavorable.InstrumentoLegal != null && model.AsignacionInstrumentoLegalZonaDesfavorable.InstrumentoLegal.Id.HasValue && model.AsignacionInstrumentoLegalZonaDesfavorable.InstrumentoLegal.Id > 0)
            {
                asignacionInstrumentoLegal.InstrumentoLegal =
                    DaoProvider.GetDaoInstrumentoLegal().GetById(model.AsignacionInstrumentoLegalZonaDesfavorable.InstrumentoLegal.Id.Value);
            }

            asignacionInstrumentoLegal.Empresa = entidad;
            asignacionInstrumentoLegal.TipoMovimientoInstrumentoLegal = tipoMovimientoInstrumento;
            //asigna la fecha de notificacion de la asignacion de il para zona desf.
            asignacionInstrumentoLegal.FecNotificacion = model.FecNotificacionAsignacionILZD;
            ReglaAsignacion.AsignacionInstrumentoLegalSave(asignacionInstrumentoLegal, model.AsignacionInstrumentoLegalZonaDesfavorable);
        }

        public void ComunicacionSave(EmpresaRegistrarModel model, int idEntidad)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Telefono) || model.Telefono.Length < 6)
                    throw new BaseException(Resources.Empresa.TelefonoNoValido);

                Comunicacion com = new Comunicacion();
                com.Entidad = idEntidad.ToString();
                com.Valor = model.Telefono;
                com.TipoComunicacion = TipoComunicacionEnum.TELEFONO_PRINCIPAL;
                com.Origen = OrigenEnum.T_EM_EMPRESAS.ToString();
                DaoProvider.GetDaoComunicacion().Save(com);
            }
            catch (Exception)
            {
                throw new BaseException(Resources.Empresa.ErrorGuardandoTelefono);
            }
        }

        /// <summary>
        /// Persiste en base de datos activacion o rechazo de activacion de empresa
        /// </summary>
        /// <param name="model">Modelo con empresa a activar o rechazar</param>
        /// <returns>Modelo de empresa activada o rechazada</returns>
        public void VisarActivacion(EmpresaVisarModel model)
        {
            var entidad = DaoEmpresa.GetById(model.Id);
            entidad.Observaciones = model.ObservacionesVisarActivacion;

            if(new EntidadesGeneralesRules().GetValorParametroBooleano(ParametroEnum.SOLO_VISADO_EN_CREACIÓN_O_REACTIVACIÓN_DE_EMPRESA))
            
                foreach (var vinculo in entidad.VinculoEmpresaEdificio)
                    vinculo.Estado =  EstadoVinculoEmpresaEdificioEnum.ACTIVO;
            
            else
            if(model.Accion==AccionVisadoActivacionEmpresaEnum.AUTORIZAR)
                foreach (var vinculo in entidad.VinculoEmpresaEdificio)
                    vinculo.Estado = vinculo.Estado == EstadoVinculoEmpresaEdificioEnum.PROVISORIO
                                         ? EstadoVinculoEmpresaEdificioEnum.ACTIVO
                                         : vinculo.Estado;

            if(model.Accion==AccionVisadoActivacionEmpresaEnum.RECHAZAR)
                foreach (var vinculo in entidad.VinculoEmpresaEdificio)
                    vinculo.Estado = vinculo.Estado == EstadoVinculoEmpresaEdificioEnum.PROVISORIO
                                         ? EstadoVinculoEmpresaEdificioEnum.INACTIVO
                                         : vinculo.Estado;

            validarEmpresaParaVisarActivacion(entidad);

            switch (model.Accion)
            {
                case AccionVisadoActivacionEmpresaEnum.VISADA:
                case AccionVisadoActivacionEmpresaEnum.AUTORIZAR:
                    entidad.Estado = EstadoEmpresaEnum.ACTIVA;
                    entidad.HistorialEstados.Add(NuevoEstado(EstadoEmpresaEnum.ACTIVA, entidad));
                    EmpresaAceptarActivacion(entidad);
                    break;
                case AccionVisadoActivacionEmpresaEnum.RECHAZAR:
                    entidad.Estado = EstadoEmpresaEnum.RECHAZADA;
                    entidad.HistorialEstados.Add(NuevoEstado(EstadoEmpresaEnum.RECHAZADA, entidad));

                    EmpresaRechazarActivacion(entidad);
                    break;
                default:
                    break;
            }

            DaoEmpresa.SaveOrUpdate(entidad);
        }

        /// <summary>
        /// Cierre de empresa, que se hace efectivo si el usuario tiene permiso de cierre;
        /// caso contrario se genera un pedido de cierre
        /// </summary>
        /// <param name="model">Modelo de la empresa a cerrar </param>
        /// <returns>Modelo de la empresa cerrada</returns>
        public EmpresaCierreModel EmpresaCerrar(EmpresaCierreModel model)
        {
            AsignacionInstrumentoLegal asignacionInstrumentoLegal = null;
            Resolucion resolucion = null;
            var entidad = DaoEmpresa.GetById(model.IdEmpresa);
            entidad.FechaCierre = model.FechaCierre;

            // Valido si la empresa está en condiciones de ser cerrada
            ValidarCierreEmpresa(model);

            //Cargo la asignación al instrumento legal
            asignacionInstrumentoLegal = GetAsignacionInstrumentoLegalCierre(model,entidad);
            
            //Cargo el pedido de autorización de cierre
            var pedidoAutorizacionCierre = CargarPedidoAutorizacionCierre(model, entidad, asignacionInstrumentoLegal);

            //Seteo el estado del cierre de la empresa
            SetEstadoEmpresaCierre(entidad);

            //Persisto las entidades
            PersistirCierreEmpresa(model,entidad, asignacionInstrumentoLegal,pedidoAutorizacionCierre);

            return Mapper.Map<EmpresaBase,EmpresaCierreModel>(entidad);
        }

        private void PersistirCierreEmpresa(EmpresaCierreModel model, EmpresaBase entidad, AsignacionInstrumentoLegal asignacionInstrumentoLegal, PedidoAutorizacionCierre pedidoAutorizacionCierre)
        {
            // Registro la modificación a la entidad cerrada
            DaoEmpresa.SaveOrUpdate(entidad); 
            // Registro la asignación instrumento legal
            var asignacionModel =
                Mapper.Map<AsignacionInstrumentoLegal, AsignacionInstrumentoLegalModel>(asignacionInstrumentoLegal);
            new AsignacionInstrumentoLegalRules().AsignacionInstrumentoLegalSave(asignacionInstrumentoLegal,asignacionModel);
            // Guardo la resolución, previa asignación del instrumento legal
            if (model.EmitirResolucionDeCierre && model.Resolucion != null)
            {
                //model.Resolucion.InstrumentoLegalResolucion = Mapper.Map<AsignacionInstrumentoLegal, AsignacionInstrumentoLegalModel>(asignacionInstrumentoLegal);
                var res = Mapper.Map<ResolucionModel, Resolucion>(model.Resolucion);
                res.ArticulosResolucion =
                    Mapper.Map<List<ArticuloResolucionModel>, List<ArticuloResolucion>>(model.Resolucion.ArticulosResolucion);
                res.InstrumentoLegal = asignacionInstrumentoLegal.InstrumentoLegal;
                if (res.ArticulosResolucion != null)
                {
                    foreach (var articuloResolucion in res.ArticulosResolucion)
                    {
                        articuloResolucion.Resolucion = res;
                    }
                }
                DaoProvider.GetDaoResolucion().SaveOrUpdate(res);
            }
            DaoProvider.GetDaoPedidoAutorizacionCierre().SaveOrUpdate(pedidoAutorizacionCierre);
        }

        private PedidoAutorizacionCierre CargarPedidoAutorizacionCierre(EmpresaCierreModel model, EmpresaBase entidad, AsignacionInstrumentoLegal asignacionInstrumentoLegal)
        {
            var pedidoAutorizacionCierre = new PedidoAutorizacionCierre();
            var entidadesGeneralesRules = new EntidadesGeneralesRules();

            var currentUsuario = UsuarioRules.Instancia.GetCurrentUserDomain;

            pedidoAutorizacionCierre.Empresa = entidad;
            pedidoAutorizacionCierre.AsignacionInstrumentoLegal = asignacionInstrumentoLegal;

            pedidoAutorizacionCierre.AddEstado(new EstadoPedidoAutorizacionCierre()
            {
                Estado = EstadoPedidoCierreEnum.GENERADO,
                FechaAlta = DateTime.Now,
                PedidoCierre = pedidoAutorizacionCierre,
                AgenteAlta = currentUsuario
            });

            pedidoAutorizacionCierre.FechaAlta = DateTime.Today;
            pedidoAutorizacionCierre.AgenteAlta = currentUsuario;
            pedidoAutorizacionCierre.FechaCierre = model.FechaCierre;

            return pedidoAutorizacionCierre;
        }

        private void SetEstadoEmpresaCierre(EmpresaBase entidad)
        {
            var entidadesGeneralesRules = new EntidadesGeneralesRules();
            bool requiereAutorizacion =
                entidadesGeneralesRules.GetValorParametroBooleano(ParametroEnum.CIERRE_DE_EMPRESA_CON_AUTORIZACIÓN);
            //Si el usuario tiene autoridad para cerrar la empresa sin autorizacion o solo si puede pasarlo a estado "EN PROCESO DE CIERRE"
            if (requiereAutorizacion) // Si requiere autorización genero un pedido de autorización
            {
                entidad.Estado = EstadoEmpresaEnum.EN_PROCESO_DE_CIERRE;
                entidad.HistorialEstados.Add(NuevoEstado(EstadoEmpresaEnum.EN_PROCESO_DE_CIERRE, entidad));
            }
            else
            {
                entidad.Estado = EstadoEmpresaEnum.CERRADA_SIN_VISADO;
                entidad.HistorialEstados.Add(NuevoEstado(EstadoEmpresaEnum.CERRADA_SIN_VISADO, entidad));
            }
        }

        private AsignacionInstrumentoLegal GetAsignacionInstrumentoLegalCierre(EmpresaCierreModel model, EmpresaBase entidad)
        {
            ValidarCargaInstrumentoLegal(model);
            var tipoMovimientoInstrumentoLegalCierreEmpresa = DaoProvider.GetDaoTipoMovimientoInstrumentoLegal().GetById((int)TipoMovimientoInstrumentoLegalEnum.CIERRE_EMPRESA);
            if (tipoMovimientoInstrumentoLegalCierreEmpresa == null)
                throw new BaseException(Resources.Empresa.TipoInstrumentoLegalCierreEmpresa);
            InstrumentoLegal ins;
            if (model.Resolucion == null) //Si no se cargó resolución obtengo el instrumento legal desde la asignación
            {
                ins = Mapper.Map<InstrumentoLegalModel, InstrumentoLegal>(model.AsignacionInstrumentoLegal.InstrumentoLegal);
                ins.TipoInstrumentoLegal = DaoProvider.GetDaoTipoInstrumentoLegal().GetById(model.AsignacionInstrumentoLegal.InstrumentoLegal.IdTipoInstrumentoLegal.Value);
            }
            else // Si se cargó la resolución obtengo de ahí el instrumento legal
            {
                ins = Mapper.Map<InstrumentoLegalModel, InstrumentoLegal>(model.Resolucion.InstrumentoLegalResolucion.InstrumentoLegal);
                ins.TipoInstrumentoLegal = DaoProvider.GetDaoTipoInstrumentoLegal().GetById(model.Resolucion.InstrumentoLegalResolucion.InstrumentoLegal.IdTipoInstrumentoLegal.Value);
            }

            if (ins.Id != 0) // Si es un instrumento guardado en la base, lo traigo de ahí
            {
                ins = DaoProvider.GetDaoInstrumentoLegal().GetById(ins.Id);
            }
            else
            {
                ins.FechaAlta = DateTime.Now;
            }
            //Creo y cargo la asignación
            var asignacionInstrumentoLegal = new AsignacionInstrumentoLegal()
            {
                TipoMovimientoInstrumentoLegal = tipoMovimientoInstrumentoLegalCierreEmpresa,
                Empresa = entidad,
                InstrumentoLegal = ins,
                FecAsociacion = DateTime.Today,
                FecNotificacion = model.FechaNotificacion,
                Observaciones = model.ObservacionesAsignacion
            };

            return asignacionInstrumentoLegal;
        }

        private void ValidarCargaInstrumentoLegal(EmpresaCierreModel model)
        {
            //Si se cargó resolución veo que tenga instrumento legal
            if (model.Resolucion != null)
            {
                if (model.Resolucion.InstrumentoLegalResolucion == null || model.Resolucion.InstrumentoLegalResolucion.InstrumentoLegal == null)
                {
                    throw new BaseException(Resources.Empresa.InstrumentoLegalRequerido);
                }
                else
                {
                    return;
                }
            }
            else
            {
                //Si no se cargó instrumento legal tiro excepción
                if (model.AsignacionInstrumentoLegal == null
                    || model.AsignacionInstrumentoLegal.InstrumentoLegal == null)
                    throw new BaseException(Resources.Empresa.InstrumentoLegalRequerido);
            }
        }

        /// <summary>
        /// Método que busca el domicilio de un edificio particular
        /// </summary>
        /// <param name="idEdificio">Id del edificio del cual buscar el domicilio </param>
        /// <returns>Modelo del domicilio del edificio</returns>
        public DomicilioEdificioModel FindDomicilioDeEdificio(int idEdificio)
        {
            var edificio = DaoProvider.GetDaoEdificio().GetById(idEdificio);
            if (edificio != null && edificio.Domicilio != null)
            {
                return Mapper.Map<Domicilio, DomicilioEdificioModel>(edificio.Domicilio);
            }
            return null;
        }

        #region Reactivación Empresa

        /// <summary>
        /// Reactivación de empresa
        /// </summary>
        /// <param name="model">Modelo de la empresa a reactivar </param>
        /// <param name="idEmpresaUsuarioLogueado">El ID de la empresa a la que el usuario logueado está asociado </param>
        /// <returns>Modelo de la empresa reactivada</returns>
        public EmpresaReactivacionModel EmpresaReactivar(EmpresaReactivacionModel model, int idEmpresaUsuarioLogueado)
        {
            var empresa = DaoEmpresa.GetById(model.Id);

            //Registrar Vínculos a Edificios
            var vinculos = RegistrarVinculosAEdificio(model, empresa);

            //Registrar Domicilio
            RegistrarDomicilio(model, empresa);

            //TODO: Falta la regla para paquete presupuestario, no entró en Fase 1

            //Registro el Instrumento Legal
            var asignacionInstrumentoLegal = RegistrarInstrumentoLegalYAsignacion(model, empresa);

            //Variable para la asignación de inspección escuela
            AsignacionInspeccionEscuela asignacionInspeccionEscuela = null;

            if (empresa is Escuela || empresa is EscuelaAnexo)
            {
                //Proceso todas las tareas correspondientes a Escuela según el UC
                var escuela = ProcesarEscuela(model, empresa, idEmpresaUsuarioLogueado);
                //Registro Estructura Escolar
                RegistrarEstructuraEscolar(model, escuela);
                //Registro Plan de estudios //TODO: No entra en fase 1
                RegistrarPlanDeEstudios();
                //Registro la asignación de Empresa de inspección (obligatorio)
                asignacionInspeccionEscuela = RegistrarInspeccionEscuela(model, escuela);
            }
            else
            {
                //Proceso las tareas correspondientes a cuando no es escuela
                ProcesarNoEscuela(model, empresa);
            }
            empresa.Estado = EstadoEmpresaEnum.EN_PROCESO_DE_REACTIVACION;
            empresa.HistorialEstados.Add(NuevoEstado(EstadoEmpresaEnum.EN_PROCESO_DE_REACTIVACION, empresa));

            //Persisto los objetos
            PersistirReactivacionEmpresa(model, empresa, asignacionInspeccionEscuela, asignacionInstrumentoLegal, vinculos);

            //Mandar mail para avisar que se reactivó
            //if (empresa.EstadoEmpresa == EstadoEmpresaEnum.EN_PROCESO_DE_REACTIVACION)
            //{
            //    EnviarMailReactivacion(empresa);
            //}

            return model;
        }

        private List<RegistrarVinculoEmpresaEdificioModel> RegistrarVinculosAEdificio(EmpresaReactivacionModel model, EmpresaBase empresa)
        {
            var vinculosModels = new List<RegistrarVinculoEmpresaEdificioModel>();
            //Convierto el model VinculoEmpresaEdificioReactivacionEmpModel en un RegistrarVinculoEmpresaEdificioModel para usar VinculoEmpresaRules.VinculoEmpresaEdificioUnicoSave()
            for (int i = 0; i < model.VinculoEmpresaEdificio.Count; i++)
            {
                var vinculoEmpresaEdificioReactivacionEmpModel = model.VinculoEmpresaEdificio[i];
                var edificios = new List<int> { vinculoEmpresaEdificioReactivacionEmpModel.Edificio };
                var v = new RegistrarVinculoEmpresaEdificioModel
                            {
                                Observacion = vinculoEmpresaEdificioReactivacionEmpModel.Observacion,
                                FechaDesde = vinculoEmpresaEdificioReactivacionEmpModel.FechaDesde,
                                Empresa = empresa.Id.ToString(),
                                ListaEdificios = edificios
                            };
                vinculosModels.Add(v);
            }
            //Si la empresa no posee vínculos a edificio activos, informo el error);
            if (!EmpresaHasVinculosActivos(empresa.Id) && vinculosModels.Count == 0)
            {
                throw new BaseException("Deben cargarse vínculos a Edificios");
            }
            return vinculosModels;
        }

        private void RegistrarDomicilio(EmpresaReactivacionModel model, EmpresaBase empresa)
        {
            if (model.Domicilio == null || model.Domicilio == 0)
            {
                throw new BaseException("Debe seleccionar un Domicilio válido");
            }
            empresa.Domicilio = DaoProvider.GetDaoDomicilio().GetById(model.Domicilio);
        }

        private AsignacionInstrumentoLegal RegistrarInstrumentoLegalYAsignacion(EmpresaReactivacionModel model, EmpresaBase empresa)
        {
            // Este parámetro si está en TRUE significa que se debe pedir OBLIGATORIA la asignación de instrumento legal y el instrumento legal
            var requiereAsignacionEInstrumentoLegal = new EntidadesGeneralesRules().GetValorParametroBooleano(ParametroEnum.INSTRUMENTO_LEGAL_EN_REACTIVACIÓN_EMPRESA);

            //if (model.Instrumento.Id == null)
            //{
            //    model.Instrumento = null;
            //}
            if (model.InstrumentoLegal != null && model.InstrumentoLegal.Id == 0) // si es un nuevo instrumento
            {
                model.InstrumentoLegal.FechaAlta = DateTime.Today;
            }
            AsignacionInstrumentoLegal asignacionInstrumentoLegal = null;
            if (model.InstrumentoLegal != null && model.InstrumentoLegal.Id != null)
            {
                InstrumentoLegal ins = null;
                if (model.InstrumentoLegal.Id != 0) // Si es un instrumento guardado en la base, lo traigo de ahí
                {
                    ins = DaoProvider.GetDaoInstrumentoLegal().GetById(model.InstrumentoLegal.Id.Value);
                    model.InstrumentoLegal = Mapper.Map<InstrumentoLegal, InstrumentoLegalModel>(ins);
                }
                else
                {
                    ins = Mapper.Map<InstrumentoLegalModel, InstrumentoLegal>(model.InstrumentoLegal);
                }
                //Creo la asignación del instrumento legal
                var tipoMovimientoInstrumentoLegalCierreEmpresa = DaoProvider.GetDaoTipoMovimientoInstrumentoLegal().GetById((int)TipoMovimientoInstrumentoLegalEnum.REAPERTURA_EMPRESA);
                asignacionInstrumentoLegal = new AsignacionInstrumentoLegal()
                {
                    TipoMovimientoInstrumentoLegal = tipoMovimientoInstrumentoLegalCierreEmpresa,
                    Empresa = empresa,
                    InstrumentoLegal = ins,
                    FecAsociacion = DateTime.Today,
                    FecNotificacion = model.FechaNotificacionAsignacionInstrumentoLegal,
                    Observaciones = model.ObservacionesAsignacionInstrumentoLegal
                };
            }
            // Si el instrumento legal y su asignación son obligatorios valido que estén
            if (requiereAsignacionEInstrumentoLegal)
            {
                if (model.InstrumentoLegal == null)// chequear tb que sea válido y que la asignación sea != null y válida
                {
                    throw new BaseException(
                        "El Instrumento Legal y su Asignación deben ser cargados obligatoriamente");
                }
            }
            return asignacionInstrumentoLegal;
        }

        private EmpresaBase ProcesarEscuela(EmpresaReactivacionModel model, EmpresaBase empresa, int idEmpresaUsuarioLogueado)
        {
            var escuela = empresa;
            //Orden de Pago y Programa Presupuestario
            //Busco la empresa asociada al usuario logueado
            EmpresaBase empresaUsuarioLogueado = DaoEmpresa.GetById(idEmpresaUsuarioLogueado);
            if (empresaUsuarioLogueado == null)
            {
                throw new BaseException("No se encontró la Empresa del usuario logueado");
            }
            //Asigno la orden de pago y el programa presupuestario de la dirección de nivel del usuario logueado
            if (empresaUsuarioLogueado.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL)
            {
                escuela.OrdenDePago = empresaUsuarioLogueado.OrdenDePago;
                escuela.ProgramaPresupuestario = empresaUsuarioLogueado.ProgramaPresupuestario;
            }
            else
            {
                throw new BaseException("La Empresa del usuario logueado no posee dirección de nivel.");
            }

            if (empresa.TipoEmpresa != TipoEmpresaEnum.ESCUELA_MADRE) // Si no es escuela Madre, o sea si es escuela anexo
            {
                if (((EscuelaAnexo)escuela).EscuelaMadre.EstadoEmpresa == EstadoEmpresaEnum.CERRADA) // Si la escuela madre está cerrada
                {
                    Escuela madre;
                    if (model.NuevaEscuelaMadre != null)
                    {
                        madre = DaoProvider.GetDaoEscuela().GetById(model.NuevaEscuelaMadre.Value);
                    }
                    else
                    {
                        throw new BaseException("La empresa en reactivación posee su Escuela Madre cerrada, seleccione una nueva Escuela Madre");
                    }
                    if (madre == null)
                    {
                        throw new BaseException("La empresa en reactivación posee su Escuela Madre cerrada, seleccione una nueva Escuela Madre");
                    }
                    if (madre.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
                    {
                        ((Escuela)escuela).EscuelaMadre = madre; //Asigno la nueva escuela madre
                    }
                    else
                    {
                        throw new BaseException("La empresa seleccionada como Madre no es de tipo Madre");
                    }
                    ((EscuelaAnexo)escuela).NumeroEscuela = ((EscuelaAnexo)escuela).EscuelaMadre.NumeroEscuela;
                    escuela.Nombre = model.NombreSugerido;
                }
            }
            return escuela;
        }

        private void RegistrarPlanDeEstudios()
        {
            //TODO: No entra en fase 1
            //Registro Plan de estudios 
            // Este parámetro si está en TRUE significa que se debe pedir OBLIGATORIA los planes de estudio (NO ENTRA EN FASE 1)
            /*var requierePlanEstudio = new EntidadesGeneralesRules().GetValorParametroBooleano(ParametroEnum.PLAN_ESTUDIO_REACTIVACION_EMPRESA);
            IList<EscuelaPlan> planes = model.PlanDeEstudio.Select(Mapper.Map<EscuelaPlanModel, EscuelaPlan>).ToList();
            escuela.EscuelaPlan = planes;
            //Si el plan de estudios es obligatorio y no se cargó informo el problema);)
            if (requierePlanEstudio && escuela.EscuelaPlan.Count == 0)
            {
                throw new BaseException(
                    "El Plan de Estudios debe ser cargado obligatoriamente");
            }*/
        }

        private void RegistrarEstructuraEscolar(EmpresaReactivacionModel model, EmpresaBase escuela)
        {
            // Este parámetro si está en TRUE significa que se debe pedir OBLIGATORIA la Estructura escolar
            var requiereEstructuraEscolar = new EntidadesGeneralesRules().GetValorParametroBooleano(ParametroEnum.ESTRUCTURA_ESCOLAR_EN_REACTIVACIÓN_EMPRESA);

            if (model.EstructuraEscolar != null && model.EstructuraEscolar.Count > 0)
            {
                var diagCursoRules = new DiagramacionCursoRules();
                foreach (var diagramacionCurso in model.EstructuraEscolar)
                {
                    if (diagramacionCurso.Id <= 0) //Si son nuevas, guardo
                    {
                        diagCursoRules.DiagramacionCursoSave(diagramacionCurso);
                    }
                    else // Si ya están en la db actualizo
                    {
                        diagCursoRules.DiagramacionCursoUpdate(diagramacionCurso);
                    }

                }
            }
            else
            {
                //Si la estructura escolar es obligatoria y no se cargó informo el problema)
                if (requiereEstructuraEscolar)
                {
                    throw new BaseException(
                        "La Estructura Escolar debe ser cargada obligatoriamente");
                }

            }
        }

        private AsignacionInspeccionEscuela RegistrarInspeccionEscuela(EmpresaReactivacionModel model, EmpresaBase escuela)
        {
            if (model.IdEmpresaInspeccion == null || model.IdEmpresaInspeccion <= 0)
                throw new BaseException("La asignación de empresa de inspección es obligatoria");
            
            var currentAgente = Mapper.Map<AgenteModel, Agente>(ServiceLocator.Current.GetInstance<IUsuarioRules>().GetCurrentAgente());
            var asignacionInspeccionEscuela = new AsignacionInspeccionEscuela
                                                  {
                                                      Estado = EstadoAsignacionInspeccionEscuelaEnum.PROVISORIO,
                                                      FechaAltaAsignacion = DateTime.Now,
                                                      Escuela = escuela,
                                                      UsuarioAlta = Usuario.CurrentDomain
                                                  };
            var inspeccion = DaoProvider.GetDaoInspeccion().GetById(model.IdEmpresaInspeccion.Value);

            if (inspeccion == null)
                throw new BaseException("No se encontró la Inspección con Id: " + model.IdEmpresaInspeccion +
                                        ". Verifique haber seleccionado una empresa de tipo Inspección");

            asignacionInspeccionEscuela.Inspeccion = inspeccion;
            
            return asignacionInspeccionEscuela;
        }

        private void ProcesarNoEscuela(EmpresaReactivacionModel model, EmpresaBase empresa)
        {
            //Asigno orden de pago y programa presupuestario seleccionados
            empresa.OrdenDePago = DaoProvider.GetDaoOrdenDePago().GetById(model.OrdenDePago);
            empresa.ProgramaPresupuestario =
                DaoProvider.GetDaoProgramaPresupuestario().GetById(model.ProgramaPresupuestario);
        }

        private void PersistirReactivacionEmpresa(EmpresaReactivacionModel model, EmpresaBase empresa, AsignacionInspeccionEscuela asignacionInspeccionEscuela, AsignacionInstrumentoLegal asignacionInstrumentoLegal, List<RegistrarVinculoEmpresaEdificioModel> vinculos)
        {
            foreach (var v in vinculos)
            {
                //Valido los vínculos con la regla. La regla originalmente da una flexibilidad en el model de RegistrarVinculoEmpresaEdificioModel que intento evitar con este método "Unico"
                var modelGuardado = new VinculoEmpresaEdificioRules().VinculoEmpresaEdificioUnicoValidar(v);
                empresa.AddVinculoEdificio(Mapper.Map<VinculoEmpresaEdificioModel, VinculoEmpresaEdificio>(modelGuardado));
            }
            if (empresa is Escuela)
            {
                DaoEscuela.SaveOrUpdate((Escuela)empresa);
            }
            else
            {
                DaoEmpresa.SaveOrUpdate(empresa); // Registro la modificación a la entidad cerrada
            }
            if (asignacionInspeccionEscuela != null)
            {
                DaoProvider.GetDaoAsignacionInspeccionEscuela().SaveOrUpdate(asignacionInspeccionEscuela); // Registro la inspección a escuela
            }
            if (asignacionInstrumentoLegal!=null && asignacionInstrumentoLegal.InstrumentoLegal != null)
            {
                new InstrumentoLegalRules().InstrumentoLegalSave(asignacionInstrumentoLegal.InstrumentoLegal,
                                                                 model.InstrumentoLegal);
                DaoProvider.GetDaoAsignacionInstrumentoLegal().SaveOrUpdate(asignacionInstrumentoLegal);// Registro la asignación instrumento legal
            }
        }

        #endregion

        /// <summary>
        /// Confirma cierre de empresa o lo rechaza y vuelve a activar la empresa
        /// </summary>
        /// <param name="model">Modelo de la empresa a confirmar o rechazar cierre</param>
        /// <returns>Modelo de la empresa confirmada o rechazada su cierre</returns>
        /// <returns>EmpresaVisadoCierreModel</returns>
        public EmpresaVisadoCierreModel VisadoCierreEmpresa(EmpresaVisadoCierreModel model)
        {
            var entidad = DaoEmpresa.GetById(model.IdEmpresa);
            var pedido = new PedidoAutorizacionCierre();
            if (!(entidad.EstadoEmpresa == EstadoEmpresaEnum.EN_PROCESO_DE_CIERRE || entidad.EstadoEmpresa == EstadoEmpresaEnum.CERRADA_SIN_VISADO))
                throw new BaseException(Resources.Empresa.EstadoIncorrecto);
            if (!model.FechaCierreEmpresa.HasValue)
                throw new BaseException("Fecha cierre empresa " + Resources.Empresa.EsDatoRequerido);
            var pedidosAutorizacionCierre =
                    DaoProvider.GetDaoPedidoAutorizacionCierre().GetByFiltros(null, model.IdEmpresa, null, null, null, null);

            if(entidad.TipoEmpresa == TipoEmpresaEnum.INSPECCION)
            {
                var asignacion = DaoProvider.GetDaoAsignacionInspeccionEscuela().GetAsignacionByEscuela(entidad.Id);
                if (asignacion != null)
                {
                    asignacion.Estado = EstadoAsignacionInspeccionEscuelaEnum.NO_VIGENTE;
                    DaoProvider.GetDaoAsignacionInspeccionEscuela().SaveOrUpdate(asignacion);
                }
            }

            if (entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO || entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
            {
                var asignacion = DaoProvider.GetDaoAsignacionInspeccionEscuela().GetAsignacionByEscuela(entidad.Id);
                if (asignacion != null)
                {
                    asignacion.Estado = EstadoAsignacionInspeccionEscuelaEnum.NO_VIGENTE;
                    DaoProvider.GetDaoAsignacionInspeccionEscuela().SaveOrUpdate(asignacion);
                }
                var diagramaciones = DaoProvider.GetDaoDiagramacionCurso().GetByEscuela(entidad.Id);
                if (diagramaciones != null && diagramaciones.Count > 0)
                {
                    foreach (var diagramacion in diagramaciones)
                    {
                        if (diagramacion.EstadoDiagramacionCurso == EstadoDiagramacionCursoEnum.HABILITADA)
                        {
                            diagramacion.EstadoDiagramacionCurso = EstadoDiagramacionCursoEnum.EN_PROCESO_DE_CIERRE;
                            DaoProvider.GetDaoDiagramacionCurso().SaveOrUpdate(diagramacion);
                        }
                    }
                }
                var asignacionesPlanDeEstudio = DaoProvider.GetDaoEscuelaPlan().GetEscuelaPlanByEscuela(entidad.Id);
                if (asignacionesPlanDeEstudio != null && asignacionesPlanDeEstudio.Count > 0)
                {
                    foreach (var asignacionPlanDeEstudio in asignacionesPlanDeEstudio)
                    {
                        asignacionPlanDeEstudio.FechaFinAsignacion = DateTime.Now;
                        DaoProvider.GetDaoEscuelaPlan().SaveOrUpdate(asignacionPlanDeEstudio);
                    }
                }
            }

            if (model.Rechazado)
            {
                entidad.Estado = EstadoEmpresaEnum.AUTORIZADA;
                entidad.HistorialEstados.Add(NuevoEstado(EstadoEmpresaEnum.AUTORIZADA, entidad));
                if (pedidosAutorizacionCierre.Count > 0)
                    pedido = pedidosAutorizacionCierre.Last();
                pedido.AddEstado(
                    new EstadoPedidoAutorizacionCierre()
                        {
                            AgenteAlta = DaoProvider.GetDaoUsuario().GetById(model.AgenteAltaPedidoId),
                            Estado = EstadoPedidoCierreEnum.RECHAZADO,
                            FechaAlta = DateTime.Today,
                            Observaciones = model.ObservacionesRechazo,
                            PedidoCierre = pedido //pedidosAutorizacionCierre[pedidosAutorizacionCierre.Count - 1]
                        });
                entidad.FechaCierre = null;
            }
            else
            {
                entidad.Estado = 
                        model.FechaCierreEmpresa.Value > DateTime.Today
                            ? EstadoEmpresaEnum.EN_PROCESO_DE_CIERRE_AUTORIZADO_NOTIFICADO
                            : EstadoEmpresaEnum.CERRADA;
                entidad.HistorialEstados.Add(
                    NuevoEstado(
                        model.FechaCierreEmpresa.Value > DateTime.Today
                            ? EstadoEmpresaEnum.EN_PROCESO_DE_CIERRE_AUTORIZADO_NOTIFICADO
                            : EstadoEmpresaEnum.CERRADA, entidad));

                if (pedidosAutorizacionCierre.Count > 0 &&
                    pedidosAutorizacionCierre.Last().Estados.Count > 0)
                {
                    pedido = pedidosAutorizacionCierre.Last();
                    pedido.Estados.Last().Estado = EstadoPedidoCierreEnum.APROBADO;
                }

                if (model.FechaCierreEmpresa.Value <= DateTime.Today)
                {
                    var paramDesvincular =
                        new EntidadesGeneralesRules().GetValorParametroBooleano(
                            ParametroEnum.DESVINCULACIÓN_DE_EDIFICIO_EN_CIERRE_DE_EMPRESA);
                    if (paramDesvincular || model.DesvincularEdificio)
                    {
                        entidad.VinculoEmpresaEdificio = DesvincularEmpresaAEdificio(entidad,
                                                                                    DateTime.Now,
                                                                                    "BAJA_EMPRESA");

                        var ordenDePagoCierre = DaoProvider.GetDaoOrdenDePago().GetByNumero(Resources.Empresa.ValorOrdenPagoCierreEmpresa);
                        if (ordenDePagoCierre == null)
                            throw new BaseException("No existe la orden de pago para cierre. Consulte con el administrador.");
                        entidad.OrdenDePago = ordenDePagoCierre;
                    }
                }
                //else LO TENGO QUE GUARDAR EN ALGUN LUGAR HASTA QUE LLEGUE EL MOMENTO DE CERRAR LA EMPRESA
            }
            if (pedido.Id > 0)
                DaoProvider.GetDaoPedidoAutorizacionCierre().SaveOrUpdate(pedido);
            DaoProvider.GetDaoEmpresaBase().SaveOrUpdate(entidad);
            return model;
        }

        private List<VinculoEmpresaEdificio> DesvincularEmpresaAEdificio(EmpresaBase entidad, DateTime fechaHasta, string motivo)
        {
            var listado = (List<VinculoEmpresaEdificio>)entidad.VinculoEmpresaEdificio.ToList();
            foreach (var vinculo in listado)
            {
                vinculo.Estado = EstadoVinculoEmpresaEdificioEnum.INACTIVO;
                vinculo.FechaHasta = fechaHasta;
                vinculo.Motivo = motivo;
            }
            return listado;
        }

        /// <summary>
        /// Cambio el estado de la empresa a autorizada dado el id de la empresa
        /// </summary>
        /// <param name="id">Id de la empresa</param>
        public void ActivarCodigoEmpresa(int id)
        {
            //TODO FEDE: Validar codigo PeopleNet
            var entidad = DaoEmpresa.GetById(id);
            entidad.Estado = EstadoEmpresaEnum.AUTORIZADA;
            entidad.HistorialEstados.Add(NuevoEstado(EstadoEmpresaEnum.AUTORIZADA, entidad));
            DaoEmpresa.SaveOrUpdate(entidad);
        }

        public List<string> VerificarCampos(EmpresaRegistrarModel model)
        {



            //model.EmpresaPadreOrganigramaId = model.EmpresaPadreOrganigramaId ?? 0;
            //model.DomicilioId = model.DomicilioId ?? 0;
            //model.ZonaDesfavorableId = model.ZonaDesfavorableId ?? 0;


            var empresaGuardadaModel = GetEmpresaById(model.Id);
            var camposNecesarios = new List<string>();
            var camposReqInstrLegal = new List<String> { "FechaInicioActividades", "DomicilioId", "EmpresaPadreOrganigramaId" };


            if (model.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE || model.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
            {
                camposReqInstrLegal.AddRange(new List<String> { "Nombre", "EmpresaInspeccionId", "Ambito", "ZonaDesfavorableId", "Albergue", "Dependencia", "TipoJornada", "ContextoDeEncierro", "EsRaiz", "EscuelaRaizId" });
                //model.EmpresaInspeccionId = model.EmpresaInspeccionId ?? 0;
                model.ZonaDesfavorableId = model.ZonaDesfavorableId ?? 0;
                //model.EscuelaRaizId = model.EscuelaRaizId ?? 0;


                if (model.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
                {
                    camposReqInstrLegal.Add("NumeroAnexo");
                }


            }
            if (model.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL)
            {
                camposReqInstrLegal.AddRange(new List<String> { "NivelEducativoId", "Sigla", "CategoriaEscuela", "TipoEducacion", "TipoEscuela" });
                model.NivelEducativoId = model.NivelEducativoId ?? 0;
            }
            if (model.TipoEmpresa == TipoEmpresaEnum.INSPECCION)
                camposReqInstrLegal.Add("TipoInspeccion");

            foreach (var campo in camposReqInstrLegal)
            {

                var prop = typeof(EmpresaRegistrarModel).GetProperty(campo);
                var propModel = prop.GetValue(model, null);
                var propEmpr = prop.GetValue(empresaGuardadaModel, null);

                if (propEmpr != null || propModel != null)
                {
                    if (propEmpr != null && !propEmpr.Equals(propModel))
                        camposNecesarios.Add(campo.EndsWith("Id") ? campo.Substring(0, campo.Length - 2) : campo);
                    else
                        if (propModel != null && !propModel.Equals(propEmpr)) camposNecesarios.Add(campo.EndsWith("Id") ? campo.Substring(0, campo.Length - 2) : campo);

                }
            }
            return camposNecesarios;
        }

        public List<TipoEducacionEnum> GetTiposEducacionByNivelEducativoId(int idNivelEducativo)
        {
            var listado = DaoProvider.GetDaoNivelEducativoPorTipoEducacion().GetByNivelEducativoId(idNivelEducativo);
            if (listado.Count > 0)
                return (from nete in listado select nete.TipoEducacion).ToList<TipoEducacionEnum>();
            return new List<TipoEducacionEnum>();
        }

        public bool EnviarMail(OpcionEnvioMailEnum opcionEnvio, int idEmpresa, out string mensaje)
        {
            try
            {
                bool exitoso = true;
                ValorParametro mailRemitente;
                ValorParametro passRemitente;
                ValorParametro nombreRemitente;
                ValorParametro mailsDestinatario;
                EmpresaBase entidad = DaoProvider.GetDaoEmpresaBase().GetById(idEmpresa);
                if (entidad == null)
                {
                    mensaje = Resources.Empresa.EmpresaNoExistente;
                    return false;
                }
                mailRemitente = passRemitente = nombreRemitente = mailsDestinatario = null;
                switch (opcionEnvio)
                {
                    case OpcionEnvioMailEnum.REACTIVACION:
                        mailRemitente =
                            DaoProvider.GetDaoParametro().GetValorParametroVigente(
                                ParametroEnum.EMAIL_AUTORIZACIÓN_REACTIVACIÓN_DE_EMPRESA, null);
                        passRemitente = null;
                            //TODO:DESA06 NO EXISTE EL PARAMETRO
                            //DaoProvider.GetDaoParametro().GetValorParametroVigente(
                            //    ParametroEnum.PASS_REACTIVACION_EMPRESA, null);
                        nombreRemitente = null;
                            //TODO:DESA06 NO EXISTE EL PARAMETRO
                            //DaoProvider.GetDaoParametro().GetValorParametroVigente(
                            //    ParametroEnum.NOMBRE_EMAIL_REACTIVACION_EMPRESA, null);
                        mailsDestinatario = null;
                            //TODO:DESA06 NO EXISTE EL PARAMETRO
                            //DaoProvider.GetDaoParametro().GetValorParametroVigente(
                            //    ParametroEnum.EMAIL_DESTINATARIOS_REACTIVACION_EMPRESA, null);
                        break;
                    case OpcionEnvioMailEnum.ACTIVACION:
                        mailRemitente =
                            DaoProvider.GetDaoParametro().GetValorParametroVigente(
                                ParametroEnum.EMAIL_ACTIVACIÓN_DE_EMPRESA, null);
                        passRemitente = null;
                            //TODO:DESA06 NO EXISTE EL PARAMETRO
                            //DaoProvider.GetDaoParametro().GetValorParametroVigente(ParametroEnum.PASS_ACTIVACION_EMPRESA, null);
                        nombreRemitente = null;
                            //TODO:DESA06 NO EXISTE EL PARAMETRO
                            //DaoProvider.GetDaoParametro().GetValorParametroVigente(
                            //    ParametroEnum.NOMBRE_EMAIL_ACTIVACION_EMPRESA, null);
                        mailsDestinatario = null;
                            //TODO:DESA06 NO EXISTE EL PARAMETRO
                            //DaoProvider.GetDaoParametro().GetValorParametroVigente(
                            //    ParametroEnum.EMAIL_DESTINATARIOS_ACTIVACION_EMPRESA, null);
                        break;
                    case OpcionEnvioMailEnum.CIERRE:
                        mailRemitente =
                            DaoProvider.GetDaoParametro().GetValorParametroVigente(ParametroEnum.EMAIL_CIERRE_DE_EMPRESA, null);
                        passRemitente = null;
                            //TODO:DESA06 NO EXISTE EL PARAMETRO
                            //DaoProvider.GetDaoParametro().GetValorParametroVigente(ParametroEnum.PASS_CIERRE_EMPRESA, null);
                        nombreRemitente = null;
                            //TODO:DESA06 NO EXISTE EL PARAMETRO
                            //DaoProvider.GetDaoParametro().GetValorParametroVigente(
                            //    ParametroEnum.NOMBRE_EMAIL_CIERRE_EMPRESA, null);
                        mailsDestinatario = null;
                            //TODO:DESA06 NO EXISTE EL PARAMETRO
                            //DaoProvider.GetDaoParametro().GetValorParametroVigente(
                            //    ParametroEnum.EMAIL_DESTINATARIOS_CIERRE_EMPRESA, null);
                        break;
                    case OpcionEnvioMailEnum.PEDIDO_CIERRE:
                        break;
                    default:
                        throw new BaseException(Resources.Empresa.OpcionEnvioNoValida);
                }

                //controlo que esten los datos necesarios para mandar el mail
                StringBuilder errores = new StringBuilder("Falta configurar: ");
                int longitudMinima = errores.Length;
                if (mailRemitente == null || string.IsNullOrEmpty(mailRemitente.Valor))
                    errores.Append("e-mail del remitente");
                if (passRemitente == null || string.IsNullOrEmpty(passRemitente.Valor))
                    errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "password del remitente");
                if (mailsDestinatario == null || string.IsNullOrEmpty(mailsDestinatario.Valor))
                    errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "e-mail(s) destinatario(s)");
                if (errores.Length > longitudMinima)
                    exitoso = false;
                if (exitoso)
                {
                    var entidadesGenerales = new EntidadesGeneralesRules();
                    entidadesGenerales.MandarMail(mailRemitente.Valor, passRemitente.Valor,
                                                  nombreRemitente == null || string.IsNullOrEmpty(nombreRemitente.Valor)
                                                      ? null
                                                      : nombreRemitente.Valor, mailsDestinatario.Valor.Split('|'),
                                                  Resources.Empresa.AsuntoMailActivarEmpresa,
                                                  ObtenerCuerpoMail(entidad, opcionEnvio));
                }
                mensaje = exitoso ? string.Empty : errores.ToString();
                return exitoso;
            }
            catch (Exception)
            {
                mensaje = "No está configurado el envío de correo electrónico. Comuníquese con el administrador.";
                return false;
            }
        }

        /// <summary>
        /// Devuelve listado de los tipos de escuelas permitidos de acuerdo con los tipos de escuelas que puede crear la direccion de nivel del usuario logueado
        /// </summary>
        /// <returns>Lista de modelo de tipo de escuela</returns>
        public List<TipoEscuelaModel> GetTiposEscuelasPermitidos()
        {
            var direccionDeNivelUsuarioActual = GetCurrentEmpresa();
            if (direccionDeNivelUsuarioActual.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL)
            {
                var listadoEscuelasPermitidas =
                    DaoProvider.GetDaoDireccionDeNivel().GetById(direccionDeNivelUsuarioActual.Id).TipoEscuelaACrear;
                if (listadoEscuelasPermitidas.Count == 0)
                    throw new BaseException(Resources.Empresa.DireccionDeNivelSinTipoEscuelaACrear);
                return listadoEscuelasPermitidas.Select(Mapper.Map<TipoEscuela, TipoEscuelaModel>).ToList();
            }
            //si es otro tipo de empresa, devolver los valores de la tabla definida
            List<TipoEscuela> listado = DaoProvider.GetDaoTipoEscuela().GetAll();
            return Mapper.Map<List<TipoEscuela>, List<TipoEscuelaModel>>(listado);
        }

        /// <summary>
        /// Método usado para llenar combo de Tipo de Inspeccion Intermedia con enumeraciones mezcladas con objetos dependientes de la dirección de nivel del usuario logueado 
        /// </summary>
        /// <returns>Lista de modelo para llenar combo</returns>
        public List<TipoInspeccionIntermediaComboModel> GetInspeccionIntermediaByDireccionDeNivelActual()
        {
            var item = new TipoInspeccionIntermediaComboModel();
            var combo = new List<TipoInspeccionIntermediaComboModel>();
            int vuelta = 1;
            foreach (TipoInspeccionEnum enumeracion in Enum.GetValues(typeof(TipoInspeccionEnum)))
            {
                if (enumeracion != TipoInspeccionEnum.OTRA)
                {
                    item = new TipoInspeccionIntermediaComboModel();
                    item.Id =
                        int.Parse(
                            Resources.Empresa.
                                ArtilugioParaDistinguirTipoInspeccionIntermediaEnumDeTipoInspeccionIntermediaObjeto) *
                        vuelta;
                    item.Descripcion = enumeracion.ToString();
                    vuelta++;
                    combo.Add(item);
                }
            }
           
                var direccionDeNivelUsuarioActual = GetCurrentEmpresa();
                if (direccionDeNivelUsuarioActual.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL)
                {

                    var listadoInspeccionIntermedia =
                        DaoProvider.GetDaoTipoInspeccionIntermedia().GetByDireccionDeNivel(
                            direccionDeNivelUsuarioActual.Id);
                    //DaoProvider.GetDaoInspeccion().GetByDireccionNivel(
                    //    DaoProvider.GetDaoDireccionDeNivel().GetById(direccionDeNivelUsuarioActual.Id));
                    //if (listadoInspeccionIntermedia.Count == 0)
                    //    throw new BaseException(Resources.Empresa.SinInspeccionesIntermedia);
                    foreach (var inspeccion in listadoInspeccionIntermedia)
                    {
                        item = new TipoInspeccionIntermediaComboModel();
                        item.Id = inspeccion.Id;
                        item.Descripcion = inspeccion.Nombre;
                        combo.Add(item);
                    }
                }
            
            return combo;
        }

        /// <summary>
        /// Obtiene listado con todas las empresas registradas
        /// </summary>
        /// <returns>Lista con modelo de empresas</returns>
        public List<EmpresaModel> GetEmpresaAll()
        {
            return Mapper.Map<List<EmpresaBase>, List<EmpresaModel>>(DaoEmpresa.GetAll());
        }

        /// <summary>
        /// Obtiene listado de empresa que cumplan los criterio de busqueda basica
        /// </summary>
        /// <param name="cue">Codigo unico de escuela</param>
        /// <param name="codigoEmpresa"></param>
        /// <param name="nombreEmpresa"></param>
        /// <param name="estadoEmpresaEnum"></param>
        /// <param name="idDepartamentoProvincial">Departamento del domicilio oficial</param>
        /// <param name="idLocalidad">Localidad del domicilio oficial</param>
        /// <param name="barrio">Barrio del domicilio oficial</param>
        /// <param name="calle">Calle del domicilio oficial</param>
        /// <param name="altura">Numeracion del domicilio oficial</param>
        /// <returns>Lista de modelo de empresa consultar con las empresas que cumplan el criterio de busqueda</returns>
        public List<EmpresaConsultarModel> GetByFiltroBasico(string cue, int? CUEAnexo, string codigoEmpresa, string nombreEmpresa, List<EstadoEmpresaEnum> estadoEmpresaEnum,
            int? idLocalidad, string barrio, string calle, int? altura, List<TipoEmpresaFiltroBusquedaEnum> tipoEmpresasPermitidas, int? idEmpresaUsuarioLogueado, int? idEmpresaDependintePadre,TipoEmpresaEnum tipoEmpresaUsuarioLogueado,bool seConsultaDesdeRegistrarEmpresa)
        {
            List<EmpresaConsultarModel> listadoEmpresa, listadoEscuela, listadoEscuelaAnexo, listadoInspeccion, listadoDireccionNivelUsuarioLogeado;
            listadoEmpresa = new List<EmpresaConsultarModel>();
            listadoEscuela = new List<EmpresaConsultarModel>();
            listadoEscuelaAnexo = new List<EmpresaConsultarModel>();
            listadoInspeccion = new List<EmpresaConsultarModel>();
            listadoDireccionNivelUsuarioLogeado = new List<EmpresaConsultarModel>();
            var idEmpresaUsuario = seConsultaDesdeRegistrarEmpresa
                                       ? (tipoEmpresaUsuarioLogueado == TipoEmpresaEnum.MINISTERIO
                                              ? null
                                              : idEmpresaUsuarioLogueado)
                                       : null;
              
           
            string cuePorAproximacion = string.IsNullOrEmpty(cue) ? null : "%" + cue.ToUpper() + "%";

            string codigoEmpresaPorAproximacion = string.IsNullOrEmpty(codigoEmpresa)
                                                      ? null
                                                      : "%" + codigoEmpresa.ToUpper() + "%";
            string nombreEmpresaPorAproximacion = string.IsNullOrEmpty(nombreEmpresa)
                                                      ? null
                                                      : "%" + nombreEmpresa.ToUpper() + "%";
            //string barrioPorAproximacion = string.IsNullOrEmpty(barrio) ? null : "%" + barrio.ToUpper() + "%";
            //string callePorAproximacion = string.IsNullOrEmpty(calle) ? null : "%" + calle.ToUpper() + "%";

            if (string.IsNullOrEmpty(cuePorAproximacion) && CUEAnexo == null && tipoEmpresasPermitidas != null &&
                tipoEmpresasPermitidas.Count > 0 &&
                (tipoEmpresasPermitidas.Contains(TipoEmpresaFiltroBusquedaEnum.TODAS) ||
                 tipoEmpresasPermitidas.Contains(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_NIVEL) ||
                 tipoEmpresasPermitidas.Contains(TipoEmpresaFiltroBusquedaEnum.MINISTERIO)))
            {
                listadoEmpresa =
                    Mapper.Map<List<EmpresaBase>, List<EmpresaConsultarModel>>(
                        DaoEmpresa.GetByFiltrosBasico(codigoEmpresaPorAproximacion,
                                                      nombreEmpresaPorAproximacion,
                                                      idLocalidad,
                                                      barrio,
                                                      calle,
                                                      altura,
                                                      estadoEmpresaEnum,
                                                      tipoEmpresasPermitidas,
                                                      null, idEmpresaUsuario));
            }
            if (tipoEmpresasPermitidas != null && tipoEmpresasPermitidas.Count > 0 &&
                tipoEmpresasPermitidas.Contains(TipoEmpresaFiltroBusquedaEnum.INSPECCION))
                listadoInspeccion =
                    Mapper.Map<List<Inspeccion>, List<EmpresaConsultarModel>>(
                        DaoInspeccion.GetByFiltrosBasico(codigoEmpresaPorAproximacion,
                                                         nombreEmpresaPorAproximacion,
                                                         idLocalidad,
                                                         barrio,
                                                         calle,
                                                         altura,
                                                         estadoEmpresaEnum,
                                                         tipoEmpresasPermitidas,
                                                         null,
                                                         null, idEmpresaUsuario, idEmpresaDependintePadre));

            if (tipoEmpresasPermitidas.Contains(TipoEmpresaFiltroBusquedaEnum.DIRECCION_NIVEL_USUARIO_LOGUEADO))
            {
                var dnUsuarioLogueado = GetCurrentEmpresa();
                if (dnUsuarioLogueado.TipoEmpresa==TipoEmpresaEnum.DIRECCION_DE_NIVEL)
                listadoDireccionNivelUsuarioLogeado.Add(new EmpresaConsultarModel()
                                                            {
                                                                Id = dnUsuarioLogueado.Id,
                                                                CodigoEmpresa = dnUsuarioLogueado.CodigoEmpresa,
                                                                FechaAlta = dnUsuarioLogueado.FechaAlta,
                                                                Nombre = dnUsuarioLogueado.Nombre,
                                                                TipoEmpresa = dnUsuarioLogueado.TipoEmpresa,
                                                                EstadoEmpresa = dnUsuarioLogueado.EstadoEmpresa
                                                            });
                else
                {
                    listadoDireccionNivelUsuarioLogeado = Mapper.Map<List<DireccionDeNivel>, List<EmpresaConsultarModel>>(DaoDireccionDeNivel.GetByFiltrosBasico(codigoEmpresaPorAproximacion, nombreEmpresaPorAproximacion,
                                                           idLocalidad, barrio, calle, altura, estadoEmpresaEnum, null,
                                                           idEmpresaUsuario));
                }
            }

            if (tipoEmpresasPermitidas != null && tipoEmpresasPermitidas.Count > 0 &&
                tipoEmpresasPermitidas.Contains(TipoEmpresaFiltroBusquedaEnum.INSPECCION_NO_ZONAL_QUE_PERTENECE_A_DIRECCION_DE_NIVEL_DEL_USUARIO_ACTUAL))
            {
                var direccionDeNivelActual = GetCurrentEmpresa();
                //tipoEmpresasPermitidas.Clear();
                //tipoEmpresasPermitidas.Add(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_NIVEL);
                var tipoInspeccionesPermitidas = new List<TipoInspeccionEnum>()
                                                     {TipoInspeccionEnum.GENERAL, TipoInspeccionEnum.OTRA};
                //if (direccionDeNivelActual.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL)
                //{
                //    var listadoAuxiliar = new List<EmpresaConsultarModel>();
                //        Mapper.Map<EmpresaBase, EmpresaConsultarModel>(
                //            DaoEmpresa.GetById(direccionDeNivelActual.Id));
                //    listadoEmpresa.AddRange(listadoAuxiliar);
                //}
                listadoInspeccion =
                    Mapper.Map<List<Inspeccion>, List<EmpresaConsultarModel>>(
                        DaoInspeccion.GetByFiltrosBasico(codigoEmpresaPorAproximacion,
                                                         nombreEmpresaPorAproximacion,
                                                         idLocalidad,
                                                         barrio,
                                                         calle,
                                                         altura,
                                                         estadoEmpresaEnum,
                                                         tipoEmpresasPermitidas,
                                                         direccionDeNivelActual.TipoEmpresa ==
                                                         TipoEmpresaEnum.DIRECCION_DE_NIVEL
                                                             ? direccionDeNivelActual.Id
                                                             : (int?)null,
                                                         tipoInspeccionesPermitidas, idEmpresaUsuario, idEmpresaDependintePadre));
            }
            if (tipoEmpresasPermitidas != null && tipoEmpresasPermitidas.Count > 0 &&
                tipoEmpresasPermitidas.Contains(
                    TipoEmpresaFiltroBusquedaEnum.INSPECCION_QUE_PERTENECE_A_DIRECCION_DE_NIVEL_DEL_USUARIO_ACTUAL))
            {
                var direccionDeNivelActual = GetCurrentEmpresa();
                tipoEmpresasPermitidas.Clear();
                tipoEmpresasPermitidas.Add(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_NIVEL);
                //la uncia direccion de nivel correcta es la del usuario actual
                if (direccionDeNivelActual.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL)
                {
                    var listadoAuxiliar = new List<EmpresaConsultarModel>();
                    Mapper.Map<EmpresaBase, EmpresaConsultarModel>(
                        DaoEmpresa.GetById(direccionDeNivelActual.Id));
                    listadoEmpresa.AddRange(listadoAuxiliar);
                }
                listadoInspeccion = Mapper.Map<List<Inspeccion>, List<EmpresaConsultarModel>>(
                    DaoInspeccion.GetByFiltrosBasico(codigoEmpresaPorAproximacion,
                                                     nombreEmpresaPorAproximacion,
                                                     idLocalidad,
                                                     barrio,
                                                     calle,
                                                     altura,
                                                     estadoEmpresaEnum,
                                                     tipoEmpresasPermitidas,
                                                     direccionDeNivelActual.TipoEmpresa ==
                                                     TipoEmpresaEnum.DIRECCION_DE_NIVEL
                                                         ? direccionDeNivelActual.Id
                                                         : (int?) null,
                                                     null, idEmpresaUsuario, idEmpresaDependintePadre));
            }
            if (tipoEmpresasPermitidas != null && tipoEmpresasPermitidas.Count > 0 &&
                tipoEmpresasPermitidas.Contains(TipoEmpresaFiltroBusquedaEnum.INSPECCION_ZONAL))
            {
                var direccionDeNivelActual = GetCurrentEmpresa();
                tipoEmpresasPermitidas.Clear();
                tipoEmpresasPermitidas.Add(TipoEmpresaFiltroBusquedaEnum.INSPECCION);
                var tipoInspeccionesPermitidas = new List<TipoInspeccionEnum>() { TipoInspeccionEnum.ZONAL };
                listadoInspeccion =
                    Mapper.Map<List<Inspeccion>, List<EmpresaConsultarModel>>(
                        DaoInspeccion.GetByFiltrosBasico(codigoEmpresaPorAproximacion,
                                                         nombreEmpresaPorAproximacion,
                                                         idLocalidad,
                                                         barrio,
                                                         calle,
                                                         altura,
                                                         estadoEmpresaEnum,
                                                         tipoEmpresasPermitidas,
                                                         direccionDeNivelActual.TipoEmpresa ==
                                                         TipoEmpresaEnum.DIRECCION_DE_NIVEL
                                                             ? direccionDeNivelActual.Id
                                                             : (int?)null,
                                                         tipoInspeccionesPermitidas, idEmpresaUsuario, idEmpresaDependintePadre));
            }
            if (tipoEmpresasPermitidas != null && tipoEmpresasPermitidas.Count > 0 &&
                (tipoEmpresasPermitidas.Contains(TipoEmpresaFiltroBusquedaEnum.TODAS) ||
                 tipoEmpresasPermitidas.Contains(TipoEmpresaFiltroBusquedaEnum.ESCUELA_MADRE) ||
                 tipoEmpresasPermitidas.Contains(TipoEmpresaFiltroBusquedaEnum.ESCUELA_MADRE_RAIZ)))
            {
                bool? esRaiz = null;
                if (tipoEmpresasPermitidas.Contains(TipoEmpresaFiltroBusquedaEnum.ESCUELA_MADRE_RAIZ))
                    esRaiz = true;
                listadoEscuela =
                    Mapper.Map<List<Escuela>, List<EmpresaConsultarModel>>(
                        DaoEscuela.GetByFiltrosBasico(cuePorAproximacion,
                                                      CUEAnexo,
                                                      codigoEmpresaPorAproximacion,
                                                      nombreEmpresaPorAproximacion,
                                                      idLocalidad,
                                                      barrio,
                                                      calle,
                                                      altura,
                                                      estadoEmpresaEnum, esRaiz, idEmpresaUsuario, idEmpresaDependintePadre));
            }

            if (tipoEmpresasPermitidas != null && tipoEmpresasPermitidas.Count > 0 &&
                (tipoEmpresasPermitidas.Contains(TipoEmpresaFiltroBusquedaEnum.TODAS) ||
                 tipoEmpresasPermitidas.Contains(TipoEmpresaFiltroBusquedaEnum.ESCUELA_ANEXO)))
                listadoEscuelaAnexo =
                    Mapper.Map<List<EscuelaAnexo>, List<EmpresaConsultarModel>>(
                        DaoEscuelaAnexo.GetByFiltrosBasico(cuePorAproximacion,
                                                           CUEAnexo,
                                                           codigoEmpresaPorAproximacion,
                                                           nombreEmpresaPorAproximacion,
                                                           idLocalidad,
                                                           barrio,
                                                           calle,
                                                           altura,
                                                           estadoEmpresaEnum, idEmpresaUsuario));
            listadoEmpresa =
                listadoEmpresa.FindAll(
                    x =>
                    x.TipoEmpresa != TipoEmpresaEnum.ESCUELA_MADRE &&
                    x.TipoEmpresa != TipoEmpresaEnum.ESCUELA_ANEXO);
            listadoEmpresa.AddRange(listadoInspeccion);
            listadoEmpresa.AddRange(listadoEscuela);
            listadoEmpresa.AddRange(listadoEscuelaAnexo);
            listadoEmpresa.AddRange(listadoDireccionNivelUsuarioLogeado);
            return listadoEmpresa.OrderBy(i => i.Id).ToList<EmpresaConsultarModel>();
        }

        /// <summary>
        /// Obtiene listado de empresa que cumplan los criterio de busqueda avanzada
        /// </summary>
        /// <param name="fechaAltaDesde"></param>
        /// <param name="fechaAltaHasta"></param>
        /// <param name="fechaDesdeInicioActividad"></param>
        /// <param name="fechaHastaInicioActividad"></param>
        /// <param name="tipoEmpresaEnum"></param>
        /// <param name="idProgramaAdministrativo"></param>
        /// <param name="estadoEmpresaEnum"></param>
        /// <param name="numeroEscuela"></param>
        /// <param name="tipoEscuelaEnum"></param>
        /// <param name="programaPresupuestarioEscuela"></param>
        /// <param name="categoriaEscuelaEnum"></param>
        /// <param name="tipoEducacionEnum"></param>
        /// <param name="nivelEducativoEnum"></param>
        /// <param name="dependenciaEnum"></param>
        /// <param name="ambitoEscuelaEnum"></param>
        /// <param name="esReligioso"></param>
        /// <param name="esArancelado"></param>
        /// <param name="tipoInspeccionEnum"></param>
        /// <param name="idDepartamentoProvincial">Departamento del domicilio oficial</param>
        /// <param name="idLocalidad">Localidad del domicilio oficial</param>
        /// <param name="barrio">Barrio del domicilio oficial</param>
        /// <param name="calle">Calle del domicilio oficial</param>
        /// <param name="altura">Numeracion del domicilio oficial</param>
        /// <param name="idObraSocial">Obra social asignada a la escuela privada</param>
        /// <param name="idPeriodoLectivo"></param>
        /// <returns></returns>
        public List<EmpresaConsultarModel> GetByFiltroAvanzado(DateTime? fechaAltaDesde, DateTime? fechaAltaHasta, DateTime? fechaDesdeInicioActividad,
            DateTime? fechaHastaInicioActividad, TipoEmpresaEnum tipoEmpresaEnum, int? idProgramaAdministrativo, List<EstadoEmpresaEnum> estadoEmpresaEnum,
            int? numeroEscuela, int? tipoEscuelaEnum, int? programaPresupuestarioEscuela, CategoriaEscuelaEnum? categoriaEscuelaEnum,
            TipoEducacionEnum? tipoEducacionEnum, int? nivelEducativoEnum, DependenciaEnum? dependenciaEnum, AmbitoEscuelaEnum? ambitoEscuelaEnum,
            NoSiEnum? esReligioso, NoSiEnum? esArancelado, TipoInspeccionEnum? tipoInspeccionEnum, int? idLocalidad, string barrio,
            string calle, int? altura, int? idObraSocial, int? idPeriodoLectivo, int? turnoEnum, string nombre, DateTime? fechaDesdeNotificacion, 
            DateTime? fechaHastaNotificacion, int? tipoInspeccionIntermediaId, List<TipoEmpresaFiltroBusquedaEnum> listadoTiposEmpresaPermitidos,
            int? idEmpresaUsuarioLogueado, TipoEmpresaEnum tipoEmpresaUsuarioLogueado, bool seConsultaDesdeRegistrarEmpresa, string fltCodigoInspeccion)
        {
            List<EmpresaConsultarModel> listadoEmpresa, listadoEscuela, listadoEscuelaAnexo, listadoInspeccion, listadoDN, listadoDireccionNivelUsuarioLogeado;
            
            listadoEmpresa = listadoEscuela = listadoEscuelaAnexo = listadoInspeccion = listadoDN = new List<EmpresaConsultarModel>();
            listadoDireccionNivelUsuarioLogeado = new List<EmpresaConsultarModel>();
            string barrioPorAproximacion = string.IsNullOrEmpty(barrio) ? null : "%" + barrio + "%";
            string callePorAproximacion = string.IsNullOrEmpty(calle) ? null : "%" + calle + "%";
            string nombrePorAproximacion = string.IsNullOrEmpty(nombre) ? null : "%" + nombre + "%";
            PeriodoLectivo periodo = null;
            Turno turno = null;
            //si la empresa del usuario logueado es miniterio puede ver todas las empresas, sino el usuario va a ser direccion de nivel por lo cual 
            //solo va a poder ver las esculas y las inspecciones que el creo, por lo cual; le pasamos el id de la empresa del usuario logueado
            //para que filtre en funcion de la empresa q registro
            var idEmpresaUsuario = seConsultaDesdeRegistrarEmpresa?(tipoEmpresaUsuarioLogueado == TipoEmpresaEnum.MINISTERIO ? null : idEmpresaUsuarioLogueado):null;
            if (idPeriodoLectivo.HasValue)
            {
                periodo = DaoProvider.GetDaoPeriodoLectivo().GetById(idPeriodoLectivo.Value);
            }
            if (turnoEnum.HasValue)
            {
                turno = DaoProvider.GetDaoTurno().GetById(turnoEnum.Value);
            }
            listadoEmpresa =
                Mapper.Map<List<EmpresaBase>, List<EmpresaConsultarModel>>(DaoEmpresa.GetByFiltroAvanzado(
                    fechaAltaDesde,
                    fechaAltaHasta,
                    fechaDesdeInicioActividad,
                    fechaHastaInicioActividad,
                    tipoEmpresaEnum,
                    idProgramaAdministrativo,
                    estadoEmpresaEnum,
                    idLocalidad,
                    barrioPorAproximacion,
                    callePorAproximacion,
                    altura, nombrePorAproximacion, fechaDesdeNotificacion, fechaHastaNotificacion, idEmpresaUsuario));

            if (tipoEmpresaEnum == TipoEmpresaEnum.ESCUELA_MADRE)

                listadoEscuela =
                    Mapper.Map<List<Escuela>, List<EmpresaConsultarModel>>(DaoEscuela.GetByFiltroAvanzado(
                        fechaAltaDesde,
                        fechaAltaHasta,
                        fechaDesdeInicioActividad,
                        fechaHastaInicioActividad,
                        tipoEmpresaEnum,
                        numeroEscuela,
                        tipoEscuelaEnum,
                        categoriaEscuelaEnum,
                        tipoEducacionEnum,
                        nivelEducativoEnum,
                        dependenciaEnum,
                        ambitoEscuelaEnum,
                        esReligioso.HasValue ? esReligioso.Value == NoSiEnum.SI ? true : false : (bool?)null,
                        esArancelado.HasValue ? esArancelado.Value == NoSiEnum.SI ? true : false : (bool?)null,
                        tipoInspeccionEnum,
                        idLocalidad,
                        barrioPorAproximacion,
                        callePorAproximacion,
                        altura,
                        estadoEmpresaEnum,
                        idObraSocial,
                        periodo,
                        turno, nombrePorAproximacion, fechaDesdeNotificacion, fechaHastaNotificacion, idEmpresaUsuario, fltCodigoInspeccion));
            if (tipoEmpresaEnum == TipoEmpresaEnum.ESCUELA_ANEXO)
                listadoEscuelaAnexo =
                    Mapper.Map<List<EscuelaAnexo>, List<EmpresaConsultarModel>>(DaoEscuelaAnexo.GetByFiltroAvanzado(
                        fechaAltaDesde,
                        fechaAltaHasta,
                        fechaDesdeInicioActividad,
                        fechaHastaInicioActividad,
                        tipoEmpresaEnum,
                        numeroEscuela,
                        tipoEscuelaEnum,
                        categoriaEscuelaEnum,
                        tipoEducacionEnum,
                        nivelEducativoEnum,
                        dependenciaEnum,
                        ambitoEscuelaEnum,
                        esReligioso.HasValue ? esReligioso.Value == NoSiEnum.SI ? true : false : (bool?)null,
                        esArancelado.HasValue ? esArancelado.Value == NoSiEnum.SI ? true : false : (bool?)null,
                        tipoInspeccionEnum,
                        idLocalidad,
                        barrioPorAproximacion,
                        callePorAproximacion,
                        altura,
                        estadoEmpresaEnum,
                        idObraSocial,
                        periodo,
                        turno, nombrePorAproximacion, fechaDesdeNotificacion, fechaHastaNotificacion, idEmpresaUsuario, fltCodigoInspeccion));
            
            if (tipoEmpresaEnum == TipoEmpresaEnum.INSPECCION)
            {
                if (listadoTiposEmpresaPermitidos != null)
                {
                    if(listadoTiposEmpresaPermitidos.Contains(TipoEmpresaFiltroBusquedaEnum.INSPECCION_ZONAL))
                    {
                        listadoInspeccion =
                            Mapper.Map<List<Inspeccion>, List<EmpresaConsultarModel>>(
                                DaoInspeccion.GetByFiltroAvanzado(fechaAltaDesde,
                                                                  fechaAltaHasta,
                                                                  fechaDesdeInicioActividad,
                                                                  fechaHastaInicioActividad,
                                                                  tipoEmpresaEnum,
                                                                  new List<TipoInspeccionEnum> { TipoInspeccionEnum.ZONAL },
                                                                  null,
                                                                  idLocalidad,
                                                                  barrioPorAproximacion,
                                                                  callePorAproximacion,
                                                                  altura,
                                                                  estadoEmpresaEnum, nombrePorAproximacion,
                                                                  fechaDesdeNotificacion, fechaHastaNotificacion, tipoInspeccionIntermediaId, idEmpresaUsuario));
                    }
                    else
                    {
                        var tipoInspeccion = tipoInspeccionEnum.HasValue
                                                 ? new List<TipoInspeccionEnum> {tipoInspeccionEnum.Value}
                                                 : new List<TipoInspeccionEnum>
                                                       {TipoInspeccionEnum.GENERAL, TipoInspeccionEnum.OTRA};
                        listadoInspeccion =
                            Mapper.Map<List<Inspeccion>, List<EmpresaConsultarModel>>(
                                DaoInspeccion.GetByFiltroAvanzado(fechaAltaDesde,
                                                                  fechaAltaHasta,
                                                                  fechaDesdeInicioActividad,
                                                                  fechaHastaInicioActividad,
                                                                  tipoEmpresaEnum,
                                                                  tipoInspeccion,
                                                                  null,
                                                                  idLocalidad,
                                                                  barrioPorAproximacion,
                                                                  callePorAproximacion,
                                                                  altura,
                                                                  estadoEmpresaEnum, nombrePorAproximacion,
                                                                  fechaDesdeNotificacion, fechaHastaNotificacion, tipoInspeccionIntermediaId, idEmpresaUsuario));
                    }
                }
            }

            //si esto da true, es por q el tipo de inspeccion es general y solo debe mostrar la DN del usuario
            if (listadoTiposEmpresaPermitidos != null)
            {
                if (listadoTiposEmpresaPermitidos.Contains(TipoEmpresaFiltroBusquedaEnum.DIRECCION_NIVEL_USUARIO_LOGUEADO))
                {
                    var dnUsuarioLogueado = GetCurrentEmpresa();
                    listadoDireccionNivelUsuarioLogeado.Add(new EmpresaConsultarModel()
                    {
                        Id = dnUsuarioLogueado.Id,
                        CodigoEmpresa = dnUsuarioLogueado.CodigoEmpresa,
                        FechaAlta = dnUsuarioLogueado.FechaAlta,
                        Nombre = dnUsuarioLogueado.Nombre,
                        TipoEmpresa = dnUsuarioLogueado.TipoEmpresa,
                        EstadoEmpresa = dnUsuarioLogueado.EstadoEmpresa
                    });
                }
                else {
                    if (tipoEmpresaEnum == TipoEmpresaEnum.DIRECCION_DE_NIVEL)
                    {
                        listadoDN =
                            Mapper.Map<List<DireccionDeNivel>, List<EmpresaConsultarModel>>(
                                DaoDireccionDeNivel.GetByFiltroAvanzado(fechaAltaDesde,
                                                                        fechaAltaHasta,
                                                                        fechaDesdeInicioActividad,
                                                                        fechaHastaInicioActividad,
                                                                        tipoEmpresaEnum,
                                                                        idProgramaAdministrativo,
                                                                        estadoEmpresaEnum,
                                                                        idLocalidad,
                                                                        barrioPorAproximacion,
                                                                        callePorAproximacion,
                                                                        altura,
                                                                        tipoEducacionEnum,
                                                                        nivelEducativoEnum, nombrePorAproximacion,
                                                                        fechaDesdeNotificacion, fechaHastaNotificacion, idEmpresaUsuario));
                    }
                }
            }

            listadoEmpresa.AddRange(listadoEscuela);
            listadoEmpresa.AddRange(listadoEscuelaAnexo);
            listadoEmpresa.AddRange(listadoInspeccion);
            listadoEmpresa.AddRange(listadoDN);
            listadoEmpresa.AddRange(listadoDireccionNivelUsuarioLogeado);
            return listadoEmpresa.OrderBy(x => x.Id).ToList<EmpresaConsultarModel>();
        }

        /// <summary>
        /// Obtiene empresa del usuario logueado
        /// </summary>
        /// <returns>Modelo de empresa</returns>
        public EmpresaModel GetCurrentEmpresa()
        {
            var usuario = Usuario.Current;
            EmpresaBase entidad = null;

            if (usuario != null && usuario.RolActual != null)
                entidad = DaoEmpresa.GetById(usuario.RolActual.EmpresaId);
            
            return Mapper.Map<EmpresaBase, EmpresaModel>(entidad);
        }

        /// <summary>
        /// Obtiene empresa del usuario logueado
        /// </summary>
        /// <returns>Modelo de empresa</returns>
        public EscuelaModel GetEscuelaById(int id)
        {
            var tipo = DaoEmpresa.GetTipoEmpresaById(id);
            EscuelaModel escuelaModel = null;
            if (tipo.TipoEmpresa ==TipoEmpresaEnum.ESCUELA_ANEXO)
            {
                var escuelaAnexo = DaoProvider.GetDaoEscuelaAnexo().GetById(id);
                escuelaModel = Mapper.Map<EscuelaAnexo, EscuelaModel>(escuelaAnexo);
            }

            if (tipo.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
            {
                var escuela = DaoProvider.GetDaoEscuela().GetById(id);
                escuelaModel = Mapper.Map<Escuela, EscuelaModel>(escuela);
            }

            if (escuelaModel == null)
                throw new BaseException("La empresa no es del tipo de gestion de escuela");

            return escuelaModel;
        }

        public EscuelaModel GetEscuelaAnexoById(int id)
        {
            return Mapper.Map<EscuelaAnexo, EscuelaModel>(DaoEscuelaAnexo.GetById(id));
        }
        /// <summary>
        /// Listado de empresas que cumplen el criterio de inspeccionadas por un tipo de inspeccion dado
        /// </summary>
        /// <param name="tipoInspeccion">Enumeracion con el tipo de inspeccion</param>
        /// <returns>Lista de modelo de empresa con empresas que cumpkan el criterio de busqueda</returns>
        public List<EmpresaModel> GetEmpresasByTipoInspeccion(TipoInspeccionEnum tipoInspeccion, int idEmpresaUsuarioLogueado)
        {
            return Mapper.Map<List<EmpresaBase>, List<EmpresaModel>>(DaoEmpresa.GetByFiltroAvanzado(null, null, null, null, TipoEmpresaEnum.ESCUELA_MADRE, null, new List<EstadoEmpresaEnum>(), null, null, null, null, null, null, null, idEmpresaUsuarioLogueado));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empresa"></param>
        public TipoEmpresaEnum GetTipoEmpresaById(int idEmpresa)
        {
            var tipoEmpresa = DaoEmpresa.GetTipoEmpresaById(idEmpresa);
            return tipoEmpresa.TipoEmpresa;
        }

        /// <summary>
        /// Actualiza la asignacion de inspeccion de la escuela dada como vigente
        /// </summary>
        /// <param name="empresa">Escuela a aceptar activacion</param>
        private void EmpresaAceptarActivacion(EmpresaBase empresa)
        {
            AsignacionInspeccionEscuela inspeccion;

            if (empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO || empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
            {
                //El sistema registra la asignación inspección a escuela en estado: Vigente
                var daoAsignacionInspeccionEscuela = DaoProvider.GetDaoAsignacionInspeccionEscuela();
                inspeccion = daoAsignacionInspeccionEscuela.GetVigenteByEscuela(empresa.Id);
                if (inspeccion != null)
                {
                    inspeccion.Estado = EstadoAsignacionInspeccionEscuelaEnum.VIGENTE;
                    daoAsignacionInspeccionEscuela.SaveOrUpdate(inspeccion);
                    DaoProvider.GetDaoAsignacionInspeccionEscuela().SaveOrUpdate(inspeccion);
                }

                // El sistema registra el/los vínculos empresa a edificio en estado: Activo.
                //TODO: registrar los vinculos de empresa-edificio
            }
        }

        /// <summary>
        /// Actualiza la asignacion de inspeccion de la escuela dada como NO vigente
        /// </summary>
        /// <param name="empresa">Escuela a rechazar activacion</param>
        private void EmpresaRechazarActivacion(EmpresaBase empresa)
        {
            AsignacionInspeccionEscuela inspeccion;

            if (empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO || empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
            {
                //TODO: poner los vinculos de empresa-edificio como no vigentes

                //El sistema registra la asignación inspección a escuela en estado: No Vigente. 
                var daoAsignacionInspeccionEscuela = DaoProvider.GetDaoAsignacionInspeccionEscuela();
                inspeccion = daoAsignacionInspeccionEscuela.GetVigenteByEscuela(empresa.Id);
                if (inspeccion != null)
                {
                    inspeccion.Estado = EstadoAsignacionInspeccionEscuelaEnum.NO_VIGENTE;
                    daoAsignacionInspeccionEscuela.SaveOrUpdate(inspeccion);
                }
            }
        }

        public void VerificarExistenciaInstrumentoLegalByNro(string numeroInstrumentoLegal)
        {
            if (DaoProvider.GetDaoInstrumentoLegal().ExisteInstrumento(numeroInstrumentoLegal, null))
                throw new BaseException(Resources.Empresa.NumeroDeInstrumentoLegalExistente);
        }



        

        /// <summary>
        /// Modifica la asignacion de las escuelas a inspeccionar
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Retorna una lista de asignaciones escuela a inspeccion.</returns>
        public List<AsignacionInspeccionEscuelaModel> ModificarAsignacionEscuelaAInspeccion(List<AsignacionInspeccionEscuelaModel> model)
        {
            AsignacionInspeccionEscuela asignacion;
            EmpresaBase escuela;
            var currentAgente = Mapper.Map<AgenteModel, Agente>(ServiceLocator.Current.GetInstance<IUsuarioRules>().GetCurrentAgente());
            var listaAsignaciones = new List<AsignacionInspeccionEscuela>();
            foreach (var aie in model)
            {
                escuela = DaoProvider.GetDaoEmpresaBase().GetById(aie.EscuelaId.Value);
                var asignacionEnEscuela = DaoProvider.GetDaoAsignacionInspeccionEscuela().GetVigenteByEscuela(escuela.Id);
                if (asignacionEnEscuela != null)
                {
                    //modifico la asignacion cambiandole el estado
                    asignacionEnEscuela.FechaBaja = DateTime.Today;
                    asignacionEnEscuela.Estado = EstadoAsignacionInspeccionEscuelaEnum.NO_VIGENTE;
                    asignacionEnEscuela.UsuarioBaja = Usuario.CurrentDomain;
                    asignacionEnEscuela.Inspeccion.Id = aie.InspeccionId.Value;
                    listaAsignaciones.Add(asignacionEnEscuela);
                }

                //si no tiene asignaciones, creo una con los datos de la nueva
                asignacion = new AsignacionInspeccionEscuela();
                asignacion.Escuela = escuela;
                asignacion.FechaAltaAsignacion = DateTime.Today;
                asignacion.UsuarioAlta = Usuario.CurrentDomain;
                asignacion.Estado = EstadoAsignacionInspeccionEscuelaEnum.VIGENTE;
                asignacion.Inspeccion = DaoProvider.GetDaoInspeccion().GetById(aie.InspeccionId.Value);
                listaAsignaciones.Add(asignacion);
            }

            var listado = new List<AsignacionInspeccionEscuela>();

            foreach (var asignacionInspeccionEscuela in listaAsignaciones)
                listado.Add(DaoProvider.GetDaoAsignacionInspeccionEscuela().SaveOrUpdate(asignacionInspeccionEscuela));

            return Mapper.Map<List<AsignacionInspeccionEscuela>, List<AsignacionInspeccionEscuelaModel>>(listado);
        }

        public ResolucionModel RegistrarResolucionVinculadaAEmpresa(ResolucionModel model)
        {
            if (model.CantidadDeArticulos > 5)
                throw new BaseException("Se deben registrar como máximo 5 articulos");
            
            InstrumentoLegalModel instrumentoLegalModel = null;
            if (model.InstrumentoLegalResolucion != null && model.InstrumentoLegalResolucion.InstrumentoLegal != null)
            {
                if(model.InstrumentoLegalResolucion.InstrumentoLegal.Id <= 0)
                {
                    instrumentoLegalModel = ReglaInstrumentoLegal.InstrumentoLegalSave(model.InstrumentoLegalResolucion.InstrumentoLegal);
                }
                else
                {
                    instrumentoLegalModel = model.InstrumentoLegalResolucion.InstrumentoLegal;
                }
            }
            var resolucion = new Resolucion()
                                 {
                                     ArticulosResolucion = new List<ArticuloResolucion>(),
                                     CantidadDeArticulos = model.ArticulosResolucion.Count,
                                     Considerando = model.Considerando,
                                     ObservacionesResolucion = model.ObservacionesResolucion,
                                     Protocolicese = model.Protocolicese,
                                     Visto = model.Visto,
                                     InstrumentoLegal = Mapper.Map<InstrumentoLegalModel, InstrumentoLegal>(instrumentoLegalModel)
            };

            foreach (var art in model.ArticulosResolucion)
            {
                var articuloResolucion = new ArticuloResolucion()
                                             {
                                                 Numero = art.Numero,
                                                 Detalle = art.Detalle,
                                                 Resolucion = resolucion
                                             };
                resolucion.AddArticulo(articuloResolucion);
            }

            ValidarResolucion(resolucion);

            var resol = DaoProvider.GetDaoResolucion().Save(resolucion);
            return Mapper.Map<Resolucion, ResolucionModel>(resol);
        }

        private void ValidarResolucion(Resolucion resolucion)
        {
            if (resolucion == null)
                throw new BaseException(Resources.Empresa.ResolucionRequerida);
            if (resolucion.InstrumentoLegal == null)
                throw new BaseException(Resources.Empresa.InstrumentoLegalRequerido);
            if (resolucion.ArticulosResolucion == null)
                throw new BaseException(Resources.Empresa.ResolucionRequerida);
            if (resolucion.CantidadDeArticulos <= 0)
                throw new BaseException(Resources.Empresa.ResolucionSinArticulos);
            if (string.IsNullOrEmpty(resolucion.Considerando.Trim()) || string.IsNullOrEmpty(resolucion.Visto.Trim()) ||
                string.IsNullOrEmpty(resolucion.Protocolicese.Trim()))
                throw new BaseException(Resources.Empresa.ResolucionIncompleta);
            if ((from a in resolucion.ArticulosResolucion where string.IsNullOrEmpty(a.Detalle.Trim()) || a.Numero <= 0 select a).ToList<ArticuloResolucion>().Count > 0)
                throw new BaseException(Resources.Empresa.ArticuloIncompleto);
        }

        private void VincularEmpresaAEdificio()
        {
            //TODO: ver como hacer
        }

        public string NombreSugeridoParaEscuelas(int idEmpresa)
        {
            var modelo = new EmpresaRegistrarModel();
            var entidad = DaoEmpresa.GetById(idEmpresa);
            if (entidad != null)
            {
                if (entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
                    modelo = Mapper.Map<Escuela, EmpresaRegistrarModel>(DaoProvider.GetDaoEscuela().GetById(idEmpresa));
                else if (entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
                    modelo =
                        Mapper.Map<EscuelaAnexo, EmpresaRegistrarModel>(
                            DaoProvider.GetDaoEscuelaAnexo().GetById(idEmpresa));
                return SugerirNombreEscuelas(modelo.DomicilioId,modelo.TipoEscuela,modelo.EscuelaRaizId,modelo.EscuelaMadreId,modelo.TipoEmpresa,modelo.NumeroAnexo);
            }
            throw new BaseException("No se encontró una Escuela con ID: " + idEmpresa + " para poder sugerir el nombre");
        }



        public string SugerirNombreEscuelas(long? domicilioId, int? tipoEscuelaId, int? escuelaRaizId, int? escuelaMadreId, TipoEmpresaEnum? tipoEmpresa, int? numeroEscuelaAnexo)
        {
            StringBuilder mensajeDeErrorDatosFaltantes = new StringBuilder("Para sugerir nombre, se requieren los siguientes datos: "); //61 caracteres
            int longitudMinima = mensajeDeErrorDatosFaltantes.Length;
            if (!domicilioId.HasValue)
                mensajeDeErrorDatosFaltantes.Append("Seleccione domicilio.");
            if (!tipoEscuelaId.HasValue)
                mensajeDeErrorDatosFaltantes.Append("Seleccione tipo de escuela.");
            if (mensajeDeErrorDatosFaltantes.Length > longitudMinima)
                throw new BaseException(mensajeDeErrorDatosFaltantes.ToString());

            var tipoEscuela = tipoEscuelaId.HasValue
                                  ? DaoProvider.GetDaoTipoEscuela().GetById(tipoEscuelaId.Value)
                                  : null;
      
            var escuelaMadre =escuelaMadreId.HasValue
                                  ? DaoProvider.GetDaoEscuela().GetById(escuelaMadreId.Value)
                                  : null;
            string valorRetorno = string.Empty;
            var domicilio = domicilioId.HasValue ? DaoProvider.GetDaoDomicilio().GetById(domicilioId.Value) : null;
            var localidad = domicilio != null ? domicilio.Localidad : null;
            if (localidad == null)
                throw new BaseException(Resources.EntidadesGenerales.LocalidadNoValida);

            if (tipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
            {
                if (escuelaRaizId.HasValue)
                    valorRetorno = tipoEscuela.Abreviatura + " Nº " +
                                   (GetCantidadDeEscuelasPorTipoEnLocalidad(localidad.Id, tipoEscuela.Id) + 1).
                                       ToString();
                else
                {
                    throw new BaseException(Resources.Empresa.NoSeAsocioEscuelaRaiz);

                }
            }
            else if (tipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
            {
                if (escuelaMadre == null)
                    throw new BaseException(Resources.Empresa.NoSeAsocioEscuelaMadre);

                valorRetorno = escuelaMadre.Nombre + (tipoEscuela.Id == int.Parse(Resources.Empresa.IdTipoEscuelaEspecial) ? " ESCUELA ESPECIAL" : string.Empty) +
                               " - Anexo Nº " + numeroEscuelaAnexo.ToString() + " - " +
                               localidad.Nombre;
            }
            return valorRetorno;
        }

        public PedidoAutorizacionCierreModel GetPedidoAutorizacionCierreById(int id)
        {
            return Mapper.Map<PedidoAutorizacionCierre, PedidoAutorizacionCierreModel>(DaoProvider.GetDaoPedidoAutorizacionCierre().GetById(id));
        }

        public EmpresaVisadoCierreModel GetPedidoAutorizacionCierreVisadoById(int id)
        {
            return Mapper.Map<PedidoAutorizacionCierre, EmpresaVisadoCierreModel>(DaoProvider.GetDaoPedidoAutorizacionCierre().GetById(id));
        }

        public ValorParametroModel GetParametroBooleanoJerarquiaInspeccionOrganigrama()
        {
            return Mapper.Map<ValorParametro, ValorParametroModel>(DaoProvider.GetDaoParametro().GetValorParametroVigente(ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA, null));
        }

        public List<PedidoAutorizacionCierreModel> GetPedidosAutorizacionCierre(string cue, int? cueAnexo, string codigoEmpresa, int? nroEscuela,
                                                  int? nroPedidoAutorizacion, DateTime? fechaDesde, DateTime? fechaHasta,
                                                  EstadoPedidoCierreEnum? estadoPedido, DateTime? fechaCierreEmpresaDesde, DateTime? fechaCierreEmpresaHasta)
        {
            
            var listado = DaoProvider.GetDaoPedidoAutorizacionCierre().GetByFiltrosEmpresa(cue,cueAnexo, codigoEmpresa, nroEscuela,
                                                                             nroPedidoAutorizacion, fechaDesde,
                                                                             fechaHasta,fechaCierreEmpresaDesde,fechaCierreEmpresaHasta, estadoPedido);
            return Mapper.Map<List<PedidoAutorizacionCierre>, List<PedidoAutorizacionCierreModel>>(listado);
        }

        public HistorialEmpresaModel GetHistorialEmpresaById(int id)
        {
            var historial = DaoProvider.GetDaoHistorialEmpresa().GetById(id);
            switch (historial.Empresa.TipoEmpresa)
            {
                case TipoEmpresaEnum.MINISTERIO:
                case TipoEmpresaEnum.DIRECCION_DE_INFRAESTRUCTURA:
                case TipoEmpresaEnum.DIRECCION_DE_RECURSOS_HUMANOS:
                case TipoEmpresaEnum.DIRECCION_DE_SISTEMAS:
                case TipoEmpresaEnum.DIRECCION_DE_TESORERIA:
                case TipoEmpresaEnum.SECRETARIA:
                case TipoEmpresaEnum.SUBSECRETARIA:
                case TipoEmpresaEnum.APOYO_ADMINISTRATIVO:
                    return Mapper.Map<HistorialEmpresa, HistorialEmpresaModel>(historial);
                    break;
                case TipoEmpresaEnum.DIRECCION_DE_NIVEL:
                    var historialDN = DaoProvider.GetDaoHistorialDireccionDeNivel().GetById(id);
                    return Mapper.Map<HistorialEmpresa, HistorialEmpresaModel>(historialDN);
                    break;
                case TipoEmpresaEnum.INSPECCION:
                    var historialInspeccion = DaoProvider.GetDaoHistorialInspeccion().GetById(id);
                    return Mapper.Map<HistorialInspeccion, HistorialEmpresaModel>(historialInspeccion);
                    break;
                case TipoEmpresaEnum.ESCUELA_MADRE:
                case TipoEmpresaEnum.ESCUELA_ANEXO:
                    var historialEscuela = DaoProvider.GetDaoHistorialEscuela().GetById(id);
                    return Mapper.Map<HistorialEmpresa, HistorialEmpresaModel>(historialEscuela);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public List<HistorialesEmpresaModel> ProcesarHistorial(int idEmpresa)
        {
            return
                Mapper.Map<List<HistorialEmpresa>, List<HistorialesEmpresaModel>>(
                    DaoProvider.GetDaoHistorialEmpresa().GetByEmpresaId(idEmpresa));
        }

        public HistorialesEmpresaModel GetHistorialById(int idHistorial)
        {
            var historial = DaoProvider.GetDaoHistorialEmpresa().GetById(idHistorial);
            HistorialesEmpresaModel retorno;
            switch (historial.TipoEmpresa)
            {
                case TipoEmpresaEnum.MINISTERIO:
                case TipoEmpresaEnum.DIRECCION_DE_INFRAESTRUCTURA:
                case TipoEmpresaEnum.DIRECCION_DE_RECURSOS_HUMANOS:
                case TipoEmpresaEnum.DIRECCION_DE_SISTEMAS:
                case TipoEmpresaEnum.DIRECCION_DE_TESORERIA:
                case TipoEmpresaEnum.SECRETARIA:
                case TipoEmpresaEnum.SUBSECRETARIA:
                case TipoEmpresaEnum.APOYO_ADMINISTRATIVO:
                    return Mapper.Map<HistorialEmpresa, HistorialesEmpresaModel>(historial);
                    break;
                case TipoEmpresaEnum.DIRECCION_DE_NIVEL:
                    return
                        Mapper.Map<HistorialDireccionDeNivel, HistorialesEmpresaModel>(
                            DaoProvider.GetDaoHistorialDireccionDeNivel().GetById(idHistorial));
                    break;
                case TipoEmpresaEnum.INSPECCION:
                    return
                        Mapper.Map<HistorialInspeccion, HistorialesEmpresaModel>(
                            DaoProvider.GetDaoHistorialInspeccion().GetById(idHistorial));
                    break;
                case TipoEmpresaEnum.ESCUELA_MADRE:
                case TipoEmpresaEnum.ESCUELA_ANEXO:
                    return
                        Mapper.Map<HistorialEscuela, HistorialesEmpresaModel>(
                            DaoProvider.GetDaoHistorialEscuela().GetById(idHistorial));
                    break;
                default:
                    throw new BaseException();
            }
        }

        public void ValidarEmpresaPadre(TipoEmpresaEnum empresaARegistrar, TipoEmpresaEnum empresaPadre)
        {
            var entidadesGeneralesRules = new EntidadesGeneralesRules();
            var ret = false;
            string empresaPadreCorrecta = null;
            switch (empresaARegistrar)
            {
                case TipoEmpresaEnum.MINISTERIO:
                    throw new BaseException("Empresa padre no aplica.");
                    break;
                case TipoEmpresaEnum.DIRECCION_DE_INFRAESTRUCTURA:
                case TipoEmpresaEnum.DIRECCION_DE_RECURSOS_HUMANOS:
                case TipoEmpresaEnum.DIRECCION_DE_SISTEMAS:
                case TipoEmpresaEnum.DIRECCION_DE_TESORERIA:
                    ret = empresaPadre == TipoEmpresaEnum.MINISTERIO;
                    empresaPadreCorrecta = TipoEmpresaEnum.MINISTERIO.ToString().Replace("_", " ");
                    break;
                case TipoEmpresaEnum.SECRETARIA:
                    ret = (empresaPadre == TipoEmpresaEnum.MINISTERIO
                           || empresaPadre == TipoEmpresaEnum.DIRECCION_DE_INFRAESTRUCTURA
                           || empresaPadre == TipoEmpresaEnum.DIRECCION_DE_RECURSOS_HUMANOS
                           || empresaPadre == TipoEmpresaEnum.DIRECCION_DE_SISTEMAS
                           || empresaPadre == TipoEmpresaEnum.DIRECCION_DE_TESORERIA);
                    empresaPadreCorrecta = TipoEmpresaEnum.MINISTERIO.ToString().Replace("_", " ") + ", " +
                                           TipoEmpresaEnum.DIRECCION_DE_INFRAESTRUCTURA.ToString().Replace("_", " ") +
                                           (", ") +
                                           TipoEmpresaEnum.DIRECCION_DE_RECURSOS_HUMANOS.ToString().Replace("_", " ") +
                                           (", ") +
                                           TipoEmpresaEnum.DIRECCION_DE_SISTEMAS.ToString().Replace("_", " ") + (", ") +
                                           TipoEmpresaEnum.DIRECCION_DE_TESORERIA.ToString().Replace("_", " ");
                    break;
                case TipoEmpresaEnum.SUBSECRETARIA:
                    ret = (empresaPadre == TipoEmpresaEnum.SECRETARIA ||
                            empresaPadre == TipoEmpresaEnum.DIRECCION_DE_NIVEL);
                    empresaPadreCorrecta = TipoEmpresaEnum.SECRETARIA.ToString().Replace("_", " ") + " ó " +
                                           TipoEmpresaEnum.DIRECCION_DE_NIVEL.ToString().Replace("_", " ");
                    break;
                case TipoEmpresaEnum.APOYO_ADMINISTRATIVO:
                    ret = (empresaPadre == TipoEmpresaEnum.DIRECCION_DE_NIVEL ||
                            empresaPadre == TipoEmpresaEnum.INSPECCION);
                    empresaPadreCorrecta = TipoEmpresaEnum.DIRECCION_DE_NIVEL.ToString().Replace("_", " ") + "ó" +
                                           TipoEmpresaEnum.INSPECCION.ToString().Replace("_", " ");
                    break;
                case TipoEmpresaEnum.DIRECCION_DE_NIVEL:
                    ret = (empresaPadre == TipoEmpresaEnum.MINISTERIO);
                    empresaPadreCorrecta = TipoEmpresaEnum.MINISTERIO.ToString().Replace("_", " ");
                    break;
                case TipoEmpresaEnum.INSPECCION:
                    ret = (empresaPadre == TipoEmpresaEnum.DIRECCION_DE_NIVEL ||
                            empresaPadre == TipoEmpresaEnum.INSPECCION);
                    empresaPadreCorrecta = TipoEmpresaEnum.DIRECCION_DE_NIVEL.ToString().Replace("_", " ") + " ó " +
                                           TipoEmpresaEnum.INSPECCION.ToString().Replace("_", " ");
                    break;
                case TipoEmpresaEnum.ESCUELA_MADRE:
                    if (entidadesGeneralesRules.GetValorParametroBooleano(ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA))
                    {
                        //LA EMPRESA PADRE ES UNA INSPECCION ZONAL DEL USUARIO LOGUEADO
                        ret = (empresaPadre == TipoEmpresaEnum.INSPECCION);
                        empresaPadreCorrecta = "INSPECCION ZONAL";
                    }
                    else
                    {
                        ret = (empresaPadre == TipoEmpresaEnum.DIRECCION_DE_NIVEL);
                        empresaPadreCorrecta = TipoEmpresaEnum.DIRECCION_DE_NIVEL.ToString().Replace("_", " ");
                    }
                    break;
                case TipoEmpresaEnum.ESCUELA_ANEXO:
                    if (entidadesGeneralesRules.GetValorParametroBooleano(ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA))
                    {
                        //LA EMPRESA PADRE ES UNA INSPECCION ZONAL DEL USUARIO LOGUEADO
                        ret = (empresaPadre == TipoEmpresaEnum.INSPECCION);
                        empresaPadreCorrecta = "INSPECCION ZONAL";
                    }
                    else
                    {
                        ret = (empresaPadre == TipoEmpresaEnum.ESCUELA_MADRE);
                        empresaPadreCorrecta = TipoEmpresaEnum.ESCUELA_MADRE.ToString().Replace("_", " ");
                    }
                    break;
                default:
                    ret = false;
                    break;
            }
            if (!ret)
            {
                throw new BaseException(
                    "No se ha seleccionado una empresa padre válida. " + (string.IsNullOrEmpty(empresaPadreCorrecta)
                                                                              ? " La empresa padre debe ser " +
                                                                                empresaPadreCorrecta
                                                                              : null));
            }
        }

        public List<EmpresaModel> GetEmpresasDependientes(EmpresaModel model)
        {
            var entidad = Mapper.Map<EmpresaModel, EmpresaBase>(model);
            var empresas = DaoProvider.GetDaoEmpresaBase().GetEmpresasDependientes(entidad);
            return Mapper.Map<List<EmpresaBase>, List<EmpresaModel>>(empresas);
        }

        /// <summary>
        /// Devuelve el valor de acuerdo al parámetro booleano ingresado como parámetro
        /// </summary>
        /// <param name="parametroEnum">Parámetro booleano</param>
        /// <returns>string con el valor del parametro "Y" o "N"</returns>
        public string GetParametroBooleano(ParametroEnum parametroEnum)
        {
            var parametro = DaoProvider.GetDaoParametro().GetValorParametroVigente(parametroEnum, null);

            if (parametro == null)
                throw new BaseException("No se encontró el parámetro: " + parametroEnum);
            
            return parametro.Valor;
        }

        public bool TieneEstructuraDefinitiva(int idEmpresa)
        {
            return DaoEmpresa.ValidarEstructuraDefinitiva(idEmpresa);
        }

        public bool EsEscuela(int idEmpresa)
        {
            return DaoEmpresa.EsEscuela(idEmpresa);
        }
        public bool EsDireccionDeNivel(int idEscuela)
        {
            return DaoEmpresa.EsDireccionDeNivel(idEscuela);
        }

        #endregion

        #region Soporte

        /// <summary>
        /// Obtiene entidad empresa desde modelo empresa registrar
        /// </summary>
        /// <param name="model">Modelo con empresa</param>
        /// <returns>Entidad empresa</returns>
        private EmpresaBase TransformarModelo(EmpresaRegistrarModel model)
        {
            var empresa = new EmpresaBase();
            var currentUsuario = UsuarioRules.Instancia.GetCurrentUserDomain;

            if (model.Id > 0)
            {
                empresa = DaoEmpresa.GetById(model.Id);
                empresa.FechaUltimaModificacion = DateTime.Now;
            }
            else
            {
                empresa.UsuarioAlta = currentUsuario;
                empresa.FechaAlta = DateTime.Now;
                empresa.Estado = EstadoEmpresaEnum.GENERADA;
                empresa.HistorialEstados.Add(NuevoEstado(EstadoEmpresaEnum.GENERADA, empresa));
                empresa.TipoEmpresa = model.TipoEmpresa;
            }

            //TODO definir algoritmo para crear codigo de empresa
            //el codigo de empresa surge de un algoritmo que no ha sido definido
            if (!string.IsNullOrEmpty(model.CodigoEmpresa))
                empresa.CodigoEmpresa = model.CodigoEmpresa.ToUpper();

            if (!string.IsNullOrEmpty(model.Nombre))
                empresa.Nombre = model.Nombre.ToUpper();
            
            empresa.FechaInicioActividad = model.FechaInicioActividades;
            
            if (!string.IsNullOrEmpty(model.Observaciones))
                empresa.Observaciones = model.Observaciones.ToUpper();
            
            if (!string.IsNullOrEmpty(model.Telefono))
                empresa.AddComunicacion(model.Telefono, TipoComunicacionEnum.TELEFONO_PARTICULAR);
                //empresa.Telefono = model.Telefono.ToUpper();
            
            empresa.EmpresaPadreOrganigrama = model.EmpresaPadreOrganigramaId == 0 ? null : DaoEmpresa.GetById(model.EmpresaPadreOrganigramaId);

            if (model.DomicilioId.HasValue)
            {
                //TODO VANESA: FALTA IDOMICILIORULES
                var domicilio =
                    Mapper.Map<DomicilioModel, Domicilio>(
                        ServiceLocator.Current.GetInstance<IDomicilioRules>().GetDomicilioById(model.DomicilioId.Value));
                empresa.Domicilio = domicilio;
            }
            else
                empresa.Domicilio = null;
            //TODO VANESA: VINCULAR EMPRESA EDIFICIO ES UNA TABLA DE MUCHOS A MUCHOS
            //EL MODELO ME PASA LA LISTA DE EDIFICIOS O LOS IDS?
            //Que hace el cu vincular edificio?
            //escuela.Edificio = model    ;
            //TODO falta IProgramaPresupuestarioRules
            //ProgramaPresupuestario pp = Mapper.Map<ProgramaPresupuestarioModel, ProgramaPresupuestario>(ServiceLocator.Current.GetInstance<IProgramaPresupuestarioRules>().GetById(model.ProgramaPresupuestarioId));
            //escuela.ProgramaPresupuestario = null;
            return empresa;
        }

        /// <summary>
        /// Obtengo entidad escuela desde un modelo empresa registrar
        /// </summary>
        /// <param name="model">modelo con escuela</param>
        /// <returns>entidad escuela</returns>
        private Escuela GenerarEntidadEscuela(EmpresaRegistrarModel model,  Escuela escuela)
        {   
            Escuela nuevaEscuela = null;
            if(escuela != null)
            {
                nuevaEscuela = escuela;
            }
            else
            {
                nuevaEscuela = new Escuela();
            }

            var empresaActual = GetCurrentEmpresa();
            if (empresaActual == null || empresaActual.TipoEmpresa != TipoEmpresaEnum.DIRECCION_DE_NIVEL)
                nuevaEscuela.TipoDireccionNivel = null;
            else
                nuevaEscuela.TipoDireccionNivel = DaoProvider.GetDaoDireccionDeNivel().GetById(empresaActual.Id);

     
            if (model.PeriodosLectivos!=null)
            {
                var periodosLectivos = new List<PeriodoLectivo>();
                foreach (var periodoLectivo in model.PeriodosLectivos)
                {
                    periodosLectivos.Add(DaoProvider.GetDaoPeriodoLectivo().GetById(periodoLectivo.Id));
                }
                nuevaEscuela.PeriodosLectivo = periodosLectivos;
            }else
                throw new BaseException("El período lectivo es requerido para una escuela");

            nuevaEscuela.FechaInicioActividad = model.FechaInicioActividades;
            nuevaEscuela.EsRaiz = model.EsRaiz;



            if (new EntidadesGeneralesRules().GetValorParametroBooleano(ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA))
            {
                if (model.EmpresaInspeccionId.HasValue)
                {
                    nuevaEscuela.EmpresaPadreOrganigrama =
                        DaoProvider.GetDaoInspeccion().GetById(model.EmpresaInspeccionId.Value);
                    
                }
            }
            else
            {
                if (model.EmpresaPadreOrganigramaId > 0)
                    nuevaEscuela.EmpresaPadreOrganigrama =
                        DaoProvider.GetDaoEmpresaBase().GetById(model.EmpresaPadreOrganigramaId);
            }
            nuevaEscuela.NumeroAnexo = model.NumeroAnexo.HasValue ? model.NumeroAnexo.Value : 0;

            int maximoNroEscuela = DaoProvider.GetDaoEscuela().GetMaximoNumeroEscuela();
            if (model.EsRaiz)
            {
                //borro la escuela raiz que tenía asignada, si es que era asi
                nuevaEscuela.EscuelaRaiz = null;
                if (model.Id == 0) //Si estoy registrando incremento el número, si no dejo el mismo
                {
                    nuevaEscuela.NumeroEscuela = maximoNroEscuela + 1;
                }
            }
            else
            {
                if (model.EscuelaRaizId.HasValue) {
                    //se verifica que la escuela modificada no sea raíz o madre de otras escuelas.
                    //si no tiene, se asigna la escuela raiz seleccionada
                    if(!TieneDependencias(nuevaEscuela.Id))
                        nuevaEscuela.EscuelaRaiz = DaoProvider.GetDaoEscuela().GetById(model.EscuelaRaizId.Value);
                    else
                        throw new ApplicationException("La escuela que quiere pasar a NO RAIZ esta asignada como RAIZ a otras escuelas.");
                }
                nuevaEscuela.NumeroEscuela = nuevaEscuela.EscuelaRaiz != null ? nuevaEscuela.EscuelaRaiz.NumeroEscuela : 0;
            }
            nuevaEscuela.Religioso = model.Religioso;
            nuevaEscuela.Arancelado = model.Arancelado;
            nuevaEscuela.Albergue = model.Albergue;

            nuevaEscuela.ContextoDeEncierro = model.ContextoDeEncierro;
            nuevaEscuela.Hospitalaria = model.EsHospitalaria;

                nuevaEscuela.CUE = model.CUE!=null?model.CUE.ToUpper():"";
                nuevaEscuela.CUEAnexo = model.CUEAnexo??0;

            if (!String.IsNullOrEmpty(model.HorarioDeFuncionamiento))
                nuevaEscuela.HorarioDeFuncionamiento = model.HorarioDeFuncionamiento.ToUpper();

            if (!String.IsNullOrEmpty(model.Colectivos))
                nuevaEscuela.Colectivos = model.Colectivos.ToUpper();

            nuevaEscuela.TipoCooperadora = model.TipoCooperadora.HasValue
                                          ? model.TipoCooperadora.Value
                                          : (TipoCooperadoraEnum?)null;

            if (model.CategoriaEscuela.HasValue)
                nuevaEscuela.TipoCategoria = model.CategoriaEscuela.Value;

            IDaoZonaDesfavorable zona = DaoProvider.GetDaoZonaDesfavorable();
            nuevaEscuela.ZonaDesfavorable = model.ZonaDesfavorableId == null
                                           ? null
                                           : zona.GetById((int)model.ZonaDesfavorableId);
            if (nuevaEscuela.ZonaDesfavorable == null)
            {
                throw new BaseException("La zona desfavorable es un dato requerido, por favor selecciónelo");
            }

            nuevaEscuela.Ambito = model.Ambito.HasValue ? model.Ambito.Value : (AmbitoEscuelaEnum?)null;
            nuevaEscuela.Dependencia = model.Dependencia.HasValue ? model.Dependencia.Value : (DependenciaEnum?)null;

            if (nuevaEscuela.Dependencia == null)
            {
                throw new BaseException("La dependencia es obligatoria para una escuela");
            }

            if (model.TipoEscuela.HasValue)
                nuevaEscuela.TipoEscuela = DaoProvider.GetDaoTipoEscuela().GetById(model.TipoEscuela.Value);

            //Buscar según nivel educativo y tipo educación la tabla intermedia
            NivelEducativoPorTipoEducacion nete = null;
            if(model.NivelEducativoId.HasValue && model.TipoEducacion.HasValue)
             nete = DaoProvider.GetDaoNivelEducativoPorTipoEducacion().GetByIdNivelEducativoYTipoEducacion(
                    model.NivelEducativoId.Value, model.TipoEducacion.Value);
            if (nete == null)
                throw new BaseException(Resources.Empresa.SinNETE);
            else
            {
                if (nuevaEscuela.Id>0){
                foreach (var nivelEducativo in nuevaEscuela.NivelesEducativo)
                    if(nivelEducativo.TipoEducacion!=model.TipoEducacion.Value && nivelEducativo.NivelEducativo.Id!=model.NivelEducativoId)
                    {
                        //borro la combinación de NE y TE q tenia antes la lista, para asignarle la nueva combinación
                        nuevaEscuela.NivelesEducativo = new List<NivelEducativoPorTipoEducacion>();
                        nuevaEscuela.NivelesEducativo.Add(nete);
                    }
                }else nuevaEscuela.NivelesEducativo.Add(nete);

                
            }
            if (nuevaEscuela.NivelEducativo.Id == (int)NivelEducativoNombreEnum.PRIMARIO )
            {
                if (model.CodigoInspeccion == null || model.CodigoInspeccion.Trim() == "")
                {
                    throw new BaseException("El código de inspección es obligatorio en el caso de ser la escuela de nivel educativo primario");
                }
                DaoEmpresa.ValidarCodigoInspeccionRepetido(model.Id,model.CodigoInspeccion);
                nuevaEscuela.CodigoInspeccion = model.CodigoInspeccion;
            }
            else
            {
                nuevaEscuela.CodigoInspeccion = null;
            }

            if (model.ModalidadJornada.HasValue)
                nuevaEscuela.ModalidadJornada = DaoProvider.GetDaoModalidadJornada().GetById(model.ModalidadJornada.Value);

            if (model.TipoJornada.HasValue)
                nuevaEscuela.TipoJornada = DaoProvider.GetDaoTipoJornada().GetById(model.TipoJornada.Value);

            nuevaEscuela.EstructuraDefinitiva = model.EstructuraDefinitiva;

            if (model.Turnos != null)
            {
                TurnoPorEscuela turnoPorEscuela;
                var turnosXEscuela = new List<TurnoPorEscuela>();
                foreach (var tt in model.Turnos)
                {
                    var turnoAux = DaoProvider.GetDaoTurno().GetById(tt.Id);
                    turnosXEscuela.Add(AgregarTurnoXEscuela(nuevaEscuela, turnoAux));
                }
                nuevaEscuela.TurnosXEscuela=turnosXEscuela;
            }
            if (model.Privado){
                nuevaEscuela.EscuelaPrivada = GenerarEntidadEscuelaPrivada(model);
                nuevaEscuela.Privado = true;
            }

            ValidarEscuela(nuevaEscuela);

            return nuevaEscuela;
        }

        //Método que agrega un turno por escuela si es nuevo y si ya está toma el id del anterior
        private TurnoPorEscuela AgregarTurnoXEscuela(EmpresaBase empresa, Turno turnoAux)
        {
            if (empresa.TurnosXEscuela != null)
            {
                foreach (var turnoXEscuela in empresa.TurnosXEscuela)
                {
                    //Checkeo si ya está en la entidad guardad el turno que se está contemplando otra vez y lo devuelvo para que no se duplique
                    if (turnoXEscuela.Escuela.Id == empresa.Id && turnoXEscuela.Turno.Id == turnoAux.Id)
                        return turnoXEscuela;
                }
            }
            //Si no está lo creo
            return new TurnoPorEscuela() { Turno = turnoAux, Escuela = empresa};
        }

        /// <summary>
        /// Obtengo entidad escuela desde un modelo empresa registrar
        /// </summary>
        /// <param name="model">modelo con escuela</param>
        /// <returns>entidad escuela</returns>
        private EscuelaAnexo GenerarEntidadEscuelaAnexo(EmpresaRegistrarModel model,EscuelaAnexo entidad)
        {
            EscuelaAnexo escuela = entidad;
            if (escuela == null)
            {
                escuela = new EscuelaAnexo();
            }

            var empresaActual = GetCurrentEmpresa();
            if (empresaActual == null || empresaActual.TipoEmpresa != TipoEmpresaEnum.DIRECCION_DE_NIVEL)
                escuela.TipoDireccionNivel = null;
            else
                escuela.TipoDireccionNivel = DaoProvider.GetDaoDireccionDeNivel().GetById(empresaActual.Id);
            if (model.PeriodosLectivos!=null)
            {
                var periodosLectivos = new List<PeriodoLectivo>();

                foreach (var periodosLectivo in model.PeriodosLectivos)
                    periodosLectivos.Add(DaoProvider.GetDaoPeriodoLectivo().GetById(periodosLectivo.Id));
                escuela.PeriodosLectivo = periodosLectivos;
            }
            else
            {
                throw new BaseException("El período lectivo es requerido para una escuela anexo");
            }
            
            escuela.FechaInicioActividad = model.FechaInicioActividades;
            //escuela.EsRaiz = model.EsRaiz;
            if (new EntidadesGeneralesRules().GetValorParametroBooleano(ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA))
            {
                if (model.EmpresaInspeccionId.HasValue){
                    escuela.EmpresaPadreOrganigrama = DaoProvider.GetDaoInspeccion().GetById(model.EmpresaInspeccionId.Value);
                }
            }
            else
            {
                if (model.EmpresaPadreOrganigramaId > 0)
                    escuela.EmpresaPadreOrganigrama =
                        DaoProvider.GetDaoEmpresaBase().GetById(model.EmpresaPadreOrganigramaId);
            }
            if (model.EscuelaMadreId.HasValue)
                escuela.EscuelaMadre = DaoProvider.GetDaoEscuela().GetById(model.EscuelaMadreId.Value);
            escuela.NumeroAnexo = model.NumeroAnexo.HasValue ? model.NumeroAnexo.Value : 0;
            escuela.NumeroEscuela = escuela.EscuelaMadre != null ? escuela.EscuelaMadre.NumeroEscuela : 0;
            escuela.Religioso = model.Religioso;
            escuela.Arancelado = model.Arancelado;
            escuela.Albergue = model.Albergue;
            escuela.ContextoDeEncierro = model.ContextoDeEncierro;
            escuela.Hospitalaria = model.EsHospitalaria;
            if (!string.IsNullOrEmpty(model.CUE))
                escuela.CUE = model.CUE.ToUpper();
            if (model.CUEAnexo.HasValue)
                escuela.CUEAnexo = model.CUEAnexo.Value;
            if (!String.IsNullOrEmpty(model.HorarioDeFuncionamiento))
                escuela.HorarioDeFuncionamiento = model.HorarioDeFuncionamiento.ToUpper();
            if (!String.IsNullOrEmpty(model.Colectivos))
                escuela.Colectivos = model.Colectivos.ToUpper();

            escuela.TipoCooperadora = model.TipoCooperadora.HasValue
                                          ? model.TipoCooperadora.Value
                                          : (TipoCooperadoraEnum?)null;
            if (model.CategoriaEscuela.HasValue)
                escuela.TipoCategoria = model.CategoriaEscuela.Value;
            IDaoZonaDesfavorable zona = DaoProvider.GetDaoZonaDesfavorable();
            escuela.ZonaDesfavorable = model.ZonaDesfavorableId == null
                                           ? null
                                           : zona.GetById((int)model.ZonaDesfavorableId);
            if (escuela.ZonaDesfavorable == null)
            {
                throw new BaseException("La zona desfavorable es un dato requerido, por favor selecciónelo");
            }
            escuela.Ambito = model.Ambito.HasValue ? model.Ambito.Value : (AmbitoEscuelaEnum?)null;
            escuela.Dependencia = model.Dependencia.HasValue ? model.Dependencia.Value : (DependenciaEnum?)null;
            if (escuela.Dependencia == null)
            {
                throw new BaseException("La dependencia es obligatoria para una escuela anexo");
            }
            if (model.TipoEscuela.HasValue)
                escuela.TipoEscuela = DaoProvider.GetDaoTipoEscuela().GetById(model.TipoEscuela.Value);

            
            //Buscar según nivel educativo y tipo educación la tabla intermedia
            NivelEducativoPorTipoEducacion nete = DaoProvider.GetDaoNivelEducativoPorTipoEducacion().GetByIdNivelEducativoYTipoEducacion(
                    model.NivelEducativoId.Value, model.TipoEducacion.Value);
            if (nete == null)
                throw new BaseException(Resources.Empresa.SinNETE);
            else
            {
                if (escuela.NivelesEducativo.Count == 0)
                {
                    escuela.NivelesEducativo.Add(nete);
                }
                foreach (var nivelEducativo in escuela.NivelesEducativo)
                {
                    if (nivelEducativo.TipoEducacion != model.TipoEducacion.Value && nivelEducativo.NivelEducativo.Id != model.NivelEducativoId)
                    {
                        escuela.NivelesEducativo.Add(nete);
                    }
                }
            }

            if (escuela.NivelEducativo.Id == (int)NivelEducativoNombreEnum.PRIMARIO)
            {
                if (model.CodigoInspeccion == null || model.CodigoInspeccion.Trim() == "")
                {
                    throw new BaseException("El código de inspección es obligatorio en el caso de ser la escuela de nivel educativo primario");
                }
                DaoEmpresa.ValidarCodigoInspeccionRepetido(model.Id,model.CodigoInspeccion);
                escuela.CodigoInspeccion = model.CodigoInspeccion;
            }
            else
            {
                escuela.CodigoInspeccion = null;
            }

            if (model.ModalidadJornada.HasValue)
                escuela.ModalidadJornada = DaoProvider.GetDaoModalidadJornada().GetById(model.ModalidadJornada.Value);
            if (model.TipoJornada.HasValue)
                escuela.TipoJornada = DaoProvider.GetDaoTipoJornada().GetById(model.TipoJornada.Value);
            escuela.EstructuraDefinitiva = model.EstructuraDefinitiva;
            if (model.Turnos != null)
            {
                TurnoPorEscuela turnoPorEscuela;
                var turnosXEscuela = new List<TurnoPorEscuela>();
                foreach (var tt in model.Turnos)
                {
                    var turnoAux = DaoProvider.GetDaoTurno().GetById(tt.Id);
                    turnosXEscuela.Add(AgregarTurnoXEscuela(escuela, turnoAux));
                }
                escuela.TurnosXEscuela = turnosXEscuela;
            }
            if (model.Privado)
            {
                escuela.Privado = true;
                escuela.EscuelaPrivada = GenerarEntidadEscuelaPrivada(model);
            }
            ValidarEscuelaAnexo(escuela);
            return escuela;
        }

        /// <summary>
        /// Obtengo entidad escuela privada desde un modelo empresa registrar
        /// </summary>
        /// <param name="model">modelo con escuela privada</param>
        /// <returns>entidad escuela privada</returns>
        private EscuelaPrivada GenerarEntidadEscuelaPrivada(EmpresaRegistrarModel model)
        {
            var escuelaPrivada = new EscuelaPrivada();
            var perRules = ServiceLocator.Current.GetInstance<IPersonaFisicaRules>();

            if(model.Director.Id > 0)
                escuelaPrivada.Director = DaoProvider.GetDaoPersonaFisica().GetById(model.Director.Id.Value);
            else
                escuelaPrivada.Director = Mapper.Map<PersonaFisicaModel, PersonaFisica>(perRules.PersonaFisicaSaveOUpdate(model.Director));

            if (model.RepresentanteLegal.Id > 0)
                escuelaPrivada.RepresentanteLegal = DaoProvider.GetDaoPersonaFisica().GetById(model.RepresentanteLegal.Id.Value);
            else
                escuelaPrivada.RepresentanteLegal = Mapper.Map<PersonaFisicaModel, PersonaFisica>(perRules.PersonaFisicaSaveOUpdate(model.RepresentanteLegal));

            escuelaPrivada.NumeroCuentaBancaria = model.NumeroCuentaBancaria;
            
            if (model.PorcentajeAporteEstado.HasValue)
                escuelaPrivada.PorcentajeAporteEstado = model.PorcentajeAporteEstado.Value;
            if (model.ObraSocialId.HasValue)
                escuelaPrivada.ObraSocial = DaoProvider.GetDaoObraSocial().GetById(model.ObraSocialId.Value);
            if (model.SucursalBancariaId.HasValue)
                escuelaPrivada.SucursalBanco =
                    DaoProvider.GetDaoSucursalBancaria().GetById(model.SucursalBancariaId.Value);
            return escuelaPrivada;
        }

        /// <summary>
        /// Obtengo entidad inspeccion desde un modelo empresa registrar
        /// </summary>
        /// <param name="model">modelo con inspeccion</param>
        /// <returns>entidad inspeccion</returns>
        private Inspeccion GenerarEntidadInspeccion(EmpresaRegistrarModel model, Inspeccion entidad)
        {
            Inspeccion inspeccion = entidad;
            if (inspeccion == null)
            {
                inspeccion = new Inspeccion();
            }
            var empresaActual = GetCurrentEmpresa();

            if (empresaActual == null || empresaActual.TipoEmpresa != TipoEmpresaEnum.DIRECCION_DE_NIVEL)
                inspeccion.TipoDireccionNivel = null;
            else
                inspeccion.TipoDireccionNivel = DaoProvider.GetDaoDireccionDeNivel().GetById(empresaActual.Id);

            inspeccion.AsignacionEscuela = Mapper.Map<IList<AsignacionInspeccionEscuelaModel>, IList<AsignacionInspeccionEscuela>>(model.AsignacionEscuela);

            if (model.TipoInspeccion.HasValue)
            {
                if (model.TipoInspeccion.Value < 0)
                {
                    //es una enumeracion!!!
                    inspeccion.TipoInspeccion =
                        (TipoInspeccionEnum)
                        (model.TipoInspeccion.Value/
                         int.Parse(
                             Resources.Empresa.
                                 ArtilugioParaDistinguirTipoInspeccionIntermediaEnumDeTipoInspeccionIntermediaObjeto));
                }
                else
                {
                    inspeccion.TipoInspeccion = TipoInspeccionEnum.OTRA;
                    inspeccion.TipoInspeccionIntermedia =
                        DaoProvider.GetDaoTipoInspeccionIntermedia().GetById(model.TipoInspeccion.Value);
                }
            }else
            {
                if (model.TipoInspeccionEnum.HasValue)
                    inspeccion.TipoInspeccion = (TipoInspeccionEnum)model.TipoInspeccionEnum;
                else
                {
                    if (model.TipoInspeccion.HasValue)
                        inspeccion.TipoInspeccion = (TipoInspeccionEnum)Math.Abs(model.TipoInspeccion.Value / (-10));
                    else throw new ApplicationException("Se debe seleccionar el tipo de inspeccion");
                }
                //if (inspeccion.EmpresaPadreOrganigrama != null)
                //{
                //    var inspeccionSuperior =
                //        DaoProvider.GetDaoInspeccion().GetById(inspeccion.EmpresaPadreOrganigrama.Id);
                //    inspeccion.EmpresaDeInspeccionSuperior = inspeccionSuperior;
                //}
            }


            if (inspeccion.TipoInspeccion == TipoInspeccionEnum.GENERAL)
            {
                inspeccion.EmpresaDeInspeccionSuperior = null;
                inspeccion.EmpresaPadreOrganigrama = DaoDireccionDeNivel.GetById(model.EmpresaPadreOrganigramaId);
            }
            else
            {
                if (model.EmpresaInspeccionSupervisoraId.HasValue)
                 if (new EntidadesGeneralesRules().GetValorParametroBooleano(ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA))
                {
                    inspeccion.EmpresaPadreOrganigrama = DaoInspeccion.GetById(model.EmpresaInspeccionSupervisoraId.Value);
                    inspeccion.EmpresaDeInspeccionSuperior = inspeccion.EmpresaPadreOrganigrama;
                }else
                 {
                     inspeccion.EmpresaPadreOrganigrama = DaoInspeccion.GetById(model.EmpresaPadreOrganigramaId);
                     inspeccion.EmpresaDeInspeccionSuperior = DaoInspeccion.GetById(model.EmpresaInspeccionSupervisoraId.Value);
                 }
            }

            inspeccion.EmpresaRegistro = Mapper.Map<EmpresaModel, EmpresaBase>(empresaActual);

            ValidarInspeccion(inspeccion);
            return inspeccion;
        }

        /// <summary>
        /// Obtengo entidad direccion de nivel desde un modelo empresa registrar
        /// </summary>
        /// <param name="model">modelo con direccion de nivel</param>
        /// <returns>entidad direccion de nivel</returns>
        private DireccionDeNivel GenerarEntidadDireccionDeNivel(EmpresaRegistrarModel model, DireccionDeNivel entidad)
        {
            DireccionDeNivel dn = null;
            if (entidad != null)
            {
                dn = entidad;
            }
            else
            {
                dn = new DireccionDeNivel();
            }
            //dn.HistorialEstadoEmpresa = Mapper.Map<IList<HistorialEstadoEmpresaModel>, IList<HistorialEstadoEmpresa>>(model.HistorialEstadoEmpresa);
            //dn.NivelEducativo = model.NivelEducativo;
            //TODO dsps cambiar esto
            if (!string.IsNullOrEmpty(model.Sigla))
                dn.Sigla = model.Sigla.ToUpper();
            //dn.TipoDireccionNivel = model.direccion;
            dn.TipoEmpresa = model.TipoEmpresa;
            //if (model.TipoEducacion.HasValue)
            //    dn.TipoEducacion = model.TipoEducacion.Value;
            NivelEducativoPorTipoEducacion nete;
            dn.NivelesEducativoXTE = new List<NivelEducativoPorTipoEducacion>();
            if (model.NivelEducativoPorTipoEducacion != null && model.NivelEducativoPorTipoEducacion.Count > 0)
            {
                foreach (var neteModel in model.NivelEducativoPorTipoEducacion)
                {
                    nete =
                        DaoProvider.GetDaoNivelEducativoPorTipoEducacion().GetByIdNivelEducativoYTipoEducacion(
                            neteModel.NivelEducativo.Id, neteModel.TipoEducacion);
                    if (nete != null && dn.NivelesEducativoXTE.FirstOrDefault(x => x.NivelEducativo.Id == nete.NivelEducativo.Id) == null)
                        dn.NivelesEducativoXTE.Add(nete);//DaoProvider.GetDaoNivelEducativo().GetById(nete.NivelEducativo.Id));
                }
                //dn.NivelEducativo = DaoProvider.GetDaoNivelEducativo().GetById(NETE.NivelEducativoId);
                //dn.TipoEducacion = NETE.TipoEducacion;
            }
            else
                throw new ApplicationException("El nivel educativo es requerido");

            TipoEscuela tipoEscuela;
            if (model.TiposEscuelas != null && model.TiposEscuelas.Count > 0)
            {
                var tipoEscuelas = new List<TipoEscuela>();

                foreach (var teModel in model.TiposEscuelas)
                    tipoEscuelas.Add(DaoProvider.GetDaoTipoEscuela().GetById(teModel.Id));
                dn.TipoEscuelaACrear = tipoEscuelas;
            }
            else
            {
                dn.TipoEscuelaACrear = null;
            }

            ValidarDireccionDeNivel(dn);
            return dn;
        }

        private void ValidarCierreEmpresa(EmpresaCierreModel model)
        {
            EmpresaBase entidad = DaoEmpresa.GetById(model.IdEmpresa);

            if (entidad.EstadoEmpresa != EstadoEmpresaEnum.AUTORIZADA)
                throw new BaseException(Resources.Empresa.SinRequisitos);

            if (entidad.FechaInicioActividad != null)// Valido que la fecha de cierre sea mayor o igual a la fecha de inicio de actividades
            {
                if (model.FechaCierre < entidad.FechaInicioActividad)
                    throw new BaseException("La fecha de cierre de la empresa debe ser mayor o igual a la fecha de inicio de actividades " + entidad.FechaInicioActividad);

            }

            // TODO: Fede Bobbio - Falta checkear que no posea patrimonio

            // TODO: Fede Bobbio - Falta checkear que no sea beneficiario de programa presupuestario
            //if (entidad.ProgramaPresupuestario != null)
            //    throw new BaseException("La Empresa es beneficiaria de un programa presupuestario");

            // TODO: Fede Bobbio - Falta checkear que no posea saldo de rendición

            // TODO: Fede Bobbio - Falta checkear que no posea pedidos en estado: Pendiente de Autorizar, Autorizado o Pendiente de Envío

            ValidarOrganigrama(entidad);

            // Checkeo que todos sus puestos de trabajo se encuentran en estado: Cerrado o En Cierre Con Goce De Sueldo o En Cierre Con Goce De Sueldo Caducado o Para Reserva o Rechazado.
            ValidarEstadosPuestosDeTrabajo(entidad);

            if (entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE || entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
                ValidarCierreEscuela(entidad);

            if (entidad.TipoEmpresa == TipoEmpresaEnum.INSPECCION)
            {
                ValidarCierreInspeccion(entidad);
            }

            
            // TODO: Fede Bobbio - Falta checkear que todos los alumnos inscriptos tienen pase a otra escuela

            /* TODO completar validaciones cierre empresa que dependen de Infraestructura y Estudiantes
             */
        }

        private void ValidarOrganigrama(EmpresaBase entidad)
        {
            if (DaoProvider.GetDaoEmpresaBase().PoseeEmpresasHijasNoCerradasORechazadas(entidad.Id))
            {
                throw new BaseException("Imposible cerrar la empresa ya no todas sus empresas hijas están en estado cerrado o rechazado");
            }
        }

        private void ValidarCierreInspeccion(EmpresaBase entidad)
        {
            if (PoseeAsignacionVigenteOProvisoria(entidad))
            {
                throw new BaseException("La empresa a cerrar posee al menos una asignación de inspección a escuela en estado VIGENTE o PROVISORIO");
            }
            if (DaoProvider.GetDaoInspeccion().EsInspeccionSuperiorDeInspeccionNoCerradaNiRechazada(entidad.Id))
            {
                throw new BaseException("La empresa de inspección " + entidad.Nombre + " es inspección superior de otra empresa de inspección en estado distinto a CERRADA o RECHAZADA");
            }
        }

        private bool PoseeAsignacionVigenteOProvisoria(EmpresaBase entidad)
        {
            var ins = (Inspeccion) entidad;
            foreach (var asign in ins.AsignacionEscuela)
            {
                //TODO: falta el estado Provisorio en la enumeración y en la base (25/08/2011)
                if (asign.Estado == EstadoAsignacionInspeccionEscuelaEnum.VIGENTE)
                {
                    return true;
                }
            }
            return false;

        }

        private void ValidarEstadosPuestosDeTrabajo(EmpresaBase entidad)
        {
            var estadosList = new List<EstadoPuestoDeTrabajoEnum>();
            estadosList.Add(EstadoPuestoDeTrabajoEnum.CERRADO);
            estadosList.Add(EstadoPuestoDeTrabajoEnum.EN_CIERRE_CON_GOCE_DE_SUELDO);
            estadosList.Add(EstadoPuestoDeTrabajoEnum.EN_CIERRE_CON_GOCE_DE_SUELDO_CADUCADO);
            estadosList.Add(EstadoPuestoDeTrabajoEnum.PARA_RESERVA);
            estadosList.Add(EstadoPuestoDeTrabajoEnum.RECHAZADO);
            if(!DaoProvider.GetDaoPuestoDeTrabajo().VerificarTodosEstadosPuestosDeEmpresaSean(entidad.Id,estadosList))
            {
                throw new BaseException("Se han encontrado puestos de trabajo para la empresa " + entidad.Nombre + " en estados distintos a CERRADO, EN CIERRE CON GOCE DE SUELDO, EN CIERRE CON GOCE DE SUELDO CADUCADO, PARA RESERVA o RECHAZADO");
            }
        }

        private void ValidarEmpresa(EmpresaBase entidad, bool tieneIdDomicilioSeleccionado)
        {
            StringBuilder errores = new StringBuilder("Datos requeridos: "); //18 caracteres
            int longitudMinima = errores.Length;
            if (string.IsNullOrEmpty(entidad.CodigoEmpresa))
                errores.Append("Código de empresa");
            else
            {
                entidad.CodigoEmpresa = entidad.CodigoEmpresa.Trim();
                if(entidad.CodigoEmpresa.Length < 2)
                    throw new ApplicationException("Mal formato del campo código empresa");

                if (entidad.CodigoEmpresa.Substring(0, 2) != "EE" || (entidad.CodigoEmpresa.Length > 9) || (entidad.CodigoEmpresa.ToCharArray()[2] != '0' && entidad.CodigoEmpresa.ToCharArray()[2] != '1'))

                    errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + Resources.Empresa.ErrorFormatoCodigoEmpresa);

            }
            if (DaoProvider.GetDaoEmpresaBase().CodigoRepetido(entidad.CodigoEmpresa, entidad.Id))
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) +
                               "Ingrese otro código, pues el ingresado se encuentra en uso");
            if (string.IsNullOrEmpty(entidad.Nombre))
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Nombre");
            if (entidad.FechaAlta == DateTime.MinValue)
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Fecha alta");
            if (entidad.FechaInicioActividad == DateTime.MinValue)
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Fecha inicio de actividades");
            if (!tieneIdDomicilioSeleccionado)
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Domicilio");
            if (entidad.OrdenDePago == null)
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Orden de pago");
            if (entidad.ProgramaPresupuestario == null)
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Programa presupuestario");
            if (entidad.TipoEmpresa != TipoEmpresaEnum.MINISTERIO && entidad.TipoEmpresa != TipoEmpresaEnum.INSPECCION && entidad.EmpresaPadreOrganigrama == null )
            {
                if (new EntidadesGeneralesRules().GetValorParametroBooleano(ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA) && (entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE || entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO))
                    errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Empresa de inspección  ");
             else
                 errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Empresa padre organigrama");
            }
            if (entidad.TipoEmpresa == TipoEmpresaEnum.INSPECCION)
            {
                if (!new EntidadesGeneralesRules().GetValorParametroBooleano(ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA) && entidad.EmpresaPadreOrganigrama == null )
                {
                    errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Empresa padre organigrama");
                }
                else
                {
                    if (((Inspeccion)entidad).TipoInspeccion == TipoInspeccionEnum.GENERAL && entidad.EmpresaPadreOrganigrama == null)
                    {
                        errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Empresa padre organigrama");
                    }

                }
            }
            if (entidad.TipoEmpresa != TipoEmpresaEnum.ESCUELA_MADRE &&
                    entidad.TipoEmpresa != TipoEmpresaEnum.ESCUELA_ANEXO)
            {
                if (DaoProvider.GetDaoEmpresaBase().NombreRepetido(entidad.Nombre, entidad.Id))
                    errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) +
                                   "Ingrese otro nombre, pues el nombre ingresado se encuentra en uso");
            }
            if (errores.Length > longitudMinima)
                throw new BaseException(errores.ToString());
        }

        private bool validarEmpresaParaVisarActivacion(EmpresaBase entidad)
        {
            if (entidad.EstadoEmpresa != EstadoEmpresaEnum.GENERADA &&
                entidad.EstadoEmpresa != EstadoEmpresaEnum.EN_PROCESO_DE_REACTIVACION)
            {
                throw new BaseException(Resources.Empresa.EstadoIncorrecto);
            }
            return true;
        }

        private void ValidarCierreEscuela(EmpresaBase entidad)
        {
            if (!DaoEscuela.ComprobarEscuelasAsociadas(entidad.Id))
                throw new BaseException(Resources.Empresa.EscuelaMadre);

            if (!DaoEscuela.ComprobarRaizEscuelas(entidad.Id))
                throw new BaseException(Resources.Empresa.EscuelaRaiz);

            //Si posee alumnos inscriptos en el ciclo lectivo vigente cuyas inscripciones no estén ni cerradas ni anuladas
            //TODO: SE COMENTA EL 02/09/2011 porque no está la columna de inscripción en la tabla de preinscripción, en la base de datos
            /*if (DaoEscuela.EscuelaPoseeAlumnosInscriptosEnCicloLectivoVigenteNoCerradaOAnulada(entidad.Id))
            {
                throw new BaseException("La escuela tiene alumnos inscriptos en el ciclo lectivo vigente");
            }*/
        }

        private void ValidarEscuela(Escuela entidad)
        {
            //List<Escuela> res = DaoEscuela.GetByFiltrosBasico(entidad.CUE, null, string.Empty, string.Empty, null, string.Empty, string.Empty, null, null);

            StringBuilder errores = new StringBuilder("Datos requeridos: "); //18 caracteres
            int longitudMinima = errores.Length;

            if(entidad.PeriodosLectivo.Count == 0)
            {
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Período lectivo");
            }

            if (entidad.TipoEscuela == null)
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Tipo de escuela");
            else
            {
                if (!entidad.EsRaiz && entidad.EscuelaRaiz == null)
                    errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Escuela Raíz");
            }
            if (entidad.NivelEducativo == null || entidad.TipoEducacion == null)
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Tipo Educación / Nivel Educativo");
            if (entidad.Turnos == null || entidad.Turnos.Count == 0)
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Turnos");
            if (entidad.EscuelaPrivada != null)
            {
                if (entidad.EscuelaPrivada.Director == null)
                    errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Director no ingresado o persona inexistente");
                if (entidad.EscuelaPrivada.RepresentanteLegal == null)
                    errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Representante legal no ingresado o persona inexistente");
                if (entidad.Privado)
                {
                    if (string.IsNullOrEmpty(entidad.EscuelaPrivada.NumeroCuentaBancaria))
                        errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Número de cuenta bancaria");
                }
                else
                {
                    entidad.EscuelaPrivada = null;
                }
            }
            if (!string.IsNullOrEmpty(entidad.CUE))
                if (DaoEscuela.CueRepetido(entidad.CUE, entidad.CUEAnexo, entidad.Id))
                    throw new BaseException("Ya existe una Escuela con el mismo Cue-Cue Anexo");

            if (errores.Length > longitudMinima)
                throw new BaseException(errores.ToString());
        }

        private void ValidarEscuelaAnexo(EscuelaAnexo entidad)
        {
            StringBuilder errores = new StringBuilder("Datos requeridos: "); //18 caracteres
            int longitudMinima = errores.Length;
            if (entidad.PeriodosLectivo.Count == 0)
            {
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Período lectivo");
            }
            if (entidad.TipoEscuela == null)
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Tipo de escuela");
            if (entidad.EscuelaMadre == null)
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Escuela madre");
            if (entidad.NumeroAnexo <= 0)
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) +
                               "Número anexo debe ser positivo");
            if (DaoEscuelaAnexo.ExisteNumeroEscuelaAnexoParaEscuelaMadre(entidad.NumeroAnexo, entidad.EscuelaMadre.Id,entidad.Id))
                errores.Append("El numero de escuela anexo debe ser unico para una misma escuela madre");
            if (entidad.NivelEducativo == null)
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Nivel educativo");
            if (entidad.Turnos == null || entidad.Turnos.Count == 0)
                errores.Append((errores.Length > longitudMinima ? ", " : string.Empty) + "Turnos");

            if (errores.Length > longitudMinima)
                throw new BaseException(errores.ToString());
        }

        private void ValidarDireccionDeNivel(DireccionDeNivel entidad)
        {
            StringBuilder errores = new StringBuilder("Datos requeridos: "); //18 caracteres
            int longuitudMinima = errores.Length;
            if (entidad.NivelesEducativos == null || entidad.NivelesEducativos.Count == 0)
                errores.Append((errores.Length > longuitudMinima ? ", " : string.Empty) + "Niveles educativos");
            if (string.IsNullOrEmpty(entidad.Sigla))
                errores.Append((errores.Length > longuitudMinima ? ", " : string.Empty) + "Sigla");
            else
                if (DaoProvider.GetDaoDireccionDeNivel().SiglaRepetida(entidad.Sigla,entidad.Id))
                    errores.Append((errores.Length > longuitudMinima ? ", " : string.Empty) +
                                   "Ingrese otra sigla, pues la ingresada ya se encuentra en uso");
            if (errores.Length > longuitudMinima)
                throw new BaseException(errores.ToString());
        }

        private void ValidarInspeccion(Inspeccion entidad)
        {
            StringBuilder errores = new StringBuilder("Datos requeridos: "); //18 caracteres
            int longuitudMinima = errores.Length;
            if (entidad.TipoInspeccion == 0)
                errores.Append((errores.Length > longuitudMinima ? ", " : string.Empty) + "Tipo de inspección");
            if (entidad.TipoDireccionNivel == null)
                errores.Append((errores.Length > longuitudMinima ? ", " : string.Empty) + "Dirección de nivel");
            if (!(new EntidadesGeneralesRules().GetValorParametroBooleano(ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA)))
                if (entidad.EmpresaDeInspeccionSuperior == null && entidad.TipoInspeccion != TipoInspeccionEnum.GENERAL)
                    errores.Append((errores.Length > longuitudMinima ? ", " : string.Empty) +
                                   "Empresa de inspección supervisora");

            if (errores.Length > longuitudMinima)
                throw new BaseException(errores.ToString());
        }

        private long GetCantidadDeEscuelasPorTipoEnLocalidad(int idLocalidad, int tipoEscuela)
        {
            return DaoEscuela.GetCantidadDeEscuelasPorTipoYLocalidad(idLocalidad, tipoEscuela);
        }

        private string GetPrefijoNombreEscuelaByTipoEscuela(int tipoEscuela)
        {
            var tipo = DaoProvider.GetDaoTipoEscuela().GetById(tipoEscuela);
            if (tipo == null)
                throw new BaseException(Resources.Empresa.TipoEscuelaNoValido);
            var prefijo = tipo.Abreviatura;
            return prefijo;
            //switch (tipoEscuela)
            //{
            //    case TipoEscuelaEnum.IPEM:
            //        return "I.P.E.M.";
            //        break;
            //    case TipoEscuelaEnum.CAPACITACION_LABORAL:
            //        return "CENTRO DE CAPACITACION LABORAL";
            //        break;
            //    case TipoEscuelaEnum.ESPECIAL:
            //        return "ESCUELA ESPECIAL";
            //        break;
            //    case TipoEscuelaEnum.CENPA:
            //        return "C.E.N.P.A.";
            //        break;
            //    case TipoEscuelaEnum.CENMA:
            //        return "C.E.N.M.A.";
            //        break;
            //    //case TipoEscuelaEnum.NOCTURNA:
            //    //    break;
            //    case TipoEscuelaEnum.INSTITUTO:
            //        return "INSTITUTO";
            //        break;
            //    case TipoEscuelaEnum.ESCUELA_NORMAL:
            //        return "ESCUELA NORMAL";
            //        break;
            //    case TipoEscuelaEnum.CONSERVATORIO_SUPERIOR_DE_MUSICA:
            //        return "CONSERVATORIO SUPERIOR DE MUSICA";
            //        break;
            //    case TipoEscuelaEnum.ESCUELA_SUPERIOR_DE_ARTES_APLICADAS:
            //        return "ESCUELA SUPERIOR DE ARTES APLICADAS";
            //        break;
            //    case TipoEscuelaEnum.CEPEA:
            //        return "C.E.P.E.A.";
            //        break;
            //    case TipoEscuelaEnum.ESCUELA_SUPERIOR:
            //        return "ESCUELA SUPERIOR";
            //        break;
            //    case TipoEscuelaEnum.ESCUELA_SUPERIOR_DE_BELLAS_ARTES:
            //        return "ESCUELA SUPERIOR DE BELLAS ARTES";
            //        break;
            //    case TipoEscuelaEnum.ESCUELA_SUBOFICIALES_Y_AGENTES:
            //        return "ESCUELA SUBOFICIALES Y AGENTES";
            //        break;
            //    case TipoEscuelaEnum.ESCUELA_SUPERIOR_DE_COMERCIO:
            //        return "ESCUELA_SUPERIOR_DE_COMERCIO";
            //        break;
            //}
            //return string.Empty;
        }

        private HistorialEstadoEmpresa NuevoEstado(EstadoEmpresaEnum Estado, EmpresaBase Empresa)
        {
            var currentUsuario = Usuario.CurrentDomain;
            //Mapper.Map<AgenteModel, Agente>(ServiceLocator.Current.GetInstance<IUsuarioRules>().GetCurrentAgente());
            var newEstado = new HistorialEstadoEmpresa();

            newEstado.UsuarioModificacion = currentUsuario;
            newEstado.Empresa = Empresa;
            newEstado.Estado = Estado;
            newEstado.FechaModificacion = DateTime.Today;
            
            return newEstado;
        }

        private string ObtenerCuerpoMail(EmpresaBase entidad, OpcionEnvioMailEnum tipoMail)
        {
            Escuela escuela = null;
            Inspeccion inspeccion = null;
            DireccionDeNivel direccionDeNivel = null;
            var pedidos = DaoProvider.GetDaoPedidoAutorizacionCierre().GetByFiltros(null, entidad.Id, null, null, null,
                                                                                    null);
            var asignaciones = DaoProvider.GetDaoAsignacionInstrumentoLegal().GetByEmpresaId(entidad.Id);
            PedidoAutorizacionCierre pedido = null;
            AsignacionInstrumentoLegal asignacion = null;
            switch (entidad.TipoEmpresa)
            {
                case TipoEmpresaEnum.DIRECCION_DE_NIVEL:
                    direccionDeNivel = Mapper.Map<EmpresaBase, DireccionDeNivel>(entidad);
                    break;
                case TipoEmpresaEnum.INSPECCION:
                    inspeccion = Mapper.Map<EmpresaBase, Inspeccion>(entidad);
                    break;
                case TipoEmpresaEnum.ESCUELA_MADRE:
                case TipoEmpresaEnum.ESCUELA_ANEXO:
                    escuela = (Escuela)entidad;//Mapper.Map<EmpresaBase, Escuela>(entidad);
                    break;
            }
            StringBuilder cuerpo = new StringBuilder();
            switch (tipoMail)
            {
                case OpcionEnvioMailEnum.ACTIVACION:
                    if (asignaciones.Count == 0)
                        throw new BaseException("Falta la asignacion de instrumento legal");
                    asignacion = asignaciones.Last();
                    cuerpo.AppendLine("Fecha inicio de actividades: " + entidad.FechaInicioActividad.ToShortDateString());
                    cuerpo.AppendLine("Fecha inicio de notificación: " + DateTime.Today.ToShortDateString());
                    cuerpo.AppendLine("Teléfono: " + (string.IsNullOrEmpty(entidad.Telefono) ? "-" : entidad.Telefono));
                    cuerpo.AppendLine("Fecha alta: " + entidad.FechaAlta.ToShortDateString());
                    cuerpo.AppendLine("Tipo de empresa: " + entidad.TipoEmpresa.ToString().Replace("_", " "));
                    cuerpo.AppendLine("Código de empresa: " + entidad.CodigoEmpresa);
                    cuerpo.AppendLine("Nombre de empresa: " + entidad.Nombre);
                    cuerpo.AppendLine("Estado de empresa: " + EstadoEmpresaEnum.GENERADA.ToString());
                    cuerpo.AppendLine("Empresa padre organigrama: " +
                                      (entidad.EmpresaPadreOrganigrama == null
                                           ? "-"
                                           : entidad.EmpresaPadreOrganigrama.Nombre));
                    cuerpo.AppendLine("Programa presupuestario: " +
                                      (entidad.ProgramaPresupuestario == null
                                           ? "-"
                                           : entidad.ProgramaPresupuestario.Codigo));
                    cuerpo.AppendLine("Orden de pago: " +
                                      (entidad.OrdenDePago == null
                                           ? "-"
                                           : entidad.OrdenDePago.Numero));
                    cuerpo.AppendLine("Asignacion de instrumento legal: " +
                                      asignacion.TipoMovimientoInstrumentoLegal.Nombre + ". Instrumento legal Nro: " +
                                      asignacion.InstrumentoLegal.NroInstrumentoLegal);
                    cuerpo.AppendLine("Domicilio: " + (entidad.Domicilio == null ? "-" : entidad.Domicilio.ToString()));
                    //cuerpo.AppendLine("Paquete presupuestado: "
                    cuerpo.AppendLine("Edificio(s): " +
                                      (entidad.VinculoEmpresaEdificio == null ||
                                       entidad.VinculoEmpresaEdificio.Count == 0
                                           ? "-"
                                           : entidad.VinculoEmpresaEdificio.Count.ToString()));
                    if (direccionDeNivel != null)
                    {
                        cuerpo.AppendLine("Sigla: " + direccionDeNivel.Sigla);
                        if (direccionDeNivel.NivelesEducativos.Count > 0)
                        {
                            cuerpo.Append("Nivel(es) educativo(s): ");
                            foreach (var nivel in direccionDeNivel.NivelesEducativos)
                            {
                                cuerpo.Append(nivel.Nombre + ", ");
                            }
                            cuerpo.Remove(cuerpo.Length - 2, 2);
                            cuerpo.AppendLine();
                        }
                        if (direccionDeNivel.TipoEscuelaACrear.Count > 0)
                        {
                            cuerpo.Append("Tipo(s) de escuela(s) a crear: ");
                            foreach (var tipo in direccionDeNivel.TipoEscuelaACrear)
                            {
                                cuerpo.Append(tipo.Nombre + ", ");
                            }
                            cuerpo.Remove(cuerpo.Length - 2, 2);
                            cuerpo.AppendLine();
                        }
                    }
                    else if (inspeccion != null)
                    {
                        cuerpo.AppendLine("Tipo de inspección: " + inspeccion.TipoInspeccion.ToString());
                        if (
                            new EntidadesGeneralesRules().GetValorParametroBooleano(
                                ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA))
                            cuerpo.AppendLine("Empresa de inspección que la supervisará: " +
                                              inspeccion.EmpresaPadreOrganigrama.Nombre);
                        else
                        {
                            cuerpo.AppendLine("Empresa de inspección que la supervisará: " +
                                              inspeccion.EmpresaDeInspeccionSuperior.Nombre);
                            cuerpo.AppendLine("Tipo de inspección intermedia: " +
                                              inspeccion.TipoInspeccionIntermedia.Nombre);
                        }
                    }
                    else if (escuela != null)
                    {
                        cuerpo.AppendLine("Tipo de dirección de nivel: " + escuela.TipoDireccionNivel.Sigla);
                        cuerpo.AppendLine("CUE: " + escuela.CUE + "-" + escuela.CUEAnexo.ToString());
                        cuerpo.AppendLine("Tipo de escuela: " + escuela.TipoEscuela.Abreviatura);
                        cuerpo.AppendLine("Nivel educativo: " + escuela.NivelEducativo.Nombre);
                        cuerpo.AppendLine("Nro. de escuela: " + escuela.NumeroEscuela.ToString());
                        //TODO: VER Cómo mostrar los períodos lectivos (cambiado el 23/08/2011)
                        //cuerpo.AppendLine("Período lectivo: " + escuela.PeriodoLectivo.Nombre);
                        cuerpo.AppendLine("Categoría: " + escuela.TipoCategoria.ToString());
                        cuerpo.AppendLine("Tipo de educación: " + escuela.TipoEducacion.ToString());
                        cuerpo.AppendLine("Ámbito: " + escuela.Ambito.ToString());
                        cuerpo.AppendLine("Zona desfavorable: " + escuela.ZonaDesfavorable.Nombre);
                        cuerpo.AppendLine("Privada: " + escuela.EscuelaPrivada == null ? "No" : "Si");
                        cuerpo.AppendLine("Religiosa: " + (escuela.Religioso ? "Si" : "No"));
                        cuerpo.AppendLine("Arencelada: " + (escuela.Arancelado ? "Si" : "No"));
                        cuerpo.AppendLine("Albergue: " + (escuela.Albergue ? "Si" : "No"));
                        cuerpo.AppendLine("Tipo cooperadora: " +
                                          (escuela.TipoCooperadora == null
                                               ? "-"
                                               : escuela.TipoCooperadora.Value.ToString().Replace("_", " ")));
                        cuerpo.AppendLine("Dependencia: " + escuela.Dependencia.ToString());
                        cuerpo.AppendLine("Tipo de jornada: " + escuela.TipoJornada.Nombre);
                        cuerpo.AppendLine("Modalidad de jornada: " + escuela.ModalidadJornada.Nombre);
                        //cuerpo.AppendLine("Contexto de encierro: " + (escuela.ContextoDeEncierro? "Si" : "No"));
                        //cuerpo.AppendLine("Hospitalaria: " + (escuela.Hospitalaria ? "Si" : "No"));
                        cuerpo.AppendLine("Colectivos: " +
                                          (string.IsNullOrEmpty(escuela.Colectivos) ? "-" : escuela.Colectivos));
                        cuerpo.AppendLine("Horario de funcionamiento: " + (string.IsNullOrEmpty(escuela.HorarioDeFuncionamiento) ? "-" : escuela.HorarioDeFuncionamiento));
                        if (escuela.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
                            cuerpo.AppendLine("Escuela raíz: " + (escuela.EsRaiz ? "Si" : escuela.EscuelaRaiz.Nombre));
                        else//es TipoEmpresaEnum.ESCUELA_ANEXO
                        {
                            cuerpo.AppendLine("Nro. anexo: " + escuela.NumeroAnexo.ToString());
                            cuerpo.AppendLine("Escuela madre: " + escuela.EscuelaMadre.Nombre);
                        }
                    }
                    break;
                case OpcionEnvioMailEnum.PEDIDO_CIERRE:
                    if (pedidos.Count == 0)
                        throw new BaseException("Falta el pedido");
                    pedido = pedidos.Last();
                    if (asignaciones.Count == 0)
                        throw new BaseException("Falta la asignacion de instrumento legal");
                    asignacion = asignaciones.Last();
                    cuerpo.AppendLine("Nro. de pedido: " + pedido.Id.ToString());
                    cuerpo.AppendLine("Fecha alta: " + pedido.FechaAlta.ToShortDateString());
                    cuerpo.AppendLine("Estado: " + pedido.Estados.Last().Estado.ToString());
                    cuerpo.AppendLine("Agente alta: " + pedido.AgenteAlta.NombreAMostrar);
                    cuerpo.AppendLine("Empresa que se desea cerrar: " + entidad.TipoEmpresa.ToString().Replace("_", " ") + " " +
                                      entidad.Nombre);
                    cuerpo.AppendLine("Asignacion de instrumento legal: " +
                                      asignacion.TipoMovimientoInstrumentoLegal.Nombre + ". Instrumento legal Nro: " +
                                      asignacion.InstrumentoLegal.NroInstrumentoLegal);
                    break;
                case OpcionEnvioMailEnum.CIERRE:
                    if (asignaciones.Count == 0)
                        throw new BaseException("Falta la asignacion de instrumento legal");
                    asignacion = asignaciones.Last();
                    cuerpo.AppendLine("Fecha cambio de estado: " +
                                      entidad.FechaUltimaModificacion.Value.ToShortDateString());
                    cuerpo.AppendLine("Agente cambio de estado: " + entidad.UsuarioModificacion.NombreAMostrar);
                    cuerpo.AppendLine("Empresa que es cerrada: " + entidad.Nombre);
                    cuerpo.AppendLine("Estado de la empresa: " + EstadoEmpresaEnum.CERRADA.ToString());
                    cuerpo.AppendLine("Asignacion de instrumento legal: " +
                                      asignacion.TipoMovimientoInstrumentoLegal.Nombre + ". Instrumento legal Nro: " +
                                      asignacion.InstrumentoLegal.NroInstrumentoLegal);
                    break;
                case OpcionEnvioMailEnum.REACTIVACION:
                    asignacion = asignaciones.Last();
                    cuerpo.AppendLine("Código de empresa: " + entidad.CodigoEmpresa);
                    cuerpo.AppendLine("Nombre de empresa: " + entidad.Nombre);
                    cuerpo.AppendLine("Fecha cambio estado: " + (new DateTime()).ToShortDateString());
                    cuerpo.AppendLine("Estado de empresa: " + EstadoEmpresaEnum.EN_PROCESO_DE_REACTIVACION.ToString());
                    cuerpo.AppendLine("Teléfono: " + entidad.Telefono);
                    cuerpo.AppendLine("Tipo de empresa: " + entidad.TipoEmpresa.ToString().Replace("_", " "));
                    cuerpo.AppendLine("Observaciones: " + entidad.Observaciones);
                    cuerpo.AppendLine("Empresa padre organigrama: " +
                                      (entidad.EmpresaPadreOrganigrama == null
                                           ? "-"
                                           : entidad.EmpresaPadreOrganigrama.Nombre));
                    cuerpo.AppendLine("Programa presupuestario: " +
                                      (entidad.ProgramaPresupuestario == null
                                           ? "-"
                                           : entidad.ProgramaPresupuestario.Codigo));
                    cuerpo.AppendLine("Orden de pago: " +
                                      (entidad.OrdenDePago == null
                                           ? "-"
                                           : entidad.OrdenDePago.Numero));
                    //TODO: acá falta checkear si se cargó o no asignación, sino va a mandar datos equivocados el mail
                    cuerpo.AppendLine("Asignacion de instrumento legal: " +
                                      asignacion.TipoMovimientoInstrumentoLegal.Nombre + ". Instrumento legal Nro: " +
                                      asignacion.InstrumentoLegal.NroInstrumentoLegal);
                    cuerpo.AppendLine("Domicilio: " + (entidad.Domicilio == null ? "-" : entidad.Domicilio.ToString()));
                    //cuerpo.AppendLine("Paquete presupuestado: "
                    cuerpo.AppendLine("Edificio(s): " +
                                      (entidad.VinculoEmpresaEdificio == null ||
                                       entidad.VinculoEmpresaEdificio.Count == 0
                                           ? "-"
                                           : entidad.VinculoEmpresaEdificio.Count.ToString()));
                    if (entidad.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL)
                    {
                        cuerpo.AppendLine("Sigla: " + direccionDeNivel.Sigla);
                        if (direccionDeNivel.NivelesEducativos.Count > 0)
                        {
                            cuerpo.Append("Nivel(es) educativo(s): ");
                            foreach (var nivel in direccionDeNivel.NivelesEducativos)
                            {
                                cuerpo.Append(nivel.Nombre + ", ");
                            }
                            cuerpo.Remove(cuerpo.Length - 2, 2);
                            cuerpo.AppendLine();
                        }
                        if (direccionDeNivel.TipoEscuelaACrear.Count > 0)
                        {
                            cuerpo.Append("Tipo(s) de escuela(s) a crear: ");
                            foreach (var tipo in direccionDeNivel.TipoEscuelaACrear)
                            {
                                cuerpo.Append(tipo.Nombre + ", ");
                            }
                            cuerpo.Remove(cuerpo.Length - 2, 2);
                            cuerpo.AppendLine();
                        }
                    }
                    if (entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE || entidad.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
                    {
                        cuerpo.AppendLine("Tipo de dirección de nivel: " + escuela.TipoDireccionNivel.Sigla);
                        cuerpo.AppendLine("CUE: " + escuela.CUE + "-" + escuela.CUEAnexo.ToString());
                        cuerpo.AppendLine("Tipo de escuela: " + escuela.TipoEscuela.Abreviatura);
                        cuerpo.AppendLine("Nivel educativo: " + escuela.NivelEducativo.Nombre);
                        cuerpo.AppendLine("Nro. de escuela: " + escuela.NumeroEscuela.ToString());
                        //cuerpo.AppendLine("Empresa de Inspección: " + );
                        //cuerpo.AppendLine("Escuela de la Asignación Inspección a Escuela: " + );
                        cuerpo.AppendLine("Categoría: " + escuela.TipoCategoria.ToString());
                        cuerpo.AppendLine("Tipo de educación: " + escuela.TipoEducacion.ToString());
                        cuerpo.AppendLine("Ámbito: " + escuela.Ambito.ToString());
                        cuerpo.AppendLine("Zona desfavorable: " + escuela.ZonaDesfavorable.Nombre);
                        cuerpo.AppendLine("Privada: " + escuela.EscuelaPrivada == null ? "No" : "Si");
                        cuerpo.AppendLine("Religiosa: " + (escuela.Religioso ? "Si" : "No"));
                        cuerpo.AppendLine("Arencelada: " + (escuela.Arancelado ? "Si" : "No"));
                        cuerpo.AppendLine("Albergue: " + (escuela.Albergue ? "Si" : "No"));
                        cuerpo.AppendLine("Tipo cooperadora: " +
                                          (escuela.TipoCooperadora == null
                                               ? "-"
                                               : escuela.TipoCooperadora.Value.ToString().Replace("_", " ")));
                        cuerpo.AppendLine("Dependencia: " + escuela.Dependencia.ToString());
                        cuerpo.AppendLine("Tipo de jornada: " + escuela.TipoJornada.Nombre);
                        cuerpo.AppendLine("Modalidad de jornada: " + escuela.ModalidadJornada.Nombre);
                        cuerpo.AppendLine("Contexto de encierro: " + (escuela.ContextoDeEncierro ? "Si" : "No"));
                        cuerpo.AppendLine("Hospitalaria: " + (escuela.Hospitalaria ? "Si" : "No"));
                        cuerpo.AppendLine("Colectivos: " +
                                          (string.IsNullOrEmpty(escuela.Colectivos) ? "-" : escuela.Colectivos));
                        cuerpo.AppendLine("Horario de funcionamiento: " + (string.IsNullOrEmpty(escuela.HorarioDeFuncionamiento) ? "-" : escuela.HorarioDeFuncionamiento));
                        if (escuela.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
                            cuerpo.AppendLine("Escuela raíz: " + (escuela.EsRaiz ? "Si" : escuela.EscuelaRaiz.Nombre));
                        else//es TipoEmpresaEnum.ESCUELA_ANEXO
                        {
                            cuerpo.AppendLine("Nro. anexo: " + escuela.NumeroAnexo.ToString());
                            cuerpo.AppendLine("Escuela madre: " + escuela.EscuelaMadre.Nombre);
                        }
                    }
                    if (entidad.TipoEmpresa == TipoEmpresaEnum.INSPECCION)
                    {
                        cuerpo.AppendLine("Tipo de inspección: " + inspeccion.TipoInspeccion.ToString());
                        if (
                            new EntidadesGeneralesRules().GetValorParametroBooleano(
                                ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA))
                            cuerpo.AppendLine("Empresa de inspección que la supervisará: " +
                                              inspeccion.EmpresaPadreOrganigrama.Nombre);
                        else
                        {
                            cuerpo.AppendLine("Empresa de inspección que la supervisará: " +
                                              inspeccion.EmpresaDeInspeccionSuperior.Nombre);
                            cuerpo.AppendLine("Tipo de inspección intermedia: " +
                                              inspeccion.TipoInspeccionIntermedia.Nombre);
                        }
                    }
                    break;
            }
            return cuerpo.ToString();
        }

        public void DomicilioSave(long domicilioId, EmpresaRegistrarModel model)
        {
            if(!string.IsNullOrEmpty(model.CalleNuevoDomicilio) && !string.IsNullOrEmpty(model.AlturaNuevoDomicilio))
            {
                var domicilio = DaoProvider.GetDaoDomicilio().GetById(domicilioId);
                domicilio.Id = 0;
                domicilio.Calle = DaoProvider.GetDaoCalle().GetByFiltros(model.CalleNuevoDomicilio,
                                                                         domicilio.Calle.Localidad.Id, null).FirstOrDefault();
                domicilio.Altura = Convert.ToInt32(model.AlturaNuevoDomicilio);
                domicilio.Origen = OrigenEnum.T_EM_EMPRESAS;
                domicilio.EntidadId = model.Id.ToString();
                DaoProvider.GetDaoDomicilio().Save(domicilio);/*
                var entidad = DaoEmpresa.GetById(model.Id);
                entidad.Domicilio = domResultado;
                DaoEmpresa.SaveOrUpdate(entidad);*/
            }
            else
            {
                var domicilio = DaoProvider.GetDaoDomicilio().GetById(domicilioId);
                domicilio.Id = 0;
                domicilio.Origen = OrigenEnum.T_EM_EMPRESAS;
                domicilio.EntidadId = model.Id.ToString();
                DaoProvider.GetDaoDomicilio().Save(domicilio);
            }
        }

        public string GetDepartamentoProvincialById(int idDpto)
        {
            var departamento = DaoProvider.GetDaoDepartamentoProvincial().GetById(idDpto);
            return departamento.Nombre;
        }

        public string GetTipoMovimientoAsginacionInstrumentoLegal(int idTipoMov)
        {
            var tipoMov = DaoProvider.GetDaoTipoMovimientoInstrumentoLegal().GetById(idTipoMov);
            return tipoMov.Nombre;
        }

        #endregion

        /// <summary>
        /// Busca las diagramaciones curso por escuela
        /// </summary>
        /// <param name="idEscuela">id escuela</param>
        /// <returns></returns>
        public List<DiagramacionCursoModel> GetDiagramacionCursoByEscuelaId(int idEscuela)
        {
            var diagramaciones = DaoProvider.GetDaoDiagramacionCurso().GetByEscuela(idEscuela);
            return Mapper.Map<List<DiagramacionCurso>, List<DiagramacionCursoModel>>(diagramaciones);
        }

        public FechaYUsuarioCierreEmpresaModel GetUsuarioDeCierre(int empresaId)
        {
            var pedidoAutorizacionCierre = DaoProvider.GetDaoPedidoAutorizacionCierre().GetUsuarioPedidoByIdEmpresa(empresaId);
            return Mapper.Map<DtoFechaYUsuarioCierreEmpresa, FechaYUsuarioCierreEmpresaModel>(pedidoAutorizacionCierre);
        }

        public List<DtoEscuelaReporte> GetEscuelasByFiltros(string filtroCUE, string filtroCUEAnexo, string filtroCodigoEmpresa, string filtroNombreEmpresa, CategoriaEscuelaEnum? filtroTipoCategoria,
                                                            TipoEducacionEnum? filtroTipoEducacion, int? filtroNivelEducativo, EstadoEmpresaEnum? filtroEstado, int? filtroDepartamento, int? filtroLocalidad,
                                                            string filtroOrdenPago, string filtroProgPresupuestario, DependenciaEnum? filtroDependencia, AmbitoEscuelaEnum? filtroAmbito, List<int> listaZonas, bool filtroPublica, bool filtroPrivada)
        {
            var lista = DaoProvider.GetDaoEscuela().GetEscuelasByFiltros(filtroCUE, filtroCUEAnexo, filtroCodigoEmpresa, filtroNombreEmpresa, filtroTipoCategoria, filtroTipoEducacion, filtroNivelEducativo, filtroEstado, filtroDepartamento, 
                                                                         filtroLocalidad, filtroOrdenPago, filtroProgPresupuestario, filtroDependencia, filtroAmbito, listaZonas, filtroPublica, filtroPrivada);
            return lista;
        }

        public List<DtoEscuelaAnexoReporte> GetEscuelasAnexoByFiltros(string filtroCUE, string filtroCUEAnexo, string filtroCodigoEmpresa, string filtroNombreEmpresa, CategoriaEscuelaEnum? filtroTipoCategoria,
                                                            TipoEducacionEnum? filtroTipoEducacion, int? filtroNivelEducativo, EstadoEmpresaEnum? filtroEstado, int? filtroDepartamento, int? filtroLocalidad,
                                                            string filtroOrdenPago, string filtroProgPresupuestario, DependenciaEnum? filtroDependencia, AmbitoEscuelaEnum? filtroAmbito, List<int> listaZonas, bool filtroPublica, bool filtroPrivada)
        {
            var lista = DaoProvider.GetDaoEscuela().GetEscuelasAnexoByFiltros(filtroCUE, filtroCUEAnexo, filtroCodigoEmpresa, filtroNombreEmpresa, filtroTipoCategoria, filtroTipoEducacion, filtroNivelEducativo, filtroEstado, filtroDepartamento,
                                                                         filtroLocalidad, filtroOrdenPago, filtroProgPresupuestario, filtroDependencia, filtroAmbito, listaZonas, filtroPublica, filtroPrivada);
            return lista;
        }

        public bool EsDireccionNivelSuperior(int idEscuelaLogueado)
        {
            return DaoDireccionDeNivel.EsNivelSuperior(idEscuelaLogueado);
        }
       
    }
}