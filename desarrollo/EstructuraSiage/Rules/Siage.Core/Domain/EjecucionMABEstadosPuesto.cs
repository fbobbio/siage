using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Siage.Core.Domain
{
    /** 
    * <summary> Clase EjecucionMABEstadosPuesto
    *	
    * </summary>
    * <remarks>
    *		Autor: transito
    *		Fecha: 8/11/2011 6:56:53 PM
    * </remarks>
    */
    public class EjecucionMABEstadosPuesto
    {
        public virtual int Id { get; set; }
        public virtual EjecucionMab EjecucionMab { get; set; }
        public virtual EstadoPuesto EstadoPuesto { get; set; }
        public virtual Boolean EsPosterior { get; set; } 
    }
}
