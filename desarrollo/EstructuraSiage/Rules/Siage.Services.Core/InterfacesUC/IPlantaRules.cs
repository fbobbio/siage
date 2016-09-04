using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Services.Core.Models;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IPlantaRules
    {
        List<PlantaModel> GetPlantaByFiltros(string nombre, int? idTipoLocal, int? numeroLocal);
        PlantaModel PlantaDelete(PlantaModel entidad);
        PlantaModel GetPlantaById(int id);
        PlantaModel PlantaSave(PlantaModel entidad);
    }
}
