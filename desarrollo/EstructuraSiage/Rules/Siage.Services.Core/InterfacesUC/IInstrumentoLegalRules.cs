using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IInstrumentoLegalRules
    {
        InstrumentoLegalModel InstrumentoLegalDelete(InstrumentoLegalModel entidad);
        InstrumentoLegalModel GetInstrumentoLegalById(int id);
        InstrumentoLegalModel InstrumentoLegalSave(InstrumentoLegalModel entidad);

        List<InstrumentoLegalModel> GetInstrumentoLegalByFiltros(string filtroNumeroInstrumentoLegal);
        InstrumentoLegalModel GetInstrumentoLegalByNumeroDeInstrumento(string filtroNumeroInstrumentoLegal);
        bool ValidarInstrumentoLegal(InstrumentoLegalModel model);
    }
}

