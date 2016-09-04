using System;
using System.Collections.Generic;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    /** 
    * <summary> Interfaz IDaoAccidenteLaboral
    *	
    * </summary>
    * <remarks>
    *		Autor: fbobbio
    *		Fecha: 9/9/2011 1:52:07 PM
    * </remarks>
    */
    public interface IDaoAccidenteLaboral : IDao<AccidenteLaboral,int>
    {
        #region Constantes
        #endregion

        #region Métodos

        List<DtoConsultaAccidenteLaboral> GetAccidenteLaboralByFiltros(DateTime? filtroFechaDesde,
                                                                       DateTime? filtroFechaHasta, string filtroApellido,
                                                                       string filtroNombre,
                                                                       TipoSiniestroEnum? filtroTipoSiniestro,
                                                                       DateTime? filtroFechaSiniestro,
                                                                       DateTime? filtroFechaAnulacion, int? idAgente, int idEmpresaUsuarioLogueado);

        #endregion
    }
}
