using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Siage.Base;
using Siage.Core.Domain;
using Siage.Data.DAO;
using Siage.Services.Core.Models;

namespace Siage.UCControllers.AutoMapperResolvers
{
    public class AsignacionInspeccionEscuelaResolver : ValueResolver<EmpresaBase, List<AsignacionInspeccionEscuelaModel>>
    {
        protected override List<AsignacionInspeccionEscuelaModel> ResolveCore(EmpresaBase source)
        {
            var daoProvider = new DaoProvider();
            var listadoAsignaciones = new List<AsignacionInspeccionEscuela>();
            switch (source.TipoEmpresa)
            {
                case TipoEmpresaEnum.INSPECCION:
                    listadoAsignaciones =
                        daoProvider.GetDaoAsignacionInspeccionEscuela().GetVigentesByInspeccion(source.Id);
                    break;
                case TipoEmpresaEnum.ESCUELA_MADRE:
                case TipoEmpresaEnum.ESCUELA_ANEXO:
                    var asignacionInspeccionEscuela =
                        daoProvider.GetDaoAsignacionInspeccionEscuela().GetVigenteByEscuela(source.Id);
                    if (asignacionInspeccionEscuela != null)
                        listadoAsignaciones.Add(asignacionInspeccionEscuela);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Mapper.Map<List<AsignacionInspeccionEscuela>,
                List<AsignacionInspeccionEscuelaModel>>(listadoAsignaciones);
        }
    }

    public class EmpresaInspeccionNombreResolver : ValueResolver<EmpresaBase, string>
    {
        protected override string ResolveCore(EmpresaBase source)
        {
            var daoProvider = new DaoProvider();
            var asignacionInspeccionEscuela = daoProvider.GetDaoAsignacionInspeccionEscuela().GetVigenteByEscuela(source.Id);

            return asignacionInspeccionEscuela != null && asignacionInspeccionEscuela.Inspeccion != null
                       ? asignacionInspeccionEscuela.Inspeccion.Nombre
                       : string.Empty;
        }
    }

    public class EmpresaInspeccionCodResolver : ValueResolver<EmpresaBase, string>
    {
        protected override string ResolveCore(EmpresaBase source)
        {
            var daoProvider = new DaoProvider();
            var asignacionInspeccionEscuela = daoProvider.GetDaoAsignacionInspeccionEscuela().GetVigenteByEscuela(source.Id);
          
            return asignacionInspeccionEscuela != null && asignacionInspeccionEscuela.Inspeccion != null
                       ? asignacionInspeccionEscuela.Inspeccion.CodigoEmpresa
                       : string.Empty;
        }
    }
}
