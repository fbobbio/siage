using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IHorarioPuestoDeTrabajoRules
    {        
        List<PuestoDeTrabajoModel> GetPuestosDeTrabajoByFiltros(string filtroCodigoEmpresa, string filtroNombreEmpresa, string filtroCue, int? filtroCueAnexo, string filtroNombreTipoCargo);
        List<PuestoDeTrabajoModel> GetAll();
        AgenteModel GetDatosAgenteByPuestoDeTrabajo(int idPuestoDeTrabajo);
        PuestoDeTrabajoModel GetPuestoDeTrabajoById(int idPuesto);
        void AsignarHorariosPuestoDeTrabajoAPuesto(List<HorarioPuestoDeTrabajoModel> horarios, int idPuestoTrabajo);
        List<string[]> HorarioByPuestoTrabajoUnidadAcademica(PuestoDeTrabajoModel puesto);        
    }
}