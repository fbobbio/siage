using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;


namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface ITipoLocalUC
    {
        
		[OperationContract] List<TipoLocal> GetTipoLocalByFiltros();
		[OperationContract] TipoLocal TipoLocalDelete(TipoLocal entidad);
		[OperationContract] TipoLocal GetTipoLocalById(int id);
		[OperationContract] TipoLocal TipoLocalSave(TipoLocal entidad);
    }
}

