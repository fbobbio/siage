using System.Collections.Generic;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IUnidadAcademicaRules
    {
        UnidadAcademicaModel GetUnidadAcademicaByFiltros(int idDiagramacion, int idEscuela);

        UnidadAcademicaModel UnidadAcademicaSave(UnidadAcademicaModel model);        
        UnidadAcademicaModel GetUnidadAcademicaById(int id);
        UnidadAcademicaModel GetUnidadAcademicaByIdDiagramacionCurso(int idDiagramacion);        
        DiagramacionCursoModel GetDiagramacionCursoByUnidadAcademica(int diagramacionCursoId);        

        List<UnidadAcademicaModel> GetHorarioEscuelaByFiltros(int? escuelaId, int? carreraId, int? turno, int? gradoAño, DivisionEnum? division);
        List<UnidadAcademicaModel> GetUnidadesAcademicasByDiagramacionCurso(int turno, int gradoAño, DivisionEnum division, int carreraId, int escuelaId);
        List<UnidadAcademicaModel> GetUnidadesAcademicasByConfiguracionAsignaturaEspecial(int? escuelaId, int? turno, int? asignaturaId, int? cargaHoraria, string divisionEspecial);
        List<UnidadAcademicaModel> GetUnidadesAcademicasParaRegistrarConfiguracionAsignaturaEspecial(int? escuelaId);
        //Devuelve las asignaturas simples, se usa para nivel Primario, Medio y Superior
        List<AsignaturaModel> GetAsignaturasHorarioEscuela(int? turno, int? gradoAño, DivisionEnum? division, int? carreraId, int? escuelaId);
        //Devuelve las asignaturas especiales, se usa para nivel Basico y Primario
        List<AsignaturaModel> GetAsignaturasEspecialesHorarioEscuela(int? turno, int? gradoAño, DivisionEnum? division, int? escuelaId);
        //Devuelve las asignaturas especiales, se usa para nivel Medio
        List<AsignaturaModel> GetAsignaturasEspecialesHorarioEscuela(int? escuelaId, int? turno);
        //Devuelve las asignaturas de definicion institucional, se usa para nivel Primario y Medio
        List<AsignaturaModel> GetAsignaturasDefinicionInsitucionalHorarioEscuela(int? turno, int? gradoAño, DivisionEnum? division, int? carreraId, int? escuelaId);
        List<AsignaturaModel> GetAsignaturasByUnidadAcademica(int unidadAcademicaId);
        List<ConfiguracionAsigEspecialModel> GetConfiguracionesAsignaturaEspecialByUnidadAcademicaRegistro(int turnoId, int escuelaId, int unidadAcademicaId);
        List<DetalleHorarioModel> GetDetalleHorarioByUnidadAcademica(int unidadAcademicaId);
        
        void ConfiguracionAsignaturaEspecialSave(int unidadAcademicaId, IEnumerable<ConfiguracionAsigEspecialModel> modelLista);
        void SetHorarioByUnidadAcademica(HorarioModel horario, int escuelaId);                                                      
        void UpdateHorarioEscuelaByUnidadAcademica(HorarioModel horario, int escuelaId);               
    }
}