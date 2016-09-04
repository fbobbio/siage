using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.UCControllers.AutoMapperResolvers
{
    public class EmpresaExternaResolver : ValueResolver<EmpresaExterna, MotivoBajaEnum?>
    {
        protected override MotivoBajaEnum? ResolveCore(EmpresaExterna source)
        {
            return string.IsNullOrEmpty(source.MotivoBaja)
                       ? (MotivoBajaEnum?) null
                       : (MotivoBajaEnum) Enum.Parse(typeof (MotivoBajaEnum), source.MotivoBaja);
        }
    }
}
