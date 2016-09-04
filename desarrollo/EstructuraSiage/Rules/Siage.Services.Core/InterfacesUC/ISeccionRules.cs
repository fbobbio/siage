using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ISeccionRules
    {
        List<SeccionModel>GetSeccionesByIdEscuelaPlan(int filtroEscuelaPlan);
        List<DiagramacionCursoModel>GetDiagramacionesVigentesByEscuela(int idEscuela,int idSeccion);
        List<DiagramacionCursoModel> GetDiagramacionesByEscuela(int idEscuela);
        List<int> GetIdsDiagramacionesByIdSeccion(int? idSeccion);
        List<SeccionModel> GetDetalleSeccionByFiltrosSinEscuela(string nombre);
        List<DetalleSeccionModel> GetDetalleSeccionByFiltros(int idEscuela,string nombre);
        List<SeccionModel> GetSeccionesByIdEscuela(int idEscuela, string nombre);
        SeccionModel SeccionDelete(SeccionModel model, int idEscuela);
        SeccionModel GetSeccionById(int turnoId);
        SeccionModel SeccionSave(SeccionModel model, int idEscuela);
        SeccionModel SeccionUpdate(SeccionModel model, int idEscuela);
        List<DetalleSeccionModel> GetDetalleSeccionBySeccionId(int id);
        List<SeccionModel> GetSeccionAll();
        SeccionModel GetSeccionVigente(string nombre);       
        List<DiagramacionCursoModel> GetEstructuraEscuelaByFiltrosSeccion(int? turno, int? gradoAño);
        DiagramacionCursoModel GetEstructuraEscuelaById(int id);
        List<DiagramacionCursoModel> GetDivisionesByIdSeccion(int id);
        bool ValidarEmpresaRural(int idEscuela);
        bool ValidarEmpresaLogueada(int idEscuela);
        List<SeccionModel> GetSeccionesParaEditar(int filtroEscuela, int filtroEscuelaPlan);
    }
}

