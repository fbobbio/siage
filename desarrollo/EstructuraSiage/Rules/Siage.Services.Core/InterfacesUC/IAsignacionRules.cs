using System;
using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IAsignacionRules
    {
        List<AsignacionConsultaModel> GetAsignacionAgenteByFiltros(int? filtroNumeroDocumento, string filtroTipoDocumento,
                                                                   int? filtroSexo, string filtroApellido,
                                                                   string filtroNombre, DateTime filtroFechaDesde,
                                                                   DateTime filtroFechaHasta, int idEmpresa);
        AsignacionModel GetAsignacionAgenteById(int id);
    }
}

