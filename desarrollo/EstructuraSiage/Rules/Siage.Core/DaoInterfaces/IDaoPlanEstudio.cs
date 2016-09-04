using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.Domain;
using System.Collections.Generic;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoPlanEstudio : IDao<PlanEstudio, int>
    {        
        List<PlanEstudio> GetByFiltros(string codigoPlan, string nroInstrumentoLegal, int? idNivel, int? idTitulo, int? añosDuracion, EstadoPlanEstudioEnum? estado);
        List<PlanEstudio> GetByNivel(int filtroNivel);        
        List<PlanEstudio> GetByNivelVigente(int idNivelEducativo);
        List<PlanEstudio> GetByNivelVigentesNoAsignadosACarrera(int idNivelEducativo, int? idCarrera);
        List<PlanEstudio> GetPlanesByEscuelaGrado(int idEscuela, int idGrado);
        List<PlanEstudio> GetByIdOrientacion(int idOrientacion);
        List<PlanEstudio> GetByIdSubOrientacion(int idSubOrientacion);
        List<PlanEstudio> GetByIdEspecialidad(int idEspecialidad);
        List<PlanEstudio> GetPlanesByCicloNivelEducativo(int cicloId, int nivelEducativoEscuela);
        List<PlanEstudio> GetByIdCarrera(int idCarrera);
        List<PlanEstudio> GetPlanesVigentesByCarreraId(int carreraId);
        List<DetalleAsignatura> GetAllAsignaturasByPlan(int planId);
        List<DtoPlanConsulta> GetPlanesImplementadosEnEscuela(int idEscuela);

        DtoPlanConsulta GetByEscuelaPlanId(int idEscuelaPlan);
        PlanEstudio GetPlanEstudioByTitulo(int idTitulo);
        PlanEstudio GetPlanEstudioByEspecialidad(int idEspecialidad);

        void QuitarReferenciaCarrera(List<int> planes);
        int GetCantidadDePlanesDeEstudioByNivelNivelEducativo(int idNivelEducativo);
        string GetCodigoSecuencialDePlan(TipoEducacionEnum tipoEducacionId, int nivelEducativoId);
        bool VerificarCodigoPlan(string codigoPlanEstudio);
        bool ValidarExistenciaAsignaturaEspecial(int idPlan, int? idCiclo);                
        bool ValidarExistenciaPlanesNoProvisoriosByEspecialidad(int idEspecialidad);
        bool ValidarExistenciaPlanesProvisoriosYVigentesByEspecialidad(int idEspecialidad);
        bool VerificarImplementacionVigente(int idPlanEstudio);
    }
}