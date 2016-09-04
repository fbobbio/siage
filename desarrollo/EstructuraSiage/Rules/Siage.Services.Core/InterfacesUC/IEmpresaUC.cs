using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;


namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface IEmpresaUC
    {
        
		[OperationContract] List<Empresa> GetEmpresaByFiltros();
		[OperationContract]  Empresa EmpresaDelete(Empresa entidad);
		[OperationContract] Empresa GetEmpresaById(int id);
		[OperationContract] Empresa EmpresaSave(Empresa entidad);
    }
}

