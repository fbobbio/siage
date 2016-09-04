using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoServicioEntidad : IDao<ServicioEntidad, int>
    {
        List<ServicioEntidad> GetByFiltros();

        ServicioEntidad GetByEdificio(int idEdificio);
    }
}

