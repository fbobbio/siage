using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Services.Core.Models;


namespace Siage.Services.Core.InterfacesUC
{
    public interface IPruebaTipoAdquisicionRules
    {
        PruebaTipoAdquisicionModel GetPruebaTipoAdquisicionById(int id);
        PruebaTipoAdquisicionModel PruebaTipoAdquisicionSave(PruebaTipoAdquisicionModel entidad);
        List<PruebaTipoAdquisicionModel> GetPruebaTipoAdquisicionByFiltros(int? id, string nombre, string descripcion);
    }
}

