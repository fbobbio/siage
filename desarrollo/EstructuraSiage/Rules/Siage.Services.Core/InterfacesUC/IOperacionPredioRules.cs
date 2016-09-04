using Siage.Services.Core.Models;
using Siage.Core.Domain;

namespace Siage.Services.Core.InterfacesUC
{
    /** 
    * <summary> IOperacionPredioRules IOperacionPredioRules
    *	
    * </summary>
    * <remarks>
    *		Autor: Owner
    *		Fecha: 8/12/2011 1:04:15 PM
    * </remarks>
    */
    public interface IOperacionPredioRules
    {
        OperacionPredioModel GetOperacionPredioById(int id);
        PredioModel GetPredioById(int id);
        OperacionPredioModel OperacionPredio(OperacionPredioModel model);
        void ValidarOperacionPredio(OperacionPredio operacion, Predio predio);
    }
}