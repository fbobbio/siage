using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ITurnoRules
    {
        List<TurnoModel> GetTurnosByEscuela(int idEscuela);
        List<TurnoModel> GetTurnoByEscuelaLogueada();
        List<TurnoModel> GetTurnoByEscuelaSeleccionada(int idEmpresa);
        List<TurnoModel> GetByEscuelaYCarrera(int idEscuela, int idCarrera);
    }
}