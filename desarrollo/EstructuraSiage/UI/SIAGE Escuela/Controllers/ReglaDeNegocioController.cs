using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Controllers;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using SIAGE.UI_Common.Content;

namespace SIAGE_Ministerio.Controllers
{
    public class ReglaDeNegocioController : AjaxAbmcController<ValorParametroModel, IParametroRules>
    {
        private int _idEscuela;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Rule = ServiceLocator.Current.GetInstance<IParametroRules>();
            AbmcView = "ParametroEditor";

            _idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
        }

        public override ActionResult Editar(int id)
        {
            ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Editar.ToString();
            ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Editar;

            var valor = Rule.GetValorParametroVigente((ParametroEnum)id, _idEscuela);
            return PartialView(AbmcView, valor);
        }

        public override void EditarPost(ValorParametroModel model)
        {
            model.Id = 0;
            model.Escuela = _idEscuela;
            Rule.SaveParametroEscuela(model);
        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, string filtroParametro)
        {
            Func<ValorParametroModel, IComparable> funcOrden =
                sidx == "Parametro" ? x => x.Parametro.Nombre :
                sidx == "Valor" ? x => x.Valor :
                sidx == "Descripcion" ? x => x.Parametro.Descripcion :
                sidx == "FechaVigencia" ? x => x.FechaVigencia :
                (Func<ValorParametroModel, IComparable>)(x => x.Id);

            List<ValorParametroModel> registros = Rule.GetValorParametroVigenteByFiltro(null, filtroParametro, _idEscuela);
            registros = sord == "asc" ? registros.OrderBy(funcOrden).ToList() : registros.OrderByDescending(funcOrden).ToList();

            int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            registros = registros.Skip((page - 1) * rows).Take(rows).ToList();

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRegistros,
                rows = from a in registros
                       select new
                       {
                           cell = new string[] {
                                a.Parametro.Id.ToString(),
                                a.Parametro.Nombre.Replace("_", " "),
                                a.Parametro.Descripcion,
                                a.FechaVigencia.Value.ToString(),
                                (a.Parametro.TipoDato == TipoDatoEnum.BOOLEAN) ? (a.Valor == "Y" ? "ACTIVO" : "INACTIVO") : a.Valor
                            }
                       }
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarBusquedaHistorial(int id)
        {
            var jsonData = new
            {
                rows = Rule.GetHistorialParametro((ParametroEnum)id, _idEscuela).Select(a =>
                    new
                    {
                        cell = new string[]
                            {
                                a.FechaVigencia.Value.ToString("dd/MM/yyyy"), 
                                (a.Parametro.TipoDato == TipoDatoEnum.BOOLEAN) ? (a.Valor == "Y" ? "ACTIVO" : "INACTIVO") : a.Valor
                            }
                    })
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
