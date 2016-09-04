using System.Collections.Generic;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IMatriculaRules
    {
        //Un metodo que valide si el alumno ya tiene una matricula para la carrera seleccionada
        bool ExisteMatriculaEnCarrera(int alumnoId, int carreraId, int escuelaId);
        //Un metodo que valida que el estudiante no posea matrículas en otras carreras de esa escuela.
        bool ExisteMatriculaEnCarrerasDeEscuela(int alumnoId, int carreraId, int escuelaId);
        //El sistema verifica que ese número de matrícula no esté asignado a otro estudiante.
        bool ExisteAsignacionNumeroDeMatricula(int numeroMatricula, int escuelaId, int? estudianteId);
        //valida que el estudiante tenga la preinscripcion en estado favorecido  para la carrera seleccionada
        bool ExistePreinscripcionPorCarrera(int alumnoId, int carreraId, int escuelaId);
        //Para registrar la matrícula
        //El sistema registra la matrícula de ese estudiante: carrera, fecha (Ver observaciones), número de matrícula, estado.
        //Si la matrícula es definitiva se registra la fecha del sistema, caso contrario en la fecha se registra campo vacío.
        MatriculaModel MatriculaSave(MatriculaModel matriculaModel);
        MatriculaModel MatriculaUpdate(MatriculaModel matriculaModel);
        MatriculaModel MatriculaDelete(MatriculaModel matriculaModel);
        MatriculaModel GetMatriculaById(int id);
        ValorParametroModel RequierePreinscripcion(int idEscuela);
        int GetNumeroMatriculaByEstudianteYCarrera(int idEstudiante, int idCarrera);
        List<PreinscripcionModel> GetPreinscripcionByFiltros(int? idCarrera, int? nroDni, string idSexo, EstadoMatriculaEnum? estado, int idEscuela);
        EstadoMatriculaEnum GetEstadoMatriculaById(int id);
    }
}