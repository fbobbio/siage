using System.Collections.Generic;
using Siage.Base.Dto;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoNivelEducativo : IDao<NivelEducativo, int>
    {
        List<NivelEducativo> GetNivelEducativoPorDireccionNivel(int direccionNivelId);
        NivelEducativo GetFiltroNombre(string nombre);
        int GetIdNivelEducativoByEscuela(int idEscuela);
        NivelEducativo GetNivelEducativoByGradoAnio(int idGradoAnio);
        DtoConsultaNivelEducativo GetDtoNivelEducativoByGradoAnio(int idGradoAnio);
    }
}
