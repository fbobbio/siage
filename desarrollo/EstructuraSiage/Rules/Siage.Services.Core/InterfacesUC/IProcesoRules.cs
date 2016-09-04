using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IProcesoRules
    {
        List<ProcesoModel> GetProcesosByNivelEducativo(int idNivel);
    }
}
