using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Siage.Base;
using Siage.Core.Domain;
using Siage.Data.DAO;

namespace Siage.UCControllers.AutoMapperResolvers
{
    public class EmpresaVisadoCierreModelEstadoPedido : ValueResolver<PedidoAutorizacionCierre, EstadoPedidoCierreEnum>
    {
        protected override EstadoPedidoCierreEnum ResolveCore(PedidoAutorizacionCierre source)
        {
            return source.Estados.Last().Estado;
        }
    }

    public class EmpresaVisadoCierreModelNivelEducativo : ValueResolver<PedidoAutorizacionCierre, string>
    {
        protected override string ResolveCore(PedidoAutorizacionCierre source)
        {
            var empresa = source.Empresa as EmpresaBase;
            if (empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
                return new DaoEscuela().GetById(empresa.Id).NivelEducativo.Nombre;
            else if(empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
                return new DaoEscuelaAnexo().GetById(empresa.Id).NivelEducativo.Nombre;
            else if (empresa.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL)
                return new DaoDireccionDeNivel().GetById(empresa.Id).NivelesEducativos.FirstOrDefault().Nombre;
            return string.Empty;
        }
    }

    public class EmpresaVisadoCierreModelTipoEducacion : ValueResolver<PedidoAutorizacionCierre, string>
    {
        protected override string ResolveCore(PedidoAutorizacionCierre source)
        {
            var empresa = source.Empresa as EmpresaBase;
            if (empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
                return new DaoEscuela().GetById(empresa.Id).TipoEducacion.ToString();
            else if(empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
                return new DaoEscuelaAnexo().GetById(empresa.Id).TipoEducacion.ToString();
            if (empresa.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL)
                return new DaoDireccionDeNivel().GetById(empresa.Id).TipoEducacion.ToString();
            return string.Empty;
        }
    }
}
