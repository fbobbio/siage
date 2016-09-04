using System.Collections.Generic;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoUnidadAcademica : IDao<UnidadAcademica, int>
    {
        List<UnidadAcademica>GetUnidadesEspecialesByEscuelaPlan(int filtroEscuelaPlan);
        List<UnidadAcademica> GetByFiltros(int? carreraId, int? turno, int? grado, DivisionEnum? division,  int? asignaturaId, int? escuelaId);
        List<UnidadAcademica> GetByFiltrosEspecial(int? turno, int? gradoAño, DivisionEnum? division, int? escuelaId);
        List<UnidadAcademica> GetUnidadAcademicaByConfiguracionAsignaturaEspecial(int? escuelaId, int? turno, int? asignaturaId, int? cargaHoraria, string divisionEspecial);
        List<UnidadAcademica> GetUnidadesAcademicasByDiagramacionCurso(int turno, int gradoAño, DivisionEnum division, int carreraId, int escuelaId);
        List<UnidadAcademica> GetUnidadesAcademicasByConsultarHorarioEscuela(int? escuelaId, int? carreraId, int? turnoId, int? gradoAño, DivisionEnum? division);
        List<UnidadAcademica> GetUnidadesAcademicasParaRegistrarConfiguracionAsignaturaEspecial(int? escuelaId);
        List<DetalleHorario> GetDetalleHorario(int unidadAcademicaId);        
        List<Asignatura> GetAsignaturasByUnidadAcademica(int unidadAcademicaId);        
        UnidadAcademica GetUnidadAcademicaParaRegistrarHorarioByAsignaturaId(int turno, int gradoAño, DivisionEnum division, int asignaturaId, int escuelaId);
        UnidadAcademica GetUnidadAcademicaByIdDiagramacionCurso(int diagramacionId);        
        UnidadAcademica ConfiguracionAsignaturaEspecialSave(UnidadAcademica entidad);
        UnidadAcademica GetByFiltros(int idDiagramacion, int idEscuela);
        DiagramacionCurso GetDiagramacionCurso(int unidadAcademicaId);                
        bool ExisteConfiguracionAsignaturaEspecial(int diagramacionCursoId, string sexoId, int asignaturaId);
        void ObtenerAsignaturaYCargaHorarioaByUnidadAcademica(int unidadAcademicaId, out int asignaturaId);        
        List<UnidadAcademica> GetUnidadesAcademicasParaPuestoTrabajo(int idEscuela, MotivoAltaPuestoTrabajoEnum motivo);        
        List<UnidadAcademica> GetAsignaturasDefinicionInsitucionalHorarioEscuela(int? turno, int? gradoAño, DivisionEnum? division, int? carreraId, int? escuelaId);
        List<CodigoAsignatura> GetAsignaturasByEscuelaGradoPlan(int idEscuela, int idGrado,int idPlan);
        UnidadAcademica GetUltimaUnidadAcademicaVigenteByPuestoDeTrabajoId(int puestoDeTrabajoId);
        UnidadAcademica GetUnidadAcademicaVigenteByIdDiagramacionCurso(int idDiagramacionCurso);
        UnidadAcademica GetUnidadesByDiagramacionCurso(int idPlan, int idDiagramacion, int idEscuela);

        List<UnidadAcademica> GetByFiltrosAsignaturaEspecialLoco(int? turno, int? gradoAnio, DivisionEnum? division, int? escuelaId);
    }
}