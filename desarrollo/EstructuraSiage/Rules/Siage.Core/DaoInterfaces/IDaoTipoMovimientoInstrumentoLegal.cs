using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoTipoMovimientoInstrumentoLegal : IDao<TipoMovimientoInstrumentoLegal, int>
    {
        List<TipoMovimientoInstrumentoLegal> GetByFiltros(int? id, string filtroNombre, string filtroDescripcion, bool? dadosBaja);

        bool ExisteMovimientoConNombre(string nombreTipoMovimiento, int? id, bool inactivo);

        bool EstaRelacionadoConAlgunaAsignacionInstrumentoLegal(int idTipoMovimientoInstrumentoLegal);

        TipoMovimientoInstrumentoLegal GetByName(string nombreTipoMovimiento);
    }
}
