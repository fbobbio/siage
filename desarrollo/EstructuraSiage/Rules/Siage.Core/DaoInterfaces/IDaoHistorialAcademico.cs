using Siage.Core.Domain;
using Siage.Base.Dto;
using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoHistorialAcademico : IDao<HistorialAcademico, int>
    {
        List<DetalleAsignatura> GetMateriasAdeudadasByEstudiante(int idEstudiante);
        List<HistorialAcademico> GetByFiltros(int? escuelaId, int? turnoId, int? gradoAñoId, DivisionEnum? division);
        List<DtoHistorialAcademicoRegistrar> GetHistorialAdeudadasByEstudiante(int idEstudiante);
    }
}

