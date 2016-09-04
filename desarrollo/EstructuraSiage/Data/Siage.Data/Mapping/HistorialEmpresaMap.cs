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
    public class HistorialEmpresaMap : ClassMap<HistorialEmpresa >
    {

        public HistorialEmpresaMap()
        {
            Id(x => x.Id, "ID_SEQ_HIST_EMPRESA").GeneratedBy.Native("SEQ_EM_HIST_EMPRESA");
            
            Map(x => x.FechaModificacion , "FEC_MODIFICACION").Not.Nullable();
            Map(x => x.Nombre, "N_HIST_EMPRESA").Length(200).CustomType<UpperTrimString>();
            Map(x => x.Telefono , "TELEFONO").Length(10);
            Map(x => x.Observaciones, "OBSERVACIONES").Length(200);
            Map(x => x.FechaInicioActividades, "FEC_INICIO_ACTIVIDADES").Not.Nullable();
            Map(x => x.FechaNotificacion, "FEC_NOTIFICACION").Not.Nullable();
            Map(x => x.FechaDesde, "FEC_DESDE").Not.Nullable();
            Map(x => x.FechaHasta, "FEC_HASTA").Not.Nullable();
            References<Usuario>(x => x.UsuarioModificacion, "ID_SEQ_USUARIO").Not.Nullable();
            Map(x => x.VinculoDomicilio, "ID_VIN");
            References<EmpresaBase>(x => x.Empresa, "ID_SEQ_EMPRESA").Not.Nullable();
            References<EmpresaBase>(x => x.EmpresaPadreOrganigrama, "ID_EMPRESA_PADRE_ORGANIGRAMA").Not.Nullable();
            References<AsignacionInstrumentoLegal>(x => x.AsignacionInstrumentoLegal, "ID_SEQ_ASIGNACION_INSTR_LEG").Not.Nullable();
            References<VinculoEmpresaEdificio>(x => x.VinculoEmpresaEdificio, "ID_SEQ_VINCULO_EMPRESA_EDIF").Not.Nullable();

            Map(x => x.TipoEmpresa, "ID_SEQ_TIPO_EMPRESA").CustomType(typeof(Siage.Base.TipoEmpresaEnum)).ReadOnly().CustomSqlType(EnumType.CustomSqlTypeByEnum);
            DiscriminateSubClassesOnColumn<string>("ID_SEQ_TIPO_EMPRESA");

            Table("T_EM_HIST_EMPRESA");
            Cache.ReadWrite();

        }

    }
}

