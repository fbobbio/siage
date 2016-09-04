using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IGrupoRules
    {
        List<GrupoModel> GetGrupoByCicloUsuario(int idEscuelaLogueado);
        List<GrupoModel> GetGrupoAllVigentes();
        GrupoModel GrupoDelete(GrupoModel model);
        GrupoModel GetGrupoById(int tituloId);
        GrupoModel GrupoSave(GrupoModel model);
        GrupoModel GrupoUpdate(GrupoModel model);
        List<GrupoModel> GetGrupoByFiltros(string nombre, int? ciclo, int idLogueado);
        List<GrupoModel> GetGrupoByIdSubGrupo(int id);
        List<SubGrupoModel> GetSubgrupoByGrupoId(int grupoId);
        List<GrupoModel> GetGruposByCicloEducativo(int idCiclo);
    }
}

