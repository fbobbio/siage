using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;


namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface ITipoOperacionPredioUC
    {
        
		[OperationContract] List<TipoOperacionPredio> GetTipoOperacionPredioByFiltros();
		[OperationContract] TipoOperacionPredio TipoOperacionPredioDelete(TipoOperacionPredio entidad);
		[OperationContract] TipoOperacionPredio GetTipoOperacionPredioById(int id);
		[OperationContract] TipoOperacionPredio TipoOperacionPredioSave(TipoOperacionPredio entidad);
    }
}

