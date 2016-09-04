using Siage.Base.Dto;
using Siage.Core.Domain;
using System.Collections.Generic;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoEstudiante : IDao<Estudiante, int>
    {
        List<Estudiante> GetByFiltros(string filtroSexo, string filtroDni, string filtroNombre, string filtroApellido, int idEscuela, int idNivel);
        List<Estudiante> GetByFiltros(string filtroSexo, string filtroDni, string filtroNombre, string filtroApellido);
        Estudiante GetByPersonaId(int? personaId);
        DtoEstudianteConsulta GetByInscripcion(int idInscripcion);
        bool ExisteEstudiante(int idPersona);
    }
}

