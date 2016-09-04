using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IUnionEdificioRules
    {
        List<PredioConsultaModel> GetPrediosParaUnionDeEdificios();
        EdificioModel GuardarUnionEdificios(EdificioModel edificioNuevoModel, List<int> listaEdificiosAUnir);


        void GuardarDomicilio(EdificioModel modelo);

        List<EmpresaConsultaEdificioModel> GetEmpresasByIdEdificio(List<int> list);

        List<CalleConsultaModel> GetCallesPredio(int idPredio);
    }
}