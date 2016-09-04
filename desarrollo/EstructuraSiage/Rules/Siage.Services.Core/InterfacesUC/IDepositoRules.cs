using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IDepositoRules
    {
        DepositoModel GetByContrato(int idContrato);
    }
}