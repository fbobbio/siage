using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IConfiguracionTurnoRules
    {
        List<ConfiguracionTurnoModel> GetConfiguracionTurnoByFiltros(int? carreraId, int? turno, int idEscuela);
        ConfiguracionTurnoModel ConfiguracionTurnoDelete(ConfiguracionTurnoModel model);      
        ConfiguracionTurnoModel GetConfiguracionTurnoById(int turnoId);       
        ConfiguracionTurnoModel ConfiguracionTurnoSave(ConfiguracionTurnoModel model);        
        ConfiguracionTurnoModel ConfiguracionTurnoUpdate(ConfiguracionTurnoModel model);
        List<DetalleHoraTurnoModel> GetDetalleHoraTurnoByConfiguracionTurnoId(int id);
        List<ConfiguracionTurnoModel> GetConfiguracionTurnoAll();
        ConfiguracionTurnoModel GetConfiguracionTurnoVigente(int? carredaId, int? turno, int? escuelaId);
        DetalleHoraTurnoModel DetalleHoraTurnoSave(DetalleHoraTurnoModel model, int idPadre);
        DetalleHoraTurnoModel DetalleHoraTurnoUpdate(DetalleHoraTurnoModel model, int idPadre);
        List<DetalleHoraTurnoModel> ActualizarHoras(int diferencia, bool agregarHoras, List<DetalleHoraTurnoModel> detalles);
    }
}

