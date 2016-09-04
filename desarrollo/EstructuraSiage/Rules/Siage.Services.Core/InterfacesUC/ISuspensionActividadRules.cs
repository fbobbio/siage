using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Base.Dto;
using Siage.Services.Core.Models;
using Siage.Core.Domain;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ISuspensionActividadRules
    {
        SuspensionActividadModel SuspensionActividadDelete(SuspensionActividadModel modelo);
        SuspensionActividadModel SuspensionActividadUpdate(SuspensionActividadModel modelo);
        SuspensionActividadModel SuspensionActividadSave(SuspensionActividadModel modelo);
        List<SuspensionActividadModel> GetByFiltros(int idEscuela, DateTime? fechaDesde, DateTime? fechaHasta, string apellido, string nonbre, bool incluirDadosDeBaja);
        SuspensionActividadModel GetSuspensionActividadById(int id);
        List<DtoAgenteCargo> GetAgentesbyEscuelaHorario(int idEscuela, DateTime fechaDesde, DateTime fechaHasta);
        List<int> GetAgentesByIdSuspensionActividad(int idSuspencionActividad);
        List<DtoAgenteCargo> GetAgentesByIdSuspension(int idSuspension);

    }
}

