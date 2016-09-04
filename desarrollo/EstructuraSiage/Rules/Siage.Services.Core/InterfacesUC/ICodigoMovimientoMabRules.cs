using System;
using System.Collections.Generic;
using Siage.Base;
using Siage.Core.Domain;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    /** 
    * <summary> Interfaz ICodigoMovimientoMabRules
    *	
    * </summary>
    * <remarks>
    *		Autor: Ale
    *		Fecha: 6/27/2011 4:15:19 PM
    * </remarks>
    */
    public interface ICodigoMovimientoMabRules
    {
        void DesvincularCodigoMovimientoMABdeGrupoMab(int codigoMovimientoMabModel);
        List<CodigoMovimientoMabModel> GetCodigoMovimientoByTipoGrupoMab(TipoGrupoMabEnum tipoGrupoMab);
        List<CodigoMovimientoMabModel> GetAllCodigosMovimientoMabSinGrupoMabAsignado();
        List<CodigoMovimientoMabModel> GetCodigosMovimientoByGrupoMab(int grupoMabId);
        CodigoMovimientoMabModel CodigoMovimientoMabSave(CodigoMovimientoMabModel codigoMovimientoMabModel);
        List<CodigoMovimientoMabModel> GetByFiltro(string codigo, string descripcionCodigoMovimientoMab);
        List<String> GetAllUsosCodigoMovimientoMab();
        CodigoMovimientoMabModel GenerarModel(CodigoMovimientoMab codigoMovimientoMabs);
        List<CodigoMovimientoMabModel> GenerarModelos(List<CodigoMovimientoMab> codigosMovimientoMabs);
        GrupoMabModel GetGrupoMabById(int grupoMabId);
        bool TieneAsociadoMab(int idCodigoMovimiento);
        CodigoMovimientoMabModel GetCodigoMovimientoMabById(int codigoMovimientoId);
        void EliminarCodigoMovimientoMab(CodigoMovimientoMabModel codigoMovimientoMabModel);
        List<CodigoMovimientoMabModel> GetAllCodigosMovimientoMab();
    }
}
