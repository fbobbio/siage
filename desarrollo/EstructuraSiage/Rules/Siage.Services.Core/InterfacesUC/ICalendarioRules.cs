using System;
using System.Collections.Generic;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ICalendarioRules
    {
        bool ValidarDiaHabil(DateTime fecha, int localidadId);
        CalendarioModel GetCalendarioById(int id);
        CalendarioModel CalendarioSave(CalendarioModel modelo);
        CalendarioModel CalendarioDelete(CalendarioModel modelo);
        List<CalendarioModel> GetByFiltros(int? anio, DateTime? fechaDesde, DateTime? fechaHasta, AmbitoAplicacionEnum? ambito, TipoFechaEnum? esHabil, EstadoCalendarioEnum? estado);
        List<DateTime> DiasHabilesAgente(DateTime fechaDesde, DateTime fechaHasta, int empresaId, int agenteId);
    }
}
