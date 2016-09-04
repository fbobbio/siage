using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IDireccionDeNivelRules
    {
        DireccionDeNivelModel GetDireccionDeNivelById(int id);
        List<DireccionDeNivelModel> GetAllDireccionDeNivel();
        List<NivelEducativoModel> GetNivelesEducativosByDireccionDeNivel(int id);
        List<DireccionDeNivelModel> GetDireccionesDeNivelByNivelEducativo(int nivelId);
    }
}