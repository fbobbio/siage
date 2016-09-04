using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoAsignaturaEscuela : IDao<AsignaturaEscuela, int>
    {
        AsignaturaEscuela GetByFiltros(int idEscuela,int idGrado,int idPlan,int idCodigoAsignatura);

        List<AsignaturaEscuela> GetAsignaturasPorFechaDesdeHasta(DateTime fechaDesde, DateTime fechaHasta);
    }
}
