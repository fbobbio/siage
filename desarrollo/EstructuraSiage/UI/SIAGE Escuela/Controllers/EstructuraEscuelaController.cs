using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using Siage.UCControllers;
using Siage.Base;
using SIAGE.UI_Common.Controllers;
using SIAGE.UI_Common.Content;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class EstructuraEscuelaController : AjaxAbmcController<DiagramacionCursoModel, IDiagramacionCursoRules>
    {
        #region Atributos / Propiedades
        private int idEscuela;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "EstructuraEscuelaEditor";
            Rule = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>();
            idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
        }

        public override ActionResult Index()
        {
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(idEscuela)) // validar si estoy logueado como escuela
            {
                ViewData.Add(ViewDataKey.CICLO_EDCUCATIVO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetCicloEducativoPorEscuela(idEscuela));
                ViewData.Add(ViewDataKey.TURNO.ToString(), ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnosByEscuela(idEscuela));
                ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoPorNivelEducativoDeEscuela(idEscuela));
                
                //Estos 2 view data (EstructuraDefinitiva y NivelEducativo)no son para cargar combos sino que son para armar la vista
                ViewData["EstructuraDefinitiva"] = ServiceLocator.Current.GetInstance<IEmpresaRules>().TieneEstructuraDefinitiva(idEscuela);
                var nivel = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetNivelEducativoByEscuela(idEscuela);
                ViewData["NivelEducativo"] = nivel != null ? nivel.Nombre : string.Empty;
                if (nivel.Id == (int)NivelEducativoNombreEnum.SUPERIOR)
                    ViewData.Add(ViewDataKey.CARRERA.ToString(), ServiceLocator.Current.GetInstance<ICarreraRules>().GetCarrerasByEscuela(idEscuela));
                return View();
            }

            TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela.";
            return RedirectToAction("Error", "Home");

        }

        public override void CargarViewData(EstadoABMC estado)
        {
            //ViewData["TurnoList"] = new SelectList(ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnosByEscuela(idEscuela), "Id", "Nombre");
            ViewData.Add(ViewDataKey.TURNO.ToString(), ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnosByEscuela(idEscuela));
            ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoPorNivelEducativoDeEscuela(idEscuela));

            //Estos 2 view data (EstructuraDefinitiva y NivelEducativo)no son para cargar combos sino que son para armar la vista
            ViewData["EstructuraDefinitiva"] = ServiceLocator.Current.GetInstance<IEmpresaRules>().TieneEstructuraDefinitiva(idEscuela);
            var nivel = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetNivelEducativoByEscuela(idEscuela);
            ViewData["NivelEducativo"] = nivel != null ? nivel.Nombre : string.Empty;
            if (nivel.Id == (int)NivelEducativoNombreEnum.SUPERIOR)
                ViewData.Add(ViewDataKey.CARRERA.ToString(), ServiceLocator.Current.GetInstance<ICarreraRules>().GetCarrerasByEscuela(idEscuela));
            
            //ViewData de instrumento legal
            if ((bool)ViewData["EstructuraDefinitiva"])
                CargarViewDataInstrumentoLegal();
        }


        #endregion

        #region POST EstructuraEscuela

        public override void RegistrarPost(DiagramacionCursoModel model)
        {
            model.Escuela = idEscuela;
            Rule.DiagramacionCursoSave(model);
        }

        public override void EditarPost(DiagramacionCursoModel model)
        {
            model.Escuela = idEscuela;
            Rule.DiagramacionCursoUpdate(model);
            Session["ListaInstrumentoLegal"] = null;
        }

        public override void EliminarPost(DiagramacionCursoModel model)
        {
            Rule.DiagramacionCursoDelete(model);
            Session["ListaInstrumentoLegal"] = null;
        }

        #endregion

        #region Procesamiento Busquedas

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
        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, int? filtroTurno, int? filtroGrado, int? filtroCarrera, int? filtroCicloEducativo)
        {
            Func<DiagramacionCursoModel, IComparable> funcOrden = null;
            var nivel = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetNivelEducativoByEscuela(idEscuela);
            
            if(nivel.Id == (int)NivelEducativoNombreEnum.SUPERIOR )
            {
                funcOrden =
                    sidx == "Carrera" ? x => x.CarreraNombre :
                    sidx == "Turno" ? x => x.TurnoNombre :
                    sidx == "Grado" ? x => x.GradoAnioNombre:
                    sidx == "Division" ? x => x.Division :
                    sidx == "Cupo" ? x => x.Cupo:
                (Func<DiagramacionCursoModel, IComparable>)(x => x.Id);
            }
            else
            {
                funcOrden =
                    sidx == "Turno" ? x => x.TurnoNombre  :
                    sidx == "Grado" ? x => x.GradoAnioNombre:
                    sidx == "Division" ? x => x.Division :
                    sidx == "Cupo" ? x => x.Cupo:
                (Func<DiagramacionCursoModel, IComparable>)(x => x.Id);
            }

            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetDiagramacionCursoByFiltros(filtroTurno, filtroGrado, filtroCarrera, filtroCicloEducativo, idEscuela);
            /******************************** FIN AREA EDITABLE *******************************/

            // Ordeno los registros
            registros = sord == "asc" ? registros.OrderBy(funcOrden).ToList() : registros.OrderByDescending(funcOrden).ToList();

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            registros = registros.Skip((page - 1) * rows).Take(rows).ToList();

            // Construyo el json con los valores que se mostraran en la grilla
            var jsonData = new object();

            if (nivel.Id == (int)NivelEducativoNombreEnum.SUPERIOR)
            {
                jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRegistros,
                    rows = from a in registros
                           select new
                           {
                                   cell = new string[] {
                                a.Id.ToString(),
                                a.CarreraNombre,
                                a.TurnoNombre,
                                a.GradoAnioNombre,
                                a.Division.ToString(),
                                a.Cupo.ToString()
                                }
                           }
                };
            }
            else
            {
                jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRegistros,
                    rows = from a in registros
                           select new
                           {
                               cell = new string[] {
                            a.Id.ToString(), 
                            a.TurnoNombre,
                            a.GradoAnioNombre,
                            a.Division.ToString(),
                            a.Cupo.ToString()
                        }
                           }
                };
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGradoByCicloEducativo(int ciclo)
        {
            var json = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetGradosByCicloEducativo(ciclo);

            return Json(new SelectList(json, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Soporte

        /// <summary>
        /// 
        ///
        /// </summary>
        /// <param name="idDiagramacion">Campo por el cual se ordenan los registros</param>
        /// <returns>Lista de errores que se muestran cuando se valida la diagramacion curso que se quiere eliminar</returns>
        [HttpGet]
        public ActionResult ValidarDiagramacionConEstructuraCompleta(int idDiagramacion)
        {
            var errores = new List<string>();
            if (Rule.ValidarExistenciaEstructuraCompleta(idDiagramacion))
            {
                errores = Rule.ValidarDiagramacionConEstructuraCompleta(idDiagramacion);
                return Json(errores, JsonRequestBehavior.AllowGet);
            }

            return Json(errores, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Registra a la escuela logueada con estructura definitiva.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RegistrarEstructuraEscuelaDefinitiva()
        {
            Rule.RegistrarEstructuraEscuelaDefinitiva(idEscuela);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAsignacionesInstrumentoLegalByIdDiagramacion(int idDiagramacion)
        {
            var rows = from AsignacionInstrumentoLegalConsultaModel e in ServiceLocator.Current.GetInstance<IAsignacionInstrumentoLegalRules>().GetAsignacionesInstrumentoLegalByIdDiagramacion(idDiagramacion)
                       select new { cell = new string[] { e.Id.ToString(), e.NroInstrumentoLegal, e.Numero, e.TipoInstrumentoLegal, e.FecAsociacion.ToShortDateString() } };

            return Json(new { rows }, JsonRequestBehavior.AllowGet);
        }

        private void CargarViewDataInstrumentoLegal()
        {
            var entidadesGeneralesRule = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            var idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            
            ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(), entidadesGeneralesRule.GetDepartamentoProvincialByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), new List<LocalidadModel>());
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), entidadesGeneralesRule.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), entidadesGeneralesRule.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.DIRECCION_NIVEL.ToString(), entidadesGeneralesRule.GetDireccionDeNivelAll());
            ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), entidadesGeneralesRule.GetTipoCargoAll());
            ViewData.Add(ViewDataKey.ASIGNATURA.ToString(), entidadesGeneralesRule.GetAsignaturaAll());
            ViewData.Add(ViewDataKey.TITULO.ToString(), entidadesGeneralesRule.GetTituloAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), entidadesGeneralesRule.GetSituacionRevistaAll());
            //ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(), entidadesGeneralesRule.GetEstadoCivilAll());
            //ViewData.Add(ViewDataKey.SEXO.ToString(), entidadesGeneralesRule.GetSexoAll());
            //ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(), entidadesGeneralesRule.GetOrganismoEmisorDocumentoAll());
            //ViewData.Add(ViewDataKey.PAIS.ToString(), entidadesGeneralesRule.GetPaisAll());
            ViewData.Add(ViewDataKey.TIPO_INSTRUMENTO_LEGAL.ToString(), ServiceLocator.Current.GetInstance<ITipoInstrumentoLegalRules>().GetAll());
            //ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(), entidadesGeneralesRule.GetTipoCalleAll());
        }
        
        #endregion
    }
}
