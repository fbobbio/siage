using System.Collections.Generic;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IDocumentoRequeridoRules
    {
        void DocumentoRequeridoDelete(DocumentoRequeridoModel modelo);
        DocumentoRequeridoModel GetDocumentoRequeridoRegistraById(int id);
        DocumentoRequeridoModel GetDocumentoRequeridoById(int id);
        DocumentoRequeridoModel DocumentoRequeridoSave(DocumentoRequeridoModel modelo, int idEmpresa);
        DocumentoRequeridoModel DocumentoRequeridoDarDeBaja(int modeloId);
        List<DocumentoRequeridoModel> DocumentoRequeridoDireccionDeNivelByFiltros(ProcesoEnum proceso, int gradoanio);
        List<DocumentoRequeridoModel> DocumentoRequeridoByEscuelaLogeadaYProceso(ProcesoEnum proceso);        
        List<DocumentoRequeridoModel> DocumentoRequeridoByEscuelaCarreraYProcesoMenosPresentadosPorEstudiante(ProcesoEnum proceso, int idEstudiante, int idEscuela, int? idCarrera, int? idGradoAnio);
        List<DocumentoRequeridoModel> DocumentoRequeridoByEscuelaCarreraYProcesoPresentadosPorEstudiante(ProcesoEnum proceso, int idEstudiante, int idEscuela, int? idCarrera, int? idGradoAnio);
        List<DocumentoRequeridoModel> DocumentoRequeridoByFiltros(int idNivel, int? idCarrera, int idProceso, int idGradoAnio);
        List<DocumentoRequeridoModel> DocumentoRequeridoByFiltros(ProcesoEnum proceso, int? idGrado, int? idCarrera);
    }
}