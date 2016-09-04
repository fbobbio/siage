using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    /** 
    * <summary> Interfaz INivelEducativoRules
    *	
    * </summary>
    * <remarks>
    *		Autor: gabriel
    *		Fecha: 6/10/2011 12:15:27 PM
    * </remarks>
    */
    public interface INivelEducativoRules
    {
        List<NivelEducativoModel> GetAllNivelEducativo();
        List<CicloEducativoModel> GetCiclosEducativosByNivelId(int nivelEducativoId);        
        NivelEducativoModel GetNivelEducativoByNivelId(int idNivelEducativo);
        List<NivelEducativoModel> GetNivelesEducativosByEmpresa(int idEmpresa);
    }
}