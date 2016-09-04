using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.Domain
{
  public class EscuelaPrivada
  { 
	    public EscuelaPrivada()
		{
			
      	}
		public virtual int Id {get; set; }
        public virtual PersonaFisica Director { get; set; }
        public virtual PersonaFisica RepresentanteLegal { get; set; }
		public virtual float PorcentajeAporteEstado { get; set; } 
		public virtual string NumeroCuentaBancaria { get; set; } 
		public virtual SucursalBanco SucursalBanco {get; set;}
        public virtual ObraSocial ObraSocial { get; set; }

	
        
  }
}

