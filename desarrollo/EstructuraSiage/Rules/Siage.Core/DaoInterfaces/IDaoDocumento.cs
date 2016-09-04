using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoDocumento : IDao<Documento, int>
    {
        List<Documento> GetDocumentoByDescripcion(string descripcion);
        List<Documento> GetDocumentoByNombre(string nombre);
        List<Documento> GetDocumentoByFilters(string descripcion, string nombre);
        bool ValidarSiExisteNombre(int? id, string nombre);
    }
}

