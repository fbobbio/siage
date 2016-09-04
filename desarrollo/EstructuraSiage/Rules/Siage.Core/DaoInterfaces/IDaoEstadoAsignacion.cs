using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    /** 
    * <summary> Interfaz IDaoEstadoAsignacion
    *	
    * </summary>
    * <remarks>
    *		Autor: Ale
    *		Fecha: 6/30/2011 8:49:26 PM
    * </remarks>
    */
    public interface IDaoEstadoAsignacion : IDao<EstadoAsignacion, int>
    {
        #region Constantes
        #endregion

      #region Métodos
        EstadoAsignacion GetEstadoAsignacionByEnumaracion(EstadoAsignacionEnum estadoAsignacionEnum);
      EstadoAsignacion GetByFilter(EstadoAsignacionEnum estadoEnum);
        #endregion
    }
}
