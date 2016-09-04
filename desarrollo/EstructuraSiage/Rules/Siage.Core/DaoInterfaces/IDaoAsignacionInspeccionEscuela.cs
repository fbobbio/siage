using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoAsignacionInspeccionEscuela : IDao<AsignacionInspeccionEscuela, int>
    {
        List<AsignacionInspeccionEscuela> GetByFiltros();
        AsignacionInspeccionEscuela GetVigenteByEscuela(int id);
        List<AsignacionInspeccionEscuela> GetVigentesByInspeccion(int idInspeccion);
        AsignacionInspeccionEscuela GetAsignacionByEscuela(int idEscuela);
    }
}

