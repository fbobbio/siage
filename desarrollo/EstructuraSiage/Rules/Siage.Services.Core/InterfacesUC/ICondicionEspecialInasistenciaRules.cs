using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ICondicionEspecialInasistenciaRules
    {
        List<InscripcionModel> GetInscripcionesByFiltros(DateTime? fechaDesde, DateTime? fechaHasta, TipoCondicionInasistenciaEnum? condicion, string nroDocumento, int? sexo, int? turno, int? gradoAnio, DivisionEnum? division, int escuela);
        List<CondicionEspecialInasistenciaModel> GetCondicionEspecialInasistenciaByInscripcionId(int idInscripcion);
        CondicionEspecialInasistenciaModel CondicionEspecialInasistenciaDelete(CondicionEspecialInasistenciaModel modelo);
        CondicionEspecialInasistenciaModel CondicionEspecialInasistenciaSave(CondicionEspecialInasistenciaModel modelo);
        CondicionEspecialInasistenciaModel GetCondicionEspecialInasistenciaById(int id);
    }
}
