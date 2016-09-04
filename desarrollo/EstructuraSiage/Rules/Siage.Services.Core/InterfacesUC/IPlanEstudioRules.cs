using System.Collections.Generic;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{ 
    public interface IPlanEstudioRules
    {
        List<PlanConsultaModel>GetPlanesImplementadosEnEscuela(int idEscuela);
        EscuelaModel GetEscuelaByIdEscuelaPlan(int idEscuelaPlan);
        PlanEstudioConsultaModel GetPlanEstudioConsultaById(int id);
        PlanEstudioModel GetPlanEstudioById(int id);
        PlanEstudioModel RegistrarPlanEstudio(PlanEstudioModel model, List<int> listaIdANoBorrar);
        PlanEstudioModel ActualizarPlanEstudio(PlanEstudioModel model, List<int> listaIdANoBorrar);
        List<PlanEstudioConsultaModel> GetPlanEstudioByFiltros(string codigoPlan, string nroInstrumentoLegal, int? idNivel, int? idTitulo, int? añosDuracion, EstadoPlanEstudioEnum? estado);
        List<PlanEstudioModel> GetPlanEstudioVigenteByNivel(int idNivelEducativo);
        List<PlanEstudioModel> GetPlanesEstudioVigentesByNivelNoAsignadosACarrera(int idNivelEducativo, int? idCarrera);
        List<DetalleAsignaturaModel> GetAllAsignaturasByPlan(int planId);
        bool VerificarEstadoPlanEstudioEdicion(int id);
        bool VerificarCodigoPlan(string codigoPlanEstudio);
        List<PlanEstudioModel> GetPlanesVigentesByCarreraId(int carreraId);
        void RegistrarFinDeVigenciaDePlanDeEstudios(PlanEstudioModel model);
    }
}