using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoSistemaCuantitativo : IDao<SistemaCuantitativo, int>
    {
        SistemaCuantitativo GetSistemaNotaByCodigoAsignatura(int idCodigoAsignatura);
        List<SistemaCuantitativo> GetByFiltros(string filtroNombre, int? nivelEducativo, int? cicloEducativo);
    }
}
