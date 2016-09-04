using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class SancionController : AjaxAbmcController<SancionModel, ISancionRules>
    {
        int idEscuela;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            AbmcView = "SancionEditor";
            Rule = ServiceLocator.Current.GetInstance<ISancionRules>();
            idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
        }

        public override ActionResult Index()
        {
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(idEscuela)) // validar si estoy logueado como escuela
            {
                if (ServiceLocator.Current.GetInstance<IEmpresaRules>().GetEscuelaById(idEscuela).NivelEducativoNombre != "INICIAL")
                {
                    CargarViewData();
                    return View();
                }
                TempData[Constantes.ErrorVista] = "Opción no es válida para usuarios de nivel educativo inicial.";
                return RedirectToAction("Error", "Home");
            }
            TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela.";
            return RedirectToAction("Error", "Home");
        }

        public override ActionResult Registrar()
        {
            CargarViewData();
            return ProcesarAbmGet(null, EstadoABMC.Registrar);
        }

        public override ActionResult Ver(int id)
        {
            CargarViewData();
            return ProcesarAbmGet(id, EstadoABMC.Ver);
        }

        [HttpGet]
        public override ActionResult Editar(int id)
        {
            CargarViewData();
            return ProcesarAbmGet(id, EstadoABMC.Editar);
        }

        [HttpGet]
        public override ActionResult Eliminar(int id)
        {
            CargarViewData();
            return ProcesarAbmGet(id, EstadoABMC.Eliminar);
        }

        public override void RegistrarPost(SancionModel model)
        {
            model.IdEscuelaLogueado = idEscuela;
            Rule.SancionSave(model);
        }

        public override void EditarPost(SancionModel model)
        {
            model.IdEscuelaLogueado = idEscuela;
            Rule.SancionUpdate(model);
        }

        public override void EliminarPost(SancionModel model)
        {
            model.IdEscuelaLogueado = idEscuela;
            Rule.SancionDelete(model);
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
        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows,
            int? filtroGradoAnio,
            DivisionEnum? filtroDivision,
            int? filtroCicloLectivo,
            string filtroNroDocumento,
            SexoEnum? filtroSexo,
            string filtroNombre,
            string filtroApellido,
            int? filtroTipoSancion,
            DateTime? filtroFechaDesde,
            DateTime? filtroFechaHasta
            )
        {
            // Construyo la funcion de ordenamiento
            Func<SancionMostrarModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "TipoNombre" ? x => x.TipoNombre :
                sidx == "Fecha" ? x => x.FechaSancion :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<SancionMostrarModel, IComparable>)(x => x.Id);


            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetSancionesByFiltros(filtroGradoAnio,
                filtroDivision,
                filtroCicloLectivo,
                filtroNroDocumento,
                filtroSexo,
                filtroNombre,
                filtroApellido,
                filtroTipoSancion,
                filtroFechaDesde,
                filtroFechaHasta
                );
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
                            a.ApellidoPersona,
                            a.NombrePersona,
                            a.NumeroDocumento,
                            a.GradoAnioNombre,
                            a.DivisionNombre,
                            a.TipoNombre,
                            a.FechaSancion.ToShortDateString(),
                            a.FechaFinSancion.ToShortDateString(),
                            a.CantidadSancion.ToString()
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCantidadSancionByEstudianteId(int idEstudiante)
        {
            var cantidad = Rule.GetCantidadAmonestacionesByEstudiante(idEstudiante);
            var fecha = Rule.GetFechaFinCicloLectivo(idEscuela);
            var json2 = new
                            {
                                sancionesAnteriores = cantidad,
                                fechaFin = fecha.ToShortDateString()
                            };

            return Json(json2, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDivisionesEscuelaByGradoAnio(int idGradoAnio)
        {
            List<DivisionEnum> divisiones = ServiceLocator.Current.GetInstance<IInscripcionRules>().GetDivisionesEscuelaByGradoAnio(idGradoAnio, idEscuela);
            var items = new SelectList(
                (divisiones.Cast<object>().Select(
                    item => new { Text = item.ToString(), Value = item.ToString() })), "Value", "Text");

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public void CargarViewData()
        {
            var entidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            var escuela = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetEscuelaById(idEscuela);

            ViewData.Add(ViewDataKey.MOTIVO_BAJA_SANCION.ToString(), entidades.GetMotivoBajaSancionAll());
            ViewData.Add(ViewDataKey.MOTIVO_INCORPORACION.ToString(), entidades.GetMotivoSancionAll());
            ViewData.Add(ViewDataKey.TIPO_SANCION_ALL.ToString(), entidades.GetMotivoSancionAll());
            ViewData.Add(ViewDataKey.TIPO_SANCION.ToString(), ServiceLocator.Current.GetInstance<ISancionRules>().GetTipoSancionByNivelEducativoEscuela(idEscuela));
            ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetGradoAñoPorEscuelaLogueadaId(idEscuela));
            ViewData.Add(ViewDataKey.SEXO.ToString(), entidades.GetSexoAll());
            ViewData.Add(ViewDataKey.CICLO_LECTIVO.ToString(), ServiceLocator.Current.GetInstance<ICicloLectivoRules>().GetCicloLectivoAllByNivelEducativo(idEscuela));
            ViewData.Add(ViewDataKey.ES_SUPERIOR.ToString(), escuela.NivelEducativoNombre == NivelEducativoNombreEnum.SUPERIOR.ToString());
            ViewData.Add(ViewDataKey.COMBO_VACIO.ToString(), new List<string>());


            ViewData["EsSuperior"] = (escuela.NivelEducativoNombre == NivelEducativoNombreEnum.SUPERIOR.ToString()) ? true : false;
            //ViewData["ComboVacio"] = new SelectList(new List<string>());
            //ViewData["MotivoBajaSancionList"] = new SelectList(ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetMotivoBajaSancionAll(), "Id", "Nombre");
            //ViewData["MotivoSancionList"] = new SelectList(ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetMotivoSancionAll(), "Id", "Nombre");
            ////ViewData["CicloLectivoList"] = new SelectList(ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetCicloLectivoAll(), "Id", "AñoCiclo");
            //ViewData["TipoSancionAllList"] = new SelectList(ServiceLocator.Current.GetInstance<ISancionRules>().GetTipoSancionByNivelEducativoEscuela(idEscuela), "Id", "Nombre");
            //ViewData["GradoAnioList"] = ViewData["GradoAnioList"] ?? ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetGradoAñoPorEscuelaLogueadaId(idEscuela);
            //ViewData["TipoSancionList"] = ViewData["TipoSancionList"] ?? ServiceLocator.Current.GetInstance<ISancionRules>().GetTipoSancionByNivelEducativoEscuela(idEscuela);
            //ViewData["CicloLectivoList"] = ViewData["CicloLectivoList"] ?? ServiceLocator.Current.GetInstance<ICicloLectivoRules>().GetCicloLectivoAllByNivelEducativo(idEscuela);
            //ViewData["SexoList"] = ViewData["SexoList"] ?? ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetSexoAll();
            //ViewData["SexoBusqueda"] = new SelectList(ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetSexoAll(), "Id", "TipoSexo");
        }

        public JsonResult ValidarInscripcion(int idEstudiante)
        {
            var inscripcion = Rule.ValidarInscripcionAlumno(idEstudiante, idEscuela);
            var json2 = new
            {
                inscripto = inscripcion
            };
            return Json(json2, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarBusquedaEstudiante(string sidx, string sord, int page, int rows, int? id, string filtroSexo, string filtroDni, string filtroNombre, string filtroApellido)
        {
            Func<EstudianteModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Nombre" ? x => x.Persona.Nombre :
                sidx == "Apellido" ? x => x.Persona.Apellido :
                sidx == "Dni" ? x => x.Persona.NumeroDocumento :
                sidx == "Sexo" ? x => x.Persona.SexoNombre :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<EstudianteModel, IComparable>)(x => x.Id);
            //    // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = (ServiceLocator.Current.GetInstance<IEstudianteRules>().GetEstudianteByFiltros(filtroSexo, filtroDni, filtroNombre, filtroApellido, idEscuela));
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
                            a.Persona.Nombre,
                            a.Persona.Apellido,
                            a.Persona.SexoNombre.ToString(),
                            a.Persona.NumeroDocumento,
                            a.Persona.Sexo,
                            a.Persona.Id.ToString(),
                            a.Persona.TipoDocumento
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public SancionModel GetSancionById(int id)
        {
            return Rule.GetSancionById(id);
        }

        #region "Soporte"
        public int CalcularEdad(DateTime? fecha)
        {
            if (fecha.HasValue)
            {
                var total = DateTime.Today - fecha;
                var retorno = (int)(total.Value.TotalDays / 365);
                return retorno;
            }

            return 0;
        }

        public JsonResult HayCicloLectivo()
        {
            return Json(ServiceLocator.Current.GetInstance<ICicloLectivoRules>().GetCicloLectivoVigenteByNivelEducativo((int) NivelEducativoNombreEnum.MEDIO) != null ? true : false, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
