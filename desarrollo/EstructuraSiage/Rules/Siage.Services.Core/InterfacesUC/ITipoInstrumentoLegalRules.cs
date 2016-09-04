using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ITipoInstrumentoLegalRules
    {
        TipoInstrumentoLegalModel TipoInstrumentoLegalDelete(TipoInstrumentoLegalModel entidad);
        TipoInstrumentoLegalModel GetTipoInstrumentoLegalById(int id);
        TipoInstrumentoLegalModel TipoInstrumentoLegalSave(TipoInstrumentoLegalModel entidad);
        List<TipoInstrumentoLegalModel> GetTipoInstrumentoLegalByFiltros(int? id, string filtroNombre, string filtroCodigo);
        List<TipoInstrumentoLegalModel> GetAll();
    }
}