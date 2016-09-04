using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;


namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface IContratoUC
    {
        
		[OperationContract] List<Contrato> GetContratoByFiltros();
		[OperationContract]  Contrato ContratoDelete(Contrato entidad);
		[OperationContract] Contrato GetContratoById(int id);
		[OperationContract] Contrato ContratoSave(Contrato entidad);
    }
}

