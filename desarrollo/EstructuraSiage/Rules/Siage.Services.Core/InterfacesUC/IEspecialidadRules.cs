using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IEspecialidadRules
    {
        EspecialidadModel EspecialidadSave(EspecialidadModel modelo);
        EspecialidadModel EspecialidadUpdate(EspecialidadModel modelo);
        EspecialidadModel EspecialidadDelete(EspecialidadModel modelo);
        EspecialidadModel GetEspecialidadById(int idEspecialidad);
        List<EspecialidadModel> GetEspecialidadesByFiltros(int? idOrientacion, int? idSubOrientacion, bool dadosDeBaja);
        List<EspecialidadModel> GetEspecialidadAll();
        List<PlanEstudioModel> GetPlanesEstudioByIdEspecialidad(int idEspecialidad);
        bool EstaImplementadaEnPlan(int idEspecialidad);
        EspecialidadModel GetEspecialidadByDiagramacionCurso(int idDiagramacion);
    }
}