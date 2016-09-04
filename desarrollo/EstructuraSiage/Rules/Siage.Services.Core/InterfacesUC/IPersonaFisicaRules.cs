using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IPersonaFisicaRules
    {
        PersonaFisicaConsultaHogarModel GetPersonaFisicaByidUsuario(int idUsuario);
        PersonaFisicaModel GetPersonaFisicaByFiltros(string tipoDocumento, string nroDoc, string sexo);
        PersonaFisicaModel PersonaFisicaSaveOUpdate(PersonaFisicaModel modelo);
        PersonaFisicaModel PersonaFisicaSaveOUpdate(PersonaFisicaModel modelo, string CUIL);
        PersonaFisicaModel GetById(int idPersona);
        List<DomicilioModel> GetDomicilioByPersona(int idPersona);
        LocalidadModel GetLocalidadById(int idLocalidad);
        bool ExistePersonaEnRCivil(string tipoDocumento, string nroDoc, string sexo);
        PersonaFisicaModel GetPersonaFisicaRCivilByFiltros(string tipoDocumento, string nroDoc, string sexo);
    }
}