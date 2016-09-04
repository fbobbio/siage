using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHibernate;using Siage.Core.Domain;

namespace Siage.Data.Mapping
{
    public class EscuelaPrivadaMap : ClassMap<EscuelaPrivada>
    {

     
        public EscuelaPrivadaMap()
        {
            Id(x => x.Id, "ID_SEQ_ESCUELA_PRIVADA").GeneratedBy.Native("SEQ_EM_ESC_PRIVADA");

            Map(x => x.PorcentajeAporteEstado, "PORCENTAJE_APORTE_ESTADO").Not.Nullable();
            Map(x => x.NumeroCuentaBancaria, "CUENTA_BANCARIA").Not.Nullable().Length(30);            
            References<PersonaFisica >(x => x.Director, "ID_DIRECTOR").Not.Nullable();
            References<PersonaFisica>(x => x.RepresentanteLegal, "ID_REPRESENTANTE_LEGAL");
            References<SucursalBanco>(x => x.SucursalBanco , "ID_SUCURSAL").Not.Nullable();
            References<ObraSocial>(x => x.ObraSocial, "ID_OBRA_SOCIAL");

            Table("T_EM_ESC_PRIVADA");
            Cache.ReadWrite();
          
        }

    }
}

