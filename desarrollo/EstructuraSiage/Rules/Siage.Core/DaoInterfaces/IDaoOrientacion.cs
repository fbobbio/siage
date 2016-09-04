using System.Collections.Generic;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoOrientacion : IDao<Orientacion, int>
    {
        List<Orientacion> GetByIdOrientacionNullable(int? idOrientacion);
        List<Orientacion> GetByFiltros(int? idOrientacion, int? idSubOrientacion);
        Orientacion GetByIdEspecialidad(int idEspecialidad);
        List<Orientacion> GetOrientacionActivas();
        bool TieneSuborientacion(int idOrientacion);
        List<Orientacion> GetByNombres(string orientacion, string subOrientacion);
        bool ExisteOrientacion(string nombre, int idOrientacion);
        bool ExisteSubOrientacion(string nombre, int idSubOrientacion);
        List<Orientacion> GetOrientacionesConSuborientaciones();
    }
}

