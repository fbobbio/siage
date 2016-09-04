using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoCargoMinimo : IDao<CargoMinimo, int>
    {
        List<CargoMinimo> GetByFiltros(DateTime? fechaDesde, DateTime? fechaHasta, CategoriaEscuelaEnum? categoría,
                                       int? nivelEducativo, int? tipoCargoId, bool incluirNoVigente,
                                       DateTime? fechaNotificacionDesde, DateTime? fechaNotificacionHasta,
                                       int? idDireccionDeNivel);
        List<HistorialCargoMinimo> GetHistorialCargoMinimo(int CargoMinimoId);
        HistorialCargoMinimo GetUltimoHistorial(int CargoMinimoId);
        HistorialCargoMinimo CrearHistorial(CargoMinimo cargoMinimo);
        bool ExisteCargoMinimo(int idNivelEducativo, CategoriaEscuelaEnum categoria,int idTipoCargo, int id);
    }
}

