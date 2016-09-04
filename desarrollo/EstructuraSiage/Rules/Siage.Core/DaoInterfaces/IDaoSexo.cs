using Siage.Base;
using Siage.Core.Domain;
using System.Collections.Generic;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoSexo : IDao<Sexo, string>
    {
        List<Sexo> GetByFiltros();
    }
}

