using System.Collections.Generic;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoEspecialidad : IDao<Especialidad, int>
    {
        List<Especialidad> GetByFiltros(int? idOrientacion, int? idSubOrientacion, bool dadosDeBaja);
        List<Especialidad> GetEspecialidadByGradoAnio(int idEscuela, int idGrado);
        bool EstaImplementadaEnPlan(int idEspecialidad);
        Especialidad GetEspecialidadByDiagramacionCurso(int idDiagramacion);
    }
}

