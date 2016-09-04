using System.Collections.Generic;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoTitulo : IDao<Titulo, int>
    {
        List<Titulo> ValidarNombre(string nombre, int? nivel);
        List<Titulo> GetByFiltros(string nombre, int? nivel, bool dadosDeBaja);
        bool ExisteEnPlanEstudio(int idTitulo);
    }
}

