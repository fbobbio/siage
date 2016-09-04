using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAGE.UI_Common.Content;

namespace SIAGE.UI_Common.Controllers
{
    public abstract class AjaxAbmcDetalleController<TModel, TRule> : Controller
    {
        /// <summary>
        /// Nombre de la control de usuario o template (*.ascx) que contiene el ABMC del modelo
        /// </summary>
        public string AbmcView { get; set; }

        /// <summary>
        /// Instancia de la regla de negocio
        /// </summary>
        public TRule Rule { get; set; }

        #region Solicitudes GET

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public virtual ActionResult Registrar()
        {
            return ProcesarAbmGet(null, EstadoABMC.Registrar);
        }

        [HttpGet]
        public virtual ActionResult Ver(int id)
        {
            return ProcesarAbmGet(id, EstadoABMC.Ver);
        }

        [HttpGet]
        public virtual ActionResult Editar(int id)
        {
            return ProcesarAbmGet(id, EstadoABMC.Editar);
        }

        [HttpGet]
        public virtual ActionResult Eliminar(int id)
        {
            return ProcesarAbmGet(id, EstadoABMC.Eliminar);
        }

        [HttpGet]
        public virtual ActionResult Seleccionar(int id)
        {
            return ProcesarAbmGet(id, EstadoABMC.Seleccionar);
        }

        [HttpGet]
        public virtual ActionResult Reactivar(int id)
        {
            return ProcesarAbmGet(id, EstadoABMC.Reactivar);
        }

        /// <summary>
        /// Por defecto, crear una instancia del model (Registrar) u obtiene una instancia del mismo desde la regla de negocio
        /// y lo retorna a la vista parcial especificando el tipo de solicitud.
        /// Puede ser sobreescrito para comportarse de otra manera, pero es necesario tener en cuenta que este método procesa
        /// todas las solicitudes GET (Registrar/Editar/Eliminar/Seleccionar/Reactivar)
        /// </summary>
        /// <param name="id">ID del objeto seleccionado en la tabla. Null en caso de Registrar</param>
        /// <param name="estado">Acción que se procesa (Registrar/Editar/Eliminar/...)</param>
        /// <returns>Vista parcial (control de usario o template que contiene el formulario de ABMC) que se refresca con AJAX en la vista</returns>
        public virtual ActionResult ProcesarAbmGet(int? id, EstadoABMC estado)
        {
            ViewData[AjaxAbmc.EstadoText] = estado.ToString();
            ViewData[AjaxAbmc.EstadoId] = (int)estado;

            CargarViewData(estado);

            TModel model = Activator.CreateInstance<TModel>();
            if (id.HasValue)
                model = Util.ReflectionUtil.GetById<TModel>(Rule, id.Value);


            return PartialView(AbmcView, model);
        }

        public virtual void CargarViewData(EstadoABMC estado) { }

        #endregion

        #region Solicitudes POST

        [HttpPost]
        public virtual ActionResult Registrar(TModel model, int idPadre)
        {
            return GenericPost(RegistrarPost, model, idPadre, EstadoABMC.Registrar);
        }

        /// <summary>
        /// Metodo intermedio entre la recepción de la solicitud y la estructura completa del procesamiento.
        /// Contiene lógica de negocio (interacción con las reglas). 
        /// Evitar realizar validaciones de modelo, control de errores, etc.
        /// Sólo se accede a este método cuando el modelo es válido.
        /// </summary>
        /// <param name="model">Modelo que contiene los valores inrgesados desde los formularios de la vista</param>
        public virtual void RegistrarPost(TModel model, int idPadre) { }

        [HttpPost]
        public virtual ActionResult Editar(TModel model, int idPadre)
        {
            return GenericPost(EditarPost, model, idPadre, EstadoABMC.Editar);
        }

        public virtual void EditarPost(TModel model, int idPadre) { }

        [HttpPost]
        public virtual ActionResult Eliminar(TModel model, int idPadre)
        {
            return GenericPost(EliminarPost, model, idPadre, EstadoABMC.Eliminar);
        }

        public virtual void EliminarPost(TModel model, int idPadre) { }

        [HttpPost]
        public virtual ActionResult Seleccionar(TModel model, int idPadre)
        {
            return GenericPost(SeleccionarPost, model, idPadre, EstadoABMC.Seleccionar);
        }

        public virtual void SeleccionarPost(TModel model, int idPadre) { }

        [HttpPost]
        public virtual ActionResult Reactivar(TModel model, int idPadre)
        {
            return GenericPost(ReactivarPost, model, idPadre, EstadoABMC.Reactivar);
        }

        public virtual void ReactivarPost(TModel model, int idPadre) { }

        #endregion

        #region Procesamiento Generico

        /// <summary>
        /// Inicializa los objetos que son seleccionados desde combos o listas de seleccion multiple.
        /// Debería buscar cada uno de los objetos por su Id y asignarlo a la entidad de dominio.
        /// </summary>
        /// <param name="model">Modelo que contiene los valores inrgesados desde los formularios de la vista</param>
        public virtual void CargarObjetos(TModel model) { }

        /// <summary>
        /// Define la estructura general de los metodos de procesamiento de las solicitudes POST.
        /// Verifica que el modelo sea valido, es decir, que no contenga errores provenientes de los Decorator definidos en el modelo.
        /// Inicializa los objetos que provienen de la seleccion en combos o listas.
        /// Invoca al metodo [Accion]Post que contiene el proceso particular para la solicitud.
        /// Carga los errores que se producen en las capas inferiores de la aplicación.
        /// Si la operación es exitosa, devuelve la vista en modo Solo Lectura, y sino muestra el listado de errores 
        /// </summary>
        /// <param name="metodo">Metodo sin retorno (void) que recibe como único parámetro el modelo proveniente de la vista, y que lleva a cabo el procesamiento particular según el tipo de solicitud (estado)<param>
        /// <param name="model">Modelo que contiene los valores inrgesados desde los formularios de la vista</param>
        /// <param name="idPadre">Id de la entidad que contiene al detalle que se va a editar</param>
        /// <param name="estadoInicial">Acción que se procesa (Registrar/Editar/Eliminar/...)</param>
        /// <returns>Vista parcial (control de usario o template que contiene el formulario de ABMC) que se refresca con AJAX en la vista</returns>
        public ActionResult GenericPost(Action<TModel, int> metodo, TModel model, int idPadre, EstadoABMC estadoInicial)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CargarObjetos(model);
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

            return Json(new { status = false, details = errores.ToArray() });
        }

        #endregion
    }
}
