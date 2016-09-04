using System;
using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    /** 
    * <summary> Interfaz IMabRules
    *	
    * </summary>
    * <remarks>
    *		Autor: fede.bobbio
    *		Fecha: 6/13/2011 12:10:15 PM
    * </remarks>
    */
    public interface IMabRules
    {
        MabModel GetMabById(int id);
        MabModel MabSave(MabModel model);
        MabModel MabDelete(MabModel model);
        MabModel MabUpdate(MabModel model);
        SucursalBancariaModel GetSucursalBancariaDeAgente(int idAgente);
        SucursalBancariaModel GetSucursalBancariaById(int id);
        AgenteModel GetAgenteReemplazado(int idPuesto);
        AgenteModel GetAgenteById(int idAgente);
        AgenteModel GetAgentePuestoTrabajoSeleccionado(int idPuesto, int idEmpresa);
        EmpresaModel GetEmpresaUsuarioLogueado();
        TipoNovedadModel GetTipoNovedadById(int idTipoNovedad);
        PuestoDeTrabajoModel GetPuestoDeTrabajoById(int idPuesto);
        AsignacionModel GetAsignacionById(int id);
        SituacionDeRevistaModel GetUltimaSituacionRevistaByPuestoDeTrabajoAgenteModel(int idAgente, int idPuesto);

        List<MabModel> GetMabByTipoNovedad(int idTipoNovedad);
        List<MabModel> GetMabByFiltroCodigoBarra(string codigo);
        List<MabModel> GetMabByFiltroAgente(int idAgente, DateTime fechaInicial, DateTime fechaFinal);        
        List<SucursalBancariaModel> GetSucursalesBancarias();               
        List<AsignacionModel> GetAsignacionesByPuestoYAgente(int idAgente, int idPuesto);
        List<CodigoMovimientoMabModel> GetCodigosMovimientoByGrupoMabId(int idTipoNovedad);
        
        bool PuestoDeTrabajoHasPlanDeEstudioRelacionado(int idPuesto);
        bool GetValorParametroFechasMab();

        void GestionarAsignacionPorMab(int idMab);
        List<GestionAsignacionPorMabModel> GetAsignacionesByIdEmpresa(int idEmpresa);
    }
}