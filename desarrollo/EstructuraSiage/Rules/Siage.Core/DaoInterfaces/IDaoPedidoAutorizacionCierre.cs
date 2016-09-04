using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoPedidoAutorizacionCierre :IDao<PedidoAutorizacionCierre, int>
    {
        List<PedidoAutorizacionCierre> GetByFiltros(int? id, int? idEmpresa, int? idEstadoPedidoAutorizacionCierre,
                                                    DateTime? fechaAltaDesde, DateTime? fechaAltaHasta,
                                                    int? idAsignacionInstrumentoLegal);
        List<PedidoAutorizacionCierre> GetByFiltrosEmpresa(string cue,int? cueAnexo, string codigoEmpresa, int? nroEscuela,
                                                  int? idPedidoAutorizacion, DateTime? fechaAltaDesde, DateTime? fechaAltaHasta,DateTime? fechaCierreDesde,DateTime? fechaCierreHasta,
                                                  EstadoPedidoCierreEnum? estadoPedido);

        DtoFechaYUsuarioCierreEmpresa GetUsuarioPedidoByIdEmpresa(int empresaId);

    }
}
