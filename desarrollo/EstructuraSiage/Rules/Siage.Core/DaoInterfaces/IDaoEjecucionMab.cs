using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    /** 
    * <summary> Interfaz IDaoEjecucionMab
    *	
    * </summary>
    * <remarks>
    *		Autor: Ale
    *		Fecha: 6/30/2011 6:18:39 PM
    * </remarks>
    */

    public interface IDaoEjecucionMab : IDao<EjecucionMab, int>
    {
        

        #region Constantes
        #endregion
            
        #region Métodos

        List<EstadoPuestoDeTrabajoEnum> GetEstadoPuestosPorEjecucionMabId(int ejecucionMabId);

        #endregion
    }
}
