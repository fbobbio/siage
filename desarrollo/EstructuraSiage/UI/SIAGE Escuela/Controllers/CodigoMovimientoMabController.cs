using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Controllers;
using Enumerable = System.Linq.Enumerable;

namespace SIAGE_Escuela.Controllers
{
    /** 
    * <summary> Clase de implementación del controlador CodigoMovimientoMabController
    *	
    * </summary>
    * <remarks>
    *		Autor: Ale
    *		Fecha: 6/27/2011 5:16:03 PM
    * </remarks>
    */
    public class CodigoMovimientoMabController : AjaxAbmcController<CodigoMovimientoMabModel, ICodigoMovimientoMabRules>
    {

        /** Región para la declaración de atributos de clase */
        #region Atributos
        #endregion

        /** Región para declarar la inicialización del controller */
        #region Inicialización

     

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            
            base.Initialize(requestContext);
            AbmcView = "CodigoMovimientoMabEditor";
            
            Rule = ServiceLocator.Current.GetInstance<ICodigoMovimientoMabRules>();
            //ViewData["Usos"] = Rule.GetAllUsosCodigoMovimientoMab();
            
            #region Inicialización de Combos

            #endregion
        }
        #endregion
        
        [HttpGet]
        public ActionResult Desvincular()
        {
            return PartialView("DesvincularCodigoMovimientoEditor");
        }
        [HttpPost]
        public JsonResult Desvincular(int IdCodigo)
        {
            try
            {
                Rule.DesvincularCodigoMovimientoMABdeGrupoMab(IdCodigo);
            }
            catch (Exception e)
            {
                return Json(new { status = false, details = e.Message});
            }
            return Json(new {status = true});
        }

        public JsonResult GetAllCodigosMovimientoMab(string sidx, string sord, int page, int rows)
       {
           var registros = Rule.GetAllCodigosMovimientoMabSinGrupoMabAsignado();
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
      
        public override void RegistrarPost(CodigoMovimientoMabModel model)
        {
            Rule.CodigoMovimientoMabSave(model);
        }

        public override void EditarPost(CodigoMovimientoMabModel model)
        {
            if(string.IsNullOrEmpty(model.Uso))
                throw new ApplicationException("Falta el campo Uso(*)");
            Rule.CodigoMovimientoMabSave(model);
        }

        public JsonResult TieneAsociadoGrupoMab(int idCodigo)
        {
            return Json(Rule.TieneAsociadoMab(idCodigo), JsonRequestBehavior.AllowGet);
        }





        public override void EliminarPost(CodigoMovimientoMabModel model)
        {
            Rule.EliminarCodigoMovimientoMab(model);
        }
        /** Región para cuando se use ABMCMixto. Declaraciones de métodos POST de los detalles (Agregar, Editar y Eliminar) */
        #region Post Detalle
        #endregion

        /** Región para declarar métodos de procesamiento que devuelvan JsonResults*/
        #region Procesamiento Busquedas


        //string filtroCodigo, string filtroDescripcion)

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, string filtroCodigo, string filtroDescripcion)
        {
            // Construyo la funcion de ordenamiento
            Func<CodigoMovimientoMabModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                //  propiedades = ['codigo', 'descripcion', 'uso', 'nGrupo', 'tGrupo'];
                sidx == "codigo" ? x => x.Codigo :
                sidx == "descripcion" ? x => x.Descripcion :
                sidx == "uso" ? x => x.Uso :
                sidx == "nGrupo" ? x => Rule.GetGrupoMabById((int)(x.GrupoMabId.HasValue ? x.GrupoMabId : 0)).NumeroGrupo :
                sidx == "tGrupo" ? x => Rule.GetGrupoMabById((int)(x.GrupoMabId.HasValue ? x.GrupoMabId : 0)).TipoGrupo.ToString() :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<CodigoMovimientoMabModel, IComparable>)(x => x.Id);


            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetByFiltro(filtroCodigo, filtroDescripcion);
            /******************************** FIN AREA EDITABLE *******************************/

            // Ordeno los registros
            if (sord == "asc")
                registros = Enumerable.ToList<CodigoMovimientoMabModel>(registros.OrderBy(funcOrden));
            else
                registros = Enumerable.ToList<CodigoMovimientoMabModel>(registros.OrderByDescending(funcOrden));

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = Enumerable.Count<CodigoMovimientoMabModel>(registros);
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            registros = Enumerable.ToList<CodigoMovimientoMabModel>(registros.Skip((page - 1) * rows).Take(rows));

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
                                //  propiedades = ['codigo', 'descripcion', 'uso', 'nGrupo', 'tGrupo'];
                                a.Codigo,
                                a.Descripcion,
                                a.Uso,
                                a.GrupoMabId != 0 ?  Rule.GetGrupoMabById((int)(a.GrupoMabId.HasValue ? a.GrupoMabId : 0)).NumeroGrupo.ToString():"-",
                                //Rule.GetGrupoMabById(a.GrupoMabId).NumeroGrupo.ToString() ? null :"-",
                                //Rule.GetGrupoMabById(a.GrupoMabId).TipoGrupo.ToString() 
                                a.GrupoMabId != 0 ?  Rule.GetGrupoMabById((int)(a.GrupoMabId.HasValue ? a.GrupoMabId : 0)).TipoGrupo.ToString(): "-"
                                /******************************** FIN AREA EDITABLE *******************************/
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /** Región para la declaración de métodos de validación y soporte en general */
        #region Soporte
        #endregion
    }
}