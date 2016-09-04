using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoHistorialLocal:IDao<HistorialLocal,int>
    {
        /// <summary>
        /// Retorna el último registro del historial del local.
        /// </summary>
        /// <param name="idLocal">Id del local actual</param>
        /// <returns></returns>
        HistorialLocal GetUltimoHistorialLocal(int idLocal);
    }
}
