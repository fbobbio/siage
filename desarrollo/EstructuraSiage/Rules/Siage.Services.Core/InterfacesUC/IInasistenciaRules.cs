using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IInasistenciaRules
    {
        List<InasistenciaModel> GetInasistenciasByPersonaId(int? idPersona);
        List<InasistenciaModel> GetInasistenciasByInscripcionId(int id);
    }
}