using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoTipoEstructuraEdilicia : IDao<TipoEstructuraEdilicia, int>
    {
        List<TipoEstructuraEdilicia> GetByFiltros();

        List<TipoEstructuraEdilicia> GetByFiltros(int? idTipoEstructura, string nombre, string descripcion, TipoEdificioEnum? tipoEdificio, bool? dadosDeBaja);

        List<TipoEstructuraEdilicia> ValidarTipoEstructuraExistente(int? idTipoEstructura, string nombre, bool? dadosDeBaja);
    }
}

