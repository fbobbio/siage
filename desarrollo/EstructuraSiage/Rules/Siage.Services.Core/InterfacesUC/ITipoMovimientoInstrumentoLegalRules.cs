using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Services.Core.Models;


namespace Siage.Services.Core.InterfacesUC
{
    public interface ITipoMovimientoInstrumentoLegalRules
    {
        TipoMovimientoInstrumentoLegalModel TipoMovimientoInstrumentoLegalDelete(TipoMovimientoInstrumentoLegalModel entidad);
        TipoMovimientoInstrumentoLegalModel GetTipoMovimientoInstrumentoLegalById(int id);
        TipoMovimientoInstrumentoLegalModel TipoMovimientoInstrumentoLegalSave(TipoMovimientoInstrumentoLegalModel entidad);
        TipoMovimientoInstrumentoLegalModel TipoMovimientoInstrumentoLegalReactivate(TipoMovimientoInstrumentoLegalModel entidad);
        List<TipoMovimientoInstrumentoLegalModel> GetTipoMovimientoInstrumentoLegalByFiltros(int? id, string filtroNombre, string filtroDescripcion, bool? dadosBaja);
    }
}

