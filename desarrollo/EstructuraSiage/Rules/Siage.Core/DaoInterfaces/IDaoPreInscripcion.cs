using System.Collections.Generic;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoPreInscripcion : IDao<Preinscripcion, int>
    {
        Preinscripcion GetPreinscripcionByEstudiante(int idEstudiante, int idEscuela, int idGrado);
        List<Preinscripcion> GetPreinscripcionesByFiltrosMatricula(int? idCarrera, int? nroDni, string idSexo, EstadoMatriculaEnum? estado, int idEscuela);
    }
}
