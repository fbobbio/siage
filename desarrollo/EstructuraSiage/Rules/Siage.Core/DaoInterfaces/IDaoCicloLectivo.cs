using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoCicloLectivo : IDao<CicloLectivo, int>
    {
       List<CicloLectivo> GetCicloLectivoAllByNivelEducativo(int nivel);
        CicloLectivo GetCicloLectivoVigente();
        CicloLectivo GetCicloLectivoVigenteByNivelEducativo(int nivel);
        List<CicloLectivo> GetCiclosLectivosByFiltros(int? FiltroAnio, int? FiltroPeriodo, int? FiltroNivelEducativo, DateTime? FiltroFechaDesde, DateTime? FiltroFechaHasta, int? FiltroProceso);
        bool ExisteCicloLectivo(int año, int periodo, int id, int nivel);
        bool ExisteCicloLectivo(DateTime fecha, int idNivelEducativo);
    }
}
