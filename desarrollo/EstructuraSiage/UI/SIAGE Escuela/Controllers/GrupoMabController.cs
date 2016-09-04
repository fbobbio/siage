using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;

namespace SIAGE_Escuela.Controllers
{
    public class GrupoMabController : AjaxAbmcController<GrupoMabModel, IGrupoMabRules>
    {
        //
        // GET: /GrupoMab/

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "GrupoMabEditor";
            Rule = ServiceLocator.Current.GetInstance<IGrupoMabRules>();
        }

        public override void CargarViewData(SIAGE.UI_Common.Content.EstadoABMC estado)
        {
            ViewData.Add(ViewDataKey.ESTADO_PUESTO.ToString(), Rule.GetAllEstadosPuesto());
            ViewData.Add(ViewDataKey.ESTADO_ASIGNACION.ToString(), Rule.GetAllEstadosAsignacion());
        }

        public override ActionResult Index()
        {
            CargarViewData(EstadoABMC.Consultar);
            return View();
        }

        public override void RegistrarPost(GrupoMabModel model)
        {
            Rule.GrupoMabSave(model);
        }

        public override void EditarPost(GrupoMabModel model)
        {
            Rule.GrupoMabSave(model);
        }

        public override ActionResult Eliminar(GrupoMabModel model)
        {
            return base.Eliminar(model);
        }

        public override void EliminarPost(GrupoMabModel model)
        {
            Rule.GrupoMabDelete(model);
        }

        public override void ReactivarPost(GrupoMabModel model)
        {
            //Rule.CargoMinimoReactivar(model);
        }

        public JsonResult GetCodigosMovimientoByGrupoMabId(string sidx, string sord, int page, int rows, int grupoMabId)
        {
            List<CodigoMovimientoMabModel> resultados = Rule.GetCodigosMovimientoByGrupoMabId(grupoMabId);
            //return Json((resultados.Select(p => new { Id = p.Id, Codigo = p.Codigo, Descripcion = p.Descripcion, Uso = p.Uso, GrupoMabId = p.GrupoMabId })).ToList(), JsonRequestBehavior.AllowGet);
            int totalRegistros = resultados.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            resultados = resultados.Skip((page - 1) * rows).Take(rows).ToList();
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRegistros,
                rows = from a in resultados
                       select new
                       {
                           cell = new string[]
                            {
                                a.Id.ToString(),
                                a.Codigo,
                                a.Descripcion,
                                a.Uso.ToString(),
                                a.GrupoMabId.ToString()
                            }
                       }
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, TipoGrupoMabEnum? filtroTipoGrupoMab, int? filtroNumeroGrupoMab, string filtroCodigoMovimientoMab)
        {
            // Construyo la funcion de ordenamiento
            Func<GrupoMabModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                 //var propiedades = ['Id', 'nGrupo', 'tGrupo', 'nCodigosMovimiento'];
                sidx == "NumeroGrupo" ? x => x.NumeroGrupo :
                sidx == "TipoGrupo" ? x => x.TipoGrupo :
                sidx == "nCodigosMovimiento" ? x => Rule.GetCantidadCodigosMovimientoMab(x.Id) :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<GrupoMabModel, IComparable>)(x => x.Id);


            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetGrupoMabByFiltros(filtroTipoGrupoMab,filtroNumeroGrupoMab,filtroCodigoMovimientoMab);
            /******************************** FIN AREA EDITABLE *******************************/

            // Ordeno los registros)
            if (sord == "asc")
                registros = Enumerable.ToList<GrupoMabModel>(registros.OrderBy(funcOrden));
            else
                registros = Enumerable.ToList<GrupoMabModel>(registros.OrderByDescending(funcOrden));

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = Enumerable.Count<GrupoMabModel>(registros);
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            registros = Enumerable.ToList<GrupoMabModel>(registros.Skip((page - 1) * rows).Take(rows));

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
                                // Respetar el orden en que se mostrarán las columnas
                                /****************************** INICIO AREA EDITABLE ******************************/
                                   //var propiedades = ['Id', 'nGrupo', 'tGrupo', 'NCodigosMovimiento'];
                                a.NumeroGrupo.ToString(),
                                a.TipoGrupo.ToString(),
                                Rule.GetCantidadCodigosMovimientoMab(a.Id).ToString()
                                
                                /******************************** FIN AREA EDITABLE *******************************/
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
      

    }
}
