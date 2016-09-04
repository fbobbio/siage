using Siage.Base.Dto;
using Siage.Core.Domain;

namespace Siage.Core.DomainServiceInterfaces
{
    public interface IContextProvider
    {
        DtoUsuario GetCurrentUser();
        Usuario GetCurrentUserDomain();
        void SetData(string key, object data);
        object GetData(string key);
    }
}