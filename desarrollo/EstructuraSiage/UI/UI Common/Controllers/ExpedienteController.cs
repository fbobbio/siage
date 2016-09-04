using System;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;

namespace SIAGE.UI_Common.Controllers
{
    public class ExpedienteController : AjaxAbmcController<ExpedienteModel, IExpedienteRules>
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "ExpedienteEditor";
            Rule = ServiceLocator.Current.GetInstance<IExpedienteRules>();
        }

        public JsonResult GetExpedienteByFilters(string numero, int? id)
        {
            ExpedienteModel expediente = null;
            if (id.HasValue)
                expediente = Rule.GetExpedienteById(id.Value);
            numero = numero.ToUpper();
            if(!string.IsNullOrEmpty(numero) && numero != "NULL" && expediente == null)
                expediente = ServiceLocator.Current.GetInstance<IExpedienteRules>().GetExpedienteByNumero(numero);

            if (expediente == null)
                return null;

            var jsonRetorno = new
            {
                Id = expediente.Id,
                FechaInicio = expediente.FechaInicio != null ? ((DateTime)expediente.FechaInicio).ToString("dd/MM/yyyy") : string.Empty,
                Numero = expediente.Numero,
                Asunto = expediente.Asunto,
                PersonaInicio = expediente.PersonaInicio != null ? expediente.PersonaInicio.Id.ToString() : string.Empty
            };

            return Json(jsonRetorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExpedienteByInstrumento(int? id)
        {
            ExpedienteModel expediente = null;
            if (id.HasValue)
            {
                var instrumento =
                    ServiceLocator.Current.GetInstance<IInstrumentoLegalRules>().GetInstrumentoLegalById(id.Value);
                if (instrumento.Expediente != null)
                    expediente = instrumento.Expediente;
            }

            if (expediente == null)
                return null;

            var jsonRetorno = new
            {
                Id = expediente.Id,
                FechaInicio = expediente.FechaInicio != null ? ((DateTime)expediente.FechaInicio).ToString("dd/MM/yyyy") : string.Empty,
                Numero = expediente.Numero,
                Asunto = expediente.Asunto,
                PersonaInicio = expediente.PersonaInicio != null ? expediente.PersonaInicio.Id.ToString() : string.Empty
            };

            return Json(jsonRetorno, JsonRequestBehavior.AllowGet);
        }
    }
}

