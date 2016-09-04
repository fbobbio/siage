using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IHistorialAcademicoRules
    {
        List<HistorialAcademicoModel> GetCalificacionesMesByInscripcionId(int idInscripcion, int idMes);
        List<HistorialAcademicoModel> GetCalificacionesEtapaByInscripcionId(int idInscripcion, int idMes);
    }
}