using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHibernate;
using Siage.Core.Domain;
using Siage.Data.CustomTypes;
using Siage.Base;

namespace Siage.Data.Mapping
{
    public class EscuelaPorTipoNivelEducativoMap : ClassMap<EscuelaPorTipoNivelEducativo>
    {

        public EscuelaPorTipoNivelEducativoMap()
        {
            CompositeId()
                .KeyProperty(x => x.EscuelaId, "ID_SEQ_EMPRESA")
                .KeyReference(x => x.NivelEducativoPorTipoEducacion, "ID_SEQ_TEDUC_X_NIV_EDUCAT")
                //.KeyProperty(x => x.TipoEducacionId, "ID_SEQ_TIPO_EDUCACION")
                ;
            //Id(x => x.Id, "ID_SEQ_EMPR_X_TED_NIV").GeneratedBy.Native("SEQ_PE_EMPR_X_TED_NIV");

            //References<Escuela>(x => x.Escuela, "ID_SEQ_EMPRESA").Not.Nullable();
            //References<NivelEducativo>(x => x.NivelEducativo, "ID_SEQ_NIVEL_EDUCATIVO").Not.Nullable();
            //Map(x => x.TipoEducacion , "ID_SEQ_TIPO_EDUCACION").CustomType(typeof(TipoEducacionEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);

            
            Table("T_EM_EMPR_X_TED_NIV");
            Cache.ReadWrite();
         
        }

    }
}
