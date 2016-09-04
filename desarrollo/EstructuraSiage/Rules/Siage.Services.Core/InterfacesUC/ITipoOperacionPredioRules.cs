using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Services.Core.Models;


namespace Siage.Services.Core.InterfacesUC
{
    public interface ITipoOperacionPredioRules
    {
        
        List<OperacionPredioModel> GetTipoOperacionPredioByFiltros();
        OperacionPredioModel TipoOperacionPredioDelete(OperacionPredioModel entidad);
        OperacionPredioModel GetTipoOperacionPredioById(int id);
        OperacionPredioModel TipoOperacionPredioSave(OperacionPredioModel entidad);
    }
}

