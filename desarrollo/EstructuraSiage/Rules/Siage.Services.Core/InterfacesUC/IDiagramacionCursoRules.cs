using System;
using System.Collections.Generic;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IDiagramacionCursoRules
    {
        List<DiagramacionCursoModel>GetDiagramacionesImplemetadasEnPlanVigente(int? idCarrera,int idEscuela);
        List<DiagramacionCursoModel> GetDiagramacionByEscuelaPlan(int filtroEscuela , int filtroGradoAnio );
        List<DiagramacionCursoModel> GetDiagramacionesNoImplementadasEnUnidadesAcademicasByFiltros(int idEscuela,int? idGradoAnio,int? idCiclo, DateTime? fechaInicioVigencia);
        List<DiagramacionCursoModel> GetDiagramacionCursoByEscuela(int filtroEscuela, int? filtrogradoAnio);
        bool ValidarNivel(int idEscuela);
        List<DiagramacionCursoModel> GetEstructuraEscuelaByFiltros(int? turno, int? gradoAño, bool? filtroPorCierre, DateTime? fechaCierre, int? carreraId, int? escuelaId);
        List<DiagramacionCursoModel> GetDiagramacionCursoByFiltros(int? turno, int?  grado, int? carrera , int? nivelEducativo, int? idEscuela);
        List<DiagramacionCursoModel> GetDiagramacionCursoByFiltrosRegistrar(int? carrera, int? turno, int? grado);
        List<DiagramacionCursoModel> GetDiagramacionCursoByFiltros(int? turno, int? grado, DateTime? fechaApertura, DateTime? fechaCierre);
        List<DiagramacionCursoModel> GetDiagramacionTurnoByHabilitadasYSinIncripcion(int? escuelaId, int? turno, int? gradoAño);

        DiagramacionCursoModel DiagramacionCursoDelete(DiagramacionCursoModel entidad);
        DiagramacionCursoModel DiagramacionCursoUpdate(DiagramacionCursoModel entidad);
        DiagramacionCursoModel GetDiagramacionCursoById(int id);
        DiagramacionCursoModel DiagramacionCursoSave(DiagramacionCursoModel entidad);
        DiagramacionCursoModel DiagramacionCursoSave(DiagramacionCursoRegistrarModel entidad);
        DiagramacionCursoModel DiagramacionCursoUpdate(DiagramacionCursoRegistrarModel entidad);
        DiagramacionCursoModel GetDiagramacionCursoByFiltros(int? escuelaId, int? turno, int? gradoAño, DivisionEnum? division);
        DiagramacionCursoModel GetDiagramacionCursoByInscripcion(int inscripcionId);

        List<TurnoModel> GetTurnoByCarrera(int idEscuela, int? idCarrera);
        List<GradoAñoModel> GetGradosByCarreraTurno(int? escuelaId, int? carreraId, int? turno);
        NivelEducativoModel GetNivelByEscuela(int idEscuela);
        bool AsignarParaReserva(int idDiagramacion);        
        bool ValidarExistenciaEstructuraCompleta(int idDiagramacion);
        void RegistrarEstructuraEscuelaDefinitiva(int idEscuela);

        List<GradoAñoModel> GetGradosByCicloEducativo(int cicloEducativoId);
        List<string> ValidarDiagramacionConEstructuraCompleta(int idDiagramacion);        
        List<string> GetDivisionesByCarreraTurnoGrado(int? escuelaId, int? carreraId, int? turno, int? gradoAño);
        List<string> GetDivisionesByCarreraTurnoGradoRegistro(int? escuelaId, int? carreraId, int? turno, int? gradoAño);
        List<string> GetDivisionesByCarreraTurnoGradoEdicion(int? escuelaId, int? carreraId, int? turno, int? gradoAño);
        List<DivisionEnum> GetDivisionesByGradoAnioTurno(int? idGradoAnio, int? idTurno, int idEscuela, int? idCarrera);
        List<DiagramacionCursoModel> GetDiagramacionesByEscuelaPlan(int filtroEscuelaPlan, int filtroCiclo);
        List<DiagramacionCursoModel> GetDiagramacionPerteneceAUnidadAcademicaByEscuelaGradoAño(int idEscuela, int idGrado);
        List<DiagramacionCursoModel> GetDiagramacionesNoimPlementadasEnsuperior(int idCarrera, int idEscuela, DateTime? fechaAsignacion);
        bool TieneFechaCierre(int idDiagramacion);
        bool TieneInscriptos(int idDiagramacion);
    }
}