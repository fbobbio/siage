using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IAsignacionInstrumentoLegalRules
    {
        AsignacionInstrumentoLegalModel AsignacionInstrumentoLegalSave(AsignacionInstrumentoLegalModel model);
        AsignacionInstrumentoLegalModel GetAsignacionInstrumentoLegalLegalById(int id);
        List<AsignacionInstrumentoLegalConsultaModel> GetAsignacionInstrumentoLegalLegalByPredioId(int id);
        List<AsignacionInstrumentoLegalConsultaModel> GetAsignacionesInstrumentoLegalByIdDiagramacion(int idDiagramacion);
        bool ValidarAsignacion(AsignacionInstrumentoLegalModel model);
    }
}

