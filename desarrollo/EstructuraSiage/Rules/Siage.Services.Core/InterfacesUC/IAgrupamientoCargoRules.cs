using System.Collections.Generic;
using Siage.Services.Core.Models;
using System;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IAgrupamientoCargoRules
    {
        AgrupamientoCargoModel GetAgrupamientoCargoById(int id);
        void AgrupamientoCargoDelete(AgrupamientoCargoModel modelo);
        AgrupamientoCargoModel AgrupamientoCargoSave(AgrupamientoCargoModel modelo);
        AgrupamientoCargoModel AgrupamientoCargoSaveOrUpdate(AgrupamientoCargoModel model);
        List<AgrupamientoCargoModel> GetAgrupamientoCargoByFilters(string filtroIdentAgrupamientoCargo, string filtroNombreAgrupamientoCargo, DateTime? filtroFechaVigenciaDesde, DateTime? filtroFechaVigenciahasta, int idAgrupamiento);

    }
}
