using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoDomicilio : IDao<Domicilio, int>
    {
        List<Domicilio> GetByFiltros();
        Domicilio GetById(long id);
        Domicilio GetByEntidad(long id_vin, int? id_entidad);
    }
}

