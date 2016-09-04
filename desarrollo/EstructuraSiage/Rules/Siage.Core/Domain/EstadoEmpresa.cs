using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;

namespace Siage.Core.Domain
{
    public class EstadoEmpresa
    {
        public virtual int Id { get; set; }

        public virtual EstadoEmpresaEnum Estado { get; set; }
        public virtual DateTime FechaModificacion { get; set; }
        public virtual Usuario UsuarioModificacion { get; set; }
        public virtual EmpresaBase Empresa { get; set; }
    }
}
