using System;
using System.Collections.Generic;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ISistemaNotaRules
    {
        //SistemaNotaModel GetSistemaNotaById(int id);
        //SistemaNotaModel SistemaNotaSave(SistemaNotaModel modelo);
        //SistemaNotaModel SistemaNotaDelete(SistemaNotaModel modelo);
        List<SistemaNotaSeleccionGrupoModel> GetGruposParaSeleccion(int idEmpresaLogueada, int idNivelEducativo);
        List<SistemaNotaModel> GetSistemaNotaAll();

        void SistemaNotaSave(SistemaNotaModel model);
        SistemaNotaModel GetSistemaNotaByCodigoAsignatura(int idCodigoAsignatura);
        List<SistemaNotaModel> GetSistemaNotaByFiltros(string filtroNombre, string filtroTipo, string filtroNivelEducativo, string filtroCicloEducativo);

        SistemaNotaModel GetSistemaNotaById(int id);
        int GetCicloEducativoByIdSistemaNota(int id);
        void SistemaNotaDelete(SistemaNotaModel model);
    }
}
