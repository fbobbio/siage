using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoValorParametro : IDao<ValorParametro, int>
    {
        bool ValidarValorParametroByFechaVigencia(ParametroEnum parametro, DateTime fechaVigencia);
    }
}
