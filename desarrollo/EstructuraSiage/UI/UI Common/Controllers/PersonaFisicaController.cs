using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Base;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;

namespace SIAGE.UI_Common.Controllers
{
    public class PersonaFisicaController : AjaxAbmcController<PersonaFisicaModel, IPersonaFisicaRules>
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "PersonaFisicaEditor";
            Rule = ServiceLocator.Current.GetInstance<IPersonaFisicaRules>();
        }

        public override void CargarViewData(EstadoABMC estadoAbmc)
        {
            var entidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();

            ViewData.Add(ViewDataKey.PAIS.ToString(), entidades.GetPaisAll());
            ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(), entidades.GetTipoCalleAll());
            ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(), entidades.GetEstadoCivilAll());
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), entidades.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.SEXO.ToString(), entidades.GetSexoAll());
            ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(), entidades.GetOrganismoEmisorDocumentoAll());
        }

        public override void RegistrarPost(PersonaFisicaModel model)
        {
            model = Rule.PersonaFisicaSaveOUpdate(model);
        }

        public object ArmarJsonPersonaFisica(PersonaFisicaModel p, bool esDeRCivil)
        {
            var localidad = Rule.GetLocalidadById(p.LocalidadNacimiento);
            var jsonRetorno = new
            {
                Id = p.Id.HasValue ? p.Id.Value.ToString() : "0",
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                FechaNacimiento = p.FechaNacimiento.HasValue ? p.FechaNacimiento.Value.ToString("dd/MM/yyyy") : string.Empty,
                TipoDocumento = p.TipoDocumento.ToString(),
                NumeroDocumento = p.NumeroDocumento,
                Sexo = p.Sexo.ToString(),
                EstadoCivil = p.EstadoCivil.ToString(),
                p.CUIL,
                Observaciones = p.Observaciones,
                OrganismoEmisorDocumento = p.OrganismoEmisorDocumento,
                IdPaisEmisorDocumento = p.IdPaisEmisorDocumento,
                IdPaisNacionalidad = localidad.DepartamentoProvincial.Provincia.Pais.Id,
                IdPaisOrigen = p.IdPaisOrigen,
                Provincia = new
                {
                    localidad.DepartamentoProvincial.Provincia.Id,
                    localidad.DepartamentoProvincial.Provincia.Nombre
                },
                DepartamentoProvincial = new
                {
                    localidad.DepartamentoProvincial.Id,
                    localidad.DepartamentoProvincial.Nombre
                },
                Localidad = new
                {
                    localidad.Id,
                    localidad.Nombre
                },
                EsDeRCivil = esDeRCivil,
                Domicilio = p.Domicilio != null ? p.Domicilio.Id : null
            };
            return jsonRetorno;
        }

        public JsonResult GetPersonaFisicaByFiltros(int? id, string tipoDocumento, string nroDocumento, string sexo)
        {
            var p = id.HasValue ? Rule.GetById(id.Value) : Rule.GetPersonaFisicaByFiltros(tipoDocumento, nroDocumento, sexo);
            object jsonRetorno;
            if (p != null)
            {
                jsonRetorno = ArmarJsonPersonaFisica(p, false);
                return Json(jsonRetorno, JsonRequestBehavior.AllowGet);
            }
            var persona = Rule.GetPersonaFisicaRCivilByFiltros(tipoDocumento, nroDocumento, sexo);
            if(persona != null)
            {
                jsonRetorno = ArmarJsonPersonaFisica(persona, true);
                return Json(jsonRetorno, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public PartialViewResult GetPersonaPartialViewById(int? id, EstadoABMC estadoText)
        {
            PersonaFisicaModel personaModel = null;
            if(id.HasValue)
                personaModel = ServiceLocator.Current.GetInstance<IPersonaFisicaRules>().GetById(id.Value);

            //Seteo el estadoText y estadoId
            ViewData[AjaxAbmc.EstadoText] = estadoText.ToString();
            ViewData[AjaxAbmc.EstadoId] = (int)estadoText;

            CargarViewData(estadoText);
            return PartialView("../Shared/EditorTemplates/PersonaFisicaEditor", personaModel);
        }

        public JsonResult CargarProvinciaByPaisOrigen(string idPais)
        {
            if (!string.IsNullOrEmpty(idPais))
            {
                var provincias =
                    ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetProvinciabyPais(
                        idPais).OrderBy(x => x.Nombre);
                return Json(new SelectList(provincias, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
            }
            return Json(new SelectList(new List<string>()));
        }

        public JsonResult CargarDepartamentoProvincialByProvincia(string idProvincia)
        {
            if (!string.IsNullOrEmpty(idProvincia))
            {
                var departamentos = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetDepartamentoProvincialByProvincia(idProvincia).OrderBy(x => x.Nombre);
                return Json(new SelectList(departamentos, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
            }
            return Json(new SelectList(new List<string>()));

        }

        public JsonResult CargarLocalidadByDepartamentoProvincial(int? idDepartamento)
        {
            if (idDepartamento.HasValue)
            {
                var localidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetLocalidadByDepartamentoProvincial(idDepartamento.Value).OrderBy(x => x.Nombre);
                return Json(new SelectList(localidades, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
            }
            return Json(new SelectList(new List<string>()));
        }
    }
}

