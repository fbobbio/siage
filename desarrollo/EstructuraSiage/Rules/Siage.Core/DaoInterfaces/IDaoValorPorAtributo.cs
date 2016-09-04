using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoValorPorAtributo : IDao<ValorPorAtributo, int>
    {
        List<ValorPorAtributo> GetByFiltros();
    }
}

