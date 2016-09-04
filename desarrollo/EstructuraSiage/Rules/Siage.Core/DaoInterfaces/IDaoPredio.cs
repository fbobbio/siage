using Siage.Base.Dto;
using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoPredio : IDao<Predio, int>
    {
        List<Predio> GetByFiltros(string filtroIdentificadorPredio, string filtroDescripcion, EstadoPredioEnum? filtroEstado, string filtroNroCatastral, int? filtroDepartamento, int? filtroLocalidad, int? filtroBarrio, int? filtroCalle, int? filtroAltura, string filtroProvincia, bool? filtroDadosDeBaja);
        List<Predio> GetPrediosParaUnionDeEdificios();
        List<Predio> GetPrediosParaDivisionDeEdificios();
        String GetUltimoIdentificador();
        bool ValidarPredioEnDomicilio(int idLocalidad, int idCalle, int idAltura);

        bool ValidarEdificiosActivosPredio(int idPredio);

        DtoCallesPredio GetCallesPredioById(int id);
        DtoCallesPredioNombres GetCallesPredioByEdificioId(int idEdificio);

        bool GetByDescripcion(string p);

        List<DtoPredioReporte> GetByFiltros(EstadoPredioEnum? filtroEstado, int? filtroDepartamento, int? filtroLocalidad, int? filtroBarrio, int? filtroCalle, string filtroAltura, DateTime? filtroFechaDesde, DateTime? filtroFechaHasta);
    }
}

