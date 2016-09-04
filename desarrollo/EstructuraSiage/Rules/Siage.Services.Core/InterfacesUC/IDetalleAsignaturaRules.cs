using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IDetalleAsignaturaRules
    {
        #region Métodos

        List<DetalleAsignaturaModel> GetDetallesAsignaturasById(List<int> idList);

        #endregion
    }
}
