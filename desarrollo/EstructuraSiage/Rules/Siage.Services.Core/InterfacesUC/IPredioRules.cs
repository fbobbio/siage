using System;
using System.Collections.Generic;
using Siage.Base.Dto;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IPredioRules
    {
        PredioConsultaModel PredioDelete(PredioModel entidad);
        PredioModel GetPredioById(int id);
        PredioConsultaModel PredioSave(PredioModel entidad);
        PredioConsultaModel PredioUpdate(PredioModel entidad);
        PredioConsultaModel ReactivarPredio(PredioModel predio);
        PredioConsultaModel GetPredioByIdConsulta(int idPredio);
        PredioConsultaModel GetPredioConsultaModel(PredioModel predioModel);
        BarrioModel GetBarrioById(int idBarrio);
        CalleModel GetCalleByFiltro(int idCalle, int? idLocalidad);
        CalleModel GetCalleById(int idCalle);
        DomicilioModel GetDomicilioPredio(int idPredio);

        List<PredioModel> GetPredioByFiltros(string filtroIdentificadorPredio, string filtroDescripcion, EstadoPredioEnum? filtroEstado, string filtroNroCatastral, int? filtroDepartamento, int? filtroLocalidad, int? filtroBarrio, int? filtroCalle, int? filtroAltura, string filtroProvincia, bool? filtroDadosDeBaja);
        List<PredioConsultaModel> GetPredioByFiltrosConsulta(string filtroIdentificadorPredio, string filtroDescripcion, EstadoPredioEnum? filtroEstado, string filtroNroCatastral, int? filtroDepartamento, int? filtroLocalidad, int? filtroBarrio, int? filtroCalle, int? filtroAltura, string filtroProvincia, bool? filtroDadosDeBaja);
        List<EdificioConsultarModel> GetEdificiosByParcela(int idPredio);

        bool VerificarParcelasConContratoAlquilerVigente(int idPredio);
        bool VerificarPredioServicio(int idPredio);
        void DomicilioSave(DomicilioModel modelo, int idPredio);

        bool ValidarEdificiosActivosPredio(int idPredio);

        PredioCallesModel GetCallesPredioById(int id);

        List<DtoPredioReporte> GetPredioReporteByFiltrosConsulta(EstadoPredioEnum? filtroEstado, int? filtroDepartamento, int? filtroLocalidad, int? filtroBarrio, int? filtroCalle, string filtroAltura, DateTime? filtroFechaDesde, DateTime? filtroFechaHasta);
    }
}