using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IUnionPredioRules
    {
        PredioModel GuardarUnionPredio(PredioModel modelo, List<int> ListaPredios);

        List<PredioModel> GetPredioUnionById(List<int> idPredio);

        void GuardarDomicilio(PredioModel modelo);
        void ActualizarEdificios(int idPredioNuevo, List<int> listaPrediosViejos);
    }
}