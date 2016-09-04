using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Base;
using Siage.Services.Core.Models;


namespace Siage.Services.Core.InterfacesUC
{
    public interface IPlantaFuncionalRules
    {
        List<PlantaPresupAsignadaModel> GetPlantaPresupuestariaAsignada(int empresaId, DateTime? fechaDesde, DateTime? fechaHasta);
        List<PlantaNoPresupLiquidadaModel> GetPlantaNoPresupuestariaLiquidada(int empresaId, DateTime? fechaDesde, DateTime? fechaHasta);
        List<PlantaProvisoriaVinculadaModel> GetPlantaProvisoriaVinculada(int empresaId, DateTime? fechaDesde, DateTime? fechaHasta);
        List<PlantaProvisoriaNoVinculadaModel> GetPlantaProvisoriaNoVinculada(int empresaId, DateTime? fechaDesde, DateTime? fechaHasta);
        List<PlantaExternaModel> GetPlantaExterna(int empresaId, DateTime fechaDesde, DateTime? fechaHasta);

        List<EmpresaConsultarModel> GetEmpresaByFiltros(int idEmpresa, string codigoEmpresa, string nombreEmpresa);
        List<TituloAgenteModel> GetTitulosByAgenteId(int idAgente);

        List<PlantaFuncionalResumenModel> GetResumenActividadPlantaFuncional(int idEmpresa, DateTime fechaDesde, DateTime? fechaHasta);
        List<PlantaFuncionalMovimientosModel> GetMovimientosPlantaFuncionalProvisoria(int idEmpresa, DateTime fechaDesde, DateTime? fechaHasta);
        List<PlantaFuncionalMovimientosModel> GetMovimientosPlantaFuncionalPresupuestariaLiquidada(int idEmpresa, DateTime fechaDesde, DateTime? fechaHasta);
        
    }
}
