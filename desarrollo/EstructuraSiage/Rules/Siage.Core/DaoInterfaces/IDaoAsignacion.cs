using System;
using Siage.Core.Domain;
using System.Collections.Generic;
using Siage.Base.Dto;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoAsignacion : IDao<Asignacion, int>
    {

        DetalleAsignacion GetUltimoDetalleAsignacion(int idAgente, int idPuestoTrabajo, DateTime? fechaHasta);

        List<Asignacion> GetByIdAgente(int idAgente);

        Asignacion GetByAgenteIdYPuestoId(int idAgente, int idPuesto);

        List<DtoConsultaAsignacion> GetAsignacionAgenteByFiltros(int? filtroNumeroDocumento, string filtroTipoDocumento,
                                                                 int? filtroSexo, string filtroApellido,
                                                                 string filtroNombre, DateTime filtroFechaDesde,
                                                                 DateTime filtroFechaHasta, int idEmpresa);

        List<Asignacion> GetAsignacionesByIdPuesto(int idPuesto);
        List<Asignacion> GetAsignacionesActivasByIdPuesto(int idPuesto);

        int GenerarCodigoAsignacion();

    }
}