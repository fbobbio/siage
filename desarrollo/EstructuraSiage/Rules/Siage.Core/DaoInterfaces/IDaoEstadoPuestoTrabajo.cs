using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoEstadoPuestoTrabajo : IDao<EstadoPuesto, int>
    {
        EstadoPuesto GetByFilter(EstadoPuestoDeTrabajoEnum estadoEnum);
        IList<EstadoPuesto> GetByFilter(IList<EstadoPuestoDeTrabajoEnum> estadoPuestoDeTrabajoEnumList);
    }
}
