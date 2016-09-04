using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IProgramaPresupuestarioRules
    {
        ProgramaPresupuestarioModel GetProgramaPresupuestarioById(int id);
    }
}
