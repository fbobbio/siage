namespace Siage.Core.DaoInterfaces
{
    public interface IDaoProvider
    {
        #region A

        IDaoAccidenteLaboral GetDaoAccidenteLaboral();
        IDaoAgente GetDaoAgente();
        IDaoAsignatura GetDaoAsignatura();
        IDaoAsignacionInspeccionEscuela GetDaoAsignacionInspeccionEscuela();
        IDaoAtributo GetDaoAtributo();
        IDaoAsignacionInstrumentoLegal GetDaoAsignacionInstrumentoLegal();
        IDaoAsignacion GetDaoAsignacion();
        IDaoAgrupamientoCargo GetDaoAgrupamientoCargo();
        IDaoAsignaturaEscuela GetDaoAsignaturaEscuela();
        IDaoActividadEspecial GetDaoActividadEspecial();
        //  IDaoActividadDefinicionInstitucional GetDaoActividadDefinicionInstitucional();
        IDaoAusenciaCorta GetDaoAusenciaCorta();
        
        #endregion

        #region B

        IDaoBarrio GetDaoBarrio();
        IDaoBeca GetDaoBeca();
        
        #endregion

        #region C
        IDaoClaustroDocente GetDaoClaustroDocente();
        IDaoCorrelatividad GetDaoCorrelatividad();
        IDaoCargoMinimo GetDaoCargoMinimo();
        IDaoCalendario GetDaoCalendario();
        IDaoCalendarioEscolar GetDaoCalendarioEscolar();
        IDaoCicloEducativo GetDaoCicloEducativo();
        IDaoCicloLectivo GetDaoCicloLectivo();
        IDaoCarrera GetDaoCarrera();
        IDaoCodigoAsignatura GetDaoCodigoAsignatura();
        IDaoCasaHabitacion GetDaoCasaHabitacion();
        IDaoConfiguracionAsignaturaEspecial GetDaoConfiguracionAsignaturaEspecial();
        IDaoConfiguracionTurno GetDaoConfiguracionTurno();
        IDaoContrato GetDaoContrato();
        IDaoCalle GetDaoCalle();
        IDaoComunicacion GetDaoComunicacion();
        IDaoCodigoMovimientoMab GetDaoCodigoMovimientoMab();
        IDaoCondicionEspecialInasistencia GetDaoCondicionEspecialInasistencia();
        IDaoCondicionIva GetDaoCondicionIva();
        #endregion

        #region D

        IDaoDetalleAsignatura GetDaoDetalleAsignatura();
        IDaoDetalleSeccion GetDaoDetalleSeccion();
        IDaoDepartamentoProvincial GetDaoDepartamentoProvincial();
        IDaoDeposito GetDaoDeposito();
        IDaoDetalleHoraTurno GetDaoDetalleHoraTurno();
        IDaoDetalleHorario GetDaoDetalleHorario();
        IDaoDetalleInasistencia GetDaoDetalleInasistencia();
        IDaoDiagramacionCurso GetDaoDiagramacionCurso();
        IDaoDireccionDeNivel GetDaoDireccionDeNivel();
        IDaoDomicilio GetDaoDomicilio();
        IDaoDesdoblamiento GetDaoDesdoblamiento();
        IDaoDocumento GetDaoDocumento();
        IDaoDocumentoRequerido GetDaoDocumentoRequerido();
        IDaoDetalleAsignacion GetDaoDetalleAsignacion();
        IDaoDocenteActividadEspecial GetDaoDocenteActividadEspecial();
        IDaoDetalleClaustro GetDaoDetalleClaustro();

        #endregion

        #region E

        IDaoEdificio GetDaoEdificio();
        IDaoEscuelaPlan GetDaoEscuelaPlan();
        IDaoEspecialidad GetDaoEspecialidad();
        IDaoEmpresaBase GetDaoEmpresaBase();
        IDaoEmpresaExterna GetDaoEmpresaExterna();
        IDaoEscuela GetDaoEscuela();
        IDaoEscuelaAnexo GetDaoEscuelaAnexo();
        IDaoEscuelaPrivada GetDaoEscuelaPrivada();
        IDaoEstudiante GetDaoEstudiante();
        IDaoExpediente GetDaoExpediente();
        IDaoEjecucionMab GetDaoEjecucionMab();
        IDaoEstadoCivil GetDaoEstadoCivil();
        IDaoEscuelaPorTipoNivelEducativo GetDaoEscuelaPorTipoNivelEducativo();
        IDaoEstadoPuestoTrabajo GetDaoEstadoPuestoTrabajo();
        IDaoEstadoAsignacion GetDaoEstadoAsignacion();
        IDaoEstatuto GetDaoEstatuto();
        IDaoEtapaNivel GetDaoEtapaNivel();
        #endregion

        #region F

        IDaoFuncionEdificio GetDaoFuncionEdificio();
        
        #endregion

        #region G

        IDaoGradoAño GetDaoGradoAño();
        IDaoGrupo GetDaoGrupo();
        IDaoGrupoMab GetDaoGrupoMab();
        
        #endregion

        #region H
        IDaoHistorialAcademico GetDaoHistorialAcademico();
        IDaoHistorialEdificio GetDaoHistorialEdificio();
        IDaoHistorialDocumento GetDaoHistorialDocumento();
        IDaoHistorialInspeccion GetDaoHistorialInspeccion();
        IDaoHistorialPredio GetDaoHistorialPredio();
        IDaoHistorialLocal GetDaoHistorialLocal();
        IDaoHistorialEmpresa GetDaoHistorialEmpresa();
        IDaoHistorialEscuela GetDaoHistorialEscuela();
        IDaoHistorialDireccionDeNivel GetDaoHistorialDireccionDeNivel();
        IDaoHorarioPuestoDeTrabajo GetDaoHorarioPuestoDeTrabajo();
        
        #endregion

        #region I

        IDaoInasistenciaDocente GetDaoInasistenciaDocente();
        IDaoInscripcion GetDaoInscripcion();
        IDaoInspeccion GetDaoInspeccion();
        IDaoInasistencia GetDaoInasistencia();
        IDaoInstrumentoLegal GetDaoInstrumentoLegal();
        IDaoItemStock GetDaoItemStock();
        
        #endregion

        #region L

        IDaoLocal GetDaoLocal();
        IDaoLocalidad GetDaoLocalidad();
        
        #endregion

        #region M

        IDaoMab GetDaoMab();
        IDaoMotivoBajaSancion GetDaoMotivoBajaSancion();
        IDaoMotivoSancion GetDaoMotivoSancion();
        IDaoModalidadJornada GetDaoModalidadJornada();
        IDaoModalidadMab GetDaoModalidadMab();
        IDaoMotivoBajaAgente GetDaoMotivoBajaAgente();
        IDaoMatricula GetDaoMatricula();
        IDaoMotivoIncorporacion GetDaoMotivoIncorporacion();
        #endregion

        #region N

        IDaoNivelEducativo GetDaoNivelEducativo();
        IDaoNivelEducativoPorTipoEducacion GetDaoNivelEducativoPorTipoEducacion();
        IDaoNivelCargo GetDaoNivelCargo();
        
        #endregion

        #region O

        IDaoOrdenDePago GetDaoOrdenDePago();
        IDaoObraSocial GetDaoObraSocial();
        IDaoOrientacion GetDaoOrientacion();
        IDaoOrientacionSubOrientacionYEspecialidad GetDaoOrientacionSubOrientacionYEspecialidad();
        IDaoOrganismoEmisorDocumento GetDaoOrganismoEmisorDocumento();
        IDaoOperacionEdificio GetDaoTipoOperacionEdificio();
        IDaoOperacionPredio GetDaoTipoOperacionPredio();       

        #endregion

        #region P

        IDaoPais GetDaoPais();
        IDaoParcela GetDaoParcela();
        IDaoPersonaFisica GetDaoPersonaFisica();
        IDaoPersonaJuridica GetDaoPersonaJuridica();
        IDaoPlanEstudio GetDaoPlanEstudio();
        IDaoPlano GetDaoPlano();
        IDaoPredio GetDaoPredio();
        IDaoProvincia GetDaoProvincia();
        IDaoProgramaPresupuestario GetDaoProgramaPresupuestario();
        IDaoPlanta GetDaoPlanta();
        IDaoPeriodoLectivo GetDaoPeriodoLectivo();
        IDaoParametro GetDaoParametro();
        IDaoPuestoDeTrabajo GetDaoPuestoDeTrabajo();
        IDaoPedidoAutorizacionCierre GetDaoPedidoAutorizacionCierre();
        IDaoPlantaFuncional GetDaoPlantaFuncional();
        IDaoProceso GetDaoProceso();
        IDaoParametroIngresoEscolar GetDaoParametroIngresoEscolar();
        IDaoParoAgenteAcatamiento GetDaoParoAgenteAcatamiento();
        IDaoPreInscripcion GetDaoPreInscripcion();
        IDaoPrevisionAusencia GetDaoPrevisionAusencia();

        #endregion

        #region R

        IDaoResolucion GetDaoResolucion();
        IDaoRolPorUsuario GetDaoRolPorUsuario();
        IDaoReincorporacion GetDaoReincorporacion();
        IDaoRetrasoAgente GetDaoRetrasoAgente();
        #endregion

        #region S

        IDaoSancion GetDaoSancion();
        IDaoServicio GetDaoServicio();
        IDaoSeccion GetDaoSeccion();
        IDaoServicioConfigurado GetDaoServicioConfigurado();
        IDaoServicioEntidad GetDaoServicioEntidad();
        IDaoSucursalBancaria GetDaoSucursalBancaria();
        IDaoSubGrupo GetDaoSubGrupo();
        IDaoSubOrientacion GetDaoSubOrientacion();
        IDaoSexo GetDaoSexo();
        IDaoSituacionDeRevista GetDaoSituacionDeRevista();
        IDaoSolicitudCreacionPuestoDeTrabajo GetDaoSolicitudPT();
        IDaoSuspensionActividad GetDaoSuspensionActividad();
        IDaoSistemaCualitativo GetDaoSistemaCualitativo();
        IDaoSistemaCuantitativo GetDaoSistemaCuantitativo();
        #endregion

        #region T
       
        IDaoTitulo GetDaoTitulo();
        IDaoTituloAgente GetDaoTituloAgente();
        IDaoTipoAdquisicion GetDaoTipoAdquisicion();
        IDaoTipoPuesto GetDaoTipoPuesto();
        IDaoTipoNovedad GetDaoTipoNovedad();        
        IDaoTipoEntidad GetDaoTipoEntidad();
        IDaoTipoEstructuraEdilicia GetDaoTipoEstructuraEdilicia();
        IDaoTipoInspeccionIntermedia GetDaoTipoInspeccionIntermedia();
        IDaoTipoLocal GetDaoTipoLocal();
        IDaoTipoInstrumentoLegal GetDaoTipoInstrumentoLegal();
        IDaoTipoJornada GetDaoTipoJornada();
        IDaoTipoMovimientoInstrumentoLegal GetDaoTipoMovimientoInstrumentoLegal();
        IDaoTramiteAdquisicion GetDaoTramiteAdquisicion();
        IDaoTurno GetDaoTurno();
        IDaoTurnoPorEscuela GetDaoTurnoPorEscuela();
        IDaoTipoEscuela GetDaoTipoEscuela();
        IDaoTipoDocumento GetDaoTipoDocumento();
        IDaoTipoDomicilio GetDaoTipoDomicilio();
        IDaoTipoCalle GetDaoTipoCalle();
        IDaoTipoCargo GetDaoTipoCargo();
        IDaoTipoCargoEspecial GetDaoTipoCargoEspecial();
        IDaoTipoSancion GetDaoTipoSancion();
        IDaoTipoAgente GetDaoTipoAgente();
        IDaoTipoNota GetDaoTipoNota();
        IDaoTipoVinculo GetDaoTipoVinculo();

        #endregion

        #region U

        IDaoUnidadAcademica GetDaoUnidadAcademica();
        IDaoUsuario GetDaoUsuario();

        #endregion

        #region V

        IDaoValorParametro GetDaoValorParametro();
        IDaoValorPorAtributo GetDaoValorPorAtributo();
        IDaoVinculoFamiliar GetDaoVinculoFamiliar();
        IDaoVinculoEmpresaEdificio GetDaoVinculoEmpresaEdificio();
        
        #endregion

        #region Z

        IDaoZonaDesfavorable GetDaoZonaDesfavorable();
        
        #endregion

        IDaoDenominacionTipoAgente GetDaoDenominacionTipoAgente();
    }
}