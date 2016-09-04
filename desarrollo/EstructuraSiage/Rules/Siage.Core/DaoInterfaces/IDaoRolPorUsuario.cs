using System.Collections.Generic;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoRolPorUsuario : IDao<RolPorUsuario, int>
    {        
        IList<DtoRolPorUsuario> GetRolesByAplicacion(int usuarioId, PortalEnum portal);
        DtoRolPorUsuario GetDtoById(int idRol);
        int CantidadRolPorAplicacion(int usuarioId, PortalEnum portal);
    }
}