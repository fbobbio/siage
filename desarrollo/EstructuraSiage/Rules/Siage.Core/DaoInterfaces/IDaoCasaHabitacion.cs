using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoCasaHabitacion : IDao<CasaHabitacion, int>
    {
        List<CasaHabitacion> GetByFiltros(string filtroNombre, EstadoCasaHabitacionEnum? filtroEstado, string filtroNombreResponsable, string filtroApellidoResponsable, string filtroDniResponsable, bool? filtroDadosDeBaja);
  
    }
}

