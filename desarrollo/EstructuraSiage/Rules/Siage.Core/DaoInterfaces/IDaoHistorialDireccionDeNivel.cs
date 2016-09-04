using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoHistorialDireccionDeNivel : IDao<HistorialDireccionDeNivel, int>
    {
        List<HistorialDireccionDeNivel> GetByEmpresaId(int id);
    }
}
