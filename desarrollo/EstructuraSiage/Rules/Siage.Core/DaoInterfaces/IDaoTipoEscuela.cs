using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoTipoEscuela : IDao<TipoEscuela, int>
    {
        List<TipoEscuela> GetByFilters(string nombre, string abreviatura, int? idDireccionDeNivel);
    }
}
