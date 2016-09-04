using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoEscuelaPorTipoNivelEducativo : IDao<EscuelaPorTipoNivelEducativo,int>
    {
        //List<EscuelaPorTipoNivelEducativo> GetByDireccionDeNivelCodigo(string codigoDireccionDeNivel);
        //List<EscuelaPorTipoNivelEducativo> GetByNivelEducativoPorTipoEducacion(int idNivelEducativoPorTipoEducacion);
        EscuelaPorTipoNivelEducativo GetByDireccionDeNivel(int idDN, int idNivelEduc, TipoEducacionEnum tipoEducacion);
        TipoEducacionEnum GetTipoEducacionByEscuelaNivelEducativo(int idEscuela, int idNivel);
    }
}
