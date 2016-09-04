using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Util;

namespace SIAGE.UI_Common.Controllers
{
	public abstract class AjaxAbmcController<TModel, TRule> : Controller
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

		[HttpGet]
		public virtual ActionResult Historial(int id)
		{
			return ProcesarAbmGet(id, EstadoABMC.Historial);
		}

        [HttpGet]
        public ActionResult Imprimir()
        {
            return ViewPdf(TempData["Imprimir"] as AjaxAbmcImprimirModel);
        }

        [HttpPost]
        public void ImprimirData(AjaxAbmcImprimirModel model)
        {
            TempData["Imprimir"] = model;
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
		        model = ReflectionUtil.GetById<TModel>(Rule, id.Value);


		    return PartialView(AbmcView, model);
		}

        public virtual void CargarViewData(EstadoABMC estado) {}

	    #endregion

		#region Solicitudes POST

		[HttpPost]
		public virtual ActionResult Registrar(TModel model)
		{
			return GenericPost(RegistrarPost, model, EstadoABMC.Registrar);
		}

		/// <summary>
		/// Metodo intermedio entre la recepción de la solicitud y la estructura completa del procesamiento.
		/// Contiene lógica de negocio (interacción con las reglas). 
		/// Evitar realizar validaciones de modelo, control de errores, etc.
		/// Sólo se accede a este método cuando el modelo es válido.
		/// </summary>
		/// <param name="model">Modelo que contiene los valores inrgesados desde los formularios de la vista</param>
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

		[HttpPost]
		public virtual ActionResult Historial(TModel model)
		{
			return GenericPost(ReactivarPost, model, EstadoABMC.Reactivar);
		}

		public virtual void HistorialPost(TModel model) { }

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
		/// <param name="estadoInicial">Acción que se procesa (Registrar/Editar/Eliminar/...)</param>
		/// <returns>Vista parcial (control de usario o template que contiene el formulario de ABMC) que se refresca con AJAX en la vista</returns>
		public ActionResult GenericPost(Action<TModel> metodo, TModel model, EstadoABMC estadoInicial)
		{
			try
			{
				if (ModelState.IsValid)
				{
					CargarObjetos(model);
					metodo.DynamicInvoke(model);

					ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Revisar.ToString();
					ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Revisar;

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
		
		#region Soporte

		protected JsonResult CustomJson<TEntity>(TEntity data)
		{
			var writer = new StringWriter();
			var serializer = new JsonSerializer();
			serializer.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
			serializer.Converters.Add(new StringEnumConverter());
			serializer.NullValueHandling = NullValueHandling.Ignore;
			serializer.Serialize(writer, data);

			return Json(Convert.ToString(writer), JsonRequestBehavior.AllowGet);
		}

        protected ActionResult ViewPdf(object model)
        {
            // Create the iTextSharp document.
            Document doc = new Document(PageSize.A4, 50, 50, 50, 50);
            // Set the document to write to memory.
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, memStream);
            writer.CloseStream = false;
            doc.Open();

            // Render the view xml to a string, then parse that string into an XML dom.
            string xmltext = RenderActionResultToString(View(model));
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.InnerXml = xmltext.Trim();

            // Parse the XML into the iTextSharp document.
            ITextHandler textHandler = new ITextHandler(doc);
            textHandler.Parse(xmldoc);

            // Close and get the resulted binary data.
            doc.Close();
            byte[] buf = new byte[memStream.Position];
            memStream.Position = 0;
            memStream.Read(buf, 0, buf.Length);

            // Send the binary data to the browser.
            return new BinaryContentResult(buf, "application/pdf");
        }

        protected string RenderActionResultToString(ViewResult result)
        {
            result.ViewName = "AjaxAbmcImprimir";

            // Create memory writer.
            var sb = new StringBuilder();
            var memWriter = new StringWriter(sb);

            // Create fake http context to render the view.
            var fakeResponse = new HttpResponse(memWriter);
            var fakeContext = new HttpContext(System.Web.HttpContext.Current.Request,
                fakeResponse);
            var fakeControllerContext = new ControllerContext(
                new HttpContextWrapper(fakeContext),
                ControllerContext.RouteData,
                ControllerContext.Controller);
            var oldContext = System.Web.HttpContext.Current;
            System.Web.HttpContext.Current = fakeContext;

            // Render the view.
            result.ExecuteResult(fakeControllerContext);

            // Restore old context.
            System.Web.HttpContext.Current = oldContext;

            // Flush memory and return output.
            memWriter.Flush();
            return sb.ToString();
        }

		#endregion
	}
}