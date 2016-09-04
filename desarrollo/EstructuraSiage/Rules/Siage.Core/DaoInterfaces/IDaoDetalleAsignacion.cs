using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    /** 
    * <summary> Interfaz IDaoDetalleAsignacion
    *	
    * </summary>
    * <remarks>
    *		Autor: fbobbio
    *		Fecha: 7/20/2011 10:08:20 AM
    * </remarks>
    */
    public interface IDaoDetalleAsignacion : IDao<DetalleAsignacion, int>
    {
        #region Constantes
        #endregion

        #region Métodos

        DetalleAsignacion GetUltimoDetalleAsignacion(int idAsignacion);
        int GetNumeroDetalleCorrelativo(int idAsignacion);

        #endregion

    }
}
