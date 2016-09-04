using Siage.Base.Dto;
using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoInscripcion : IDao<Inscripcion, int>
    {
        Inscripcion GetByEstudianteId(int EstudianteId, int idEscuela);
        List<Inscripcion> GetByFiltrosSuperior(int idEscuela, int? gradoAnio, int? turno, DivisionEnum? division, int? carrera, int? asignatura);
        List<Inscripcion> GetByFiltros(int? escuelaId, int? turnoId, int? gradoAñoId, DivisionEnum? division, bool conlibre);
        List<Inscripcion> GetByDiagramacionCurso(int diagramacionCursoId, int nivelEducativo);

        /// <summary>
        /// Obtener todas las inscripciones de un estudiante por su numero de documento, si hay varios el mismo documento obtiene el primero
        /// </summary>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        List<Inscripcion> GetByEstudiante(string numeroDocumento);
        bool ValidarCupo(int diag);
        List<DtoInscripcionConsulta> GetByFiltros(int? escuela, int? gradoAnio, string documento, string sexo, int? especialidad, DivisionEnum? division,
            int? turno, DateTime? desde, DateTime? hasta, int? ciclo);
        bool VerificarInscripciones(int idEstudiante, int idEscuela);
        Inscripcion GetUltimaInscripcionByEstudianteEscuela(int idEstudiante, int idEscuela);
        List<DtoInscripcionLibroFolioConsulta> GetByFiltros(int escuelaId, int? turnoId, int? gradoAñoId, DivisionEnum? division, int? ciclo);
        List<DtoInscripcionLibroFolioConsulta> GetByFiltros(int escuelaId, int? turnoId, int? gradoAñoId, DivisionEnum? division, int? carreraId, int? ciclo);
        bool ExisteLibroFolio(int idEscuela, int idEstudiante, string libro, string folio);
        List<DetalleAsignatura> GetAsignaturasHabilitadasParaCursarEnCarreraEscuelaByEstudiante(int idCarrera, int idEstudiante, int idEscuela);
        List<DtoInscripcionDivisionesConsulta> GetDivisionesPorEscuelaCarreraDetalleAsignatura(int idCarrera, int idEscuela, int idDetalleAsignatura);
        Inscripcion GetByUnidadAcademicaEstudiante(int idUnidad, int idEstudiante);
        List<DtoInscripcionSuperiorConsulta> GetInscripcionesEstudianteByEscuelaCarrera(int idCarrera, int idEscuela, int idEstudiante);
        bool ValidarCupoCompletoDivision(int idDiagramacion);
        bool HayHorariosInscripcionSuperpuestos(int idEstudiante, int idEscuela, int idUnidadAcademica);
        EstadoInscripcionEnum GetEstadoInscripcionById(int idInscripcion);
        bool TieneInscripcionesParaCicloLectivoActual(int idEstudiante, int? idInscripcion);
        bool TieneInscripcionParaEseGradoAnio(int idEstudiante, int idGradoAnio);
        bool TieneInscripcionCursadoNormalParaCicloLectivoActual(int idEstudiante);
        DtoInscripcionLibroFolioConsulta GetLibroFolioUltimaInscripcion(int idEstudiante);
        DateTime GetFechaInscripcionById(int idInscripcion);
    }
}