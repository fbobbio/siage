using System.Collections.Generic;
using Siage.Base.Dto;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IEntidadesGeneralesRules
    {
        List<MotivoBajaSancionModel> GetMotivoBajaSancionAll();
        List<DetalleAsignaturaModel> GetDetalleAsignaturaAll();
        List<MotivoSancionModel> GetMotivoSancionAll();
        List<AgrupamientoCargoModel> GetAgrupamientoCargoAll();
        List<AsignaturaModel> GetAsignaturaAll();
        List<SeccionModel> GetSeccionAll();
        List<GrupoModel> GetGrupoAll();
        List<SubGrupoModel> GetSubGrupoAll();
        List<EscuelaModel> GetEscuelaAll();
        List<EscuelaModel> GetEscuelasMadres();
        List<InspeccionModel> GetEmpresasInspeccion(int idEmpresaUsuario);
        List<CondicionIvaModel> GetCondicioIvaAll();
        List<GradoAñoModel> GetGradoAñoAll();
        List<TipoCargoModel> GetTipoCargoAll();
        List<CarreraModel> GetCarreraAll();
        List<DireccionDeNivelComboModel> GetDireccionDeNivelAll();
        List<TipoLocalModel> GetTipoLocalAll();
        List<TipoAdquisicionModel> GetTipoAdquisicionAll();
        List<DetalleHoraTurnoModel> GetDetalleHorasTurnoAll();
        List<CicloEducativoModel> GetCicloEducativoAll();
        List<CicloEducativoModel> GetCicloEducativoPorEscuela(int idEscuela);
        List<FuncionEdificioModel> GetFuncionEdificioAll();
        List<PredioModel> GetPredioAll();
        List<CalleModel> GetCalleAll();
        List<TipoEstructuraEdiliciaModel> GetTipoEstructuraEdilicia();
        List<OrdenDePagoModel> GetOrdenDePagoAll();
        List<ProgramaPresupuestarioModel> GetProgramaPresupuestarioAll();
        List<InspeccionModel> GetEmpresaInspeccionAll();
        List<TipoInspeccionIntermediaModel> GetTipoInspeccionIntermediaAll();
        List<TipoInstrumentoLegalModel> GetTipoInstrumentoLegalAll();
        List<SucursalBancariaModel> GetSucursalBancariaAll();
        NivelEducativoModel GetNivelEducativoByEscuela(int idEscuela);
        List<NivelEducativoModel> GetNivelEducativoPorDireccionNivel();
        List<NivelEducativoPorTipoEducacionModel> GetNivelEducativoPorTipoEducacionByDireccionDeNivel();
        List<PaisModel> GetPaisAll();
        PaisModel GetPaisById(string id);
        List<LocalidadModel> GetLocalidadAll();
        List<DepartamentoProvincialModel> GetDepartamentoProvincialByProvincia(string idProvincia);
        List<DepartamentoProvincialModel> GetDepartamentoProvincialAll();
        List<LocalidadModel> GetLocalidadByDepartamentoProvincial(int idDepartamentoProvincial);
        List<LocalidadModel> GetLocalidadByProvincia(string idProvincia);
        List<ProvinciaModel> GetProvinciabyPais(string idPais);
        List<ProvinciaModel> GetProvinciaAll();
        List<CalleModel> GetCalleByFiltro(string nombre, int? idLocalidad, int? idTipoCalle);
        List<BarrioModel> GetBarrioByLocalidad(int idLocalidad);
        List<PlanEstudioModel> GetPlanEstudioAll();
        List<TipoMovimientoInstrumentoLegalModel> GetTipoMovimientoInstrumentoLegalAll();
        List<ZonaDesfavorableModel> GetZonaDesfavorableAll();
        List<NivelEducativoModel> GetNivelEducativoAll();
        List<NivelCargoModel> GetNivelCargoAll();
        List<PeriodoLectivoModel> GetPeriodoLectivoAll();
        List<ObraSocialModel> GetObraSocialAll();
        List<TurnoModel> GetTurnoAll();
        EscuelaModel GetEscuelaById(int id);
        bool GetValorParametroBooleano(ParametroEnum parametro);
        List<TipoEscuelaModel> GetTipoEscuelaAll();
        List<TipoJornadaModel> GetTipoJornadaAll();
        List<ModalidadJornadaModel> GetModalidadJornadaAll();
        List<TipoDomicilioModel> GetTipoDomicilioAll();
        List<TipoCalleModel> GetTipoCalleAll();
        List<TipoDocumentoModel> GetTipoDocumentoAll();
        List<EstadoCivilModel> GetEstadoCivilAll();
        List<SexoModel> GetSexoAll();
        List<OrganismoEmisorDocumentoModel> GetOrganismoEmisorDocumentoAll();
        List<TipoNovedadModel> GetTipoNovedadAll();
        List<SituacionDeRevistaModel> GetSituacionRevistaAll();
        List<SituacionDeRevistaModel> GetSituacionDeRevistaMab();
        List<ModalidadMabModel> GetModalidadMabAll();
        List<CodigoMovimientoMabModel> GetCodigoMovimientoMabAll();
        List<EstadoPuestoModel> GetEstadoPuestoTrabajoAll();
        void MandarMail(string mailRemitente, string passRemitente, string nombreRemitente, string[] mailsDestinatario, string asunto, string cuerpo);
        List<TituloModel> GetTituloAll();
        PersonaFisicaModel GetPersonaFisicaById(int idPersona);
        List<EstatutoModel> GetEstatutoAll();
        List<MotivoBajaAgenteModel> GetMotivoBajaAgenteAll();
        List<CalleConsultaModel> GetCalleConsultaByFiltro(string nombre, int? idLocalidad, int? idTipoCalle);
        List<DocumentoRequeridoModel> GetDocumentosRequeridoPorProceso(ProcesoEnum proceso);
        List<DocumentoRequeridoModel> GetDocumentosRequeridoPorProcesoYCarrera(ProcesoEnum proceso, int idCarrera);
        List<ComboModel> GetTipoCargoEspecialAll();
        List<DocumentoRequeridoModel> GetDocumentosRequeridoPorProcesoMenosPresentadosEstudiante(ProcesoEnum proceso, int idEstudiante, int idEscuela, int? idGradoAnio, int? idCarrera);
        List<DocumentoRequeridoModel> GetDocumentosRequeridoPorProcesoPresentadosEstudiante(ProcesoEnum proceso, int idEstudiante, int idEscuela, int? idGradoAnio, int? idCarrera);
        List<CicloLectivoModel> GetCicloLectivoAll();
        List<TipoSancionModel> GetTipoSancionAll();
        List<TipoVinculoModel> GetTiposVinculoAll();
        List<MotivoIncorporacionModel> GetMotivoIncorporacionAll();

        List<LocalidadConsultaModel> GetLocalidadByProvinciaConsulta(string idProvincia);

        List<LocalidadConsultaModel> GetLocalidadByDepartamentoProvincialConsulta(int idDepartamento);
        List<EtapaNivelModel> GetEtapasNivelByNivelEducativo(int idNivel);

        DtoConsultaEscuela GetEscuelaDtoById(int id);
    }
}