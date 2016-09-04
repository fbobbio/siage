using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;


namespace Siage.Core.DaoInterfaces
{
    /** 
    * <summary> Interfaz IDaoGrupoMab
    *	
    * </summary>
    * <remarks>
    *		Autor: Ale
    *		Fecha: 6/30/2011 5:04:10 PM
    * </remarks>
    */
    public interface IDaoGrupoMab:IDao<GrupoMab, int>
    {
        #region Constantes
        #endregion

        List<GrupoMab> GetGrupoMabByFiltros(TipoGrupoMabEnum? tipoGrupo,int? numeroGrupo,string codigoMovimiento);
        int GetUltimoNumeroCorrelativo();

        #region Métodos

        #endregion
    }
}
