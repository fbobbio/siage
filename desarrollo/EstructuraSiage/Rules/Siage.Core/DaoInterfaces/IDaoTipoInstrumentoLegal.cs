using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoTipoInstrumentoLegal : IDao<TipoInstrumentoLegal, int>
    {
        List<TipoInstrumentoLegal> GetByFiltros(int? id, string filtroNombre, string filtroCodigo);
    }
}
