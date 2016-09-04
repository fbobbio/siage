using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IDocumentacionRequeridaRegistracion
    {
        void DocumentoRequeridoDelete(DocumentoRequeridoModel modelo);
        DocumentoRequeridoModel DocumentoRequeridoSave(DocumentoRequeridoModel modelo);
        List<DocumentoRequeridoModel> DocumentoRequeridoByFiltros(string proceso, string gradoanio);
        DocumentoRequeridoModel DocumentoRequeridoUpdate(DocumentoRequeridoModel modelo);
    }
}

