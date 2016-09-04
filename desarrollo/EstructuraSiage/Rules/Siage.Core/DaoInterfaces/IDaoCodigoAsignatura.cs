using Siage.Base.Dto;
using Siage.Core.Domain;
using System.Collections.Generic;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoCodigoAsignatura : IDao<CodigoAsignatura, int>
    {
        List<DtoGrupoSubGrupoAsignatura> GetCodigosByFiltros(int? filtroGrupoId, int? filtroSubgrupoId, string filtroAsignatura, int? asignaturaId);
        List<CodigoAsignatura> GetCodigosByfiltros(int idgrupo, int idsubgrupo);
        List<CodigoAsignatura> GetCodigoByFiltros(int grupoId, int asignaturaId);
        List<CodigoAsignatura> GetCodigoByFiltros(int grupoId, int asignaturaId, string nombreGrupo, int? subGrupoId, string cicloEducativoAbrev);
        List<CodigoAsignatura> GetCodigoByFiltros(int grupoId, int asignaturaId, string nombreGrupo, int idCicloEducativo);
        List<CodigoAsignatura> GetCodigoByFiltrosAsignatura(int? grupoId, int? subgrupoId, string nombreAsignatura, int? asignaturaId);
        List<CodigoAsignatura> GetByFiltros(int? idAsignatura, int? idSubGrupo);
        List<CodigoAsignatura> GetByFiltro(int subGrupoId, int cicloId);
        List<CodigoAsignatura> GetByIdAsignatura(int? id);
        List<CodigoAsignatura> GetBySubgrupoId(int subGrupoId);
        List<CodigoAsignatura> DiagramacionCursoConAsigEspecial(int diagramacionCurso);
        List<CodigoAsignatura> GetCodigoAsignaturaByFiltros(int asignaturaId, int cicloEducativoId, bool dadosDeBaja);

        CodigoAsignatura ValidarExistenciaEnGrupo(int codigoId, int asignaturaId);
        CodigoAsignatura GetByDatosAsignatura(string nombre, string descripcion, string codigoJunta);
        CodigoAsignatura GetByCodigoAsignatura(string codigoAsignatura);

        bool ValidarExistenciaNombreAsignatura(string nombreAsignatura, int asignaturaId);

        bool ValidarExistenciaEnPlan(int idCodigoAsignatura);
    }
}