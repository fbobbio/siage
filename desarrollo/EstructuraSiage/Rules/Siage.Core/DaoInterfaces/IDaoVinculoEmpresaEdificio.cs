using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoVinculoEmpresaEdificio : IDao<VinculoEmpresaEdificio, int>
    {
        List<VinculoEmpresaEdificio> GetVinculoEmpresaEdificioByFilters(string filtroCodigoEmpresa, string filtroNombreEmpresa, string filtroCodigoEdificio, EstadoVinculoEmpresaEdificioEnum? filtroEstadoVinculo);
        List<VinculoEmpresaEdificio> GetVinculoEmpresaEdificioByFilters(string filtroCodigoEmpresa, string filtroNombreEmpresa);
        bool VerificarVinculoEmpresaEdificio(int idEdificio, int idEmpresa);

        void BorrarVinculosDeEmpresa(int idEmpresa);

        List<DtoVinculoEmpresaReporte> GetVinculosEmpresaByFilter(string filtroIdentificadorEmpresa, string filtroNombreEmpresa, int? filtroDepartamento, int? filtroLocalidad, int? filtroBarrio, int? filtroCalle);
    }
}
