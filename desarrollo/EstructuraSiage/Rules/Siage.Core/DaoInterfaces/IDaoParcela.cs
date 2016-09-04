using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoParcela : IDao<Parcela, int>
    {
        List<Parcela> GetByFiltros(int? id, string nroCatastral, string nroRentas, string nroMensura, bool activo);
        List<Parcela> GetByFiltros(string nroCatastral, string nroRentas, string nroMensura);
        List<Parcela> GetParcelaByPredio(int idPredio);
        Parcela GetParcelaByContrato(int idContrato);
        
        bool GetParcelaDarBaja(Parcela parcela);
        bool GetParcelaReactivar(Parcela parcela);
        bool VerificarExistenciaParcela(string nroCatastral, string nroRentas, string nroMensura);
        string GetUltimoIdentificador();
    }
}

