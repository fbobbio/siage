using Siage.Base.Dto;
using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoEmpresaExterna : IDao<EmpresaExterna, int>
    {
        bool GetByFiltrosVerificarPersonaAsociadaAempresa(string cuil, string cuit);
        int VerificarPersonaYgetByPersonaId(string documentoPf, string cuitPj);
        bool GetByFiltrosExisteEmpresa(string nombre);
        List<DtoEmpresaExternaConsulta> GetByFiltros(string nombre, string razonSocial, string cuil, string cuit, TipoEmpresaExternaEnum? tipoEmpresa, bool estado);
        bool VerificarExistenciaEnPuestoTrabajo(int empresaExternaId);
    }
}

 //List<EmpresaExterna> GetByFiltros();
 //       List<EmpresaExterna> GetByFiltros(string nombre, string cuit, TipoEmpresaExternaEnum? tipoEmpresa, bool estado);
 //       List<EmpresaExterna> GetByFiltros(string cuil, string cuit);
 //       EmpresaExterna GetByFiltroIdPersona(int id);
 //       int VerificarExistenciaEnPuestoTrabajo(int empresaExternaId);
 //       int GetByPersonaId(string cuil,string cuit);