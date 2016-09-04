using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoPersonaJuridica : IDao<PersonaJuridica, int>
    {
        PersonaJuridica GetPersonaJuridicaByFiltro(string cuit);
        bool VerificarDistintaRazonSocial(string razonSocial, int id);

    }
}

