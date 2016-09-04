using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoEmpresa : IDao<EmpresaBase, int>
    {
        List<EmpresaBase> GetByFiltrosBasico(string codigoEmpresa, string nombreEmpresa, int? idDepartamentoProvincial, int? idLocalidad, string barrio, string calle, string altura, List<EstadoEmpresaEnum> estadoEmpresaEnum);
        List<EmpresaBase> GetByFiltroAvanzado(DateTime? fechaAltaDesde, DateTime? fechaAltaHasta, DateTime? fechaInicioActividadDesde, DateTime? fechaInicioActividadHasta, TipoEmpresaEnum TipoEmpresaEnum, int? idProgramaPresupuestario, List<EstadoEmpresaEnum> estadoEmpresaEnum, int? idDepartamentoProvincial, int? idLocalidad, string barrio, string calle, string altura);        
        List<string> GetCamposConInstrumentoLegal();

    }
}

