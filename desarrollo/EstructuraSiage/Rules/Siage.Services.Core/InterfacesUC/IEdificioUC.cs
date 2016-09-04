using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;


namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface IEdificioUC
    {
        
		[OperationContract] List<Edificio> GetEdificioByFiltros();
		[OperationContract]  Edificio EdificioDelete(Edificio entidad);
		[OperationContract] Edificio GetEdificioById(int id);
		[OperationContract] Edificio EdificioSave(Edificio entidad);
    }
}

