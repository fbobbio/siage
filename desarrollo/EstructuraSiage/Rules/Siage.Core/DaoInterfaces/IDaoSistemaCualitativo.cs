using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoSistemaCualitativo : IDao<SistemaCualitativo, int>
    {
        SistemaCualitativo GetSistemaNotaByCodigoAsignatura(int idCodigoAsignatura);
        List<SistemaCualitativo> GetByFiltros(string filtroNombre, int? nivelEducativo, int? cicloEducativo);
    }
}
