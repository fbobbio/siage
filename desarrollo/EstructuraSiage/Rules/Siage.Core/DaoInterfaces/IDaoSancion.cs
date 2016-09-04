using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;
using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoSancion : IDao<Sancion, int>
    {
        int GetAmonestacionesByEstudiante(int EstudianteId);
        List<Sancion> GetSancionesByInscripcionId(int id);
        List<Sancion> GetSancionesByEstudiante(int id);
        List<Sancion> GetSancionesByFiltro(int? gradoAñoId,
            DivisionEnum? divisionId,
            int? cicloLectivoId,
            string numeroDocumento,
            SexoEnum? sexo,
            string nombre,
            string apellido,
            int? tipoSancionId, 
            DateTime? fechaDesde, 
            DateTime? fechaHasta);
        bool EstudianteConExpulsion(int id);
    }
}

