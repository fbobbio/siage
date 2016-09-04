using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IBecaRules
    {
        List<BecaModel> GetBecasByEstudiante(string dniEstudiante);
    }
}