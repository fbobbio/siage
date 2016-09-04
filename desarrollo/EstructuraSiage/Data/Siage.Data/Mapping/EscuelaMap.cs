using System;
using FluentNHibernate.Mapping;
using Siage.Core.Domain;
using Siage.Base;
using Siage.Data.CustomTypes;

namespace Siage.Data.Mapping
{
    public class EscuelaMap : SubclassMap<Escuela>
    {
        public EscuelaMap()
        {
            DiscriminatorValue(Convert.ToInt32(TipoEmpresaEnum.ESCUELA_MADRE));
            References<Escuela>(x => x.EscuelaMadre, "ID_ESCUELA_MADRE");
            References<Escuela>(x => x.EscuelaRaiz, "ID_ESCUELA_RAIZ");
            Map(x => x.Dependencia, "ID_SEQ_DEPENDENCIA").CustomType(typeof(DependenciaEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);
            Map(x => x.TipoCooperadora, "ID_SEQ_TIPO_COOPERADORA").CustomType(typeof(TipoCooperadoraEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);
            Map(x => x.TipoCategoria, "ID_SEQ_CATEGORIA_ESCUELA").CustomType(typeof(CategoriaEscuelaEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);
            Map(x => x.Ambito, "ID_SEQ_AMBITO_ESCUELA").CustomType(typeof(AmbitoEscuelaEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);
            Map(x => x.NumeroEscuela, "NUMERO_ESCUELA").Length(5);
            Map(x => x.Religioso, "RELIGIOSO").CustomType<YesNoType>().Length(1);
            Map(x => x.Arancelado, "ARANCELADO").CustomType<YesNoType>().Length(1);
            Map(x => x.Albergue, "ALBERGE").CustomType<YesNoType>().Length(1);
            Map(x => x.CUE, "CUE").Length(10);
            Map(x => x.CUEAnexo, "CUE_ANEXO").Length(2);
            Map(x => x.NumeroAnexo, "NUMERO_ANEXO");
            Map(x => x.HorarioDeFuncionamiento, "HORARIO_FUNCIONAMIENTO").Length(50);
            Map(x => x.Colectivos, "COLECTIVOS").Length(50);
            Map(x => x.EstructuraDefinitiva, "ESTRUCTURA_DEFINITIVA").CustomType<YesNoType>().Length(1);
            Map(x => x.ContextoDeEncierro, "CONTEXTO_ENCIERRO").CustomType<YesNoType>().Length(1);
            Map(x => x.Hospitalaria, "HOSPITALARIA").CustomType<YesNoType>().Length(1);
            Map(x => x.EsRaiz, "RAIZ").CustomType<YesNoType>().Length(1);
            Map(x => x.Privado, "PRIVADO").Not.Nullable().CustomType<YesNoType>().Length(1);
            Map(x => x.CodigoInspeccion, "CODIGO_INSPECCION").Length(6);
            References<EscuelaPrivada>(x => x.EscuelaPrivada, "ID_SEQ_ESCUELA_PRIVADA").Cascade.SaveUpdate();
            References<TipoJornada>(x => x.TipoJornada, "ID_SEQ_TIPO_JORNADA");
            References<ZonaDesfavorable>(x => x.ZonaDesfavorable, "ID_SEQ_ZONA_DESFAVORABLE");
            HasManyToMany<NivelEducativoPorTipoEducacion >(x => x.NivelesEducativo)
                     .ChildKeyColumn("ID_SEQ_TEDUC_X_NIV_EDUCAT")
                     .ParentKeyColumn("ID_SEQ_EMPRESA")
                     .Table("T_EM_EMPR_X_TED_NIV")
                         .AsBag()
                         .Cache.ReadWrite();   

            References<TipoEscuela>(x => x.TipoEscuela, "ID_SEQ_TIPO_ESCUELA");
            References<ModalidadJornada>(x => x.ModalidadJornada, "ID_SEQ_MODALIDAD_JORNADA");
            HasMany<EscuelaPlan>(x => x.EscuelaPlan)
            .KeyColumn("ID_SEQ_EMPRESA")
            .AsBag()
            .Inverse()
            .Cascade.SaveUpdate()
            .Cache.ReadWrite();

            HasManyToMany<PeriodoLectivo>(x => x.PeriodosLectivo)
                     .ChildKeyColumn("ID_SEQ_PERIODO_LECTIVO")
                     .ParentKeyColumn("ID_SEQ_ESCUELA")
                     .Table("T_EM_PER_LECT_X_ESCUELA")
                         .AsBag()
                         .Cache.ReadWrite();

            HasMany<ActividadEspecial>(x => x.ActividadesEspeciales)
               .KeyColumn("ID_SEQ_ESCUELA")
               .Inverse()
               .AsBag()
               .Cascade.All()
               .Cache.ReadWrite();

            Table("T_EM_EMPRESAS");
        }
    }
}