using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoCalle : IDao<Calle, int>
    {
        Calle GetByFiltros(int idCalle, int? idLocalidad);
        List<Calle> GetByFiltros(string nombre, int? idLocalidad, int? idTipoCalle);
    }
}
