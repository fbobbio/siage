using System;
using System.Collections.Generic;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IClaustroDocenteRules
    {
        ClaustroDocenteModel ClaustroDocenteSave(ClaustroDocenteModel modelo);
        ClaustroDocenteModel ClaustroDocenteUpdate(ClaustroDocenteModel modelo);
        List<DocentesYAsignaturasAsignadasModel> DocentesInvolucrados(int idDiagramacion);
        List<ClaustroDocenteConsultaModel> GetByFiltros(int idEscuela, int? turno, int? grado, DivisionEnum? division, string dni, string sexo, string nombre, string apellido, MotivoClaustroEnum? motivo, DateTime? fecha);
        ClaustroDocenteModel GetClaustroDocenteById(int id);
        List<int> GetIdDocentesInvolucrados(int idClaustro);
    }
}