using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Siage.Services.Core.Models
{
    public class EmpresaVisadoCierreModel
    {
        public int Id { get; set; }

        [DisplayName("Observaciones")]
        public string ObservacionesRechazo { get; set; }

        public bool DesvincularEdificio { get; set; }

        public bool Rechazado { get; set; }

        //Datos del pedido
        public int IdPedido { get; set; }

        [DisplayName("Fecha alta pedido")]
        public DateTime FechaAltaPedido { get; set; }

        [DisplayName("Estado del pedido")]
        public EstadoPedidoCierreEnum EstadoPedido { get; set; }

        public int AgenteAltaPedidoId { get; set; }

        [DisplayName("Agente alta del pedido")]
        public string  AgenteAltaPedido { get; set; }

        //Datos identificador empresa
        public int IdEmpresa { get; set; }

        [DisplayName("Codigo empresa")]
        public string CodigoEmpresa { get; set; }

        [DisplayName("Nombre empresa")]
        public string NombreEmpresa { get; set; }

        [DisplayName("CUE empresa")]
        public string CUE { get; set; }

        [DisplayName("Nivel educativo")]
        public string NivelEducativo { get; set; }

        [DisplayName("Tipo educacion")]
        public string TipoEducacion { get; set; }

        [DisplayName("Tipo empresa")]
        public TipoEmpresaEnum TipoEmpresa { get; set; }

        [DisplayName("Estado empresa")]
        public EstadoEmpresaEnum EstadoEmpresa { get; set; }

        [Required, DisplayName("Fecha de cierre empresa")]
        public DateTime? FechaCierreEmpresa { get; set; }

        //Datos asignacion instrumento legal
        [UIHint("AsignacionInstrumentoLegalEditor")]
        public AsignacionInstrumentoLegalModel AsignacionIL { get; set; }

    }
}
