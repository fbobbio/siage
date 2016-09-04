using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IAsignacionPlanEstudioRules
    {
        List< GradoAñoModel>GetGradoAnioByEscuelaPlanVer(int idEscuelaPlan);
        PlanConsultaModel GetPlanByEscuelaPlan(int idEscuelaPlan);
        EscuelaPlanMostrarModel GetDatosByEscuelaPlanId(int idEscuelaPlan);
        List<UnidadEspecialModel> GetUnidadesEspecialesByEscuelaPlan(int filtroEscuelaPlan );
        AsignacionPlanEstudioModel AsignacionPlanEstudioEditar(AsignacionPlanEstudioModel model);
        List< DetalleAsignaturaModel>GetAsignaturasEspecialesByEscuelaPlan(int filtroEscuelaPlan);
        List< DiagramacionCursoModel>GetDiagramacionesByEscuelaPlan(int? filtroEscuelaPlan);
        bool ValidarAmbitoEscuelaPlan(int idEscuelaPlan);
        bool ValidarDireccionNivelSuperior(int idEmpresaLogueado);
        List<CicloEducativoModel> GetCiclosByEscuela(int idEscuela, int idEscuelaLogueado);
        bool ValidarExistenciaSubGruposParaGradoAnio(int idEscuelaPlan ,int? idGradoAnio);
        List<GradoAñoModel> GetGradoAnioByEscuelaPlan(int idEscuelaPlan);
        //AsignacionPlanEstudioModel AsignacionPlanEstudioSave(List<int> unidadesAcademicas,int planEstudioId, int escuelaId);
        AsignacionPlanEstudioModel AsignacionPlanEstudioSave(AsignacionPlanEstudioModel model);
        List<PlanEstudioModel> GetPlanesByCicloEscuela(int cicloId, int escuelaId);
        List<GradoAñoModel> GetGradoAnioByPlanEstudioId(int planEstudioId);
        List<DetalleAsignaturaModel> GetAsignaturasEspecialesByIdPlan(int filtroIdPlan);
        EscuelaPlanModel GetEscuelaPlanByid(int idEscuelaPlan);
        bool ValidarExistenciaAsignaturaEspecial(int idPlan, int idEscuela, int? idCiclo);
        bool ValidarEstructuraEscolar(int escuelaId, int planEstudioId);
        bool ValidarEstructuraDefinitiva(int escuelaId);
      
        AsignacionPlanEstudioModel GetAsignacionPlanEstudioById(int id);
        bool ValidarExistenciaUnidadesParaGradoAnio(int idEscuelaPlan,int? idGradoAnio);
        List<DetalleAsignaturaModel>GetAsignaturasByEscuelaPlan(int idEscuelaPlan,int? idGradoAnio);
        List<DetalleAsignaturaModel> GetExistenciaAsignaturaEspecialByEscuelaPlan(int idEscuelaPlan, int? idGradoAnio);

        void ValidarExistenciaSecciones(int idEscuela);

        void ValidarExistenciaDiagramaciones(int idEscuela, int idCiclo);
    }
}