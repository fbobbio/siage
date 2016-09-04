using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Siage.Base;

namespace Siage.Services.Core.Models
{
    public class EscuelaPlanConsultaModel
    {
        public virtual int Id { get; set; }
        public virtual string NombreEscuela { get; set; }
        public virtual string NivelEducativoNombreEscuela { get; set; }
        public virtual string TipoEducacionEscuela { get; set; }
        public virtual string CodigoPlan { get; set; }
        public virtual DateTime FechaAsignacion { get; set; }
    }
}
