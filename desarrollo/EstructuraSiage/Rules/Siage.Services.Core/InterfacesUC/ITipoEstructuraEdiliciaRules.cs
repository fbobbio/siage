using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ITipoEstructuraEdiliciaRules
    {
        List<TipoEstructuraEdiliciaModel> GetTipoEstructuraEdiliciaByFiltros(int? idTipoDireccionNivel, string nombre, string descripcion, TipoEdificioEnum tipoEdificio);
        TipoEstructuraEdiliciaModel TipoEstructuraEdiliciaDelete(TipoEstructuraEdiliciaModel entidad);
        TipoEstructuraEdiliciaModel GetTipoEstructuraEdiliciaById(int id);
        TipoEstructuraEdiliciaModel TipoEstructuraEdiliciaSave(TipoEstructuraEdiliciaModel entidad);

        TipoEstructuraEdiliciaModel TipoEstructuraEdiliciaEditar(TipoEstructuraEdiliciaModel model);
        List<TipoEstructuraEdiliciaModel> GetTipoEstructuraEdiliciaByFiltros(int? idTipoDireccionNivel, string nombre, string descripcion, TipoEdificioEnum? tipoEdificio);
    }
}
