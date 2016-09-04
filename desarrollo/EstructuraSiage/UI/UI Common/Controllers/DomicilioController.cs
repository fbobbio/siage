using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Services.Core.Models;
using System.Web.Routing;
using Siage.Services.Core.InterfacesUC;

namespace SIAGE.UI_Common.Controllers
{
    public class DomicilioController : AjaxAbmcController<DomicilioModel, IDomicilioRules>
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "DomicilioEditor";
            Rule = ServiceLocator.Current.GetInstance<IDomicilioRules>();
        }
       
        #region POST - Procesamiento

        public override void RegistrarPost(DomicilioModel model)
        {
            model = Rule.DomicilioSaveOUpdate(model);
        }

        public override void EditarPost(DomicilioModel model)
        {
            Rule.DomicilioSaveOUpdate(model);
        }

        public override void EliminarPost(DomicilioModel model)
        {
            Rule.DomicilioDelete(model);
        }

        #endregion
        
        public JsonResult CargarProvinciaByPais(string idPais)
        {
            var provincias = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetProvinciabyPais(idPais).OrderBy(x => x.Nombre);
           return Json((provincias.Select(p => new {p.Id, p.Nombre})).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarDepartamentoLocalidadByProvinciaPorDefecto()
        {
            string idPais = "ARG";
            string idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            var provincias = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetProvinciabyPais(idPais);
            var departamentos = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetDepartamentoProvincialByProvincia(idProvincia).OrderBy(x => x.Nombre);

            var jsonDepartamentos = (departamentos.Select(d => new { Id = d.Id, Nombre = d.Nombre })).ToList();
            return Json(new { Departamentos = departamentos, Provincias = provincias }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarDepartamentoLocalidadByProvincia(string idProvincia)
        {
            var departamentos = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetDepartamentoProvincialByProvincia(idProvincia).OrderBy(x => x.Nombre);
            var localidad = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetLocalidadByProvincia(idProvincia);

            var jsonDepartamentos = (departamentos.Select(d => new { Id = d.Id, Nombre = d.Nombre })).ToList();
            return Json(new { Departamentos = departamentos, Localidades = localidad }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarLocalidadByDepartamentoProvincial(int idDepartamento)
        {
            var localidad = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetLocalidadByDepartamentoProvincialConsulta(idDepartamento).OrderBy(x => x.Nombre);
            return Json(localidad, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDomicilioById(int id)
        {
            var domicilio =  ServiceLocator.Current.GetInstance<IDomicilioRules>().GetDomicilioById(id);
            return Json(domicilio, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDomicilioCompletoById(int id)
        {
            var domicilio = ServiceLocator.Current.GetInstance<IDomicilioRules>().GetDomicilioById(id);
            var Provincias = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetProvinciabyPais("ARG").Select(x => new {x.Id, x.Nombre});
            var Departamentos = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetDepartamentoProvincialByProvincia(domicilio.IdProvincia).Select(x => new { x.Id, x.Nombre });
            var Localidades = domicilio.IdDepartamentoProvincial.HasValue ? 
                    ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetLocalidadByDepartamentoProvincial(domicilio.IdDepartamentoProvincial.Value).Select(x => new {x.Id, x.Nombre}) :
                    ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetLocalidadByProvincia(domicilio.IdProvincia).Select(x => new {x.Id, x.Nombre});
            var Barrios = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetBarrioByLocalidad(domicilio.IdLocalidad).Select(x => new {x.Id, x.Nombre});
            var Calles = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetCalleByFiltro(null, domicilio.IdLocalidad, domicilio.IdTipoCalle).OrderBy(x => x.Nombre).Select(x => new { x.Id, x.Nombre });

            var json = new {
                domicilio.Altura,
                domicilio.Departamento,
                domicilio.Torre,
                domicilio.Piso,
                domicilio.IdTipoCalle,
                idPais = "ARG",
                domicilio.IdProvincia,
                Provincias,
                domicilio.IdDepartamentoProvincial,
                Departamentos,
                domicilio.IdLocalidad,
                Localidades,
                domicilio.IdBarrio,
                Barrios,
                domicilio.IdCalle,
                Calles
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDomicilioByPersona(int idPersona)
        {
            List<DomicilioModel> domicilios =
                ServiceLocator.Current.GetInstance<IPersonaFisicaRules>().GetDomicilioByPersona(idPersona);
            return Json(domicilios, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarBarrioByLocalidad(int idLocalidad)
        {
            var barrio = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetBarrioByLocalidad(idLocalidad).OrderBy(x => x.Nombre);
            return Json(barrio, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarCalleByTipoCalleYLocalidad(int idLocalidad, int idTipoCalle)
        {
            var calle = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetCalleByFiltro(null, idLocalidad, idTipoCalle).OrderBy(x => x.Nombre);
            return Json(calle, JsonRequestBehavior.AllowGet);
        }
    }
}
