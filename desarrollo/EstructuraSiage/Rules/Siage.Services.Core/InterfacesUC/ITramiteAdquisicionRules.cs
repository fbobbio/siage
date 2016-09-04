using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ITramiteAdquisicionRules
    {
        TramiteAdquisicionModel TramiteAdquisicionSave(TramiteAdquisicionModel model);
        TramiteAdquisicionModel GetTramiteAdquisicionById(int id);
        void ValidarTramiteAdquisicion(TramiteAdquisicionModel model);
    }
}
