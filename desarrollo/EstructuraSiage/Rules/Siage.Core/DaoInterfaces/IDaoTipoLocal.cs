using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoTipoLocal : IDao<TipoLocal, int>
    {
        List<TipoLocal> GetByFiltros();

        List<TipoLocal> GetByFiltros(int? id, string filtroNombre, string filtroDescripcion, bool inactivo);

        bool ExisteTipoLocalConNombre(string nombreTipoLocal, int idTipoLocal);

        List<TipoLocal> GetTipoLocalVigentes();
    }
}

