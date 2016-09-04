using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHibernate;
using Siage.Base;
using Siage.Core.Domain;
using Siage.Data.CustomTypes;


namespace Siage.Data.Mapping
{
    public class GrupoMabMap:ClassMap<GrupoMab>
    {
        public GrupoMabMap()
        {

            Id(x => x.Id, "ID_SEQ_GRUPO_MAB").GeneratedBy.Native("SEQ_DO_GRUPOS_MAB");
            Map(x => x.NumeroGrupo, "NUMERO_GRUPO").Length(5).Not.Nullable();
            
            References<EjecucionMab>(x => x.Enpt, "ID_SEQ_EJECUCION_MAB_EN_PT").Not.Nullable().Cascade.SaveUpdate();
            References<EjecucionMab>(x => x.EnPTanterior, "ID_SEQ_EJEC_MAB_EN_PT_ANTERIOR").Not.Nullable().Cascade.SaveUpdate();

            Map(x => x.GeneraPtp, "GENERA_PTP").CustomType<YesNoType>().Length(1);
            Map(x => x.TipoGrupo, "ID_SEQ_TIPOS_GRUPO").CustomType(typeof(TipoGrupoMabEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);


            Table("T_DO_GRUPOS_MAB");
            Cache.ReadWrite();
        }


    }
}
       