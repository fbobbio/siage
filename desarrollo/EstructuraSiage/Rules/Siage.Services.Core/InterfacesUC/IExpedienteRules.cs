using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IExpedienteRules
    {
        ExpedienteModel GetExpedienteByFiltros(int? idInstrumentoLegal, string numeroExpediente);
        ExpedienteModel ExpedienteDelete(ExpedienteModel entidad);
        ExpedienteModel GetExpedienteById(int id);
        ExpedienteModel ExpedienteSave(ExpedienteModel entidad);
        ExpedienteModel GetExpedienteByNumero(string numeroExpediente);
        bool ValidarExpediente(ExpedienteModel model);
    }
}

