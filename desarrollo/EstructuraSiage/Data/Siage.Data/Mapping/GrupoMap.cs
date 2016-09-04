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
    public class GrupoMap : ClassMap<Grupo>
    {
        public GrupoMap()
        {
            Id(x => x.Id, "ID_SEQ_GRUPO").GeneratedBy.Native("SEQ_PE_GRUPOS");
            Map(x => x.FechaBaja, "FEC_BAJA");
            Map(x => x.Nombre, "N_GRUPO").Not.Nullable().Length(50).CustomType<UpperTrimString>();
            References<CicloEducativo>(x => x.CicloEducativo, "ID_SEQ_CICLO_EDUCATIVO");

            HasManyToMany<SubGrupo>(x => x.SubGrupos)
            .ChildKeyColumn("ID_SEQ_SUB_GRUPO")
            .ParentKeyColumn("ID_SEQ_GRUPO")
            .Table("T_PE_GRUPOS_X_SUB_GRUPO")
                .AsBag()
                .Cascade.All()
                .Cache.ReadWrite();
              

            Table("T_PE_GRUPOS");
            Cache.ReadWrite();

        }

    }
}
