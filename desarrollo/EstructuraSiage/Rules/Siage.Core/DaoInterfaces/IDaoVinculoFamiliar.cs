using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoVinculoFamiliar : IDao<VinculoFamiliar, int>
    {
        List<VinculoFamiliar> GetByPersona(int idPersona);
    }
}

