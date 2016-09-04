using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    /** 
    * <summary> Interfaz IReincorporacionRules
    *	
    * </summary>
    * <remarks>
    *		Autor: Adolfo Alberti
    *		Fecha: 6/30/2011 9:35:04 PM
    * </remarks>
    */
    public interface IReincorporacionRules
    {
        #region Constantes
        #endregion

        #region Métodos
        ReincorporacionRegistrarModel ReincorporacionSave(ReincorporacionRegistrarModel modelo);
        void ReincorporacionDelete(ReincorporacionRegistrarModel modelo);
        ReincorporacionRegistrarModel ReincorporacionUpdate(ReincorporacionRegistrarModel modelo);

        List<ReincorporacionModel> getReincorporacionByInscripcionId(int? id);
        ReincorporacionRegistrarModel getDatosReincorporaciones(int idEstudiante);
        List<InscripcionModel> GetInscripcionConReincorporacionByFiltros(int? id, SexoEnum? filtroSexo, string filtroTipoDNI, long? filtroNroDNI, DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, int? filtroAnio, int? filtroTurno, DivisionEnum? filtroDivision, ReincorporacionEnum? filtroReincorporacion);
        #endregion
    }
}