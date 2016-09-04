using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Siage.Base.Dto
{
    public class DtoEstadoEmpresa
    {
        public virtual int Id { get; set; }
        public EstadoEmpresaEnum Estado { get; set; }
    }
}
