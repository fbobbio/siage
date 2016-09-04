using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoCondicionEspecialInasistencia : IDao<CondicionEspecialInasistencia, int>
    {

        List<CondicionEspecialInasistencia> GetByInscripcion(int inscripcionId, DateTime? fechaInasistencia);
        List<Inscripcion> GetInscripcionesByFiltros(DateTime? fechaDesde, DateTime? fechaHasta, TipoCondicionInasistenciaEnum? condicion, string nroDocumento, int? sexo, int? turno, int? gradoAnio, DivisionEnum? division, int escuela);
    }
}
