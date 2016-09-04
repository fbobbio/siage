using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;

namespace Siage.Services.Core.Models
{
    public class EscuelaConsultaModel : EmpresaModel
    {
        public virtual string NivelEducativoNombre { get; set; }
        public virtual TipoEducacionEnum TipoEducacion { get; set; }
    }
}
