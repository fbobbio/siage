using Siage.Core.Domain;
using System.Collections.Generic;
using System;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoCalendario : IDao<Calendario, int>
    {
        bool ExisteCalendario(DateTime fecha, AmbitoAplicacionEnum ambito, int? localidad, int id);
        List<Calendario> GetByFiltros(int? anio, DateTime? fechaDesde, DateTime? fechaHasta, AmbitoAplicacionEnum? ambito, bool? esHabil, EstadoCalendarioEnum? estado);
        bool GetByFeriado(DateTime fecha,int localidad);
        bool FechaEsHabil(DateTime fecha, int idEscuela);
        List<Calendario> GetFeriados(DateTime fechaDesde, DateTime fechaHasta, int idEscuela);
        bool ExisteConceptoCalendario(DateTime fecha,string Concepto);
    }
}