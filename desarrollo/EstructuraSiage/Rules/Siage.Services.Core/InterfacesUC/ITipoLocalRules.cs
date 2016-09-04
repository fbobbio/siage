using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Services.Core.Models;


namespace Siage.Services.Core.InterfacesUC
{
    public interface ITipoLocalRules
    {
        void TipoLocalDelete(TipoLocalModel entidad);
        TipoLocalModel GetTipoLocalById(int id);
        TipoLocalModel TipoLocalSave(TipoLocalModel entidad);
        List<TipoLocalModel> GetTipoLocalByFiltros(int? id, string filtroNombre, string filtroDescripcion);
        TipoLocalModel TipoLocalUpdate(TipoLocalModel modelo);
    }
}

