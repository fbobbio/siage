using System.Collections.Generic;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.Domain;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IUsuarioRules
    {
        Usuario GetCurrentUserDomain { get; }
        DtoUsuario GetCurrentUser();
        AgenteModel GetCurrentAgente();
        DtoUsuario IniciarSesion(string usuario, string clave);
        void ConfigurarRolUsuarioActual(int idRol);
        void UsuarioSave(UsuarioRegistrarModel model);
        bool ValidarPermiso();
        bool TieneMasDeUnRol();
        IList<DtoRolPorUsuario> GetRolesByAplicacion();
        UsuarioModel ValidarExistencia(int id, TipoRolUsuarioEnum tipo);
    }
}
