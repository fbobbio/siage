using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping; 
using NHibernate;using Siage.Core.Domain;
using Siage.Data.CustomTypes;
using Siage.Base;

namespace Siage.Data.Mapping
{
    public class EmpresaExternaMap : ClassMap<EmpresaExterna>
    {
       
        public EmpresaExternaMap()
        {
            Id(x => x.Id, "ID_SEQ_EMPRESA_EXTERNA").GeneratedBy.Native("SEQ_DO_EMP_EXTERNA");
            Map(x => x.Nombre, "N_EMPRESA_EXTERNA").Length(100).CustomType<UpperTrimString>();
            Map(x => x.Descripcion, "DESCRIPCION").Length(100).CustomType<UpperTrimString>();
            Map(x => x.Observaciones, "OBSERVACIONES").Length(100);
            Map(x => x.FechaBaja, "FEC_BAJA");
            Map(x => x.FechaAlta, "FEC_ALTA");
            Map(x => x.FechaCreacion, "FEC_CREACION");
            Map(x => x.FechaUltimaActivacion, "FEC_ULTIMA_ACTIVACION");
            Map(x => x.Activo, "ACTIVO").CustomType<YesNoType2>().Length(1);
            Map(x => x.MotivoBaja, "MOTIVO_BAJA").CustomType<UpperTrimString>();
            Map(x => x.Sucursal, "SUCURSAL").CustomType<UpperTrimString>();
            Map(x => x.NumeroAnses , "NRO_ANSES");
            Map(x => x.NumeroIngBrutos, "NRO_ING_BRUTOS");
            Map(x => x.TipoEmpresaExterna, "ID_SEQ_TIPO_EMPRESA_EXTERNA").Not.Nullable().CustomType(typeof(Siage.Base.TipoEmpresaExternaEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);
            References<CondicionIva>(x => x.CondicionIva, "CONDICION_IVA");
            References<PersonaJuridica>(x => x.PersonaJuridica  , "ID_SEQ_PERSONA_JURIDICA");
            References<PersonaFisica>(x => x.Referente, "ID_SEQ_PERSONA_FISICA");
            
            References<Domicilio>(x => x.Domicilio, "ID_VIN").ReadOnly();
            HasMany<Comunicacion>(x => x.Comunicaciones)
               .KeyColumn("ID_ENTIDAD")
                //.Where("\"TABLA_ORIGEN\"='" + System.Configuration.ConfigurationManager.AppSettings["Schema"] + ".T_ES_ESTUDIANTES' AND \"ID_APLICACION\"=" + System.Configuration.ConfigurationManager.AppSettings["AplicacionID"])
                //Por ahora lo dejamos asi hasta que Silvio consiga updatear los valores de la columna tabla origen en la base de datos con el nombre del schema
               .Where("\"TABLA_ORIGEN\"='T_DO_EMP_EXTERNA' AND \"ID_APLICACION\"=" + System.Configuration.ConfigurationManager.AppSettings["AplicacionID"])
               .Cascade.All()
               .Inverse();

            Table("T_DO_EMP_EXTERNA");
            Cache.ReadWrite();
           
        }

    }
}

