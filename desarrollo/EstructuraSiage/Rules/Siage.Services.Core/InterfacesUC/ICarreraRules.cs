using System.Collections.Generic;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ICarreraRules
    {
        List<CarreraModel> GetCarrerasPorEscuelaLogueada();
        List<CarreraModel> GetCarrerasByEscuela(int idEscuela);
        CarreraModel CarreraSave(CarreraModel entidad);
        CarreraModel CarreraUpdate(CarreraModel modelo);
        CarreraModel CarreraDelete(CarreraModel entidad);
        List<CarreraModel> GetCarreraByFiltros(string filtroNombre, EstadoCarreraEnum? filtroEstado);
        List<CarreraModel> GetCarreraByFiltros(int? id, string filtroNombre,  EstadoCarreraEnum? filtroEstado);
        List<CarreraModel> GetCarrerasVigentesByNivel(int idNivel);
        CarreraModel GetCarreraById(int id);
        List<PlanEstudioModel> GetPlanesAsociadosACarrera(int idCarrera);
        List<CarreraModel> GetCarrerasByEstudianteMatriculado(int idEstudiante);
        bool EsDireccionDeNivelSuperior(int idEmpresa);
        List<CarreraModel> GetCarrerasVigentes();
        List<int> PlanesConEscuelaPlan(List<int> planes);
    }
}