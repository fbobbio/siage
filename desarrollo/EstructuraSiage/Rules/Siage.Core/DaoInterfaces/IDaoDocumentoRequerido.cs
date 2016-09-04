using Siage.Core.Domain;
using System.Collections.Generic;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoDocumentoRequerido : IDao<DocumentoRequerido, int>
    {
        List<DocumentoRequerido> DocumentoRequeridoByFiltros(ProcesoEnum? proceso, int? gradoanio, int? idCarrera);
        List<Documento> DocumentoObligatorioByFiltros(ProcesoEnum proceso, int gradoanio);
        List<DocumentoRequerido> DocumentoRequeridoVigentesByFiltros(int idNivel, int? idCarrera, int idProceso, int idGradoAnio);
        List<DocumentoRequerido> DocumentoRequeridoByFiltrosPresentadosNoPresentadosEstudiante(ProcesoEnum proceso, int? gradoanio, int idNivel, int idEstudiante, int idEscuela, int? idCarrera, bool presentados);
    }
}