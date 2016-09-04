using System;
using System.Collections.Generic;
using Siage.Base.Dto;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IEdificioRules
    {
        EdificioConsultarModel GetEdificioConsultaById(int idEdificio);
        EdificioConsultarModel GetEdificioByContrato(int idContrato);                
        EdificioModel GetEdificioById(int id);
        EdificioModel EdificioSave(EdificioModel modelo);
        EdificioModel EdificioEditar(EdificioModel model, List<int> listaPlantas, List<int> listaLocales);
        EdificioModel ReactivarEdificio(EdificioModel model);

        FuncionEdificioModel GetFuncionEdificioById(int p);
        TipoAdquisicionModel GetTipoadquisicion(int idTipoAdquisicion);
        TipoEstructuraEdiliciaModel GetTipoEdificioById(int p);
        TipoCalleModel GetTipoCalleById(int id);

        List<EdificioModel> GetEdificioByFiltros();        
        List<EdificioModel> GetEdificioAll();
        List<EdificioConsultarModel> GetEdificioByFiltros(string tipoEdificio, string identificadorEdificio, string funcionEdificio, string identificadorPredio, string descripcionPredio, string nombreCasaHabitacion, int? filtroDepartamentoProvincial, int? filtroLocalidad, int? filtroBarrio, int? filtroCalle, int? filtroAltura, bool? filtroDadosDeBaja);
        List<DtoEdificioReporte> GetEdificioByFiltros(string filtroTipoEdificio, string filtroFuncionEdificio, int? filtroDepartamento, int? filtroLocalidad, int? filtroBarrio, int? filtroCalle, DateTime? filtroFechaDesde, DateTime? filtroFechaHasta);
        List<DtoEdificioReporte> GetEdificioByFiltros(string filtroIdentificadorPredio, string filtroIdentificadorEdificio, string filtroTipoEdificio, string filtroFuncionEdificio, int? filtroDepartamentoPredio, int? filtroLocalidadPredio, int? filtroBarrioPredio, int? filtroCallePredio); 
        
        bool ValidarSuperficieTotal(EdificioModel edificioModel);
        bool GetTipoadquisicionById(int idTipoAdquisicion);
        void DomicilioSave(EdificioModel modelo);
        void EdificioDelete(EdificioModel modelo);
    }
}

