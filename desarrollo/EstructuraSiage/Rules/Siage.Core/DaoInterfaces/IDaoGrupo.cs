using Siage.Core.Domain;
using System.Collections.Generic;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoGrupo : IDao<Grupo, int>
    {
        bool ExisteEnPlanEstudioVigente(int idGrupo);
        bool ExisteNombreGrupo(string nombre);
        List<Grupo> GetGruposVigentes();
        List<Grupo> GetByIdSubGrupo(int? idSubGrupo);
        List<int> GetIdsGruposByidSubGrupo(int idSubGrupo);
        List<Grupo> GetByFiltros(string nombre, int? ciclo);
        Grupo GetGrupoVigente(string nombre);
        List<SubGrupo> GetSubgrupoByGrupoId(int id);
        Grupo GetByNombre(string nombre);
    }
}