using System;
using System.Collections.Generic;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IAgenteRules
    {        
        List<AgenteModel> GetAgenteByFilters(string filtroNumeroDocumento, string filtroTipoDocumento, SexoEnum? filtroSexo, string filtroApellido, string filtroNombre, int? filtroDepartamentoProvincial, int? filtroLocalidad, string filtroNroLegajoSiage, string filtroNroLegajoMedia, string filtroNroLegajoInicial, string filtroNombreEmpresa, int? filtroNivelEducativo, int? filtroDireccionNivel, int? filtroSituacionRevista, int? filtroTipoCargo, int? filtroAsignatura, DateTime? filtroFechaDesdeAlta, DateTime? filtroFechaHastaAlta, DateTime? filtroFechaDesdeBaja, DateTime? filtroFechaHastaBaja, int? filtroTitulo, TipoAgenteEnum? filtroTipoAgente,int? filtroEmpresaId);
        AgenteModel GetAgenteById(int id);
        AgenteModel AgenteSave(AgenteModel modelo);        
        AgenteModel AgenteUpdate(AgenteModel modelo, List<int> listaVinculos);
        bool ValidarAgenteByPersona(string tipoDocumento, string nroDoc);
        bool ValidarAgenteDelete(int idAgente);
        void AgenteDelete(AgenteModel modelo);

        AgenteConsultaModel GetAgenteConsultaById(int idAgente);
    }
}
