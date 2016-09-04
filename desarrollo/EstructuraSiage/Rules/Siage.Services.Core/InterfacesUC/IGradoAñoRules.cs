using System.Collections.Generic;
using Siage.Base.Dto;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IGradoAñoRules
    {
        List<GradoAñoModel> GetGradoAñoPorEscuelaLogueadaId(int idEscuela);
        GradoAñoModel GetGradoAñoById(int idGradoAño);
        List<GradoAñoModel> GetGradoAñoByEscuela(int idEscuela);        
        List<GradoAñoModel> GetGradoAñoPorEscuelaLogueada();
        List<GradoAñoModel> GetAllGradoAñoPorNivelEducativoDeEscuela(int idEscuela);
        List<DtoConsultaGradoAnio> GetAllDtoGradoAñoPorNivelEducativoDeEscuela(int idEscuela);//DTO PARA COMBO

        List<GradoAñoModel> GetGradoAñoByFiltros(int nivelEducativoId, TipoEducacionEnum tipoEducacion, int cicloEducativoId);
        List<GradoAñoModel> GetAllGradoAñoByNivelEducativoTipoEducacion(int idNivel, TipoEducacionEnum idTipoEducacion);
        List<GradoAñoModel> GetGradoAñoByDN(int id);
        List<GradoAñoModel> GetGradoAñoByNivelEducativo(int idNivel);
        List<GradoAñoModel> GetGradoAnioByTurnoCarreraEscuela(int idTurno, int? idCarrera, int escuelaId);
    }
}