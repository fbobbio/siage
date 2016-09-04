using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;


namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface ILocalUC
    {
		[OperationContract] List<Local> GetLocalByFiltros();
		[OperationContract]  Local LocalDelete(Local entidad);
		[OperationContract] Local GetLocalById(int id);
		[OperationContract] Local LocalSave(Local entidad);
    }
}

