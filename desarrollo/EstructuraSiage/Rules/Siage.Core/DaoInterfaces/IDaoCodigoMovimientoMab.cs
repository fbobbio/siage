using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoCodigoMovimientoMab : IDao<CodigoMovimientoMab, int>
    {
        
        List<CodigoMovimientoMab> GetCodigoMovimientoMabByTipoGrupo(TipoGrupoMabEnum tipoGrupoMabEnum);
        CodigoMovimientoMab GetCodigoMovimientoMabById(int codigoMovimientoMabId);
        List<CodigoMovimientoMab> GetByFiltros(string codigo, string descripcionCodigoMovimientoMab);
        bool TieneAsociadoGrupoMab(int codigoMovimientoMabId);
        bool ExisteCodigoMovimientoMab(string codigoMovimientoMab);
        bool EstaCodigoMovimientoMabAsignadoAGrupoMab(int idGrupoMab);
        List<CodigoMovimientoMab> GetAllCodigosMovientoMabSinGrupoMabAsignado();
            List<CodigoMovimientoMab> GetCodigosMovimientoByGrupoMab(int grupoMabId);
         List<CodigoMovimientoMab> GetCodigosMovimientoQueNoEstanEn(List<CodigoMovimientoMab> codigosMovimientoMab, int grupoMabId);
        int GetCantidadCodigoMovimientoMabPorGrupoMab(int grupoMabId);
    }
}
