using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;

namespace Siage.Services.Core.Models
{
    public class EscuelaPrivadaModel
    {
        public virtual int Id { get; set; }
        public virtual PersonaFisicaModel Director { get; set; }
        public virtual PersonaFisicaModel RepresentanteLegal { get; set; }

        public virtual float PorcentajeAporteEstado { get; set; }
        public virtual string NumeroCuentaBancaria { get; set; }
        public virtual SucursalBancariaModel SucursalBanco { get; set; }
        public virtual string ObraSocial { get; set; }
    }
}
