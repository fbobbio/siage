using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;

namespace Siage.Core.Domain
{
    public class EmpresaConsultaEdificio
    {
        public virtual int Id { get; set; }

        public virtual EstadoEmpresaEnum EstadoEmpresa { get; set; }

        public virtual string Nombre { get; set; }

        public virtual string CUE { get; set; }

        public virtual string CodigoEmpresa { get; set; }

        public virtual TipoEmpresaEnum TipoEmpresa { get; set; }
    }
}
