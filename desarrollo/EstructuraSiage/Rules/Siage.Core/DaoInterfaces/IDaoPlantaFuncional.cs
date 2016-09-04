using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoPlantaFuncional : IDao<Asignacion, int>
    {
        List<PlantaPresupAsignada> GetPlantaPresupuestariaAsignada(int idEmpresa, DateTime? fechaDesde, DateTime? fechaHasta);
        List<PlantaNoPresupLiquidada> GetPlantaNoPresupuestariaLiquidada(int idEmpresa, DateTime? fechaDesde, DateTime? fechaHasta);
        List<PlantaProvisoriaVinculada> GetPlantaProvisoriaVinculada(int idEmpresa, DateTime? fechaDesde, DateTime? fechaHasta);
        List<PlantaProvisoriaNoVinculada> GetPlantaProvisoriaNoVinculada(int idEmpresa, DateTime? fechaDesde, DateTime? fechaHasta);
        List<PlantaExterna> GetPlantaExterna(int idEmpresa, DateTime fechaDesde, DateTime? fechaHasta);
        List<PlantaFuncionalResumen> GetPlantaFuncionalResumen(int idEmpresa, DateTime fechaDesde, DateTime? fechaHasta);
        List<PlantaFuncionalPuestoAsociado>  GetPlantaFuncionalMovimientosPresupuestaria(int idEmpresa, DateTime fechaDesde, DateTime? fechaHasta);
        List<PlantaFuncionalPuestoAsociado> GetPlantaFuncionalMovimientosVinculada(int idEmpresa, DateTime fechaDesde, DateTime? fechaHasta);
        
    }
}
