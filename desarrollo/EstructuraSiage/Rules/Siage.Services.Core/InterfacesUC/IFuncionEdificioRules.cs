using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IFuncionEdificioRules
    {       
        FuncionEdificioModel FuncionEdificioDelete(FuncionEdificioModel modelo);
        FuncionEdificioModel GetFuncionEdificioById(int id);
        FuncionEdificioModel FuncionEdificioSave(FuncionEdificioModel modelo);
        FuncionEdificioModel FuncionEdificioEditar(FuncionEdificioModel model);
        List<FuncionEdificioModel> GetFuncionEdificioByFiltros(int? id, string nombre, string descripcion);
    }
}
