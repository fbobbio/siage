using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IDivisionEdificioRules
    {
        List<PredioConsultaModel> GetPrediosParaDivisionDeEdificios();        
        List<EdificioConsultarModel> GetEdificiosEnPredio(int idPredio);
        PredioModel GetPredioById(int idPredio);
        EdificioModel GetEdificioById(int idSeleccionado);
    }
}