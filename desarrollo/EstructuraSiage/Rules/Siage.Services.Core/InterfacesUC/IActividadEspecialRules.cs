using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Services.Core.Models;
using Siage.Base;
using Siage.Core.Domain;
using Siage.Base.Dto;


namespace Siage.Services.Core.InterfacesUC
{
   public interface IActividadEspecialRules
    {
       ActividadEspecialModel ActividadSave(ActividadEspecialModel modelo);
       ActividadEspecialModel ActividadUpdate(ActividadEspecialModel modelo);
       void ActividadDelete(ActividadEspecialModel modelo);
       List<ActividadEspecialModel> ActividadGetByFiltros(DateTime? fechaDesde, DateTime? fechaHasta, string apellido, string nombre, TipoActividadEspecialEnum? tipoActividad, bool bajas, int idEscuela);
       List<DtoAgenteCargo> DocentesAsignadosYNoAsignadosByIdEscuela(int idEscuela, int idActividad);
       string[] GetDocentesAsignadosByIdActividad(int idActividad);
       List<DtoAgenteCargo> DocentesAsignadosByIdActividad(int idEscuela, int idActividad);
       ActividadEspecialModel AsignarDocenteActividadSave(ActividadEspecialModel modelo);
       ActividadEspecialModel GetActividadEspecialById(int id);
       ActividadEspecialModel RegistrarAsistenciaDocente(ActividadEspecialModel modelo);
       string ValidarExistenciaDeSuspensionActividades(DateTime fechaActividad, string horaDesde, string horaHasta, int idEscuela);
       bool ValidadFechaConCalendario(DateTime fechaActividad, int idEscuela);
    }
}
