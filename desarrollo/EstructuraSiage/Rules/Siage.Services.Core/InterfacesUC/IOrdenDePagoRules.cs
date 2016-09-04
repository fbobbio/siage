using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IOrdenDePagoRules
    {
        OrdenDePagoModel GetOrdenDePagoById(int id);
    }
}
