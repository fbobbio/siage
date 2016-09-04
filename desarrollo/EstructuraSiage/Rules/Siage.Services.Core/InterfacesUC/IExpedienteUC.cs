using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;


namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface IExpedienteUC
    {
        
		[OperationContract] List<Expediente> GetExpedienteByFiltros();
		[OperationContract]  Expediente ExpedienteDelete(Expediente entidad);
		[OperationContract] Expediente GetExpedienteById(int id);
		[OperationContract] Expediente ExpedienteSave(Expediente entidad);
    }
}

