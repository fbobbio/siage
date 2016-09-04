using Siage.Core.Domain;
using System.Collections.Generic;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoConfiguracionTurno : IDao<ConfiguracionTurno, int>
    {
        List<ConfiguracionTurno> GetByFiltros(int? carreraId, int? turno, int idEscuela);
        List<ConfiguracionTurno> GetConfiguracionesTurnoVigentesByEscuela(int escuelaId);
        ConfiguracionTurno GetConfiguracionTurnoVigente(int? carredaId, int? turno, int? escuelaId);
    }
}