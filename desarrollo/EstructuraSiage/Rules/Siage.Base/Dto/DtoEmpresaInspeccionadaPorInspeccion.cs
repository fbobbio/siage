using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Siage.Base.Dto
{
 public  class DtoEmpresaInspeccionadaPorInspeccion
    {
     /*
        public String CodigoEmpresa { get; set; }
        public String NombreEmpresa { get; set; }
        public TipoEmpresaEnum TipoEmpresa { get; set; }
        public EstadoEmpresaEnum EstadoEmpresa { get; set; }
        public String NivelEducativo { get; set; }
        public String Localidad { get; set; }
        public String Departamento { get; set; }
        public EstadoAsignacionInspeccionEscuelaEnum EstadoDeAsignacion { get; set; }
        public TipoInspeccionEnum TipoInspeccion { get; set; }
        public int idVinculoDomicilio { get; set; }
     */

     public string CODIGOEMPRESA { get; set; }
        public string NOMBREEMPRESA { get; set; }



        public string NIVELEDUCATIVO { get; set; }
        public string LOCALIDAD { get; set; }
        public string DEPARTAMENTO { get; set; }
        public string ESTADOASIGNACION { get; set; }

        public string TIPOINSPECCION { get; set; }
        public string TIPOEMPRESA { get; set; }
        public string ESTADOEMPRESA { get; set; }
        public string NOMBREINSPECCION { get; set; }

       
        
        public int idVinculoDomicilio { get; set; }

        
    }
}
