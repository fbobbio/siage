using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IDetalleSeccionRules
    {
        DetalleSeccionModel GetDetalleSeccionById(int id);
        List<DetalleSeccionModel> GetDetalleSeccionByIdSeccion(int id);
    }
}
