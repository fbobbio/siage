using System;
using Siage.Base;

namespace Siage.Core.Domain
{
    public class InstrumentoLegal
    {
        public InstrumentoLegal()
        {

        }

        public virtual int Id { get; set; }

        public virtual DateTime? FechaAlta { get; set; }
        public virtual string NroInstrumentoLegal { get; set; }
        public virtual DateTime FechaEmision { get; set; }
        public virtual string Observaciones { get; set; }
        public virtual EmisorInstrumentoLegalEnum? EmisorInstrumentoLegal { get; set; }
        public virtual TipoInstrumentoLegal TipoInstrumentoLegal { get; set; }
        public virtual Expediente Expediente { get; set; }
    }
}