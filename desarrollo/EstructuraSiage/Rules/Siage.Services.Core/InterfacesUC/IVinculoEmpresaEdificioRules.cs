using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IVinculoEmpresaEdificioRules
    {
        VinculoEmpresaEdificioModel VinculoEmpresaEdificioDelete(VinculoEmpresaEdificioModel entidad);
        VinculoEmpresaEdificioModel GetVinculoEmpresaEdificioById(int id);
        VinculoEmpresaEdificioModel VinculoEmpresaEdificioUnicoSave(RegistrarVinculoEmpresaEdificioModel entidad); 
        List<VinculoEmpresaEdificioModel> VinculoEmpresaEdificioSave(RegistrarVinculoEmpresaEdificioModel entidad);

        List<VinculoEmpresaEdificioModel> GetVinculoEmpresaEdificioByFiltros(string filtroCodigoEmpresa, string filtroNombreEmpresa, string filtroCodigoEdificio, EstadoVinculoEmpresaEdificioEnum? filtroEstadoVinculo);
        VinculoEmpresaEdificioModel GetRegistrarVinculoEmpresaEdificioById(int id);
        PredioModel GetPredioById(int id);
        LocalidadModel GetLocalidadById(int p);

        VinculoEmpresaEdificioModel VinculoEmpresaEdificioUnicoValidar(RegistrarVinculoEmpresaEdificioModel modelo);

        List<DtoVinculoEmpresaReporte> GetVinculosEmpresaByFilter(string filtroIdentificadorEmpresa, string filtroNombreEmpresa, int? filtroDepartamento, int? filtroLocalidad, int? filtroBarrio, int? filtroCalle);

        bool ValidarEliminarVinculo(int idVinculo);
    }
}
