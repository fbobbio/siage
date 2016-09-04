using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ICicloEducativoRules
    {
        List<CicloEducativoModel> GetCicloByEmpresaLogueado(int idEscuelaLogueado);
        List<CicloEducativoModel> GetCiclosByEscuela(int idEscuela);
    }
}