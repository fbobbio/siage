using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoExpediente : IDao<Expediente, int>
    {
        List<Expediente> GetByFiltros();
        Expediente GetByFiltros(int? idInstrumentoLegal, string numeroExpediente);
        Expediente GetByNumero(string numeroExpediente);

        bool ExisteExpediente(string numeroExpediente, int? id);
    }
}

