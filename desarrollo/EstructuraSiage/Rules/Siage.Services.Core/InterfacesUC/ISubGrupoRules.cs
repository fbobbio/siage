using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ISubGrupoRules
    {
        bool ValidarRelacionPlanEstudio(int idSubgrupo);
        bool ValidarGeneracionCodigo(int id);
        GrupoModel GetGrupoById(int id);
        List<SubGrupoModel> GetSubGrupoAllVigentes();
        List<GrupoModel> GetGruposByIdSubGrupo(int id);
        List<int> GetIdsGruposByIdSubGrupo(int idSubGrupo);
        List<SubGrupoModel> GetSubGrupoByFiltros(string nombre, Siage.Base.TipoSubGrupoEnum? tipoSubGrupo, int? idEscuelaLogueado);
        SubGrupoModel GetSubGrupoById(int id);
        List<SubGrupoModel> GetSubGrupoByIdGrupo(int id);
        SubGrupoModel SubGrupoDelete(SubGrupoModel model);
        SubGrupoModel SubGrupoSave(SubGrupoModel model);
        SubGrupoModel SubGrupoUpdate(SubGrupoModel model);
        List<SubGrupoModel> GetSubGrupoByCiclo(int cicloId);
        List<SubGrupoModel> GetSubGrupoAsignaturaByCiclo(int cicloId);
    }
}
