using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;

namespace SIAGE_Ministerio.Controllers
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
            ViewData["EstadosPuesto"] = Rule.GetAllEstadosPuesto();
            ViewData["EstadosAsignacion"] = Rule.GetAllEstadosAsignacion();

        }



        public override ActionResult Registrar(GrupoMabModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var modelReturned = Rule.GrupoMabSave(model);
                    //         var propiedades = ['Id', 'NumeroGrupo', 'TipoGrupo', 'nCodigosMovimiento'];
                    ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Registrar.ToString();
                    ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Registrar;
                  
                    return Json(new
                    {
                        status = true,
                        model = new
                        {
                            Id=modelReturned.Id,
                            NumeroGrupo=modelReturned.NumeroGrupo,
                            TipoGrupo=modelReturned.TipoGrupo.ToString(),
                            nCodigosMovimiento = Rule.GetCantidadCodigosMovimientoMab(modelReturned.Id)
                           
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Registrar.ToString();
                    ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Registrar;
                }
            }
            catch (Exception e)
            {
                ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Registrar.ToString();
                ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Registrar;

                while (e.InnerException != null)
                    e = e.InnerException;

                ModelState.AddModelError(string.Empty, e.Message);
            }

            var errores = new List<string>();
            for (int i = 0; i < ModelState.Values.Count; i++)
            {
                var propiedad = ModelState.Values.ElementAt(i);
                if (propiedad.Errors.Count != 0)
                {
                    errores.AddRange(propiedad.Errors.Select(item => string.IsNullOrEmpty(item.ErrorMessage) ? item.Exception.Message : item.ErrorMessage));
                }
            }

            return Json(new { status = false, details = errores.ToArray() }, JsonRequestBehavior.AllowGet);

            
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
        [HttpPost]
        public override void EliminarPost(GrupoMabModel model)
        {
            Rule.GrupoMabDelete(model);
        }

        public override void ReactivarPost(GrupoMabModel model)
        {
            //Rule.CargoMinimoReactivar(model);
        }

        public JsonResult GetEstadoPuestosPorEjecucionMabId(int ejecucionMabId)
        {
            
            IList<EstadoPuestoDeTrabajoEnum> registros = Rule.GetEstadoPuestosPorEjecucionMabId(ejecucionMabId);
            IList lista = new List<object>();
            var i = 0;
            foreach (var estadoPuestoDeTrabajoEnum in registros)
            {
                var EPT = new
                              {
                                  Id = i,
                                  EstadoAnteriorPTtext = estadoPuestoDeTrabajoEnum.ToString(),
                                  IdEstadoAnteriorPT = estadoPuestoDeTrabajoEnum

                        };

                lista.Add(EPT);
                i++;
            }



            
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCodigosMovimientoByGrupoMabId(string sidx, string sord, int page, int rows, int grupoMabId)
        {
           

            var registros = Rule.GetCodigosMovimientoByGrupoMabId(grupoMabId);
            int totalRegistros = Enumerable.Count<CodigoMovimientoMabModel>(registros);
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            registros = Enumerable.ToList<CodigoMovimientoMabModel>(registros.Skip((page - 1) * rows).Take(rows));
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRegistros,
                rows = from a in registros
                       select new
                       {
                           cell = new string[]
                                {
                                    a.Id.ToString(),
                                    a.Codigo,
                                    a.Descripcion,
                                    a.Uso,
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
                registros = Enumerable.ToList<GrupoMabModel>(Enumerable.OrderBy(registros, funcOrden));
            else
                registros = Enumerable.ToList<GrupoMabModel>(Enumerable.OrderByDescending(registros, funcOrden));

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = Enumerable.Count<GrupoMabModel>(registros);
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            registros = Enumerable.ToList<GrupoMabModel>(Enumerable.Take<GrupoMabModel>(registros.Skip((page - 1) * rows), rows));

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
