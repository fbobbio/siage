using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoTurno : IDao<Turno, int>
    {
        List<Turno> GetTurnoByEscuela(int idEscuela);
        List<Turno> GetByGradoAnio(int idGradoAnio, int idEscuela);
        List<Turno> GetByEscuelaYCarrera(int idEscuela, int idCarrera);
    }
}
