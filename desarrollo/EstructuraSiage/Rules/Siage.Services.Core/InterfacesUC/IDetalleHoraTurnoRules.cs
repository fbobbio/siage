using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IDetalleHoraTurnoRules
    {
        DetalleHoraTurnoModel GetDetalleHoraTurnoById(int id);
        List<DetalleHoraTurnoModel> GetDetallesHoraTurnoByIdConfiguracionTurno(int id);
    }
}
