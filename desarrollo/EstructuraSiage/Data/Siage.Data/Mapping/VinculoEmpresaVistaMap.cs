using FluentNHibernate.Mapping;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Data.CustomTypes;

namespace Siage.Data.Mapping
{
    public class VinculoEmpresaVistaMap : ClassMap<DtoVinculoEmpresaReporte>
    {
        public VinculoEmpresaVistaMap()
        {
            Id(x => x.IdVinculoEmpresa, "ID_EMPRESA");
            Map(x => x.IdEmpresa, "ID_VIN_EMPRESA");
            Map(x => x.IdentificadorEmpresa, "CODIGO_EMPRESA");
            Map(x => x.NombreEmpresa, "NOMBRE_EMPRESA");
            Map(x => x.IdDeptoProvincialEmpresa, "ID_DEPARTAMENTO_EMPRESA");
            Map(x => x.DeptoProvincialEmpresa, "DEPARTAMENTO_EMPRESA");
            Map(x => x.IdLocalidadEmpresa, "ID_LOCALIDAD_EMPRESA");
            Map(x => x.LocalidadEmpresa, "LOCALIDAD_EMPRESA");
            Map(x => x.IdBarrioEmpresa, "ID_BARRIO_EMPRESA");
            Map(x => x.BarrioEmpresa, "BARRIO_EMPRESA");
            Map(x => x.IdCalleEmpresa, "ID_CALLE_EMPRESA");
            Map(x => x.CalleEmpresa, "CALLE_EMPRESA");
            Map(x => x.AlturaEmpresa, "ALTURA_EMPRESA");

            Map(x => x.IdEdificio, "ID_EDIFICIO");
            Map(x => x.IdentificadorEdificio, "IDENTIFICADOR_EDIFICIO");
            Map(x => x.IdTipoEdificio, "ID_TIPO_EDIFICIO");
            Map(x => x.TipoEdificio, "NOMBRE_TIPO_EDIFICIO");
            Map(x => x.IdFuncionEdificio, "ID_FUNCION_EDIFICIO");
            Map(x => x.FuncionEdificio, "NOMBRE_FUNCION_EDIFICIO");
            Map(x => x.SuperficieTotalEdificio, "SUPERFICIE_TOTAL");
            Map(x => x.IdDeptoProvincialEdificio, "ID_DEPARTAMENTO_EDIFICIO");
            Map(x => x.DeptoProvincialEdificio, "DEPARTAMENTO_EDIFICIO");
            Map(x => x.IdLocalidadEdificio, "ID_LOCALIDAD_EDIFICIO");
            Map(x => x.LocalidadEdificio, "LOCALIDAD_EDIFICIO");
            Map(x => x.IdBarrioEdificio, "ID_BARRIO_EDIFICIO");
            Map(x => x.BarrioEdificio, "BARRIO_EDIFICIO");
            Map(x => x.IdCalleEdificio, "ID_CALLE_EDIFICIO");
            Map(x => x.CalleEdificio, "CALLE_EDIFICIO");
            Map(x => x.AlturaEdificio, "ALTURA_EDIFICIO");

            Table("VT_VINCULOS_ACTIVOS_EMPRESA");
            
            Cache.ReadWrite();
        }
    }
}
