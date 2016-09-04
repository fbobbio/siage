using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ITipoAdquisicionRules
    {
        TipoAdquisicionModel GetTipoAdquisicionById(int id);
        TipoAdquisicionModel TipoAdquisicionInsert(TipoAdquisicionModel entidad);
        TipoAdquisicionModel TipoAdquisicionUpdate(TipoAdquisicionModel entidad);
        bool ValidarTipoAdquisicion(TipoAdquisicionModel model);
    }
}
