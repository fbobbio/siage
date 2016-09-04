using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;


namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface IPredioUC
    {
        
		[OperationContract] List<Predio> GetPredioByFiltros();
		[OperationContract]  Predio PredioDelete(Predio entidad);
		[OperationContract] Predio GetPredioById(int id);
		[OperationContract] Predio PredioSave(Predio entidad);
    }
}

