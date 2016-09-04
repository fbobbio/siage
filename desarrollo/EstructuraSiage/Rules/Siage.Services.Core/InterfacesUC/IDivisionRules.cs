using System.Collections.Generic;
using Siage.Base;
using Siage.Services.Core.Models;
namespace Siage.Services.Core.InterfacesUC
{
    public interface IDivisionRules
    {
        List<DivisionEnum> GetDivisionesByEscuelaId(int idEscuela);
    }
}

