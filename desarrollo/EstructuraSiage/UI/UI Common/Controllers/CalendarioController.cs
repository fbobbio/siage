using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Content;

namespace SIAGE.UI_Common.Controllers
{
    public class CalendarioController : AjaxAbmcController<CalendarioModel, ICalendarioRules>
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            AbmcView = "CalendarioEditor";
            Rule = ServiceLocator.Current.GetInstance<ICalendarioRules>();
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            string idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            var entidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();

            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), entidades.GetLocalidadByProvincia(idProvincia));
        }

        public override void RegistrarPost(CalendarioModel model)
        {
            Rule.CalendarioSave(model);
        }

        public override void EditarPost(CalendarioModel model)
        {
            Rule.CalendarioSave(model);
        }

        public override void EliminarPost(CalendarioModel model)
        {
            Rule.CalendarioDelete(model);
        }
        
        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? anio, DateTime? fechaDesde, DateTime? fechaHasta, AmbitoAplicacionEnum? ambito, TipoFechaEnum? tipo, EstadoCalendarioEnum? estado)
        {
            // Construyo la funcion de ordenamiento
            Func<CalendarioModel, IComparable> funcOrden =
                sidx == "Fecha" ? x => x.Fecha :
                sidx == "Concepto" ? x => x.Concepto :
                sidx == "Tipo" ? x => x.Tipo :
                sidx == "Ambito" ? x => x.Ambito :
                sidx == "Estado" ? x => x.EstadoCalendario :
                (Func<CalendarioModel, IComparable>)(x => x.Id);
            
            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetByFiltros(anio, fechaDesde, fechaHasta, ambito, tipo, estado);

            // Ordeno los registros
            registros = sord == "asc" ? registros.OrderBy(funcOrden).ToList() : registros.OrderByDescending(funcOrden).ToList();

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling(totalRegistros / (decimal)rows);
            registros = registros.Skip((page - 1) * rows).Take(rows).ToList();

            // Construyo el json con los valores que se mostraran en la grilla
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRegistros,
                rows = from a in registros
                    select new
                    {
                        cell = new string[] {
                            a.Id.ToString(), 
                            a.Fecha.ToString(),
                            a.Concepto,
                            a.Tipo.ToString(),
                            a.Ambito.ToString(),
                            a.Localidad_Nombre,
                            a.EstadoCalendario.ToString()
                        }
                    }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}