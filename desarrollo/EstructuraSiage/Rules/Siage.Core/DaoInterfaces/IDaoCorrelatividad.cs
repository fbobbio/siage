using Siage.Core.Domain;
using System.Collections.Generic;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoCorrelatividad : IDao<Correlatividad, int>
    {
        List<Correlatividad> GetByFiltros(int idAsignatura, EstadoAsignaturaCorrelativaEnum? idEstado, CondicionCorrelatividadEnum? idCondicion);  
       
    }
}