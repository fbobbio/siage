using System;
using System.Collections.Generic;
using Siage.Core.Domain;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ISancionRules
    {
        bool ValidarInscripcionAlumno(int idEstudiante, int idEscuela);
        int GetCantidadAmonestacionesByEstudiante(int idEstudiante);
        void SancionDelete(SancionModel entidad);
        DateTime GetFechaFinCicloLectivo(int idEscuela);
        SancionModel SancionUpdate(SancionModel modelo);
        SancionModel GetSancionById(int id);
        SancionModel SancionSave(SancionModel entidad);
        List<SancionMostrarModel> GetSancionesByFiltros(int? gradoAñoId, DivisionEnum? divisionId, int? cicloLectivoId, string numeroDocumento, SexoEnum? sexoId, string nombre, string apellido, int? tipoSancionId, DateTime? fechaDesde, DateTime? fechaHasta);
        List<TipoSancionModel> GetTipoSancionByNivelEducativoEscuela(int idEscuela);
        List<SancionModel> GetSancionesByInscripcionId(int id);
        Sancion CrearSancionParaClaustro(SancionModel model);
        Sancion UpdateSancionParaClaustro(SancionModel model);
    }
}