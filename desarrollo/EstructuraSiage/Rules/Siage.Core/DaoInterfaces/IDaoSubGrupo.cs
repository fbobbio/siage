using Siage.Base;
using Siage.Core.Domain;
using System.Collections.Generic;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoSubGrupo : IDao<SubGrupo, int>
    {
        bool ValidarGeneracionCodigo(int id);
        bool ExisteEnPlanEstudioVigente(int subgrupoId);
        bool ExisteNombreSubGrupo(string nombre, int idNivel);
        List<SubGrupo> GetSubGruposVigentes();
        List<SubGrupo> GetByFiltros(string nombre, TipoSubGrupoEnum? tipoSubGrupo, List<CicloEducativo> cicloEducativos);
        List<SubGrupo> GetByFiltros(string nombre, TipoSubGrupoEnum? tipoSubGrupo);
        List<SubGrupo> GetByIdGrupo(int id);
        List<SubGrupo> GetSubgruposByCiclo(int cicloId);
        List<SubGrupo> GetSubGrupoAsignaturaByCiclo(int cicloId);
    }
}