using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoPersonaFisica : IDao<PersonaFisica, int>
    {
        PersonaFisica GetPersonaFisicaByFiltros(TipoDocumento tipoDocumento, string nroDoc, string sexo);
        //se usa en registrar empresa hasta que se haga un control de busqueda de usuario
        PersonaFisica GetPersonaFisicaByFiltrosProvisorio(string nroDoc, string tipoSexoId, string nombre,
                                                          string apellido);
        bool ExistePersonaEnRCivil(string tipoDocumento, string numeroDocumento, string sexo);
        void DarDeBajaVinculo(List<int> listaVinculos);
        PersonaFisica GetPersonaFisicaRCivilByFiltros(string tipoDocumento, string nroDoc, string sexo);
    }
}

