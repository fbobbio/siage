using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ICodigoAsignaturaRules
    {
        CodigoAsignaturaModel GetCodigoAsignaturaById(int id);
        List<CodigoAsignaturaModel> GetCodigoAsignaturaByIdAsignatura(int id);
        List<CodigoAsignaturaModel> GetCodigoAsignaturaBySubGrupo(int? subGrupoId, int? cicloId);
    }
}
