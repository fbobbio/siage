using System;
using System.Collections.Generic;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IContratoRules
    {        
        ContratoModel GetContratoById(int id);
        ContratoModel ContratoSaveOUpdate(ContratoModel modelo);
        List<ContratoConsultaAvanzadaModel> GetContratoByFiltros(DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, EntidadAlquiladaEnum? filtroEntidadAlquilada, TipoContratoEnum? filtroTipoContrato, bool? rescindido = null, bool? vigente = null, bool? dadosBaja = null);
        List<ContratoConsultaModel> GetContratoByFiltros(DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, EntidadAlquiladaEnum? filtroEntidadAlquilada, TipoContratoEnum? filtroTipoContrato, bool? rescindido = null, bool? vigente = null, bool? dadosBaja = null, int? parametroVencimientoAlquilerOComodato = null);
        List<ContratoConsultaModel> GetContratosVencidos(TipoContratoEnum? tipoContrato,EntidadAlquiladaEnum? tipoEntidadAlquilada);                
        bool ValidarExistencia(int identificador, EntidadAlquiladaEnum entidadAlquilada);
        void ContratoDelete(ContratoModel modelo);
        void RescindirContrato(ContratoModel model);
        void RescindirContrato(AsignacionInstrumentoLegalModel asignacionInstrumentoLegalRescicion, string observacionesRescicion, MotivoRescisionContratoEnum motivo, int id);
        void ValidarContrato(ContratoModel model);
    }
}