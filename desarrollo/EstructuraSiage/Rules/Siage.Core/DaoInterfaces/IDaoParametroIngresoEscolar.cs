using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoParametroIngresoEscolar : IDao<ParametroIngresoEscolar, int>
    {
        List<ParametroIngresoEscolar> ParametroIngresoPorEscuela(int escuelaId, int gradoAnio);
    }
}
