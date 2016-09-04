using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;


namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface ICargoMinimoUC
    {
        
		[OperationContract] List<CargoMinimo> GetCargoMinimoByFiltros();
		[OperationContract] List<TipoCargo> GetTipoCargoByFiltros();
		[OperationContract]  CargoMinimo CargoMinimoDelete(CargoMinimo entidad);
		[OperationContract]  TipoCargo TipoCargoDelete(TipoCargo entidad);
		[OperationContract] CargoMinimo GetCargoMinimoById(int id);
		[OperationContract] TipoCargo GetTipoCargoById(int id);
		[OperationContract] CargoMinimo CargoMinimoSave(CargoMinimo entidad);
		[OperationContract] TipoCargo TipoCargoSave(TipoCargo entidad);
    }
}

