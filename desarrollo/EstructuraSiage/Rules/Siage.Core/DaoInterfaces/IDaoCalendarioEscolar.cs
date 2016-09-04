using Siage.Core.Domain;
using System.Collections.Generic;
using System;
using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoCalendarioEscolar: IDao<CalendarioEscolar, int>
    {
        bool? FechaEsHabil(DateTime fecha, int idEscuela);
        bool EstaEnPeriodoInscripcionSuperior();
        List<CalendarioEscolar> EstaEnPeriodoInscripcionInicialPrimarioMedio(int idNivel);
        List<CalendarioEscolar> GetByCicloLectivo(int idCicloLectivo);
        bool ExisteCalendarioEscolar(DateTime FechaInicio, DateTime FechaFin, int? ProcesoId, string Concepto, int id);
        List<DateTime> GetDiasNoHabiles(DateTime fechaDesde, DateTime fechaHasta, int idEscuela);
    }
}
