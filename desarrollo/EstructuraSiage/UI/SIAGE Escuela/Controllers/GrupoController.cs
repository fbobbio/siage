using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using Siage.UCControllers;
using Siage.Base;
using SIAGE.UI_Common.Controllers;

namespace SIAGE_Escuela.Controllers
{
    public class GrupoController : AjaxAbmcMixtoController<GrupoModel, IGrupoRules, SubGrupoModel, ISubGrupoRules>
    {
       
        private IEntidadesGeneralesRules entidadesGeneralesRules;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            entidadesGeneralesRules = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            Rule = ServiceLocator.Current.GetInstance<IGrupoRules>();
            AbmcView = "GrupoEditor";

            RuleDetalle = ServiceLocator.Current.GetInstance<ISubGrupoRules>();
            AbmcViewDetalle = "SubGrupoEditor";
            ViewData["CicloEducativoList"] = new SelectList(ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetCicloEducativoAll(), "Id", "Descripcion");
        }

        public override void RegistrarPost(GrupoModel model)
        {
            Rule.GrupoSave(model);
        }
        public override void EditarPost(GrupoModel model)
        {
            Rule.GrupoSave(model);
        }

        public override void EliminarPost(GrupoModel model)
        {
            Rule.GrupoDelete(model);
        }

        #region POST Detalle

        public  void RegistrarSubGrupoPost(SubGrupoModel model, int idPadre)
        {
            Rule.SubGrupoSave(model, idPadre);
        }

        public  void EditarSubGrupoPost(SubGrupoModel model, int idPadre)
        {
            Rule.SubGrupoUpdate(model, idPadre);
        }

        public  void EliminarSubGrupoPost(SubGrupoModel model, int idPadre)
        {
            //En teoria no se puede eliminar
            Rule.SubGrupoUpdate(model, idPadre);
        }

        #endregion

        [HttpPost]
        public void GuardarGrupo(GrupoModel model)
        {

            Rule.GrupoSave(model);

        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, string filtroNombre)
        {
            Func<GrupoModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Nombre" ? x => x.Nombre :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<GrupoModel, IComparable>)(x => x.Id);
            //    // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetSubGrupoByFiltros(filtroNombre);
            //    /******************************** FIN AREA EDITABLE *******************************/

            // Ordeno los registros
            if (sord == "asc")
                registros = registros.OrderBy(funcOrden).ToList();
            else
                registros = registros.OrderByDescending(funcOrden).ToList();

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
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
                            // Respetar el orden en que se mostrarán las columnas
                           /****************************** INICIO AREA EDITABLE ******************************/
                            a.Nombre
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarDetalle(string sidx, string sord, int page, int rows, int id)
        {
            Func<SubGrupoModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                 sidx == "Nombre" ? x => x.Nombre :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<SubGrupoModel, IComparable>)(x => x.Id);
            //    // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = RuleDetalle.GetSubGrupoByIdGrupo(id);
            //    /******************************** FIN AREA EDITABLE *******************************/

            // Ordeno los registros)
            if (sord == "asc")
                registros = registros.OrderBy(funcOrden).ToList();
            else
                registros = registros.OrderByDescending(funcOrden).ToList();

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
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
                            // Respetar el orden en que se mostrarán las columnas
                            /****************************** INICIO AREA EDITABLE ******************************/
                            a.Nombre,
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }       
}
