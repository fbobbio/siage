using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IAsignacionPlanEstudioSuperiorRules
    {
        AsignacionPlanEstudioSuperiorModel AsignacionPlanEstudioSuperiorEditar(AsignacionPlanEstudioSuperiorModel model);
        AsignacionPlanEstudioSuperiorModel AsignacionPlanEstudioSuperiorSave(AsignacionPlanEstudioSuperiorModel model);
        bool ValidarDireccionNivelSuperior(int idEmpresaLogueado);
        AsignacionPlanEstudioSuperiorModel GetAsignacionPlanEstudioSuperiorById(int id);

    }
}