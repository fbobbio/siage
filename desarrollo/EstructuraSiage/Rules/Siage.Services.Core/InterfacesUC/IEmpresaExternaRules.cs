using System.Collections.Generic;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.Domain;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    
    public interface IEmpresaExternaRules
    {
        EmpresaExternaModel GetEmpresaExternaById(int id);
        List<DtoEmpresaExternaConsultaModel> GetFiltroBusquedaEmpresaExterna(string nombre, string razonSocial, string cuil, string cuit, TipoEmpresaExternaEnum? tipoEmpresa, bool estado);
        bool VerificarExistenciaEmpresaXpersona(string cuil, string cuit);
        int GetIdPersona(string cuil,string cuit);
        EmpresaExternaModel TomarIdDeEmpresa(int id);
        PersonaFisicaModel TomarPersonaFisicaById(int id);
        List<CondicionIvaModel> GetCondicionIvaAll();
        EmpresaExternaModel EmpresaExternaSave(EmpresaExternaModel modelo);
        void DomicilioSave(DomicilioModel modelo, int idEmpresa);
        void ValidarExistenciaEmpresaExterna(string nombre);
        EmpresaExternaModel EmpresaExternaUpdateOrSave(EmpresaExternaModel modelo);
        void DeleteEmpresaExterna(EmpresaExternaModel modelo);
        EmpresaExternaModel ReactivarEmpresaExterna(EmpresaExternaModel modelo);
        bool ConsultarExistenciaCuil(int id);
        bool ConsultarExistenciaCuit(int id);
    }
}
