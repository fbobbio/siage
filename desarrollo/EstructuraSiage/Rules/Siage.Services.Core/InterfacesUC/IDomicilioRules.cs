using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IDomicilioRules
    {
        DomicilioModel GetDomicilioById(long id);
        DomicilioModel DomicilioSaveOUpdate(DomicilioModel modelo);
        void DomicilioDelete(DomicilioModel entidad);
        void DomicilioPersonaFisicaSaveOUpdate(DomicilioModel modelo,int id,OrigenEnum origen);
    }
}