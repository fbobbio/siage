using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;

namespace SIAGE.UI_Common.Controllers
{
    public class AsignacionInstrumentoLegalController : AjaxAbmcController<AsignacionInstrumentoLegalModel, IAsignacionInstrumentoLegalRules>
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            Rule = ServiceLocator.Current.GetInstance<IAsignacionInstrumentoLegalRules>();
            AbmcView = "AsignacionInstrumentoLegalEditor";
        }

        public override void RegistrarPost(AsignacionInstrumentoLegalModel model)
        {
            Rule.AsignacionInstrumentoLegalSave(model);
        }

        public override void EditarPost(AsignacionInstrumentoLegalModel model)
        {
            Rule.AsignacionInstrumentoLegalSave(model);
        }

        [HttpGet]
        public JsonResult GetAsignacionInstrumentoLegalById(int id)
        {
            var asignacion = Rule.GetAsignacionInstrumentoLegalLegalById(id);
            var json = new {
                Id = asignacion != null? asignacion.Id: null,
                InstrumentoLegal = asignacion != null? asignacion.IdInstrumentoLegal: null
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
