using System.Collections.Generic;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    /** 
    * <summary> IGrupoMabRules IGrupoMabRules
    *	
    * </summary>
    * <remarks>
    *		Autor: Owner
    *		Fecha: 6/30/2011 4:32:29 PM
    * </remarks>
    */
    public interface IGrupoMabRules
    {
        List<CodigoMovimientoMabModel> GetCodigosMovimientoByGrupoMabId(int grupoMabId);
        List<EstadoPuestoModel> GetAllEstadosPuesto();
        List<GrupoMabModel> GetGrupoMabByFiltros(TipoGrupoMabEnum? tipoGrupoMabEnum, int? numeroGrupoMab, string codigoMovimientoMab);
        List<EstadoAsignacionModel> GetAllEstadosAsignacion();
        List<EstadoPuestoDeTrabajoEnum> GetEstadoPuestosPorEjecucionMabId(int ejecucionMabId);
        GrupoMabModel GrupoMabSave(GrupoMabModel model);        
        GrupoMabModel GrupoMabReactivar(GrupoMabModel model);
        GrupoMabModel GetGrupoMabById(int id);

        void GrupoMabDelete(GrupoMabModel model);
        int GetCantidadCodigosMovimientoMab(int grupoMabId);              
    }
}