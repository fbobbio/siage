using System.Collections.Generic;
using Siage.Base.Dto;
using Siage.Services.Core.Models;
using Siage.Base;
using System;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IInscripcionRules
    {
        List< DivisionEnum>GetDivisionesEscuelaByGradoAnio(int idGradoAnio,int idEscuela);
        List<InscripcionModel> GetInscripcionByFiltros(int? escuelaId, int? turno, int? gradoAño, DivisionEnum? division);
        InscripcionModel GetInscripcionById(int id);
        InscripcionRegistrarModel InscripcionDelete(InscripcionRegistrarModel modelo);
        InscripcionRegistrarModel InscripcionSave(InscripcionRegistrarModel modelo);
        InscripcionRegistrarModel InscripcionUpdate(InscripcionRegistrarModel modelo);
        List<InscripcionModel> GetInscripcionesByDiagramacionCurso(int diagramacionCursoId, int nivelEducativo);
        List<InscripcionModel> GetInscripcionesByEstudiante(string numeroDocumento);
        List<InscripcionSeleccionModel> GetInscripcionesSeleccionByEstudiante(string numeroDocumento);
        List<CodigoAsignaturaModel> GetAsignaturasByEscuelaGrado(int idEscuela, int idGrado, int idPlan);
        List<DocumentoRequeridoModel> DocumentacionRequeridaByGrado(int idGrado, int idEscuela);
        List<EspecialidadModel> GetEspecialidadByGradoAño(int idGrado, int idEscuela);
        List<DivisionEnum> GetDivisionesEscuelaByGradoAnioTurno(int idGradoAnio, int idTurno, int idEscuela);
        List<GradoAñoModel> GetGradoAñoAnteriores(int idGrado, int idEscuela);
        List<InscripcionConsultaModel> GetByFiltros(int? escuela, int? gradoAnio, string documento, string sexo, int? especialidad, DivisionEnum? division, int? turno, DateTime? desde, DateTime? hasta, int? ciclo);
        List<TurnoModel> GetTurnosByGradoAnio(int idGradoAnio, int idEscuela);
        bool VerificarInscripciones(int idEstudiante, int? idInscripcion);
        List<PlanEstudioConsultaModel> GetPlanesByEscuelaGrado(int idEscuela, int idGrado);
        EstudianteConsultaModel GetEstudianteByInscripcion(int idInscripcion);
        InscripcionRegistrarModel GetInscripcionRegistrarById(int id);
        bool VerificarEscuelaPlan(int idEscuela);
        InscripcionLibroFolioModel LibroFolioSave(InscripcionLibroFolioModel model);
        List<InscripcionLibroFolioConsultaModel> GetInscripcionesByFiltros(int escuelaId, int? turno, int? gradoAño, DivisionEnum? division, int? carrera, int? ciclo);
        InscripcionLibroFolioModel GetInscripcionLibroFolioById(int id);
        List<DetalleAsignaturaModel> GetAsignaturasHabilitadasParaCursarEnCarreraEscuelaByEstudiante(int idCarrera, int idEstudiante, int idEscuela);
        List<InscripcionDivisionesConsultaModel> GetDivisionesPorEscuelaCarreraDetalleAsignatura(int idCarrera, int idEscuela, int idDetalleAsignatura);
        InscripcionSuperiorModel InscripcionSuperiorSave(InscripcionSuperiorModel model);
        InscripcionSuperiorModel InscripcionSuperiorDelete(int idInscripcion);
        List<InscripcionSuperiorConsultaModel> GetInscripcionesEstudianteByEscuelaCarrera(int idCarrera, int idEscuela, int idEstudiante);
        bool EstaEnPeriodoInscripcionSuperior();
        //int EstaEnPeriodoInscripcionInicialPrimarioMedio(int idNivel);
        EstadoInscripcionEnum GetEstadoInscripcionById(int idInscripcion);
        bool TieneHistorial(int idEstudiante, int idEscuela);
        List<HistorialAdeudadasConsultaModel> GetHistorialAdeudadasByEstudiante(int idEstudiante);
        InscripcionLibroFolioConsultaModel GetLibroFolioUltimaInscripcion(int idEstudiante);

        List<InscripcionModel> GetByFiltrosSuperior(int idEscuela, int? gradoAnio, int? turno,
                                                            DivisionEnum? division, int? carrera, int? asignatura);
    }
}