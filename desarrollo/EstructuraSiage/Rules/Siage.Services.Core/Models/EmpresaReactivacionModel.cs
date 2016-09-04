using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Siage.Base;


namespace Siage.Services.Core.Models
{
    public class EmpresaReactivacionModel
    {

        public int Id { get; set; }

        public EstadoEmpresaEnum EstadoEmpresa { get; set; }

        public virtual IList<VinculoEmpresaEdificioReactivacionEmpModel> VinculoEmpresaEdificio { get; set; }

        public int Domicilio { get; set; }

        public int? PaquetePresupuestado { get; set; }

        [UIHint("InstrumentoLegalEditor")]
        public InstrumentoLegalModel InstrumentoLegal { get; set; }

        [DisplayName("Fecha de Notificación")]
        public DateTime? FechaNotificacionAsignacionInstrumentoLegal { get; set; }
        [DisplayName("Observaciones")]
        public string ObservacionesAsignacionInstrumentoLegal { get; set; }

        public int ProgramaPresupuestario { get; set; }

        public int OrdenDePago { get; set; }

        /** La nueva escuela madre seleccionada */
        public int? NuevaEscuelaMadre { get; set; }

        public string NombreSugerido { get; set; }

        /** La estructura escolar que se pueden haber cargado */
        //protected IList<DiagramacionCursoRegistrarModel> _estructuraEscolar;

        [UIHint("EstructuraEscuelaRegistrarEditor")]
        public virtual IList<DiagramacionCursoRegistrarModel> EstructuraEscolar {get;set;}

        /** Los planes de estudio que se pueden haber cargado*/
        public IList<EscuelaPlanModel> PlanDeEstudio { get; set; }

        /** Id de la empresa de inspección */
        public int? IdEmpresaInspeccion { get; set; }
    }
}
