using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoReincorporacion : IDao<Reincorporacion, int>
    {
        List<Reincorporacion> getReincorporacionByInscripcionId(int? id);
        List<Inscripcion> GetInscripcionesConReincorporacionByFiltros(int? id, SexoEnum? filtroSexo, string filtroTipoDni, long? filtroNroDni,
                               DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, int? filtroAnio, int? filtroTurno,
                               DivisionEnum? filtroDivision, ReincorporacionEnum? filtroReincorporacion);
    }
}

