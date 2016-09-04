using Siage.Base.Dto;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoUsuario : IDao<Usuario, int>
    {
        Usuario GetUsuarioByPersona(int personaId);
        DtoUsuario GetByNombreUsuario(string loginName);
        bool ExisteNombreUsuario(string nombre, int persona);
    }
}