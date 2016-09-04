using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;

namespace Siage.Services.Core.Models
{
   public class EmpresaInspeccionadaPorInspeccionModel
    {
        public String CodigoEmpresa { get; set; }
        public String NombreEmpresa { get; set; }
        public String TipoEmpresa { get; set; }
        public String EstadoEmpresa { get; set; }
        public String NivelEducativo { get; set; }
        public String Localidad { get; set; }
        public String Departamento { get; set; }
        public String EstadoAsignacion { get; set; }
        public String TipoInspeccion { get; set; }
    }
}
