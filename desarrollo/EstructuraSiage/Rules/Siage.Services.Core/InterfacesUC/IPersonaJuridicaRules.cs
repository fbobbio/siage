using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Base;
using Siage.Services.Core.Models;


namespace Siage.Services.Core.InterfacesUC
{
    public interface IPersonaJuridicaRules
    {
        PersonaJuridicaModel PersonaJuridicaSaveOUpdate(PersonaJuridicaModel modelo);
        PersonaJuridicaModel GetPersonaJuridicaByFiltro(string cuit);
        PersonaJuridicaModel GetById(int id);
        void DomicilioSave(PersonaJuridicaModel entidad);
    }
}

