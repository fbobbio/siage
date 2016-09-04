using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IVinculoFamiliarRules
    {
        VinculoFamiliarModel VinculoFamiliarSave(VinculoFamiliarModel modelo);
        void VinculoFamiliarDelete(VinculoFamiliarModel modelo);
        VinculoFamiliarModel VinculoFamiliarUpdate(VinculoFamiliarModel modelo);

        VinculoFamiliarModel GetVinculoFamiliarById(int id);
        List<VinculoFamiliarModel> GetVinculosByPersonaFisica(int id);
    }
}
