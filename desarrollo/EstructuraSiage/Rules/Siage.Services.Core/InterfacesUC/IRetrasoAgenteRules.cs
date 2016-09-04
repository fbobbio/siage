using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IRetrasoAgenteRules
    {
        List<RetrasoAgenteConsultaModel> GetRetrasoAgenteByFiltros(int? idRetraso,DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, string filtroApellido,
                                                string filtroNombre, EstadoRetrasoEnum? filtroEstadoRetraso);

        RetrasoAgenteModel RetrasoAgenteSave(RetrasoAgenteModel retrasoAgenteModel);
        RetrasoAgenteModel RetrasoAgenteDelete(RetrasoAgenteModel retrasoAgenteModel);
        RetrasoAgenteModel GetRetrasoAgenteById(int id);

        bool DeterminarSiSeRegistraInasistencia(int idAgente, DateTime fechaRetraso);
    }
}
