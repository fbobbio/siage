using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoDiagramacionCurso : IDao<DiagramacionCurso, int>
    {
        List<DiagramacionCurso> GetDiagramacionesImplemetadasEnPlanVigente(int? idCarrera, int idEscuela);
        List<DiagramacionCurso>GetDiagramacionesByEscuelaPlan(int filtroEscuela,int filtroGradoAnio);
        List<DiagramacionCurso>GetDiagramacionesNoImplementadasEnUnidadesAcademicasByFiltros(int idEscuela,int? idGradoAnio,int?idCiclo, DateTime? fechavigencia);
        List<DiagramacionCurso> GetDiagramacionesNoImplementadasEnUnidadesAcademicasByFiltrosAnexo(int idEscuela, int? idGradoAnio, int? idCiclo, DateTime? fechavigencia);
        List<DiagramacionCurso> GetByEscuelaSinSeccion(int escuela);
        List<DiagramacionCurso> GetDiagramacionesByEscuela(int filtroEscuela, int? filtroGradoanio);   
        List<int> GetIdsDiagramacionesByidSeccion(int? idSeccion);
        List<DiagramacionCurso> GetByIdSeccion(int? idSeccion);
        List<DiagramacionCurso> GetByEscuela(int escuelaId);
        List<DiagramacionCurso> GetEstructuraByFiltros(int? filtroTurno, int? filtroGradoAño, bool? filtroPorCierre, DateTime? filtroFechaCierre, int? filtroCarrera, int? filtroEscuela);
        List<DiagramacionCurso> GetEstructuraByFiltros(int? filtroTurno, int? filtroGradoAño);
        List<DiagramacionCurso> GetByFiltros(int? filtroTurno, int? filtroGradoAño, int? filtroCarrera, int? filtroCicloEducativo,int? filtroEscuela);
        List<DiagramacionCurso> GetByFiltros(int? escuelaId, int? turno, int? gradoAño, DivisionEnum? division);
        List<DiagramacionCurso> GetByFiltros(int? carrera, int? turno, int? grado);
        List<DiagramacionCurso> GetByHabilitadasYSinIncripcion(int? escuelaId, int? turno, int? gradoAño, EstadoDiagramacionCursoEnum? estado);
        List<DiagramacionCurso> GetDiagramacionByIdSeccion(int? filtroIdSeccion);
        List<DiagramacionCurso> GetDiagramacionByCarrera(int? idEscuela, int? idCarrera);
        List<DiagramacionCurso> GetByCarreraTurno(int? escuelaId, int? carreraId, int? turnoId);
        List<DiagramacionCurso> GetByCarreraTurnoGrado(int? escuelaId, int? carreraId, int? turnoId, int? gradoAñoId);
        List<DiagramacionCurso> GetDiagramacionByListaId(List<int> idDivision);
        List<DiagramacionCurso> GetByCarreraTurnoGradoRegistro(int? escuelaId, int? carreraId, int? turno, int? gradoAñoId);
        List<DiagramacionCurso> GetByCarreraTurnoGradoEdicion(int? escuelaId, int? carreraId, int? turno, int? gradoAñoId);
        List<DiagramacionCurso> GetByEscuelaYgradoAño(int idEscuela, int idGradoanio);
        DiagramacionCurso GetByEscuelaGradoTurno(int idEscuela, int idGradoanio, DivisionEnum division, int idTurno);
        List<DivisionEnum> GetDivisionesEscuelaByGradoAnioTurno(int? idGradoAnio, int? idTurno, int idEscuela, int? idCarrera);
        List<DiagramacionCurso> GetDiagramacionPerteneceAUnidadAcademicaByEscuelaGradoAño(int idEscuela, int idGrado);
        bool TieneInscriptos(int idDiagramacion);
        List<DiagramacionCurso> GetDiagramacionesNoimPlementadasEnsuperior(int idCarrera, int idEscuela, DateTime? fechaAsignacion);
        bool TieneFechaCierre(int idDiagramacion);
    }
}