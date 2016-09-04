using System;
using System.Collections.Generic;
using Siage.Base.Dto;
using Siage.Core.Domain;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IEmpresaRules
    {
        
        bool EsDireccionDeNivel(int idEscuela);
        EmpresaRegistrarModel GetEmpresaById(int empresaId);
        EmpresaRegistrarModel GetEmpresaRegistrarById(int id);
        EmpresaConsultarModel GetEmpresaConsultaById(int id);
        List<EmpresaInspeccionadaPorInspeccionModel> GetEmpresasInspeccionadasPorInspeccionId(int idInspeccion);
        #region Cierre de Empresa
        EmpresaCierreModel GetEmpresaCierreModelById(int id);
        EmpresaCierreModel EmpresaCerrar(EmpresaCierreModel model);
        #endregion

        #region Reactivación de Empresa
        EmpresaReactivacionModel GetEmpresaReactivacionById(int id);
        IList<DomicilioEdificioModel> GetDomiciliosDeEdificiosVinculadosAEmpresa(int idEmpresa);
        bool EmpresaHasVinculosActivos(int idEmpresa);
        bool IsEscuelaNivelSuperior(int idEmpresa);
        bool EmpresaEsEscuela(int idEmpresa);
        bool EmpresaEsMadre(int idEmpresa);
        bool GetEscuelaMadreCerrada(int idEmpresa);
        string GetLocalidadToStringById(int idLocalidad);
        DomicilioEdificioModel FindDomicilioDeEdificio(int idEdificio);
        EmpresaReactivacionModel EmpresaReactivar(EmpresaReactivacionModel model, int idEmpresaUsuarioLogueado);
        #endregion

        List<NivelEducativoPorTipoEducacionModel> GetNivelesEducativosPorTipoEducacion(TipoEducacionEnum idTipoEducacion);
        EmpresaVisarModel GetEmpresaVisarById(int id);
        ActivacionCodigoEmpresaModel GetEmpresaActivarCodigoById(int id);
        EmpresaRegistrarModel EmpresaSave(EmpresaRegistrarModel model);
        void VisarActivacion(EmpresaVisarModel model);
        List<EmpresaConsultarModel> GetByFiltroBasico(string cue, int? CUEAnexo, string codigoEmpresa, string nombreEmpresa, List<EstadoEmpresaEnum> estadoEmpresaEnum,
            int? idLocalidad, string barrio, string calle, int? altura, List<TipoEmpresaFiltroBusquedaEnum> tiposEmpresasFiltro, int? idEmpresaUsuarioLogueado, int? idEmpresaDesdeDondeSeHaceLaConsulta, TipoEmpresaEnum tipoEmpresaUsuarioLogueado, bool seConsultaDesdeRegistrarEmpresa);

        List<EmpresaConsultarModel> GetByFiltroAvanzado(DateTime? fechaAltaDesde, DateTime? fechaAltaHasta,
                                                        DateTime? fechaDesdeInicioActividad,
                                                        DateTime? fechaHastaInicioActividad,
                                                        TipoEmpresaEnum tipoEmpresaEnum, int? idProgramaAdministrativo,
                                                        List<EstadoEmpresaEnum> estadoEmpresaEnum,
                                                        int? numeroEscuela, int? tipoEscuelaEnum,
                                                        int? programaPresupuestarioEscuela,
                                                        CategoriaEscuelaEnum? categoriaEscuelaEnum,
                                                        TipoEducacionEnum? tipoEducacionEnum, int? nivelEducativoEnum,
                                                        DependenciaEnum? dependenciaEnum,
                                                        AmbitoEscuelaEnum? ambitoEscuelaEnum,
                                                        NoSiEnum? esReligioso, NoSiEnum? esArancelado,
                                                        TipoInspeccionEnum? tipoInspeccionEnum, int? idLocalidad,
                                                        string barrio,
                                                        string calle, int? altura, int? idObraSocial,
                                                        int? idPeriodoLectivo, int? turnoEnum, string nombre,
                                                        DateTime? fechaDesdeNotificacion,
                                                        DateTime? fechaHastaNotificacion,
                                                        int? tipoInspeccionIntermediaId,
                                                        List<TipoEmpresaFiltroBusquedaEnum>
                                                            listadoTiposEmpresaPermitidos,
                                                        int? idEmpresaUsuarioLogueado, TipoEmpresaEnum tipoEmpresaUsuarioLogueado, bool seConsultaDesdeRegistrarEmpresa, string fltCodigoInspeccion);
        
        EmpresaVisadoCierreModel VisadoCierreEmpresa(EmpresaVisadoCierreModel model);
        //ActivacionCodigoEmpresaModel EmpresaActivarCodigo(int id, bool codigoActivado);
        List<string> VerificarCampos(EmpresaRegistrarModel model);
        EmpresaModel GetCurrentEmpresa();
        List<EmpresaModel> GetEmpresasByTipoInspeccion(TipoInspeccionEnum tipoInspeccion, int idEmpresaUsuarioLogueado);
        EscuelaModel GetEscuelaById(int id);
        EscuelaModel GetEscuelaAnexoById(int id);
        void ActivarCodigoEmpresa(int id);
        EscuelaModificarTipoEmpresaModel ModificarTipoEmpresa(EscuelaModificarTipoEmpresaModel model);
        void VerificarExistenciaInstrumentoLegalByNro(string numeroInstrumentoLegal);
        ResolucionModel RegistrarResolucionVinculadaAEmpresa(ResolucionModel model);
        string SugerirNombreEscuelas(long? domicilioId, int? tipoEscuelaId, int? escuelaRaizId, int? escuelaMadreId, TipoEmpresaEnum? tipoEmpresa, int? numeroEscuelaAnexo);
        string NombreSugeridoParaEscuelas(int idEmpresa);
        List<PedidoAutorizacionCierreModel> GetPedidosAutorizacionCierre(string cue, int? cueAnexo, string codigoEmpresa, int? nroEscuela,
                                                  int? nroPedidoAutorizacion, DateTime? fechaDesde, DateTime? fechaHasta,
                                                  EstadoPedidoCierreEnum? estadoPedido, DateTime? fechaCierreEmpresaDesde, 
                                                  DateTime? fechaCierreEmpresaHasta);

        List<AsignacionInspeccionEscuelaModel> ModificarAsignacionEscuelaAInspeccion(List<AsignacionInspeccionEscuelaModel> model);
        PedidoAutorizacionCierreModel GetPedidoAutorizacionCierreById(int id);
        EmpresaVisadoCierreModel GetPedidoAutorizacionCierreVisadoById(int id);
        HistorialEmpresaModel GetHistorialEmpresaById(int id);
        List<HistorialesEmpresaModel> ProcesarHistorial(int idEmpresa);
        HistorialesEmpresaModel GetHistorialById(int idHistorial);
        void ValidarEmpresaPadre(TipoEmpresaEnum empresaARegistrar, TipoEmpresaEnum empresaPadre);
        List<TipoEducacionEnum> GetTiposEducacionByNivelEducativoId(int idNivelEducativo);
        bool EnviarMail(OpcionEnvioMailEnum opcionEnvio, int idEmpresa, out string mensaje);
        List<TipoEscuelaModel> GetTiposEscuelasPermitidos();
        List<TipoInspeccionIntermediaComboModel> GetInspeccionIntermediaByDireccionDeNivelActual();
        List<EmpresaModel> GetEmpresasDependientes(EmpresaModel model);
        void DomicilioSave(long domicilioId, EmpresaRegistrarModel model);
        ValorParametroModel GetParametroBooleanoJerarquiaInspeccionOrganigrama();
        string GetParametroBooleano(ParametroEnum parametroEnum);
        bool TieneEstructuraDefinitiva(int idEmpresa);
        bool EsEscuela(int idEmpresa);

        string GetCodigoEmpresaById(int id);
        CallesPredioNombresModel GetCallesPredioByEdificioId(int idEdificio);
        List<DiagramacionCursoModel> GetDiagramacionCursoByEscuelaId(int idEscuela);

        FechaYUsuarioCierreEmpresaModel GetUsuarioDeCierre(int empresaId);
        string GetDepartamentoProvincialById(int idDpto);
        string GetTipoMovimientoAsginacionInstrumentoLegal(int idTipoMov);

        List<DtoEscuelaReporte> GetEscuelasByFiltros(string filtroCUE, string filtroCUEAnexo, string filtroCodigoEmpresa, string filtroNombreEmpresa, CategoriaEscuelaEnum? filtroTipoCategoria,
                                                     TipoEducacionEnum? filtroTipoEducacion, int? filtroNivelEducativo, EstadoEmpresaEnum? filtroEstado, int? filtroDepartamento, int? filtroLocalidad,
                                                     string filtroOrdenPago, string filtroProgPresupuestario, DependenciaEnum? filtroDependencia, AmbitoEscuelaEnum? filtroAmbito, List<int> listaZonas, bool filtroPublica, bool filtroPrivada);

        List<DtoEscuelaAnexoReporte> GetEscuelasAnexoByFiltros(string filtroCUE, string filtroCUEAnexo, string filtroCodigoEmpresa, string filtroNombreEmpresa, CategoriaEscuelaEnum? filtroTipoCategoria,
                                                     TipoEducacionEnum? filtroTipoEducacion, int? filtroNivelEducativo, EstadoEmpresaEnum? filtroEstado, int? filtroDepartamento, int? filtroLocalidad,
                                                     string filtroOrdenPago, string filtroProgPresupuestario, DependenciaEnum? filtroDependencia, AmbitoEscuelaEnum? filtroAmbito, List<int> listaZonas, bool filtroPublica, bool filtroPrivada);

        bool EsDireccionNivelSuperior(int idEscuelaLogueado);
    }
}

