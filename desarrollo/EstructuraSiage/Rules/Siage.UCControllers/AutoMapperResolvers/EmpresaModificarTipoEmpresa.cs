using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.UCControllers.AutoMapperResolvers
{
    public class EmpresaModificarTipoEmpresaIdRaiz : ValueResolver<Escuela, int?>
    {
        protected override int? ResolveCore(Escuela source)
        {
            if (source.EscuelaRaiz != null)
                return source.EscuelaRaiz.Id;
            else
                return null;
        }
    }
    public class EmpresaModificarTipoEmpresaIdMadre : ValueResolver<Escuela, int?>
    {
        protected override int? ResolveCore(Escuela source)
        {
            if (source.EscuelaMadre != null)
                return source.EscuelaMadre.Id;
            else
                return null;
        }
    }
}
