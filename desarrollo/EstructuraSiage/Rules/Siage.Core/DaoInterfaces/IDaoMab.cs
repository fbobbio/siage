using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base.Dto;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    /** 
    * <summary> Interfaz IDaoMab
    *	
    * </summary>
    * <remarks>
    *		Autor: fede.bobbio
    *		Fecha: 6/13/2011 1:34:27 PM
    * </remarks>
    */

    public interface IDaoMab : IDao<Mab, int>
    {
        #region Constantes
        #endregion

        #region Métodos

        int GetUltimoNumeroMab();

        bool TieneAsociadoCodigoMovimiento(int idCodigoMovimiento);
        List<Mab> GetByTipoNovedad(int idTipoNovedad);

        List<Mab> GetByCodigoBarra(string codigo);

        List<Mab> GetByAgente(int idAgente, DateTime fechaInicial, DateTime fechaFinal);

        #endregion

        bool AgenteTieneMabAlta(int agenteId);
        List<DtoGestionAsignacionPorMab> GetByIdEmpresa(int idEmpresa);
    }
}
