using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoInstrumentoLegal : IDao<InstrumentoLegal, int>
    {
        List<InstrumentoLegal> GetByFiltros(string filtroNumeroInstrumentoLegal, bool dadosBaja);
        InstrumentoLegal GetByNumero(string filtroNumeroInstrumentoLegal);
        bool ExisteInstrumento(string nroInstrumentoLegal, int? idInstrumentoLegal);
    }
}

