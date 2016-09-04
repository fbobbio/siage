using System.Collections.Generic;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ICorrelatividadRules
    {
        List<CorrelatividadModel> GetCorrelatividadByFiltros(int idAsignatura, EstadoAsignaturaCorrelativaEnum? idEstado, CondicionCorrelatividadEnum? idCondicion);
        List<CorrelatividadModel> CrearCorrelatividad(int idAsignatura, int idAsignaturaCorrelativa, EstadoAsignaturaCorrelativaEnum idEstado, CondicionCorrelatividadEnum idCondicion);
    }
}
