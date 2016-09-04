using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Core.Domain;

namespace Siage.Services.Core.InterfacesUC
{
    [ServiceContract]
    public interface ICasaHabitacionUC
    {
        [OperationContract]
        List<CasaHabitacion> GetCasaHabitacionByFiltros();
        [OperationContract]
        CasaHabitacion CasaHabitacionDelete(CasaHabitacion entidad);     
        [OperationContract]
        CasaHabitacion GetCasaHabitacionById(int id);
        [OperationContract]
        CasaHabitacion CasaHabitacionSave(CasaHabitacion entidad);
    }
}
