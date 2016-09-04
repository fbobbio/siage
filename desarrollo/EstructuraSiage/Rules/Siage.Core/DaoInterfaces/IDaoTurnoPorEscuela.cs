using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoTurnoPorEscuela : IDao<TurnoPorEscuela, int>
    {
        TurnoPorEscuela GetByFiltros(int idEscuela, int idTurno);
    }
}
