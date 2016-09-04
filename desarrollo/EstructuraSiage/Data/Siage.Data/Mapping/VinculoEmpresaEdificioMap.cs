using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHibernate;
using Siage.Core.Domain;
using Siage.Data.CustomTypes;

namespace Siage.Data.Mapping
{
    public class VinculoEmpresaEdificioMap : ClassMap<VinculoEmpresaEdificio>
    {

        public VinculoEmpresaEdificioMap()
        {
            Id(x => x.Id, "ID_SEQ_VINCULO_EMPRESA_EDIF").GeneratedBy.Native("SEQ_IN_VIN_EMPRESA_EDIFICIO");
            Map(x => x.FechaDesde, "FEC_DESDE").Not.Nullable();
            Map(x => x.FechaHasta, "FEC_HASTA");
            Map(x => x.Estado, "ID_SEQ_EVINCULO_EMPR_EDIF").Not.Nullable().CustomType(typeof(Siage.Base.EstadoVinculoEmpresaEdificioEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);
            Map(x => x.Observacion, "OBSERVACIONES").Length(70);
            Map(x => x.Motivo , "MOTIVO").Length(60);
            Map(x => x.DeterminaDomicilio, "DETERMINA_DOMICILIO").CustomType<YesNoType>().Length(1);
            References<Edificio>(x => x.Edificio, "ID_SEQ_EDIFICIO").Not.Nullable();
            References<EmpresaBase>(x => x.Empresa, "ID_SEQ_EMPRESA").Not.Nullable();

            Table("T_IN_VIN_EMPRESA_EDIFICIO");
            Cache.ReadWrite();
        }
    }
}
