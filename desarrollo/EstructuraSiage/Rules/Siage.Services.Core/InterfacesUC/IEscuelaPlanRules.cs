using System;
using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IEscuelaPlanRules
    {
        List<EscuelaPlanConsultaModel> GetEscuelaPlanByFiltros(string codigoPlanEstudio,string  codigoEscuela, int? carreraId,bool? filtroPorFechaVigencia, DateTime? fechaInicio,DateTime? fechaFin);
        List<DetalleSubGrupoModel>GetDetalleSubGrupoByEscuelaPlan(int filtroEscuela, int? filtroGradoAnio);
        List<EscuelaPlanModel> GetEscuelaPlanByEscuela(int idEscuela);
        bool TieneEscuelaPlan(int filtroIdEscuela);
    }
}
