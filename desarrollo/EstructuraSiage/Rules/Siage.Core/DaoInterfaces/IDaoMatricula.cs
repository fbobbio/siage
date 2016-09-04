using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using Siage.Base;



namespace Siage.Core.DaoInterfaces
{
    public interface IDaoMatricula : IDao<Matricula, int>
    {
        List<Matricula> GetMatriculaByFiltros(int? idCarrera, int? nroDni, string idSexo, EstadoMatriculaEnum? estado);
        bool ExisteMatriculaEnCarrera(int alumnoId, int carreraId, int idEscuela, bool soloCarreraParametro);
        bool ExisteAsignacionNumeroDeMatriculaEnEscuela(int numeroMatricula, int escuelaId, int? estudianteId);
        int GetNumeroMatriculaByEstudianteYCarrera(int idEstudiante, int idCarrera);
        bool ExisteLibroFolio(int idEscuela, int idMatricula, string libro, string folio);
        EstadoMatriculaEnum GetEstadoMatriculaById(int id);
    }
}
