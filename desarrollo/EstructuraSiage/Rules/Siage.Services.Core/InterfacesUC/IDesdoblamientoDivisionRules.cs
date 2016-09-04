using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IDesdoblamientoDivisionRules
    {
        DesdoblamientoModel DesdoblamientoDivisionSave(int origenId, int destinoId, List<int> inscripcionesId);
    }
}
