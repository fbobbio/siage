using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ITipoInspeccionIntermediaRules
    {
        List<TipoInspeccionIntermediaModel> GetTipoInspeccionIntermediaByFiltros(int? idTipoDireccionNivel, int? idTipoInspeccionIntermedia, string nombreInspeccionIntermedia, int? idTipoInspeccion, string nombreTipoInspeccion, bool incluirNoVigente);
        List<TipoInspeccionIntermediaModel> GetTipoInspeccionIntermediaAll();
        TipoInspeccionIntermediaModel TipoInspeccionIntermediaDelete(TipoInspeccionIntermediaModel entidad);
        TipoInspeccionIntermediaModel TipoInspeccionIntermediaReactivar(TipoInspeccionIntermediaModel entidad);
        TipoInspeccionIntermediaModel GetTipoInspeccionIntermediaById(int id);
        TipoInspeccionIntermediaModel TipoInspeccionIntermediaSave(TipoInspeccionIntermediaModel entidad);        
    }
}

