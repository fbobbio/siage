using System;
using System.Collections.Generic;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ICargoMinimoRules
    {
        List<CargoMinimoModel> GetCargoMinimoByFiltros(DateTime? fechaDesde, DateTime? fechaHasta,
                                                       CategoriaEscuelaEnum? categoría, int? nivelEducativo,
                                                       int? tipoCargoId, bool incluirNoVigente,
                                                       DateTime? fechaNotificacionDesde, DateTime? fechaNotificacionHasta,
                                                       int? idDireccionDeNivel);
        CargoMinimoModel CargoMinimoDelete(CargoMinimoModel modelo);
        CargoMinimoModel CargoMinimoReactivar(CargoMinimoModel modelo);
        CargoMinimoModel GetCargoMinimoById(int id);
        CargoMinimoModel CargoMinimoSave(CargoMinimoModel modelo);
        List<HistorialCargoMinimoModel> GetHistorialCargoMinimo(int id);
        NivelEducativoModel GetNivelEducativoById(int id);
    }
}

