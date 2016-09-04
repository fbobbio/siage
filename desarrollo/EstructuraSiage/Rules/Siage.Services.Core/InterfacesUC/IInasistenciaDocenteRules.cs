using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.Domain;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
   public interface IInasistenciaDocenteRules
   {
       List<InasistenciaDocente> GenerarInasistenciasParaParo(InasistenciaDocenteModel inasistenciaDocenteModel);
       List<InasistenciaDocenteConsultaModel> GetInasistenciasDocenteModel(int? idInasistencia,DateTime? filtroFechaDesde,
                                                                   DateTime? filtroFechaHasta, string filtroLegajoAgente,
                                                                   bool? filtroAusenciaAnticipada,
                                                                   EstadoInasistenciaDocenteEnum?
                                                                       filtroEstadoInasistencia,
                                                                   TipoMotivoInasistenciaDocenteEnum?
                                                                       filtroTipoMotivoInasistencia);

       InasistenciaDocenteModel InasistenciaDocenteSave(InasistenciaDocenteModel model);
       InasistenciaDocenteModel InasistenciaDocenteUpdate(InasistenciaDocenteModel model);
       InasistenciaDocenteModel InasistenciaDocenteDelete(InasistenciaDocenteModel model);
       int CalcularCantidadDiasAusencia(DateTime fechaDesde, DateTime fechaHasta, int idEmpresaUsuarioLogueado, int idAgente, TipoMotivoInasistenciaDocenteEnum motivo);
       List<DtoPersonalPorTurno> CalcularPorcentajeParoAcatamiento(List<int> idAsignaciones);

       //List<UnidadAcademicaModel> GetHorasCatedrasYAsignaturas(DateTime fechaDesde, DateTime fechaHasta,
       //                                                        int asignacionId);


       List<PersonalPorTurnoModel> GetPorcentajeAcatamientoPorTurno(List<int> idAsignaciones);
       PrevisionAusenciaParaInasistenciaModel GetPrevisionAusenciaDocente(int idAgente, int idPuesto);
   }
   }

