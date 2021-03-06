﻿using System;
using FluentNHibernate.Mapping;
using Siage.Base.Dto;
using Siage.Core.Domain;
using Siage.Base;
using Siage.Data.CustomTypes;

namespace Siage.Data.Mapping
{
    public class EscuelaAnexoVistaMap : ClassMap<DtoEscuelaAnexoReporte>
    {
        public EscuelaAnexoVistaMap()
        {
            Id(x => x.Id, "ID_ESCUELA");
            Map(x => x.CUE, "CUE").Length(10);
            Map(x => x.CUEAnexo, "CUE_ANEXO");
            Map(x => x.CodigoEmpresa, "ID_EMPRESA");
            Map(x => x.NombreEmpresa, "N_EMPRESA");
            Map(x => x.FechaInicioActividad, "FEC_INICIO_ACTIVIDAD");
            //Map(x => x.Estado, "ID_ESTADO").CustomType(typeof(Siage.Base.EstadoEmpresaEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);

            //AGREGAR EL ID_CATEGORIA (EL NOMBRE NO ES NECESARIO SE SACA DE LA ENUMERACION)
            //Map(x => x.TipoCategoria, "N_CATEGORIA_ESCUELA").CustomType(typeof(Siage.Base.CategoriaEscuelaEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);
            Map(x => x.TipoCategoria, "N_CATEGORIA_ESCUELA");

            //Map(x => x.DescripcionProgPresupuestario, "DESCRIPCION_PROGRAMA");
            //Map(x => x.TipoEducacion, "ID_TIPO_EDUCACION").CustomType(typeof(Siage.Base.TipoEducacionEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);
            //Map(x => x.NivelEducativo, "NOMBRE_NIVEL_EDUCATIVO");
            //Map(x => x.IdNivelEducativo, "ID_NIVEL_EDUCATIVO");
            
            //AGREGAR EL ID_AMBITO (EL NOMBRE NO ES NECESARIO SE SACA DE LA ENUMERACION)
            //Map(x => x.Ambito, "N_AMBITO_ESCUELA").CustomType(typeof(Siage.Base.AmbitoEscuelaEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);
            Map(x => x.Ambito, "N_AMBITO_ESCUELA");

            //Map(x => x.Dependencia, "ID_DEPENDENCIA").CustomType(typeof(Siage.Base.DependenciaEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);
            //Map(x => x.IdZonaDesfavorable, "ID_ZONA_DESFAVORABLE");
            Map(x => x.NombreZonaDesfavorable, "N_TIPO_ZONA_DESFAVORABLE");
            Map(x => x.CodigoOrdenPago, "ID_ORDEN_PAGO");
            Map(x => x.CodigoProgPresupuestario, "ID_PROGRAMA");
            Map(x => x.Privado, "PRIVADO").CustomType<YesNoType>().Length(1);
            //Map(x => x.Telefono, "TELEFONO");
            //Map(x => x.CodigoEscuelaMadre, "");
            //Map(x => x.NombreEscuelaMadre, "");


            //Map(x => x.DeptoProvincialEmpresa, "DEPARTAMENTO");
            //Map(x => x.IdDeptoProvincialEmpresa, "ID_DEPARTAMENTO");
            //Map(x => x.LocalidadEmpresa, "LOCALIDAD");
            //Map(x => x.IdLocalidadEmpresa, "ID_LOCALIDAD");
            //Map(x => x.BarrioEmpresa, "BARRIO");
            //Map(x => x.CalleEmpresa, "CALLE");
            //Map(x => x.AlturaEmpresa, "ALTURA");
            
            Table("VT_ESCUELA_ANEXO");
        }
    }
}