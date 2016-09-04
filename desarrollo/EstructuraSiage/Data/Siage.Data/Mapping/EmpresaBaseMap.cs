/*
Juan Manuel Gonzalez: agregé acto administrativo (22/12/10)
 
 
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping; 
using NHibernate;using Siage.Core.Domain;
using Siage.Base;
using Siage.Data.CustomTypes;


namespace Siage.Data.Mapping
{
    public class EmpresaBaseMap : ClassMap<EmpresaBase>
    {
        
        public EmpresaBaseMap()
        {
            Id(x => x.Id, "ID_SEQ_EMPRESA").GeneratedBy.Native("SEQ_EM_EMPRESAS");
            References<EmpresaBase>(x => x.EmpresaRegistro, "ID_SEQ_EMPRESA_REGISTRO");
            References<EmpresaBase>(x => x.EmpresaPadreOrganigrama, "ID_SEQ_EMPRESA_PADRE");
            References<DireccionDeNivel>(x => x.TipoDireccionNivel, "ID_SEQ_DIRECCION_NIVEL");
            References<OrdenDePago>(x => x.OrdenDePago , "ID_SEQ_ORDEN_PAGO").Not.Nullable();
            References<ProgramaPresupuestario>(x => x.ProgramaPresupuestario , "ID_SEQ_PROGRAMA_PRESUPUESTARIO").Not.Nullable();
            References<Usuario>(x => x.UsuarioAlta, "ID_SEQ_USUARIO_ALTA").Not.Nullable();
            References<Usuario>(x => x.UsuarioModificacion , "ID_SEQ_USUARIO_MODIFICACION");
            Map(x => x.VinculoDomicilio, "ID_VIN");
            Map(x => x.CodigoEmpresa, "ID_EMPRESA").Not.Nullable().Length(20).CustomType<UpperTrimString>();
            Map(x => x.Nombre, "N_EMPRESA").Not.Nullable().Length(200).CustomType<UpperTrimString>();
            Map(x => x.FechaAlta , "FEC_ALTA").Not.Nullable();
            Map(x => x.FechaCierre, "FEC_CIERRE");
            Map(x => x.FechaNotificacion, "FEC_NOTIFICACION");
            Map(x => x.FechaInicioActividad , "FEC_INICIO_ACTIVIDAD");                                       
            Map(x => x.Observaciones  , "OBSERVACIONES").Length(200);
            Map(x => x.DireccionDeNivelCodigo, "ID_DIRECCION_NIVEL").Length(20).CustomType<UpperTrimString>();
            Map(x => x.EmpresaPadreCodigo, "ID_EMPRESA_PADRE").Length(20).CustomType<UpperTrimString>();
            Map(x => x.Estado, "ID_SEQ_ESTADO").CustomType(typeof(EstadoEmpresaEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);
            Map(x => x.FechaUltimaModificacion, "FEC_ULTIMA_MODIFICACION");
            HasMany<VinculoEmpresaEdificio>(x => x.VinculoEmpresaEdificio )
                .KeyColumn("ID_SEQ_EMPRESA")
                .AsBag()
                .Inverse()
                .Cascade.All()
                .Cache.ReadWrite();
            HasMany<HistorialEstadoEmpresa>(x => x.HistorialEstados)
                .KeyColumn("ID_SEQ_EMPRESA")
                .AsBag()
                .Inverse()
                .Cascade.SaveUpdate()
                .Cache.ReadWrite();

            HasMany<TurnoPorEscuela>(x => x.TurnosXEscuela)
             .KeyColumn("ID_SEQ_EMPRESA")
             .AsBag()
             .Inverse()
             .Cascade.All()
             .Cache.ReadWrite();

            HasMany<Comunicacion>(x => x.Comunicaciones)
               .KeyColumn("ID_ENTIDAD")
               .Where("\"TABLA_ORIGEN\"='T_EM_EMPRESAS' AND \"ID_APLICACION\"=" + System.Configuration.ConfigurationManager.AppSettings["AplicacionID"])
               .Cascade.All()
               .Inverse();

            Map(x => x.TipoEmpresa, "ID_SEQ_TIPO_EMPRESA").CustomType(typeof(Siage.Base.TipoEmpresaEnum)).ReadOnly().CustomSqlType(EnumType.CustomSqlTypeByEnum); 
            DiscriminateSubClassesOnColumn<string>("ID_SEQ_TIPO_EMPRESA");
            Table("T_EM_EMPRESAS");
            Cache.ReadWrite();
           
        }

    }
}

