using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;


namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface ITipoOperacionEdificioUC
    {
        
		[OperationContract] List<TipoOperacionEdificio> GetTipoOperacionEdificioByFiltros();
		[OperationContract] TipoOperacionEdificio TipoOperacionEdificioDelete(TipoOperacionEdificio entidad);
		[OperationContract] TipoOperacionEdificio GetTipoOperacionEdificioById(int id);
		[OperationContract] TipoOperacionEdificio TipoOperacionEdificioSave(TipoOperacionEdificio entidad);
    }
}

