using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Services.Core.Models;


namespace Siage.Services.Core.InterfacesUC
{
    public interface IOperacionEdificioRules
    {

        List<OperacionEdificioModel> GetTipoOperacionEdificioByFiltros();
        OperacionEdificioModel TipoOperacionEdificioDelete(OperacionEdificioModel entidad);
        OperacionEdificioModel GetTipoOperacionEdificioById(int id);
        OperacionEdificioModel TipoOperacionEdificioSave(OperacionEdificioModel entidad);
    }
}

