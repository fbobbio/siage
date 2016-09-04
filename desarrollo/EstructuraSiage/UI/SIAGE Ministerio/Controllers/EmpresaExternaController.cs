using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Siage.Base;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;

namespace SIAGE_Ministerio.Controllers
{
    public class EmpresaExternaController:AjaxAbmcController<EmpresaExternaModel, IEmpresaExternaRules>
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Rule = ServiceLocator.Current.GetInstance<IEmpresaExternaRules>();
            AbmcView = "EmpresaExternaEditor";

            var entidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            ViewData["PaisesList"] = ViewData["PaisesList"] ?? entidades.GetPaisAll();
            ViewData["TiposDocumentoList"] = ViewData["TipoDocumentoList"] ?? entidades.GetTipoDocumentoAll();
            ViewData["EstadoCivilList"] = ViewData["EstadoCivilList"] ?? entidades.GetEstadoCivilAll();
            ViewData["SexoList"] = ViewData["SexoList"] ?? entidades.GetSexoAll();
            ViewData["TipoCalleList"] = ViewData["TipoCalleList"] ?? entidades.GetTipoCalleAll();
            ViewData["OrganismoEmisorList"] = ViewData["OrganismoEmisorList"] ?? entidades.GetOrganismoEmisorDocumentoAll();


            if (ViewData["TipoCalleList"] == null)
                ViewData["TipoCalleList"] = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetTipoCalleAll();
            if (ViewData["PaisesList"] == null)
                ViewData["PaisesList"] = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetPaisAll();
          
        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, string filtroNombre,
                                            string filtroCuitCuil, TipoEmpresaExternaEnum? filtroTipoEmpresa, 
                                            bool chkBusquedaEmpresaExternasEliminadas)
        {
            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetFiltroBusquedaEmpresaExterna(filtroNombre, filtroCuitCuil, filtroTipoEmpresa, chkBusquedaEmpresaExternasEliminadas);
            
            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            registros = registros.Skip((page - 1) * rows).Take(rows).ToList();

            // Construyo el json con los valores que se mostraran en la grilla
            var jsonData = new {
                total = totalPages,
                page = page,
                records = totalRegistros,
                rows = from a in registros select new {
                    cell = new string[] {
                        a.Id.ToString(),
                        a.Nombre,
                        a.Activo ? "Activo" : "Inactivo"
                    }
                }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpresaExterna(int empresa)
        {
            return Json(Rule.GetEmpresaExternaById(empresa), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarExistenciaPersonaFisicaJuridica(string cuil,string cuit)
        {
            var band = Rule.GetFiltroBusquedaEmpresaExterna(cuil, cuit) > 0;
            return Json(band, JsonRequestBehavior.AllowGet);
        }

        public override void RegistrarPost(EmpresaExternaModel model)
        {
            var empresa=Rule.EmpresaExternaSave(model);
            if (empresa.Domicilio != null)
                Rule.DomicilioSave(empresa.Domicilio, empresa.Id);
        }

        public override void ReactivarPost(EmpresaExternaModel model)
        {
            Rule.EmpresaExternaUpdateOrSave(model);
        }

        public override void EliminarPost(EmpresaExternaModel model)
        {

        }

       

        public JsonResult TomarIdPersona(int id)
        {
            var empresaExternaEntidad = Rule.TomarIdDePersona(id);
            var json = new
            {
                PersonaFisicaId = empresaExternaEntidad.Referente != null ? empresaExternaEntidad.Referente.Id : 0,
                PersonaJuridicaId = empresaExternaEntidad.PersonaJuridica != null ? empresaExternaEntidad.PersonaJuridica.Id : 0,
                DomicilioEmpresaId = empresaExternaEntidad.Domicilio != null ? empresaExternaEntidad.Domicilio.Id : 0                
            };
	
	        return Json(json, JsonRequestBehavior.AllowGet);           
        }

        public override void EditarPost(EmpresaExternaModel model)
        {
            Rule.EmpresaExternaUpdateOrSave(model);
        }
    }
}
 