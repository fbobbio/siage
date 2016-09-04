using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAGE.UI_Common.Content;
namespace SIAGE.UI_Common.Controllers
{
    public abstract class AjaxAbmcMixtoController<TModel, TRule, TModelDetalle, TRuleDetalle> : Controller
    {
        public string AbmcView { get; set; }
        public string AbmcViewDetalle { get; set; }

        public TRule Rule { get; set; }
        public TRuleDetalle RuleDetalle { get; set; }

        #region Solicitudes GET

        [HttpGet]
        public virtual ActionResult Index()
        {
           
            return View();
        }

        [HttpGet]
        public virtual ActionResult Registrar(bool detalle)
        {
            return ProcesarAbmGet(null, detalle, EstadoABMC.Registrar);
        }

        [HttpGet]
        public virtual ActionResult Ver(int id, bool detalle)
        {
            return ProcesarAbmGet(id, detalle, EstadoABMC.Ver);
        }

        [HttpGet]
        public virtual ActionResult Editar(int id, bool detalle)
        {
            return ProcesarAbmGet(id, detalle, EstadoABMC.Editar);
        }

        [HttpGet]
        public virtual ActionResult Eliminar(int id, bool detalle)
        {
            return ProcesarAbmGet(id, detalle, EstadoABMC.Eliminar);
        }
        
        public virtual ActionResult ProcesarAbmGet(int? id, bool detalle, EstadoABMC estado)
        {
            ViewData[AjaxAbmc.EstadoText] = estado.ToString();
            ViewData[AjaxAbmc.EstadoId] = (int)estado;

            if (detalle)
            {
                CargarViewDataDetalle(estado);

                TModelDetalle model = Activator.CreateInstance<TModelDetalle>();
                if (id.HasValue)
                    model = Util.ReflectionUtil.GetById<TModelDetalle>(RuleDetalle, id.Value);
                return PartialView(AbmcViewDetalle, model);
            }
            else
            {
                CargarViewData(estado);

                TModel model = Activator.CreateInstance<TModel>();
                if (id.HasValue)
                    model = Util.ReflectionUtil.GetById<TModel>(Rule, id.Value);
                return PartialView(AbmcView, model);
            }
        }

        public virtual void CargarViewData(EstadoABMC estado) { }
        public virtual void CargarViewDataDetalle(EstadoABMC estado) { }

        #endregion 
                
        #region Solicitudes POST 

        [HttpPost]
        public virtual ActionResult Registrar(TModel model)
        {
            return GenericPost(RegistrarPost, model, EstadoABMC.Registrar);
        }

        public virtual void RegistrarPost(TModel model) { }

        [HttpPost]
        public virtual ActionResult Editar(TModel model)
        {
            return GenericPost(EditarPost, model, EstadoABMC.Editar);
        }

        public virtual void EditarPost(TModel model) { }

        [HttpPost]
        public virtual ActionResult Eliminar(TModel model)
        {
            return GenericPost(EliminarPost, model, EstadoABMC.Eliminar);
        }

        public virtual void EliminarPost(TModel model) { }

        [HttpPost]
        public virtual ActionResult Seleccionar(TModel model)
        {
            return GenericPost(SeleccionarPost, model, EstadoABMC.Seleccionar);
        }

        public virtual void SeleccionarPost(TModel model) { }

        [HttpPost]
        public virtual ActionResult Reactivar(TModel model)
        {
            return GenericPost(ReactivarPost, model, EstadoABMC.Reactivar);
        }

        public virtual void ReactivarPost(TModel model) { }

        #endregion

        #region Solicitudes POST Detalle

        [HttpPost]
        public virtual ActionResult RegistrarDetalle(TModelDetalle model, int idPadre)
        {
            return GenericPostDetalle(RegistrarDetallePost, model, idPadre, EstadoABMC.Registrar);
        }

        public virtual void RegistrarDetallePost(TModelDetalle model, int idPadre) { }

        [HttpPost]
        public virtual ActionResult EditarDetalle(TModelDetalle model, int idPadre)
        {
            return GenericPostDetalle(EditarDetallePost, model, idPadre, EstadoABMC.Editar);
        }

        public virtual void EditarDetallePost(TModelDetalle model, int idPadre) { }

        [HttpPost]
        public virtual ActionResult EliminarDetalle(TModelDetalle model, int idPadre)
        {
            return GenericPostDetalle(EliminarDetallePost, model, idPadre, EstadoABMC.Eliminar);
        }

        public virtual void EliminarDetallePost(TModelDetalle model, int idPadre) { }

        [HttpPost]
        public virtual ActionResult SeleccionarDetalle(TModelDetalle model, int idPadre)
        {
            return GenericPostDetalle(SeleccionarDetallePost, model, idPadre, EstadoABMC.Seleccionar);
        }

        public virtual void SeleccionarDetallePost(TModelDetalle model, int idPadre) { }

        [HttpPost]
        public virtual ActionResult ReactivarDetalle(TModelDetalle model, int idPadre)
        {
            return GenericPostDetalle(ReactivarDetallePost, model, idPadre, EstadoABMC.Reactivar);
        }

        public virtual void ReactivarDetallePost(TModelDetalle model, int idPadre) { }

        #endregion

        #region Procesamiento Generico

        public virtual void CargarObjetos(TModel model) { }

        public virtual void CargarObjetosDetalle(TModelDetalle model) { }

        public ActionResult GenericPost(Action<TModel> metodo, TModel model, EstadoABMC estadoInicial)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CargarObjetos(model);
                    metodo.DynamicInvoke(model);

                    ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Ver.ToString();
                    ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Ver;

                    return Json(new { status = true });
                }
                else
                {
                    ViewData[AjaxAbmc.EstadoText] = estadoInicial.ToString();
                    ViewData[AjaxAbmc.EstadoId] = (int)estadoInicial;
                }
            }
            catch (Exception e)
            {
                ViewData[AjaxAbmc.EstadoText] = estadoInicial.ToString();
                ViewData[AjaxAbmc.EstadoId] = (int)estadoInicial;

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

            return Json(new { status = false, details = errores.ToArray() });
        }

        public ActionResult GenericPostDetalle(Action<TModelDetalle, int> metodo, TModelDetalle model, int idPadre, EstadoABMC estadoInicial)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    CargarObjetosDetalle(model);
                    metodo.DynamicInvoke(model, idPadre);

                    ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Ver.ToString();
                    ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Ver;

                    return Json(new { status = true });
                }
                else
                {
                    ViewData[AjaxAbmc.EstadoText] = estadoInicial.ToString();
                    ViewData[AjaxAbmc.EstadoId] = (int)estadoInicial;
                }
            }
            catch (Exception e)
            {
                ViewData[AjaxAbmc.EstadoText] = estadoInicial.ToString();
                ViewData[AjaxAbmc.EstadoId] = (int)estadoInicial;

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

            return Json(new { status = (ModelState.IsValid), details = errores.ToArray() });
        }

        #endregion
    }
}
