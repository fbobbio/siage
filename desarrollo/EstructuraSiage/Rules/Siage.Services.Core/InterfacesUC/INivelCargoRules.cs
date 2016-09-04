using System;
using System.Collections.Generic;
using Siage.Core.Domain;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface INivelCargoRules
    {
        NivelCargoModel GetNivelCargoById(int id);
        NivelCargoModel NivelCargoSave(NivelCargoModel entidad);
        NivelCargoModel NivelCargoSaveOrUpdate(NivelCargoModel model);
        NivelCargoModel NiverCargoReactivar(NivelCargoModel model);
        HistorialNivelCargo HistorialNivelCargoSave(NivelCargo entidad);
        List<NivelCargoModel> GetNivelCargoByFilters(string filtronivelCargo, string filtroNombreNivelCargo, DateTime? filtroFechaVigenciaDesde, DateTime? filtroFechaVigenciahasta);
        List<HistorialNivelCargoModel> GetHistorialNivelCargo(int id);
        List<NivelCargoModel> GetNivelCargoAll();
        void NivelCargoDelete(NivelCargoModel modelo);
    }
}