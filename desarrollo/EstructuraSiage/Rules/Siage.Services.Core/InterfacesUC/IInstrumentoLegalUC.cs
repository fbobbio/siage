using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;


namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface IInstrumentoLegalUC
    {
        
		[OperationContract] List<InstrumentoLegal> GetInstrumentoLegalByFiltros();
		[OperationContract]  InstrumentoLegal InstrumentoLegalDelete(InstrumentoLegal entidad);
		[OperationContract] InstrumentoLegal GetInstrumentoLegalById(int id);
		[OperationContract] InstrumentoLegal InstrumentoLegalSave(InstrumentoLegal entidad);
    }
}

