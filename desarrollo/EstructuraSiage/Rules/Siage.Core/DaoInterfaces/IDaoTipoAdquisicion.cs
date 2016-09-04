using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoTipoAdquisicion : IDao<TipoAdquisicion, int>
    {
        List<TipoAdquisicion> GetByFiltros(bool dadosBaja);

        TipoAdquisicion GetTipoAdquisicionByIdContrato(int idContrato);
    }
}

