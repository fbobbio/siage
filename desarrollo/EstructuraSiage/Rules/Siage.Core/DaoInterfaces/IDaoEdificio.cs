using Siage.Base.Dto;
using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoEdificio : IDao<Edificio, int>
    {
        List<Edificio> GetByFiltros();

        List<Edificio> GetByFiltros(int? tipoEdificio, string identificadorEdificio, int? funcionEdificio, string identificadorPredio, string descripcionPredio, string nombreCasaHabitacion, int? filtroDepartamentoProvincial, int? filtroLocalidad, int? filtroBarrio, int? filtroCalle, int? filtroAltura, bool? filtroDadosDeBaja);

        List<DtoEdificioReporte> GetEdificiosyFiltros(string filtroIdentificadorPredio, string filtroIdentificadorEdificio, int? filtroTipoEdificio, int? filtroFuncionEdificio, int? filtroDepartamentoPredio, int? filtroLocalidadPredio, int? filtroBarrioPredio, int? filtroCalle);

        List<DtoEdificioReporte> GetByFiltros(int? filtroTipoEdificio, int? filtroFuncionEdificio, int? filtroDepartamento, int? filtroLocalidad, int? filtroBarrio, int? filtroCalle, DateTime? filtroFechaDesde, DateTime? filtroFechaHasta);

        Edificio GetByContrato(int idContrato);

        List<Edificio> GetByIdPredio(int idPredio);

        EmpresaBase GetEmpresaVinculada(int idEdificio);

        Edificio GetEdificioByCasaHabitacion(int idCasaHabitacion);

        string GetUltimoIdentificador();

        void DarDeBajaPlantas(List<int> plantas);
        void DarDeBajaLocales(List<int> locales);

        void ActualizarPredioEdificio(int idPredioNuevo, int idPredio);

        bool ValidarEmpresaVinculada(int idEdificio);
    }
}

