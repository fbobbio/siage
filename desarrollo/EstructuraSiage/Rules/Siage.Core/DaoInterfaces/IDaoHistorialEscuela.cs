using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoHistorialEscuela : IDao<HistorialEscuela, int>
    {
        List<HistorialEscuela> GetByEscuelaId(int id);
    }
}
