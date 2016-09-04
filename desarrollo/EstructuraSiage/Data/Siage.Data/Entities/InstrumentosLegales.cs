using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Data.MockEntities
{
    public static class InstrumentosLegales
    {
        public static InstrumentoLegal InstrumentoLegal1
        {
            get
            {
                return new InstrumentoLegal
                           {

                               EmisorInstrumentoLegal = EmisorInstrumentoLegalEnum.MINISTERIO,
                               FechaEmision = DateTime.Today.AddYears(-3),
                               Id = 1,
                               //TipoInstrumentoLegal = TipoInstrumentoLegalEnum.ACTA
                           };

            }
        }
    }
}
