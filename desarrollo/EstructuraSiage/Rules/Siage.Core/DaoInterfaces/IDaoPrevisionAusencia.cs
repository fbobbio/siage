using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;
using Siage.Base.Dto;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoPrevisionAusencia : IDao<PrevisionAusencia, int>
    {
        bool VerificarNoHorasCatedra(int idPuestoTrabajo);
        bool VerificarPrevisionCorrespondeAPuestoSinHorasCatedra(int idPrevisionAusencia);
        List<DtoPuestoDeTrabajoConsulta> GetPTAfectadosSinHorasCatedra(int idAgente, DateTime fechaDesde,
                                                                      DateTime fechaHasta);
        List<DtoPrevisionAusenciaVistaNIYNP> GetPTAfectadosConHorasCatedraNIYNP(int idAgente, DateTime fechaDesde,
                                                                                DateTime fechaHasta);
        List<DtoPrevisionAusenciaVistaNM> GetPTAfectadosConHorasCatedraNM(int idAgente, DateTime fechaDesde,
                                                                                DateTime fechaHasta);
        List<DtoPrevisionAusenciaVistaNS> GetPTAfectadosConHorasCatedraNS(int idAgente, DateTime fechaDesde,
                                                                                               DateTime fechaHasta);
        List<DtoPrevisionAusenciaDatos> GetPrevisionAusenciaMostrarById(int idPrevisionAusencia);
        bool VerificarExistenciaInasistenciaDocente(DateTime fechaDesde, DateTime fechaHasta, string legAgente);
        DtoPrevisionAusenciaParaInasistencia GetPrevisionAusenciaByAgenteYPuesto(int idAgente, int idPuesto);
        List<DtoPrevicionAusencia> GetByFiltrosDto(DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, string filtroLegajo, string filtroTipoDni, string filtroSexo, string filtroNumeroDocumento, string filtroNombre, string filtroApellido, DateTime? filtroFechaDesdeAutorizacion, DateTime? filtroFechaHastaAutorizacion, EstadoPrevisionAusenciaEnum? filtroEstado, string filtroCodigoEmpresa);
        //DtoPrevisionAusenciaDatos GetbyIdPrevision(int idPrevision);
        List<DtoPrevicionAusencia> GetPevisionConHorasCatedra(DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, string filtroLegajo, string filtroTipoDni, string filtroSexo, string filtroNumeroDocumento, string filtroNombre, string filtroApellido, DateTime? filtroFechaDesdeAutorizacion, DateTime? filtroFechaHastaAutorizacion, EstadoPrevisionAusenciaEnum? filtroEstado, string filtroCodigoEmpresa);
        DtoPrevisionAusenciaDatos GetDatosDetalladosPrevisionAusenciaById(int idPrevision);
        List<DtoPrevicionAusencia> GetPevisionSinHorasCatedra(DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, string filtroLegajo, string filtroTipoDni, string filtroSexo, string filtroNumeroDocumento, string filtroNombre, string filtroApellido, DateTime? filtroFechaDesdeAutorizacion, DateTime? filtroFechaHastaAutorizacion, EstadoPrevisionAusenciaEnum? filtroEstado, string filtroCodigoEmpresa);

    }
}
