using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoMotivo : IDao<Motivo, int>
    {
        List<Motivo> GetByFiltros(int? id, string descripcion);
        Motivo GetByDescripcion(string descripcion);
    }
}

