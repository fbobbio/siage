using FluentNHibernate.Mapping;
using Siage.Core.Domain;
using Siage.Data.CustomTypes;

namespace Siage.Data.Mapping
{
    public class SubGrupoMap: ClassMap<SubGrupo>
    {
        public SubGrupoMap()
        {
            Id(x => x.Id, "ID_SEQ_SUB_GRUPO").GeneratedBy.Native("SEQ_PE_SUB_GRUPO");
            Map(x => x.TipoSubGrupo, "ID_SEQ_TSUB_GRUPO").Not.Nullable().CustomType(typeof(Siage.Base.TipoSubGrupoEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);
            Map(x => x.Nombre, "N_SUB_GRUPO").Not.Nullable().Length(50).CustomType<UpperTrimString>();
            Map(x => x.FechaBaja, "FEC_BAJA");

            HasManyToMany<Grupo>(x => x.Grupos)
            .ChildKeyColumn("ID_SEQ_GRUPO")
            .ParentKeyColumn("ID_SEQ_SUB_GRUPO")
            .Table("T_PE_GRUPOS_X_SUB_GRUPO")
                .AsBag()
                .Cascade.SaveUpdate()
                .Cache.ReadWrite();

            HasManyToMany<CodigoAsignatura>(x => x.CodigosDeAsignaturas)
           .ChildKeyColumn("ID_SEQ_CODIGO_ASIGNATURA")
           .ParentKeyColumn("ID_SEQ_SUB_GRUPO")
           .Table("T_PE_SGRUPO_X_CASIGNATURA")
               .AsBag()
               .Cascade.SaveUpdate()
               .Cache.ReadWrite();
           

            Table("T_PE_SUB_GRUPO");
            Cache.ReadWrite();

        }
    }
}
