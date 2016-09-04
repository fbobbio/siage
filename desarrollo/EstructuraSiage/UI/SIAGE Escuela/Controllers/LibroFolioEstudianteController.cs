using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using SIAGE.UI_Common.Controllers;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class LibroFolioEstudianteController : AjaxAbmcController<InscripcionLibroFolioModel, IInscripcionRules>
    {
        #region Atributos / Propiedades
        
        private int idEscuela;
        private int idNivel;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "LibroFolioEstudianteEditor";
            idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
            Rule = ServiceLocator.Current.GetInstance<IInscripcionRules>();
        }

        public override ActionResult Index()
        {
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(idEscuela)) // validar si estoy logueado como escuela
            {
                var nivel = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetNivelEducativoByEscuela(idEscuela);
                idNivel = nivel.Id;
                ViewData["NivelEducativo"] = nivel != null ? nivel.Nombre : string.Empty;
                
                ViewData.Add(ViewDataKey.CARRERA.ToString(), ServiceLocator.Current.GetInstance<ICarreraRules>().GetCarrerasByEscuela(idEscuela));
                ViewData.Add(ViewDataKey.TURNO.ToString(), ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnosByEscuela(idEscuela));
                ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetGradoAñoByEscuela(idEscuela));
                ViewData.Add(ViewDataKey.CICLO_LECTIVO.ToString(), ServiceLocator.Current.GetInstance<ICicloLectivoRules>().GetCicloLectivoByNivelEducativo(nivel.Id));
                //ViewData["TurnoList"] = new SelectList(ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnosByEscuela(idEscuela), "Id", "Nombre");
                //ViewData["GradoAnioList"] = new SelectList(ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetGradoAñoByEscuela(idEscuela), "Id", "Nombre");
                //ViewData["CiclosList"] = new SelectList(ServiceLocator.Current.GetInstance<ICicloLectivoRules>().GetCicloLectivoByNivelEducativo(nivel.Id), "Id", "AñoCiclo");
                return View();
            }

            TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela.";
            return RedirectToAction("Error", "Home");
        }

        #endregion

        #region POST EstructuraEscuela

        public override void EditarPost(InscripcionLibroFolioModel model)
        {
            model.NivelId = idNivel;
            Rule.LibroFolioSave(model);
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
        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, int? filtroTurno, int? filtroGrado, int? filtroCarrera, DivisionEnum? filtroDivision, int? filtroCicloLectivo)
        {
            Func<InscripcionLibroFolioConsultaModel, IComparable> funcOrden =
                   sidx == "Nombre" ? x => x.Nombre :
                   sidx == "Apellido" ? x => x.Apellido :
                   sidx == "TipoDocumento" ? x => x.TipoDocumento :
                   sidx == "NroDocumento" ? x => x.NroDocumento :
                   sidx == "LibroMatriz" ? x => x.LibroMatriz :
                   sidx == "Folio" ? x => x.Folio :
                   sidx == "Turno" ? x => x.Turno :
                   sidx == "GradoAnio" ? x => x.GradoAnio :
                   sidx == "Division" ? x => x.Division :
                   sidx == "Carrera" ? x => x.Carrera :
                   (Func<InscripcionLibroFolioConsultaModel, IComparable>)(x => x.Id);

                var registros = Rule.GetInscripcionesByFiltros(idEscuela, filtroTurno, filtroGrado, filtroDivision, filtroCarrera, filtroCicloLectivo);

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
                                        a.Apellido,
                                        a.Nombre,
                                        a.TipoDocumento,
                                        a.NroDocumento,
                                        a.LibroMatriz,
                                        a.Folio,
                                        a.Turno,
                                        a.GradoAnio,
                                        a.Division.ToString(),
                                        a.Carrera
                                    }
                               }
                    };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Soporte

        public JsonResult GetTurnosByCarreraEscuela(int idCarrera)
        {

            var lista = ServiceLocator.Current.GetInstance<ITurnoRules>().GetByEscuelaYCarrera(idEscuela, idCarrera).OrderBy(x => x.Nombre);
            return Json(new SelectList(lista, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGradoAnioByTurnoCarreraEscuela(int idTurno, int? idCarrera)
        {
            var lista = ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetGradoAnioByTurnoCarreraEscuela(idTurno, idCarrera, idEscuela).OrderBy(x => x.Nombre);
            return Json(new SelectList(lista, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDivisionesByGradoAnioCarreraEscuela(int idGradoAnio, int? idCarrera)
        {
            var values = from DivisionEnum e in ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetDivisionesByGradoAnioTurno(idGradoAnio, null, idEscuela, idCarrera)
                         orderby e
                         select new { Id = (int)e, Division = e.ToString() };
            return Json(new SelectList(values, "Id", "Division"), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
