using System;
using System.Collections.Generic;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoAgente : IDao<Agente, int>
    {
        List<Agente> GetByFiltros(string filtroNumeroDocumento, string filtroTipoDocumento, SexoEnum? filtroSexo, string filtroApellido, string filtroNombre, int? filtroDepartamentoProvincial, int? filtroLocalidad, string filtroNroLegajoSiage, string filtroNroLegajoMedia, string filtroNroLegajoInicial, string filtroNombreEmpresa, int? filtroNivelEducativo, int? filtroDireccionNivel, int? filtroSituacionRevista, int? filtroTipoCargo, int? filtroAsignatura, DateTime? filtroFechaDesdeAlta, DateTime? filtroFechaHastaAlta, DateTime? filtroFechaDesdeBaja, DateTime? filtroFechaHastaBaja, int? filtroTitulo, TipoAgenteEnum? filtroTipoAgente,int? filtroEmpresaId);
        DtoDatosDniAgente GetDatosPersonalesAgenteById(int idAgente);
        string GetUltimoIdentificador();       
        bool GetByPersona(string filtroNumeroDocumento, string filtroTipoDocumento, SexoEnum? filtroSexo);        
    }
}