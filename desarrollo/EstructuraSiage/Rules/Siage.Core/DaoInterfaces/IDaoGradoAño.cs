using Siage.Base.Dto;
using Siage.Core.Domain;
using System.Collections.Generic;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoGradoAño : IDao<GradoAnio, int>
    {
        List<GradoAnio>GetGradoanioByEscuelaPlan(int idEscuelaPlan);
        List<GradoAnio> GetGradosByFiltros(int nivelEducativoId, TipoEducacionEnum tipoEducacion, int cicloEducativoId);
        List<GradoAnio> GetGradosByFiltros(int cicloEducativoId);
        List<GradoAnio> GetGradoAñoByNivelEducativo(int idNivel);
        List<GradoAnio> GetGradoAñoPorEscuela(int escuelaId);
        GradoAnioPorTipoNivelEducativo GetGradoAnioPorTipoNivelEducativoByFiltros(int gradoAño, int nivelEducativo,
                                                                                  TipoEducacionEnum tipoEducacion);
        GradoAnioPorTipoNivelEducativo GetGradoAnioPorTipoNivelEducativoByFiltros(int idEmpresa, int gradoAño, int nivelEducativo);
        List<GradoAnio> GetGradoAñoByNivelEducativoTipoEducacion(int idNivel, TipoEducacionEnum idTipoEd);
        List<GradoAnio> GetGradoanioByPlan(int planEstudioId);
        List<GradoAnio> GetGradoAñoByDireccionDeNivel(int id);
        GradoAnioPorTipoNivelEducativo GetGradoAñoPorNivelEducativo(int gradoId, int cicloId);
        List<GradoAnio> GetGradoAnioAnteriores(int gradoId, int nivelId);
        List<GradoAnio> GetByTurnoCarreraEscuela(int idTurno, int? idCarrera, int escuelaId);

        
        List<DtoConsultaGradoAnio> GetDtoGradoAñoByNivelEducativoTipoEducacion(int idNivel,
                                                                               TipoEducacionEnum tipoEducacion);
    }


}