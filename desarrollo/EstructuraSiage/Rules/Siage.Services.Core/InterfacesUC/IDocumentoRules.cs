using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IDocumentoRules
    {
        void DocumentoDelete(DocumentoRegistraModel modelo);
        DocumentoRegistraModel GetDocumentoRegistraById(int id);
        DocumentoRegistraModel GetDocumentoById(int id);
        DocumentoRegistraModel DocumentoSave(DocumentoRegistraModel modelo);        
        DocumentoRegistraModel DocumentoUpdate(DocumentoRegistraModel modelo);
        List<DocumentoRegistraModel> DocumentoByFiltros(int? id, string filtroNombre, string filtroDescripcion);
        List<DocumentoRegistraModel> DocumentoGetAll();
    }
}
