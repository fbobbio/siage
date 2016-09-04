using Siage.Core.Domain;
using System.Collections.Generic;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoAsignatura : IDao<Asignatura, int>
    {
        List<Asignatura> GetAllVigentes();
        bool ExisteEnPlanEstudioVigente(int asignaturaId);
        bool ExisteNombreAsignatura(string nombre);
        Asignatura GetByCodigoAsignatura(string codigo);
        List<Asignatura> GetBySubGrupo(int subGrupoId);
        List<Asignatura> GetByFiltros(string filtroNombre, string filtroCodigo);
    }
}