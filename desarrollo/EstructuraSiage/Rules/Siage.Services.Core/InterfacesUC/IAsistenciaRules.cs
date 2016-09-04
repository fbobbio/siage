using System;
using System.Collections.Generic;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IAsistenciaRules
    {
         List<InasistenciaRegistrarModel> GetAsistenciasByFiltros(int idEscuela, DateTime Fecha, int Turno, int Grado, DivisionEnum Division, int? Asignatura);
         List<InasistenciaRegistrarModel> AsistenciaSave(List<InasistenciaRegistrarModel> modelo,int idNivel);
         List<CodigoAsignaturaModel> GetAsignaturasEspecialesByDiagramacionCurso(int idGradoAnio, int idTurno, DivisionEnum idDivision, int idEscuela);
         List<InasistenciaRegistrarModel> VerificarInasistencias(int idEscuela, int Turno, int Grado, DivisionEnum Division, int? Asignatura);
    }
}
