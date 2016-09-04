using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;

namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface ITipoInspeccionIntermediaUC
    {
        
		[OperationContract] List<TipoInspeccionIntermedia> GetTipoInspeccionIntermediaByFiltros();
		[OperationContract] TipoInspeccionIntermedia TipoInspeccionIntermediaDelete(TipoInspeccionIntermedia entidad);
		[OperationContract] TipoInspeccionIntermedia GetTipoInspeccionIntermediaById(int id);
		[OperationContract] TipoInspeccionIntermedia TipoInspeccionIntermediaSave(TipoInspeccionIntermedia entidad);
    }
}

