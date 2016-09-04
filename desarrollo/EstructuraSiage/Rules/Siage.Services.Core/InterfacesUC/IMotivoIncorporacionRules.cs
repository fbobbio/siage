using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IMotivoIncorporacionRules
    {
        MotivoIncorporacionModel GetMotivoIncorporacionById(int id);
        List<MotivoIncorporacionModel> GetAllMotivoIncorporacion();
    }
}
