using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IAsignaturaRules
    {
        List<AsignaturaModel> GetAsignaturasBySubGrupoId(int idSubGrupo);
        List<AsignaturaModel> GetAsignaturaByFiltros(string filtroNombre, string filtroCodigo);
        List<GrupoSubGrupoAsignaturaModel> GetAsignaturasByFiltros(int? filtroGrupoId, int? filtroSubgrupoId, string filtroAsignatura, int? asignaturaId);
        AsignaturaModel AsignaturaDelete(AsignaturaModel model);
        AsignaturaModel AsignaturaSave(AsignaturaModel model);
        AsignaturaModel AsignaturaUpdate(AsignaturaModel model);
        AsignaturaModel GetAsignaturaById(int id);
        List<AsignaturaModel> GetAsignaturasVigentes();
    }
}

