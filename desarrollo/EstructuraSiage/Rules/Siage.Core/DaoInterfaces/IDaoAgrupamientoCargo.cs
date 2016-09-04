using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoAgrupamientoCargo : IDao<AgrupamientoCargo, int>
    {
        List<AgrupamientoCargo> GetAgrupamientosVigentes();
        AgrupamientoCargo GetAgrupamientoCargoById(int id);
        List<AgrupamientoCargo> GetAgrupamientoCargoByFilters(string filtroIdentAgrupamientoCargo, string filtroNombreAgrupamientoCargo, DateTime? filtroFechaVigenciaDesde, DateTime? filtroFechaVigenciahasta, int idAgrupamiento);
        bool GetAgrupamientoCargoByFilters(int id,string filtroNomAgrupamientoCargo, string filtroIdentAgrupamientoCargo);
    }
}

