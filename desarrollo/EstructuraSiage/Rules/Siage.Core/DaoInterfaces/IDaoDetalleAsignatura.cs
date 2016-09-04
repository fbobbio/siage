using Siage.Core.Domain;
using System.Collections.Generic;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoDetalleAsignatura : IDao<DetalleAsignatura, int>
    {
        DetalleAsignatura GetByIdAsignatura(int idAsignatura);
        DetalleAsignatura GetByFiltros(int idGrado, int idPlan, int idCodigoAsignatura);

        List<DetalleAsignatura> GetAsignaturasEspecialesByEscuelaPlan(int idEscuelaPlan);
        List<DetalleAsignatura> GetAsignaturasByEscuelaPlan(int idEscuelaPlan, int? idGradoAnio);
        List<DetalleAsignatura> GetAll();
        List<DetalleAsignatura> GetDetalleAsignaturaEspecialByIdEscuelaPlan(int filtroIdPlan);
        List<DetalleAsignatura> GetDetalleAsignaturaEspecialByIdPlan(int filtroIdPlan);
        List<DetalleAsignatura> GetByEscuelaPlanGrado(int idDiagramacion, int idEscuela);
        List<DetalleAsignatura> GetDetallesAsignaturasById(List<int> idList);
    }
}

