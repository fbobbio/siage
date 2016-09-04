using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siage.Base;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using SIAGE.UI_Common.Controllers;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class DocumentoRequeridoController : AjaxAbmcController<DocumentoRequeridoModel, IDocumentoRequeridoRules>
    {
        private int idEmpresa;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "DocumentoRequeridoEditor";
            Rule = ServiceLocator.Current.GetInstance<IDocumentoRequeridoRules>();
            ViewData["ComboVacio"] = new SelectList(new List<object>());
            idEmpresa = (int)Session[ConstantesSession.EMPRESA.ToString()];
        }

        public override ActionResult Index()
        {
            if (!ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(idEmpresa)) // validar si estoy logueado como escuela
            {
                TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela.";
                return RedirectToAction("Error", "Home");
            }

            CargarViewData();
            return base.Index();
        }

        private void CargarViewData()
        {
            var procesoModels = new List<ProcesoModel>();
            var lstGradoAnio = new List<GradoAñoModel>();
            var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();
            // Si la empresa es Direccion de nivel
            if (empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE || empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
            {
                //Obtengo los niveles educativos de la empresa del usuario logeado
                var niveles = ServiceLocator.Current.GetInstance<INivelEducativoRules>().GetNivelesEducativosByEmpresa(empresa.Id);
                lstGradoAnio = ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetGradoAñoByDN(empresa.Id);

                foreach (var nivel in niveles)
                {
                    var procesos = ServiceLocator.Current.GetInstance<IProcesoRules>().GetProcesosByNivelEducativo(nivel.Id);
                    foreach (var procesoModel in procesos)
                    {
                        procesoModels.Add(procesoModel);
                    }
                }

                //Para el Editor
                //Obtengo los niveles educativos de la empresa del usuario logeado
                ViewData["NivelesEditorList"] = new SelectList(niveles, "Id", "Nombre");
            }

            //List<GradoAñoModel> lstGradoAnio = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetGradoAñoAll();
            procesoModels = (from p in procesoModels select p).Distinct().ToList();

            lstGradoAnio.Add(new GradoAñoModel { Id = -1, Nombre = "(Todos)" });
            ViewData["GradoAnioList"] = (new SelectList(lstGradoAnio.OrderBy(x => x.Id), "Id", "Nombre", -1));

            var lstProcesos = (from p in procesoModels select new { id = p.Id.ToString(), nombre = p.Nombre }).ToList();
            //var lstProcesos = (from p in Enum.GetNames(typeof(ProcesoEnum))
            //                   orderby p
            //                   select
            //                       new { id = p, nombre = p.Replace('_', ' ') }).ToList();
            lstProcesos.Insert(0, new { id = "", nombre = "(Todos)" });
            ViewData["ProcesoList"] = new SelectList(lstProcesos, "id", "nombre", "");
        }

        public JsonResult GradosAniosPorNivel(int idNivel)
        {
            var gradosAniosEditorList = ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetGradoAñoByNivelEducativo(idNivel);
            return Json(gradosAniosEditorList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesosPorNivel(int idNivel)
        {
            var procesosEditorList = ServiceLocator.Current.GetInstance<IProcesoRules>().GetProcesosByNivelEducativo(idNivel);
            return Json(procesosEditorList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DocumentosPorNivelXCarreraXProcesoXGradoAnio(int idNivel, int? idCarrera, int idProceso, int idGradoAnio)
        {
            var procesosEditorList = new SelectList(ServiceLocator.Current.GetInstance<IDocumentoRules>().DocumentoGetAll(), "Id", "Nombre");
            return Json(procesosEditorList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DocumentosRequeridosPorNivelXCarreraXProcesoXGradoAnio(int idNivel, int? idCarrera, int idProceso, int idGradoAnio)
        {
            var res = (from doc in Rule.DocumentoRequeridoByFiltros(idNivel, idCarrera, idProceso, idGradoAnio)
                       select new
                       {
                           Id = doc.Id,
                           Documento = doc.Documento.Nombre,
                           Proceso = doc.Proceso,
                           GradoAnio = doc.GradoAnio,
                           Nivel = doc.Nivel.Nombre,
                           Carrera = doc.Carrera.Nombre
                       }).ToList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CarrerasPorNivel(int idNivel)
        {
            var carrerasList = ServiceLocator.Current.GetInstance<ICarreraRules>().GetCarrerasVigentesByNivel(idNivel);
            return Json(carrerasList, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Metodo que recibe desde la vista todos los parametros necesarios para la obtención de los registros a mostrar, filtrarlos y paginados.
        /// A partir del parámetro id (sin incluirlo), los parámetros siguientes son opcionales y dependientes del caso de uso.
        /// </summary>
        /// <param name="sidx">Campo por el cual se ordenan los registros</param>
        /// <param name="sord">Dirección de ordenamiento (Ascendente/Descendente)</param>
        /// <param name="page">Número de página a mostrar</param>
        /// <param name="rows">Cantidad de registros por página</param>
        /// <param name="id">Valor de filtrado por ID</param>
        /// <returns>Objeto JSON que representa la matriz de datos a ser mostrados en la grilla</returns>
        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, string filtroProceso, string filtroGradoAnio)
        {
            // Construyo la funcion de ordenamiento
            Func<DocumentoRequeridoModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Proceso" ? x => x.Proceso :
                sidx == "Grado/Año" ? x => x.GradoAnio :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<DocumentoRequeridoModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            ProcesoEnum proc;
            ProcesoEnum.TryParse(filtroProceso, out proc);

            var registros = Rule.DocumentoRequeridoDireccionDeNivelByFiltros(proc, int.Parse(filtroGradoAnio));
            /******************************** FIN AREA EDITABLE *******************************/

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
                            a.Documento.Nombre,
                            a.Documento.Descripcion,
                            a.Proceso.ToString(),
                            a.GradoAnio                         
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Metodo que recibe desde la vista todos los parametros necesarios para la obtención de los registros a mostrar, filtrarlos y paginados.
        /// A partir del parámetro id (sin incluirlo), los parámetros siguientes son opcionales y dependientes del caso de uso.
        /// </summary>
        /// <param name="sidx">Campo por el cual se ordenan los registros</param>
        /// <param name="sord">Dirección de ordenamiento (Ascendente/Descendente)</param>
        /// <param name="page">Número de página a mostrar</param>
        /// <param name="rows">Cantidad de registros por página</param>
        /// <param name="id">Valor de filtrado por ID</param>
        /// <returns>Objeto JSON que representa la matriz de datos a ser mostrados en la grilla</returns>
        public JsonResult ProcesarBusquedaDocumentos(string sidx, string sord, int page, int rows, int idNivel, int? idCarrera, int idProceso, int idGradoAnio)
        {
            // Construyo la funcion de ordenamiento
            Func<DocumentoRequeridoModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Proceso" ? x => x.Proceso :
                sidx == "Grado/Año" ? x => x.GradoAnio :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<DocumentoRequeridoModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.DocumentoRequeridoByFiltros(idNivel, idCarrera, idProceso, idGradoAnio);
            /******************************** FIN AREA EDITABLE *******************************/

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
                            a.DocumentoId.ToString(), 
                            // Respetar el orden en que se mostrarán las columnas
                            /****************************** INICIO AREA EDITABLE ******************************/
                            a.Documento.Nombre,
                            a.EsObligatorio ? "Si" : "No",
                            a.Observaciones,
                            a.Id.ToString()                   
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult RegistrarDocumentoRequerido(List<DocumentoRequeridoModel> modelo)
        //{
        //    try
        //    {
        //        Rule.DocumentoRequeridoSave(modelo);
        //        return Json(new {status = true}, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception e)
        //    {
        //        while (e.InnerException != null)
        //            e = e.InnerException;

        //        return Json(new { status = false, details = e.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public JsonResult VincularDocumento(DocumentoRequeridoModel modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var modeloGuardado = Rule.DocumentoRequeridoSave(modelo, idEmpresa);
                    return Json(new { model = modeloGuardado }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var errores = new List<string>();
                    for (var i = 0; i < ModelState.Values.Count; i++)
                    {
                        var propiedad = ModelState.Values.ElementAt(i);
                        if (propiedad.Errors.Count != 0)
                        {
                            errores.AddRange(
                                propiedad.Errors.Select(
                                    item =>
                                    string.IsNullOrEmpty(item.ErrorMessage) ? item.Exception.Message : item.ErrorMessage));
                        }
                    }

                    return Json(new { status = false, details = errores }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                    e = e.InnerException;

                return Json(new { status = false, details = new List<string> { e.Message } }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DesvincularDocumento(int id)
        {
            try
            {
                Rule.DocumentoRequeridoDarDeBaja(id);
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                    e = e.InnerException;

                return Json(new { status = false, details = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}