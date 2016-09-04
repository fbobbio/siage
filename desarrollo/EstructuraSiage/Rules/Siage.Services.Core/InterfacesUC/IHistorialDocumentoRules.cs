using System.Collections.Generic;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IHistorialDocumentoRules
    {
        List<HistorialDocumentoModel> GetHistorialDocumentosByEstudiante(string dniEstudiante);
        void CargarHistorialDocumentoEstudiante(List<int> lista, int idEscuela, int? idCarrera, int idEstudiante, int idGradoAnio, ProcesoEnum proceso);
    }
}