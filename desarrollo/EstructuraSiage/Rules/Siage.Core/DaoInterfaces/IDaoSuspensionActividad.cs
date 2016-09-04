using System;
using System.Collections.Generic;
using Siage.Base.Dto;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoSuspensionActividad : IDao<SuspensionActividad, int>
    {
        List<DtoAgenteCargo> GetAgentesbyEscuelaHorario(int idEscuela, DateTime fechaDesde, DateTime fechaHasta);
        List<SuspensionActividad> GetByFiltros(int idEscuela, DateTime? fechaDesde, DateTime? fechaHasta, string apellido, string nombre, bool incluirDadosDeBaja);
        List<int> GetAgentesIdsAfectadosByIdSuspencionActividad(int idSuspencionActividad);
        List<SuspensionActividad> GetByIdEscuelaFechaSuspension(int idEscuela, DateTime fecha);
        List<DtoAgenteCargo> GetAgentesByIdSuspension(int idSuspension);
        string ValidarExistenciaDeSuspensionActividad(DateTime fechaActividad, string horaDesde, string horaHasta, int idEscuela);
    }
}
