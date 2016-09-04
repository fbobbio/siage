using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ISubOrientacionRules
    {
        List<SubOrientacionModel> GetSubOrientacionesByIdOrientacion(int id, bool dadosDeBaja);
        SubOrientacionModel GetSubOrientacionById(int id);
    }
}
