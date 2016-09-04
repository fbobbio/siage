using System;
using System.Collections.Generic;
using System.Linq;
using Siage.Base.Dto;
using Siage.Services.Core.Models;
using Siage.Base;
using Siage.Core.DaoInterfaces;
using Siage.Core.Domain;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using AutoMapper;

namespace Siage.UCControllers.Rules
{
    public class EntidadesGeneralesRules : IEntidadesGeneralesRules
    {
        #region Atributos

        private static IEntidadesGeneralesRules _entidadesGeneralesRules;
        private IDaoProvider _daoProvider;

        #endregion

        #region Propiedades

        public static IEntidadesGeneralesRules Instancia
        {
            get
            {
                if (_entidadesGeneralesRules == null)
                    _entidadesGeneralesRules = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
                return _entidadesGeneralesRules;
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

        #endregion

        #region IEntidadesGeneralesRules Members
        public List<MotivoBajaSancionModel> GetMotivoBajaSancionAll()
        {
            var dao = DaoProvider.GetDaoMotivoBajaSancion();
            return Mapper.Map<List<MotivoBajaSancion>, List<MotivoBajaSancionModel>>(dao.GetAll());
        }
        public List<DetalleAsignaturaModel> GetDetalleAsignaturaAll()
        {
            var dao = DaoProvider.GetDaoDetalleAsignatura();
            return Mapper.Map<List<DetalleAsignatura>, List<DetalleAsignaturaModel>>(dao.GetAll());
        }
        public List<MotivoSancionModel> GetMotivoSancionAll()
        {
            var dao = DaoProvider.GetDaoMotivoSancion();
            return Mapper.Map<List<MotivoSancion>, List<MotivoSancionModel>>(dao.GetAll());
        }
        public List<AsignaturaModel> GetAsignaturaAll()
        {
            var dao = DaoProvider.GetDaoAsignatura();
            return Mapper.Map<List<Asignatura>, List<AsignaturaModel>>(dao.GetAll());
        }

        public List<SeccionModel> GetSeccionAll()
        {
            var dao = DaoProvider.GetDaoSeccion();
            return Mapper.Map<List<Seccion>, List<SeccionModel>>(dao.GetAll());
        }

        public List<SituacionDeRevistaModel> GetSituacionRevistaAll()
        {
            var dao = DaoProvider.GetDaoSituacionDeRevista();
            return Mapper.Map<List<SituacionDeRevista>, List<SituacionDeRevistaModel>>(dao.GetAll());
        }

        public List<SituacionDeRevistaModel> GetSituacionDeRevistaMab()
        {
            var dao = DaoProvider.GetDaoSituacionDeRevista();
            return Mapper.Map<List<SituacionDeRevista>, List<SituacionDeRevistaModel>>(dao.GetSituacionDeRevistaMab());
        }

        public List<ModalidadMabModel> GetModalidadMabAll()
        {
            var dao = DaoProvider.GetDaoModalidadMab();
            return Mapper.Map<List<ModalidadMab>, List<ModalidadMabModel>>(dao.GetAll());
        }

        public List<CodigoMovimientoMabModel> GetCodigoMovimientoMabAll()
        {
            var dao = DaoProvider.GetDaoCodigoMovimientoMab();
            return Mapper.Map<List<CodigoMovimientoMab>, List<CodigoMovimientoMabModel>>(dao.GetAll());
        }
        public List<AgrupamientoCargoModel> GetAgrupamientoCargoAll()
        {
            var dao = DaoProvider.GetDaoAgrupamientoCargo();
            return Mapper.Map<List<AgrupamientoCargo>, List<AgrupamientoCargoModel>>(dao.GetAll());
        }

        public List<NivelCargoModel> GetNivelCargoAll()
        {
            var dao = DaoProvider.GetDaoNivelCargo();
            return Mapper.Map<List<NivelCargo>, List<NivelCargoModel>>(dao.GetAll());
        }
        public List<CondicionIvaModel> GetCondicioIvaAll()
        {
            var dao = DaoProvider.GetDaoCondicionIva();
            return Mapper.Map<List<CondicionIva>, List<CondicionIvaModel>>(dao.GetAll());
        }
      
        public List<EstadoPuestoModel> GetEstadoPuestoTrabajoAll()
        {
            var dao = DaoProvider.GetDaoEstadoPuestoTrabajo();
            return Mapper.Map<List<EstadoPuesto>, List<EstadoPuestoModel>>(dao.GetAll());
        }

        public List<GrupoModel> GetGrupoAll()
        {
            var dao = DaoProvider.GetDaoGrupo();
            return Mapper.Map<List<Grupo>, List<GrupoModel>>(dao.GetAll());
        }

        public List<SubGrupoModel> GetSubGrupoAll()
        {
            var dao = DaoProvider.GetDaoSubGrupo();
            return Mapper.Map<List<SubGrupo>, List<SubGrupoModel>>(dao.GetAll());
        }

        public List<EscuelaModel> GetEscuelaAll()
        {
            var dao = DaoProvider.GetDaoEscuela();
            return Mapper.Map<List<Escuela>, List<EscuelaModel>>(dao.GetAll());
        }

        public List<EscuelaModel> GetEscuelasMadres()
        {
            var dao = DaoProvider.GetDaoEscuela();
            return Mapper.Map<List<Escuela>, List<EscuelaModel>>((List<Escuela>)dao.GetEscuelaByTipo(TipoEmpresaEnum.ESCUELA_MADRE));
        }

        public List<InspeccionModel> GetEmpresasInspeccion(int idEmpresaUsuario)
        {
            var daoEmpresa = DaoProvider.GetDaoEmpresaBase();
            //Traigo la empresa del usuario logueado
            var empresaUsuarioLogueado = daoEmpresa.GetById(idEmpresaUsuario);
            var daoInspecciones = DaoProvider.GetDaoInspeccion();
            //Traigo las inspecciones que tengan como dirección de nivel la empresa del usuario logueado
            var domains = daoInspecciones.GetByDireccionNivel(empresaUsuarioLogueado.TipoDireccionNivel);

            return Mapper.Map<List<Inspeccion>, List<InspeccionModel>>(domains);
        }

        public List<GradoAñoModel> GetGradoAñoAll()
        {
            var dao = DaoProvider.GetDaoGradoAño();
            return Mapper.Map<List<GradoAnio>, List<GradoAñoModel>>(dao.GetAll());
        }

        public List<TipoAdquisicionModel> GetTipoAdquisicionAll()
        {
            var dao = DaoProvider.GetDaoTipoAdquisicion();
            return Mapper.Map<List<TipoAdquisicion>, List<TipoAdquisicionModel>>(dao.GetAll());
        }

        public List<TipoLocalModel> GetTipoLocalAll()
        {
            var dao = DaoProvider.GetDaoTipoLocal();
            return Mapper.Map<List<TipoLocal>, List<TipoLocalModel>>(dao.GetAll());
        }

        public List<CarreraModel> GetCarreraAll()
        {
            var dao = DaoProvider.GetDaoCarrera();
            return Mapper.Map<List<Carrera>, List<CarreraModel>>(dao.GetAll());
        }

        public List<DetalleHoraTurnoModel> GetDetalleHorasTurnoAll()
        {
            var dao = DaoProvider.GetDaoDetalleHoraTurno();
            return Mapper.Map<List<DetalleHoraTurno>, List<DetalleHoraTurnoModel>>(dao.GetAll());
        }

        public List<FuncionEdificioModel> GetFuncionEdificioAll()
        {
            var dao = DaoProvider.GetDaoFuncionEdificio();
            return Mapper.Map<List<FuncionEdificio>, List<FuncionEdificioModel>>(dao.GetAll());
        }

        public List<TipoEstructuraEdiliciaModel> GetTipoEstructuraEdilicia()
        {
            var dao = DaoProvider.GetDaoTipoEstructuraEdilicia();
            return Mapper.Map<List<TipoEstructuraEdilicia>, List<TipoEstructuraEdiliciaModel>>(dao.GetAll());
        }

        public List<PredioModel> GetPredioAll()
        {
            var dao = DaoProvider.GetDaoPredio();
            return Mapper.Map<List<Predio>, List<PredioModel>>(dao.GetAll());
        }

        public List<DepartamentoProvincialModel> GetDepartamentoProvincialByProvincia(string idProvincia)
        {
            var dao = DaoProvider.GetDaoDepartamentoProvincial();
            return Mapper.Map<List<DepartamentoProvincial>, List<DepartamentoProvincialModel>>(dao.GetAllByProvincia(idProvincia)).OrderBy(x => x.Nombre).ToList();
        }

        public List<LocalidadModel> GetLocalidadByDepartamentoProvincial(int idDepartamentoProvincial)
        {
            var dao = DaoProvider.GetDaoLocalidad();
            return Mapper.Map<List<Localidad>, List<LocalidadModel>>(dao.GetAllByDepartamentoProvincial(idDepartamentoProvincial)).OrderBy(x => x.Nombre).ToList();
        }

        public List<LocalidadModel> GetLocalidadByProvincia(string idProvincia)
        {
            var dao = DaoProvider.GetDaoLocalidad();
            return Mapper.Map<List<Localidad>, List<LocalidadModel>>(dao.GetAllByProvincia(idProvincia)).OrderBy(x => x.Nombre).ToList();
        }

        public List<PaisModel> GetPaisAll()
        {
            var dao = DaoProvider.GetDaoPais();
            return Mapper.Map<List<Pais>, List<PaisModel>>(dao.GetAll()).OrderBy(x => x.Nombre).ToList();
        }
        public List<CalleModel> GetCalleAll()
        {
            var dao = DaoProvider.GetDaoCalle();
            return Mapper.Map<List<Calle>, List<CalleModel>>(dao.GetAll());
        }

        public PaisModel GetPaisById(string id)
        {
            var dao = DaoProvider.GetDaoPais();
            return Mapper.Map<Pais, PaisModel>(dao.GetById(id));
        }

        public List<ProvinciaModel> GetProvinciabyPais(string idPais)
        {
            var dao = DaoProvider.GetDaoProvincia();
            return Mapper.Map<List<Provincia>, List<ProvinciaModel>>(dao.GetAllByPais(idPais));
        }

        public List<TipoMovimientoInstrumentoLegalModel> GetTipoMovimientoInstrumentoLegalAll()
        {
            var dao = DaoProvider.GetDaoTipoMovimientoInstrumentoLegal();
            return Mapper.Map<List<TipoMovimientoInstrumentoLegal>, List<TipoMovimientoInstrumentoLegalModel>>(dao.GetAll());
        }

        public List<ZonaDesfavorableModel> GetZonaDesfavorableAll()
        {
            var dao = DaoProvider.GetDaoZonaDesfavorable();
            return Mapper.Map<List<ZonaDesfavorable>, List<ZonaDesfavorableModel>>(dao.GetAll());
        }

        public List<PlanEstudioModel> GetPlanEstudioAll()
        {
            var dao = DaoProvider.GetDaoPlanEstudio();
            return Mapper.Map<List<PlanEstudio>, List<PlanEstudioModel>>(dao.GetAll());
        }

        public List<LocalidadModel> GetLocalidadAll()
        {
            var dao = DaoProvider.GetDaoLocalidad();
            return Mapper.Map<List<Localidad>, List<LocalidadModel>>(dao.GetAll());
        }

        public List<CicloEducativoModel> GetCicloEducativoPorEscuela(int idEscuela)
        {
            var lista = DaoProvider.GetDaoEscuela().CicloEducativoPorEscuela(idEscuela);
            return Mapper.Map<List<CicloEducativo>, List<CicloEducativoModel>>(lista);
        }

        public List<TipoCargoModel> GetTipoCargoAll()
        {
            var dao = DaoProvider.GetDaoTipoCargo();
            return Mapper.Map<List<TipoCargo>, List<TipoCargoModel>>(dao.GetAll());
        }

        public List<ComboModel> GetTipoCargoEspecialAll()
        {
            var daoTipoCargoEspecial = DaoProvider.GetDaoTipoCargoEspecial();
            return Mapper.Map<List<TipoCargoEspecial>, List<ComboModel>>(daoTipoCargoEspecial.GetAll());
        }

        public NivelEducativoModel GetNivelEducativoByEscuela(int idEscuela)
        {
            var daoNivel = DaoProvider.GetDaoNivelEducativo();
            return Mapper.Map<NivelEducativo, NivelEducativoModel>(daoNivel.GetById(daoNivel.GetIdNivelEducativoByEscuela(idEscuela)));
        }

        public List<NivelEducativoModel> GetNivelEducativoPorDireccionNivel()
        {
            var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();

            if (empresa.TipoEmpresa != TipoEmpresaEnum.DIRECCION_DE_NIVEL)
                return new List<NivelEducativoModel>();

            var daoNivelEducativo = DaoProvider.GetDaoNivelEducativo();

            return Mapper.Map<List<NivelEducativo>, List<NivelEducativoModel>>(daoNivelEducativo.GetNivelEducativoPorDireccionNivel(empresa.Id));
        }

        public List<NivelEducativoPorTipoEducacionModel> GetNivelEducativoPorTipoEducacionByDireccionDeNivel()
        {
            var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();
            if (empresa.TipoEmpresa != TipoEmpresaEnum.DIRECCION_DE_NIVEL)
                return new List<NivelEducativoPorTipoEducacionModel>();

            var listado = DaoProvider.GetDaoNivelEducativoPorTipoEducacion().GetByDireccionDeNivel(empresa.Id);

            return Mapper.Map<List<NivelEducativoPorTipoEducacion>, List<NivelEducativoPorTipoEducacionModel>>(listado);
        }

        public List<DireccionDeNivelComboModel> GetDireccionDeNivelAll()
        {
            var dao = DaoProvider.GetDaoDireccionDeNivel();
            return Mapper.Map<List<DireccionDeNivel>, List<DireccionDeNivelComboModel>>(dao.GetAll());
        }

        public List<CicloEducativoModel> GetCicloEducativoAll()
        {
            var dao = DaoProvider.GetDaoCicloEducativo();
            return Mapper.Map<List<CicloEducativo>, List<CicloEducativoModel>>(dao.GetAll());
        }

        public List<CicloLectivoModel> GetCicloLectivoAll()
        {
            var dao = DaoProvider.GetDaoCicloLectivo();
            return Mapper.Map<List<CicloLectivo>, List<CicloLectivoModel>>(dao.GetAll());
        }

        public List<OrdenDePagoModel> GetOrdenDePagoAll()
        {
            var dao = DaoProvider.GetDaoOrdenDePago();
            return Mapper.Map<List<OrdenDePago>, List<OrdenDePagoModel>>(dao.GetAll());
        }

        public List<ProgramaPresupuestarioModel> GetProgramaPresupuestarioAll()
        {
            var dao = DaoProvider.GetDaoProgramaPresupuestario();
            return Mapper.Map<List<ProgramaPresupuestario>, List<ProgramaPresupuestarioModel>>(dao.GetAll());
        }

        public List<InspeccionModel> GetEmpresaInspeccionAll()
        {
            var dao = DaoProvider.GetDaoInspeccion();
            return Mapper.Map<List<Inspeccion>, List<InspeccionModel>>(dao.GetAll());
        }

        public List<TipoInspeccionIntermediaModel> GetTipoInspeccionIntermediaAll()
        {
            var dao = DaoProvider.GetDaoTipoInspeccionIntermedia();
            return Mapper.Map<List<TipoInspeccionIntermedia>, List<TipoInspeccionIntermediaModel>>(dao.GetAll());
        }

        public List<SucursalBancariaModel> GetSucursalBancariaAll()
        {
            var daoSucursal = DaoProvider.GetDaoSucursalBancaria();
            return Mapper.Map<List<SucursalBanco>, List<SucursalBancariaModel>>(daoSucursal.GetAll());
        }

        public List<DepartamentoProvincialModel> GetDepartamentoProvincialAll()
        {
            var dao = DaoProvider.GetDaoDepartamentoProvincial();
            return Mapper.Map<List<DepartamentoProvincial>, List<DepartamentoProvincialModel>>(dao.GetAll());
        }

        public List<NivelEducativoModel> GetNivelEducativoAll()
        {
            var dao = DaoProvider.GetDaoNivelEducativo();
            return Mapper.Map<List<NivelEducativo>, List<NivelEducativoModel>>(dao.GetAll());
        }

        public List<PeriodoLectivoModel> GetPeriodoLectivoAll()
        {
            var dao = DaoProvider.GetDaoPeriodoLectivo();
            return Mapper.Map<List<PeriodoLectivo>, List<PeriodoLectivoModel>>(dao.GetAll());
        }

        public List<EtapaNivelModel> GetEtapasNivelByNivelEducativo(int idNivel)
        {
            var dao = DaoProvider.GetDaoEtapaNivel();
            return Mapper.Map<List<EtapaNivel>, List<EtapaNivelModel>>(dao.GetEtapasNivelByNivelEducativo(idNivel));
        }
        
        public List<ObraSocialModel> GetObraSocialAll()
        {
            var dao = DaoProvider.GetDaoObraSocial();
            return Mapper.Map<List<ObraSocial>, List<ObraSocialModel>>(dao.GetAll());
        }

        public List<TurnoModel> GetTurnoAll()
        {
            var dao = DaoProvider.GetDaoTurno();
            return Mapper.Map<List<Turno>, List<TurnoModel>>(dao.GetAll());
        }

        public List<ProvinciaModel> GetProvinciaAll()
        {
            var dao = DaoProvider.GetDaoProvincia();
            return Mapper.Map<List<Provincia>, List<ProvinciaModel>>(dao.GetAll());
        }

        public EscuelaModel GetEscuelaById(int id)
        {
            var dao = DaoProvider.GetDaoEscuela();
            return Mapper.Map<Escuela, EscuelaModel>(dao.GetById(id));
        }

        public DtoConsultaEscuela GetEscuelaDtoById(int id)
        {
            return DaoProvider.GetDaoEscuela().GetDtoById(id);
        }

        /// <summary>
        /// Metodo para obtener el valor de un ValorParametro de tipo booleano
        /// </summary>
        /// <param name="parametro">enumeracion de parametro a consultar</param>
        /// <returns>TRUE o FALSE dependiendo del valor del parametro pasado</returns>
        public bool GetValorParametroBooleano(ParametroEnum parametro)
        {
            var dao = DaoProvider.GetDaoParametro();
            var valor = dao.GetValorParametroVigente(parametro, null);

            if (valor == null || string.IsNullOrEmpty(valor.Valor))
                throw new BaseException(Resources.EntidadesGenerales.PARAMETRO_SIN_VALOR + ": " + parametro);

            if (!(valor.Valor == "N" || valor.Valor == "Y"))
                throw new BaseException(Resources.EntidadesGenerales.VALOR_PARAMETRO_INCORRECTO + ": " + parametro);

            return valor.Valor == "Y" ? true : false;
        }

        /// <summary>
        /// Metodo para obtener el valor de un ValorParametro de tipo entero
        /// </summary>
        /// <param name="parametro">enumeracion de parametro a consultar</param>
        /// <returns>el valor entero del parametro pasado</returns>
        public int GetValorParametroEntero(ParametroEnum parametro)
        {
            var dao = DaoProvider.GetDaoParametro();
            var valor = dao.GetValorParametroVigente(parametro, null);

            if (valor == null || string.IsNullOrEmpty(valor.Valor))
                throw new BaseException(Resources.EntidadesGenerales.PARAMETRO_SIN_VALOR + ": " + parametro);

            if (valor.Valor == "N" || valor.Valor == "Y")
                throw new BaseException(Resources.EntidadesGenerales.VALOR_PARAMETRO_INCORRECTO + ": " + parametro);

            return Convert.ToInt32(valor.Valor);
        }

        public List<TipoEscuelaModel> GetTipoEscuelaAll()
        {
            var dao = DaoProvider.GetDaoTipoEscuela();
            return Mapper.Map<List<TipoEscuela>, List<TipoEscuelaModel>>(dao.GetAll());
        }

        public List<TipoJornadaModel> GetTipoJornadaAll()
        {
            var dao = DaoProvider.GetDaoTipoJornada();
            return Mapper.Map<List<TipoJornada>, List<TipoJornadaModel>>(dao.GetAll());
        }

        public List<ModalidadJornadaModel> GetModalidadJornadaAll()
        {
            var dao = DaoProvider.GetDaoModalidadJornada();
            return Mapper.Map<List<ModalidadJornada>, List<ModalidadJornadaModel>>(dao.GetAll());
        }

        public void MandarMail(string mailRemitente, string passRemitente, string nombreRemitente, string[] mailsDestinatario, string asunto, string cuerpo)
        {
            var email = new Email()
            {
                Asunto = asunto,
                Destinatarios = new List<string>(),
                Mensaje = cuerpo,
                RemitenteNombre = nombreRemitente,
                RemitenteCuenta = mailRemitente,
                RemitentePassword = passRemitente
            };

            foreach (var mail in mailsDestinatario)
                email.Destinatarios.Add(mail);
            email.Enviar();
        }

        public PersonaFisicaModel GetPersonaFisicaById(int idPersona)
        {
            var dao = DaoProvider.GetDaoPersonaFisica();
            return Mapper.Map<PersonaFisica, PersonaFisicaModel>(dao.GetById(idPersona));
        }

        public List<TipoDomicilioModel> GetTipoDomicilioAll()
        {
            var dao = DaoProvider.GetDaoTipoDomicilio();
            return Mapper.Map<List<TipoDomicilio>, List<TipoDomicilioModel>>(dao.GetAll());
        }

        public List<TipoCalleModel> GetTipoCalleAll()
        {
            var dao = DaoProvider.GetDaoTipoCalle();
            return Mapper.Map<List<TipoCalle>, List<TipoCalleModel>>(dao.GetAll()).OrderBy(x => x.Nombre).ToList();
        }

        public List<TipoDocumentoModel> GetTipoDocumentoAll()
        {
            var dao = DaoProvider.GetDaoTipoDocumento();
            return Mapper.Map<List<TipoDocumento>, List<TipoDocumentoModel>>(dao.GetAll().Distinct().OrderBy(x => x.Nombre).ToList());
        }

        public List<TituloModel> GetTituloAll()
        {
            var dao = DaoProvider.GetDaoTitulo();
            return Mapper.Map<List<Titulo>, List<TituloModel>>(dao.GetAll());
        }

        public List<EstadoCivilModel> GetEstadoCivilAll()
        {
            var dao = DaoProvider.GetDaoEstadoCivil();
            return Mapper.Map<List<EstadoCivil>, List<EstadoCivilModel>>(dao.GetAll()).OrderBy(x => x.Nombre).ToList();
        }
        
        public List<SexoModel> GetSexoAll()
        {
            var dao = DaoProvider.GetDaoSexo();
            return Mapper.Map<List<Sexo>, List<SexoModel>>(dao.GetAll()).OrderBy(x => x.TipoSexo.ToString()).ToList();
        }

        public List<CalleModel> GetCalleByFiltro(string nombre, int? idLocalidad, int? idTipoCalle)
        {
            var dao = DaoProvider.GetDaoCalle();
            return Mapper.Map<List<Calle>, List<CalleModel>>(dao.GetByFiltros(nombre, idLocalidad, idTipoCalle));
        }

        public List<BarrioModel> GetBarrioByLocalidad(int idLocalidad)
        {
            var dao = DaoProvider.GetDaoBarrio();
            return Mapper.Map<List<Barrio>, List<BarrioModel>>(dao.GetBarrioByLocalidad(idLocalidad));
        }

        public List<OrganismoEmisorDocumentoModel> GetOrganismoEmisorDocumentoAll()
        {
            var dao = DaoProvider.GetDaoOrganismoEmisorDocumento();
            return Mapper.Map<List<OrganismoEmisorDocumento>, List<OrganismoEmisorDocumentoModel>>(dao.GetAll()).OrderBy(x => x.Nombre).ToList();
        }

        public List<TipoNovedadModel> GetTipoNovedadAll()
        {
            var dao = DaoProvider.GetDaoTipoNovedad();
            return Mapper.Map<List<TipoNovedad>, List<TipoNovedadModel>>(dao.GetAll());
        }

        public List<EstatutoModel> GetEstatutoAll()
        {
            var dao = DaoProvider.GetDaoEstatuto();
            return Mapper.Map<List<Estatuto>, List<EstatutoModel>>(dao.GetAll());
        }

        public List<MotivoBajaAgenteModel> GetMotivoBajaAgenteAll()
        {
            var dao = DaoProvider.GetDaoMotivoBajaAgente();
            return Mapper.Map<List<MotivoBajaAgente>, List<MotivoBajaAgenteModel>>(dao.GetAll());
        }
        
        public List<CalleConsultaModel> GetCalleConsultaByFiltro(string nombre, int? idLocalidad, int? idTipoCalle)
        {
            var dao = DaoProvider.GetDaoCalle();
            return Mapper.Map<List<Calle>, List<CalleConsultaModel>>(dao.GetByFiltros(nombre, idLocalidad, idTipoCalle));
        }

        public List<DocumentoRequeridoModel> GetDocumentosRequeridoPorProceso(ProcesoEnum proceso)
        {
            return ServiceLocator.Current.GetInstance<IDocumentoRequeridoRules>().DocumentoRequeridoByEscuelaLogeadaYProceso(proceso);
        }

        public List<DocumentoRequeridoModel> GetDocumentosRequeridoPorProcesoMenosPresentadosEstudiante(ProcesoEnum proceso, int idEstudiante, int idEscuela, int? idGradoAnio, int? idCarrera)
        {
            return ServiceLocator.Current.GetInstance<IDocumentoRequeridoRules>().DocumentoRequeridoByEscuelaCarreraYProcesoMenosPresentadosPorEstudiante(proceso, idEstudiante, idEscuela, idCarrera, idGradoAnio);
        }

        public List<DocumentoRequeridoModel> GetDocumentosRequeridoPorProcesoPresentadosEstudiante(ProcesoEnum proceso, int idEstudiante, int idEscuela, int? idGradoAnio, int? idCarrera)
        {
            return ServiceLocator.Current.GetInstance<IDocumentoRequeridoRules>().DocumentoRequeridoByEscuelaCarreraYProcesoPresentadosPorEstudiante(proceso, idEstudiante, idEscuela, idCarrera, idGradoAnio);
        }

        public List<DocumentoRequeridoModel> GetDocumentosRequeridoPorProcesoYCarrera(ProcesoEnum proceso, int idCarrera)
        {
            return ServiceLocator.Current.GetInstance<IDocumentoRequeridoRules>().DocumentoRequeridoByFiltros(proceso, null, idCarrera);
        }

        public List<TipoSancionModel> GetTipoSancionAll()
        {
            var daoTipoSancion = DaoProvider.GetDaoTipoSancion();
            return Mapper.Map<List<TipoSancion>, List<TipoSancionModel>>(daoTipoSancion.GetAll());
        }

        public List<TipoVinculoModel> GetTiposVinculoAll()
        {
            return Mapper.Map<List<TipoVinculo>, List<TipoVinculoModel>>(DaoProvider.GetDaoTipoVinculo().GetAll());
        }
        public List<MotivoIncorporacionModel> GetMotivoIncorporacionAll()
        {
            var dao = DaoProvider.GetDaoMotivoIncorporacion();
            return Mapper.Map<List<MotivoIncorporacion>, List<MotivoIncorporacionModel>>(dao.GetAll());
        }

        public List<TipoInstrumentoLegalModel> GetTipoInstrumentoLegalAll()
        {
            var dao = DaoProvider.GetDaoTipoInstrumentoLegal();
            return Mapper.Map<List<TipoInstrumentoLegal>, List<TipoInstrumentoLegalModel>>(dao.GetAll()); 
        }

        #endregion

        #region Soporte
        #endregion

        public List<LocalidadConsultaModel> GetLocalidadByProvinciaConsulta(string idProvincia)
        {
            var dao = DaoProvider.GetDaoLocalidad();
            var localidades = dao.GetAllByProvinciaConsulta(idProvincia);
            return Mapper.Map<List<Localidad>, List<LocalidadConsultaModel>>(localidades).OrderBy(x => x.Nombre).ToList();
        }

        public List<LocalidadConsultaModel> GetLocalidadByDepartamentoProvincialConsulta(int idDepartamentoProvincial)
        {
            var dao = DaoProvider.GetDaoLocalidad();
            return Mapper.Map<List<Localidad>, List<LocalidadConsultaModel>>(dao.GetAllByDepartamentoProvincial(idDepartamentoProvincial)).OrderBy(x => x.Nombre).ToList();
        }





        
    }
}