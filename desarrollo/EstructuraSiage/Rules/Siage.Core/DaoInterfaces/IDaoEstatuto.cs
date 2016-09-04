using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoEstatuto: IDao<Estatuto, int>
    {
        List<DetalleHorario> GetByFiltros();
    }
}
