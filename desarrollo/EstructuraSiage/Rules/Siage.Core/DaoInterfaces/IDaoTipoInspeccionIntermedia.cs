using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoTipoInspeccionIntermedia : IDao<TipoInspeccionIntermedia, int>
    {
        List<TipoInspeccionIntermedia> GetByFiltros(int? idTipoDireccionNivel, int? idTipoInspeccionIntermedia, string nombreInspeccionIntermedia, int? idTipoInspeccion, string nombreTipoInspeccion, bool vigentes);

        bool NombreRepetido(string nombreInspeccionIntermedia, int idTipoDireccionNivel, int id);
        bool ValidarInspeccionesByTipoInspeccionIntermedia(int idTipoInspeccionIntermedia);
        List<TipoInspeccionIntermedia> GetByDireccionDeNivel(int direccionDeNivelId);
    }
}

