using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Siage.Base;

namespace Siage.Services.Core.Models
{
    public class EmpresaConsultaEdificioModel
    {
        public virtual int Id { get; set; }

        [DisplayName("Estado Empresa")]
        public virtual string EstadoEmpresa { get; set; }
        [DisplayName("Nombre Empresa")]
        public virtual string Nombre { get; set; }
        public virtual string CUE { get; set; }
        [DisplayName("Código Empresa")]
        public virtual string CodigoEmpresa { get; set; }
        [DisplayName("Tipo Empresa")]
        public virtual string TipoEmpresa { get; set; }

    }
}
