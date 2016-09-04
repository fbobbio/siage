using System.Collections.Generic;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ICasaHabitacionRules
    {
        List<CasaHabitacionModel> GetCasaHabitacionByFiltros(string filtroNombre, EstadoCasaHabitacionEnum? filtroEstado, string filtroNombreResponsable, string filtroApellidoResponsable, string filtroDniResponsable, bool? filtroDadosDeBaja);
        CasaHabitacionModel GetCasaHabitacionById(int id);
        CasaHabitacionModel CasaHabitacionDelete(CasaHabitacionModel modelo);        
        CasaHabitacionModel CasaHabitacionSaveUpdate(CasaHabitacionModel modelo);
        CasaHabitacionModel ReactivarCasaHabitacion(CasaHabitacionModel modelo);
    }
}

