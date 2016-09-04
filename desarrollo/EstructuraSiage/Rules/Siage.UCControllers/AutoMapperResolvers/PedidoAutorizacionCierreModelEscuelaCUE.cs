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
    public class PedidoAutorizacionCierreModelEscuelaCUE : ValueResolver<PedidoAutorizacionCierre, string>
    {
        protected override string ResolveCore(PedidoAutorizacionCierre source)
        {
            var escuela = source.Empresa as EmpresaBase;
            if (escuela.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
                return new DaoEscuela().GetById(escuela.Id).CUE;
            if (escuela.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
                return new DaoEscuelaAnexo().GetById(escuela.Id).CUE;
            return string.Empty;
        }
    }
}
