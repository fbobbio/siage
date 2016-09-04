using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoNivelEducativoPorTipoEducacion:IDao<NivelEducativoPorTipoEducacion,int>
    {
        NivelEducativoPorTipoEducacion GetByIdNivelEducativoYTipoEducacion(int idNivelEducativo, TipoEducacionEnum tipoEducacion);
        List<NivelEducativoPorTipoEducacion> GetByDireccionDeNivel(int idDireccionDeNivel);
        List<NivelEducativoPorTipoEducacion> GetByNivelEducativoId(int idNivelEducativo);
        List<NivelEducativoPorTipoEducacion> GetByTipoEducacionId(TipoEducacionEnum tipoEducacion);
    }
}
