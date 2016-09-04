using Siage.Base.Dto;
using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoClaustroDocente : IDao<ClaustroDocente, int>
    {
        bool ValidarExistenciaDeClaustroEnSancion(int idSancion);
        List<DtoDocentesYAsignaturasAsignadas> DocentesInvolucrados(int idDiagramacion);
        bool ExisteClaustroParaEstudianteEnFecha(int inscripcionId, DateTime fecha, int? idClaustro);
        List<DtoClaustroDocenteConsulta> GetByFiltros(int idEscuela, int? turno, int? grado, DivisionEnum? division, string dni, string sexo, string nombre, string apellido, MotivoClaustroEnum? motivo, DateTime? fecha);
        List<int> GetIdDocentesInvolucrados(int idClaustro);
    }
}

