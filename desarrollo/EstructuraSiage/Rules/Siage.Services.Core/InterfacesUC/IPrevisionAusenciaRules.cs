using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IPrevisionAusenciaRules
    {
        PrevisionAusenciaRegistrarModel PrevisionAusenciaSave(PrevisionAusenciaRegistrarModel model, int idAgenteLogueado);
        PrevisionAusenciaModel PrevisionAusenciaDelete(int idPrevision);
        void RegistrarInasistenciaPrevisionAusencia(InasistenciaDocenteModel model);
        PrevisionAusenciaModel RegistrarDecisionAutorizacionPrevisionAusencia(PrevisionAusenciaModel model, int idEscuela);
        List<DtoActividadEspecialModel> ConsultarActividadesdesEspecialesActivas(int idAgente, int idEmpresa,
                                                                         DateTime fechaDesde, DateTime fechaHasta);

        bool VerificarExistenciaInasistenciaDocente(DateTime fechaDesde, DateTime fechaHasta, string legajoAgente);
        DtoPrevisionAusenciaDatosModel GetPrevisionAusenciaById(int idPrevision);
        List<PuestoDeTrabajoConsultaModel> GetPTAfectadosSinHorasCatedra(int idAgente, DateTime fechaDesde, DateTime fechaHasta);
        List<PuestoDeTrabajoConsultaModel> GetPTAfectadosConHorasCatedra(int idAgente, DateTime fechaDesde, DateTime fechaHasta);
        bool ValidarEliminarPrevision(int idPrevision);

        //List<PrevisionAusenciaModel> GetPevisionByFiltros(DateTime? FiltroFechaDesde, DateTime? FiltroFechaHasta, string FiltroLegajo, string FiltroTipoDni, SexoEnum? filtroSexo, string FiltroNumeroDocumento, string FiltroNombre, string FiltroApellido, DateTime? FiltroFechaDesdeAutorizacion, DateTime? FiltroFechaHastaAutorizacion, EstadoPrevisionAusenciaEnum? FiltroEstado, string FiltroCodigoEmpresa);
        List<DtoPrevisionAusenciaModel> GetPevisionByFiltrosDto(DateTime? FiltroFechaDesde, DateTime? FiltroFechaHasta, string FiltroLegajo, string FiltroTipoDni, string filtroSexo, string FiltroNumeroDocumento, string FiltroNombre, string FiltroApellido, DateTime? FiltroFechaDesdeAutorizacion, DateTime? FiltroFechaHastaAutorizacion, EstadoPrevisionAusenciaEnum? FiltroEstado, string FiltroCodigoEmpresa);
        List<DtoPrevisionAusenciaModel> GetPevisionConHorasCatedra(DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, string filtroLegajo, string filtroTipoDni, string filtroSexo, string filtroNumeroDocumento, string filtroNombre, string filtroApellido, DateTime? filtroFechaDesdeAutorizacion, DateTime? filtroFechaHastaAutorizacion, EstadoPrevisionAusenciaEnum? filtroEstado, string filtroCodigoEmpresa);
        List<DtoPrevisionAusenciaModel> GetPevisionSinHorasCatedra(DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, string filtroLegajo, string filtroTipoDni, string filtroSexo, string filtroNumeroDocumento, string filtroNombre, string filtroApellido, DateTime? filtroFechaDesdeAutorizacion, DateTime? filtroFechaHastaAutorizacion, EstadoPrevisionAusenciaEnum? filtroEstado, string filtroCodigoEmpresa);
    }
}
                                                               