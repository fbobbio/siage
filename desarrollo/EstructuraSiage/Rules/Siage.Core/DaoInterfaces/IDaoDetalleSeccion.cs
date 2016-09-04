using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoDetalleSeccion : IDao<DetalleSeccion, int>
    {
        List<DetalleSeccion> GetByFiltros();
        List<DetalleSeccion> GetByIdSeccion(int? id);
        List<DetalleSeccion> GetDetalleSeccionByEscuelaRural(int idEscuela);
    }
}

