using Siage.Base.Dto;
using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoContrato : IDao<Contrato, int>
    {
        /// <summary>
        /// Lista de Contratos en base a los filtros pasados como parámetro.
        /// </summary>
        /// <param name="fechaDesde">Contratos con FechaContratoDesde mayor o igual a fechaDesde.</param>
        /// <param name="fechaHasta">Contratos con FechaContratoDesde menor o igual a fechaHasta.</param>
        /// <param name="tipoContrato"></param>
        /// <param name="rescindido">True:Contratos con FechaRescicion nula.False:Contratos con FechaRescicion no nula.</param>
        /// <param name="vigente">True:Contratos con FechaContratoHasta menor a la fecha actual. False:Contratos con FechaContratoHasta mayor o igual a la fecha actual.</param>
        /// <param name="parametroVencimientoAlquilerOComodato">Contratos en los cuales la diferencia entre la FechaContratoHasta y la fecha actual sea menor o igual a valor del parámetro.</param>
        /// <returns></returns>
        List<Contrato> GetByFiltros(DateTime? fechaDesde, DateTime? fechaHasta, EntidadAlquiladaEnum? entidadAlquilada, TipoContratoEnum? tipoContrato, bool? rescindido, bool? vigente, bool? dadosBaja, int? parametroVencimientoAlquilerOComodato, int? identificadorContrato);
        bool validarExistencia(int identificador, EntidadAlquiladaEnum entidadAlquilada);
        List<DtoContratoConsultaAvanzada> ProcesarBusquedaAvanzada(DateTime? fechaDesde, DateTime? fechaHasta,
                                                                   EntidadAlquiladaEnum? entidadAlquilada,
                                                                   TipoContratoEnum? tipoContrato);
    }
}

