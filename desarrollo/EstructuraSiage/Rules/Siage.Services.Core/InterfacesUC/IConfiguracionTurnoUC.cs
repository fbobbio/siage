using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;


namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface IConfiguracionTurnoUC
    {
		[OperationContract] List<ConfiguracionTurno> GetConfiguracionTurnoByFiltros();
		[OperationContract] List<DetalleHoraTurno> GetDetalleHoraTurnoByFiltros();
		[OperationContract]  ConfiguracionTurno ConfiguracionTurnoDelete(ConfiguracionTurno entidad);
		[OperationContract]  DetalleHoraTurno DetalleHoraTurnoDelete(DetalleHoraTurno entidad);
		[OperationContract] ConfiguracionTurno GetConfiguracionTurnoById(int id);
		[OperationContract] DetalleHoraTurno GetDetalleHoraTurnoById(int id);
		[OperationContract] ConfiguracionTurno ConfiguracionTurnoSave(ConfiguracionTurno entidad);
		[OperationContract] DetalleHoraTurno DetalleHoraTurnoSave(DetalleHoraTurno entidad);
    }
}

