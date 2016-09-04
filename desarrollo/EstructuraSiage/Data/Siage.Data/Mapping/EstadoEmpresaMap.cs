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
    public class EstadoEmpresaMap : ClassMap<EstadoEmpresa>
    {
        public EstadoEmpresaMap()
        {

            Id(x => x.Id, "ID_SEQ_ESTADO_EMPRESA").GeneratedBy.Native("SEQ_EM_EEMPRESA");

            Map(x => x.Estado, "ID_SEQ_ESTADO").CustomType(typeof(Siage.Base.EstadoEmpresaEnum)).CustomSqlType(EnumType.CustomSqlTypeByEnum);
            Map(x => x.FechaModificacion , "FEC_MODIFICACION").Not.Nullable();

            References<Usuario>(x => x.UsuarioModificacion, "ID_SEQ_USUARIO_MODIFICACION").Not.Nullable();
			References<EmpresaBase>(x => x.Empresa, "ID_SEQ_EMPRESA").Not.Nullable();
            Table("T_EM_EEMPRESA");
            Cache.ReadWrite();
          
        }
    }
}
