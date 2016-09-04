using System;
using Siage.Core.Domain;
using Siage.Base;

namespace Siage.Data.MockEntities
{
    public class Empresas
    {
        public static EmpresaBase MinisterioActivo
        {
            get
            {
                return new EmpresaBase
                {
                    Id = 1,
                    Nombre = "EMPRESA1",
                    CodigoEmpresa = "E1",
                    FechaAlta = new DateTime(2011, 1, 1),
                    FechaInicioActividad = new DateTime(2011, 1, 2),
                    TipoEmpresa = TipoEmpresaEnum.MINISTERIO,
                    //EstadoEmpresa =   EstadoEmpresaEnum.ACTIVA
                };
            }
        }

        public static EmpresaBase Secretaria1
        {
            get
            {
                return new EmpresaBase
                {
                    Id = 2,
                    Nombre = "Secretaria1",
                    CodigoEmpresa = "S1",
                    FechaAlta = new DateTime(2011, 1, 1),
                    FechaInicioActividad = new DateTime(2011, 1, 2),
                    TipoEmpresa = TipoEmpresaEnum.SECRETARIA,
                    //EstadoEmpresa = EstadoEmpresaEnum.ACTIVA
                };
            }
        }

        public static EmpresaBase Madre
        {
            get
            {
                return new EmpresaBase
                {
                    Id = 3,
                    Nombre = "Escuela Madre",
                    CodigoEmpresa = "S1",
                    FechaAlta = new DateTime(2011, 1, 1),
                    FechaInicioActividad = new DateTime(2011, 1, 2),
                    TipoEmpresa = TipoEmpresaEnum.ESCUELA_MADRE,
                    //EstadoEmpresa = EstadoEmpresaEnum.ACTIVA
                };
            }
        }

        public static EmpresaBase Anexo
        {
            get
            {
                return new EmpresaBase
                {
                    Id = 4,
                    Nombre = "Escuela Anexo",
                    CodigoEmpresa = "S1",
                    FechaAlta = new DateTime(2011, 1, 1),
                    FechaInicioActividad = new DateTime(2011, 1, 2),
                    TipoEmpresa = TipoEmpresaEnum.ESCUELA_ANEXO,
                    //EstadoEmpresa = EstadoEmpresaEnum.ACTIVA
                };
            }
        }

        public static EmpresaBase Secretaria3
        {
            get
            {
                return new EmpresaBase
                {
                    Id = 5,
                    Nombre = "Secretaria3",
                    CodigoEmpresa = "S2",
                    FechaAlta = new DateTime(2011, 1, 1),
                    FechaInicioActividad = new DateTime(2011, 1, 2),
                    TipoEmpresa = TipoEmpresaEnum.SECRETARIA,
                    //EstadoEmpresa = EstadoEmpresaEnum.ACTIVA
                };
            }
        }
    }
}

