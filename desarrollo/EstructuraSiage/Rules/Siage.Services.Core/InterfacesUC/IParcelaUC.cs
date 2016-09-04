using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;


namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface IParcelaUC
    {
        
		[OperationContract] List<Parcela> GetParcelaByFiltros();
		[OperationContract]  Parcela ParcelaDelete(Parcela entidad);
		[OperationContract] Parcela GetParcelaById(int id);
		[OperationContract] Parcela ParcelaSave(Parcela entidad);
    }
}

