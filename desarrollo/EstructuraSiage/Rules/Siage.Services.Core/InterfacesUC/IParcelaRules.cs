using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IParcelaRules
    {
        List<ParcelaModel> GetParcelaByFiltros(string nroCatastral, string nroRentas, string nroMensura);
        List<ParcelaModel> GetParcelaByFiltros(int? id,string nroCatastral, string nroRentas, string nroMensura, bool activo);
        List<ParcelaModel> GetParcelaByPredio(int idPredio);
        ParcelaModel ParcelaDelete(ParcelaModel modelo);
        ParcelaModel GetParcelaById(int id);
        ParcelaModel ParcelaSave(ParcelaModel modelo);
        ParcelaModel ParcelaUpdate(ParcelaModel modelo);
        ParcelaModel GetParcelaByContrato(int idContrato);
        ParcelaModel ReactivarParcela(ParcelaModel modelo);
        void ValidarParcelaDelete(ParcelaModel modelo);
        bool VerificarContratoVigente(int idParcela);
        bool VerificarParcelaActiva(int idParcela);
    }
}