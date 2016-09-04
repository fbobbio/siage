using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoInasistencia : IDao<Inasistencia, int>
    {
        List<Inasistencia> GetInasistenciasByEstudianteId(int estudianteId);
        List<Inasistencia> GetInasistenciasByInscripcionId(int id);
        List<Inasistencia> GetByDiagramacionCurso(int idEscuela, DateTime Fecha, int Turno, int Grado, DivisionEnum Division);
        bool EstudianteConInasistencia(int InscripcionId, DateTime Fecha);
        double ContarInasistenciasByInscripcionId(int id, out double justificadas, out double injustificadas);
        List<Inasistencia> GetByDiagramacionCurso(int idEscuela, DateTime FechaDesde, DateTime FechaHasta, int Turno, int Grado, DivisionEnum Division);
 
    }
}

