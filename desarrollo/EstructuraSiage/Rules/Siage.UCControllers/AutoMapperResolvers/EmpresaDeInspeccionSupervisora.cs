using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Siage.Core.Domain;
using Siage.Data.DAO;
using Siage.Services.Core.Models;

namespace Siage.UCControllers.AutoMapperResolvers
{
    public class EmpresaDeInspeccionSupervisora : ValueResolver<InspeccionModel, EmpresaBase>
    {
        protected override EmpresaBase ResolveCore(InspeccionModel source)
        {
            var inspeccion = new DaoProvider().GetDaoInspeccion().GetById(source.Id);
            if (inspeccion != null)
                return inspeccion.EmpresaDeInspeccionSuperior;
            else
                return null;
        }
    }
}
