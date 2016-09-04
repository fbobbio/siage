using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHibernate;
using Siage.Core.Domain;
using Siage.Base;

namespace Siage.Data.Mapping
{
    public class EmpresaMap : SubclassMap<Empresa>
    {
         //TODO VANESA: CAMBIAR POR MAPEO A SP
        public EmpresaMap()
        {
            //Table("T_EM_EMPRESAS");

        }
    }
}
