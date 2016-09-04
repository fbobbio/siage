using Siage.Base.Dto;
using Siage.Core.Domain;
using System;
using System.Collections.Generic;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoEscuelaPlan : IDao<EscuelaPlan, int>
    {
        DtoEscuelaPlan GetDatosByEscuelaPlanId(int idEscuelaPlan);
        List< DiagramacionCurso> GetDiagramacionesByEscuelaPlan(int? filtroEscuelaPlan);
        bool ValidarAmbitoEscuelaPlanRural(int idEscuelaPlan);
        List<DetalleAsignatura> GetAsignaturasEspecialesByEscuelaPlan(int idEscuelaPlan, int? idGradoAnio);
        EscuelaPlan GetByCarreraEscuela(int idCarrera, int idEscuela);
        List<DetalleSubGrupo> GetDetalleSubGrupoByEscuelaPlan(int filtroEscuela, int? filtroGradoAnio);
        List<EscuelaPlan> GetByFiltros();
        List<EscuelaPlan> GetEscuelaPlanByEscuela(int idEscuela);        
        List<EscuelaPlan> GetEscuelaPlanByFiltros(string codigoPlanEstudio, string codigoEscuela, int? carreraId,bool? filtroPorFechaVigencia, DateTime? fechaInicio, DateTime? fechaFin, int idNivel);
        bool ValidarExistenciaSubGruposParaGradoAnio(int idEscuelaPlan, int? idGradoAnio);
        bool ValidarExistenciaDivisionesEnUnidadesParaGradoAnio(int idEscuelaPlan, int? idGradoAnio);
        bool TieneEscuelaPlan(int filtroIdEscuela);
        List<int> PlanesConEscuelaPlan(List<int> planes);
    }
}