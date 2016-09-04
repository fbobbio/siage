using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;
using Siage.Base.Dto;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoInasistenciaDocente : IDao<InasistenciaDocente, int>
    {
        List<DtoConsultaInasistenciaDocente> GetInasistenciaDocenteByFiltros(int? idInasistencia,DateTime? filtroFechaDesde,
                                                                             DateTime? filtroFechaHasta,
                                                                             string filtroLegajoAgente,
                                                                             bool? filtroAusenciaAnticipada,
                                                                             EstadoInasistenciaDocenteEnum?
                                                                                 filtroEstadoInasistencia,
                                                                             TipoMotivoInasistenciaDocenteEnum?
                                                                                 filtroTipoMotivoInasistencia);

        List<InasistenciaDocente> ExisteInasistencia(DateTime fechaDesde, DateTime fechaHasta, int agenteId, int puestoId);



        List<DtoPersonalPorTurno> GetPorcentajeAcatamientoPorTurno(List<int> idAsignaciones);
    }
}
