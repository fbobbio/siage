using System;
using System.Collections.Generic;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    /** 
    * <summary> Interfaz IDaoRetrasoAgente
    *	
    * </summary>
    * <remarks>
    *		Autor: fbobbio
    *		Fecha: 9/9/2011 1:55:34 PM
    * </remarks>
    */
    public interface IDaoRetrasoAgente : IDao<RetrasoAgente, int>
    {
        #region Constantes
        #endregion

        #region Métodos
        #endregion

        int GetAcumulacionRetrasos(int idAgente, DateTime fechaRetraso);

        RetrasoAgente GetUltimoRetrasoByAgente(int idAgente);

        List<DtoRetrasoAgente> GetByFiltros(int? filtroIdRetraso,DateTime? filtroFechaDesde, DateTime? filtroFechaHasta,
                                            string filtroApellido, string filtroNombre,
                                            EstadoRetrasoEnum? filtroEstadoRetraso);

        bool ExisteRetraso(int idAgente, int idPuesto, DateTime fechaRetraso);
        List<RetrasoAgente> ExisteRetraso(int idAgente, int idPuesto, DateTime fechaDesde, DateTime fechaHasta);
    }
}
