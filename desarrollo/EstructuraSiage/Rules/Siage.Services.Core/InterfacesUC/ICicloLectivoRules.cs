using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ICicloLectivoRules
    {
        List<CicloLectivoModel> GetCicloLectivoAllByNivelEducativo(int idEmpresa);
        CicloLectivoModel GetCicloLectivoVigente();
        CicloLectivoModel CicloLectivoSave(CicloLectivoModel modelo);
        List<CalendarioEscolarModel> GetCalendariosEscolaresByCicloLectivoId(int id);
        List<CicloLectivoModel> GetCiclosLectivosByFiltros(int? FiltroAnio, int? FiltroPeriodo, int? FiltroNivelEducativo, DateTime? FiltroFechaDesde, DateTime? FiltroFechaHasta, int? FiltroProceso);
        CicloLectivoModel GetCicloLectivoById(int id);
        List<CicloLectivoModel> GetCicloLectivoByNivelEducativo(int idNivel);
        void CicloLectivoDelete(CicloLectivoModel modelo);
        CicloLectivoModel GetCicloLectivoVigenteByNivelEducativo(int idNivel);
    }
}
