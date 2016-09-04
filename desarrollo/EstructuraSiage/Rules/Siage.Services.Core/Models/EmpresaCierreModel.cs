using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Siage.Services.Core.Models
{
    public class EmpresaCierreModel
    {
        public int IdEmpresa { get; set; }
        [DisplayName("Fecha de Cierre")]
        [Required]
        public DateTime? FechaCierre { get; set; }

        [UIHint("EmitirResolucionEmpresaEditor")]
        public ResolucionModel Resolucion { get; set; }

        [UIHint("AsignacionInstrumentoLegalEditor")]
        public AsignacionInstrumentoLegalModel AsignacionInstrumentoLegal { get; set; }

        [DisplayName("Emitir resolución de cierre:")]
        public virtual bool EmitirResolucionDeCierre { get; set; }

        [DisplayName("Fecha notificación")]
        public DateTime? FechaNotificacion { get; set; }
        [DisplayName("Observaciones")]
        public string ObservacionesAsignacion { get; set; }
    }
}
