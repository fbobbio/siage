using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using SIAGE.UI_Common.Controllers;

namespace SIAGE_Escuela.Controllers
{
    public class ConfiguracionAsignaturaEspecialController : AjaxAbmcController<UnidadAcademicaModel, IUnidadAcademicaRules>
    {

        private IUnidadAcademicaRules _unidadAcademicaRules;
        public IUnidadAcademicaRules unidadAcademicaRules
        {
            get
            {
                if (_unidadAcademicaRules == null)
                    _unidadAcademicaRules = ServiceLocator.Current.GetInstance<IUnidadAcademicaRules>();
                return _unidadAcademicaRules;
            }
        }

        private EmpresaModel empresa;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            
            Rule = ServiceLocator.Current.GetInstance<IUnidadAcademicaRules>();

            AbmcView = "ConfiguracionAsignaturaEspecialEditor";

            empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();

            ViewData["FiltroAsignatura"] = new SelectList(unidadAcademicaRules.GetAsignaturasEspecialesHorarioEscuela(empresa.Id, null), "Id", "Nombre");
            ViewData["ComboVacio"] = new SelectList(new List<string>());
        }

        public JsonResult GetConfiguracionAsignaturaEspecial(int? id, string sidx, string sord, int page, int rows)
        {
            Func<UnidadAcademicaModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                //sidx == "GradoAño" ? x => x.DetalleAsignatura.FirstOrDefault().Asignatura.Asignatura.Nombre :
                sidx == "Division" ? x => x.DetalleAsignatura.FirstOrDefault().HorasSemanales.ToString() :
                sidx == "Sexo" ? x => x.DivisionAsignEspecial :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<UnidadAcademicaModel, IComparable>)(x => x.Id);

            List<UnidadAcademicaModel> registros;
            if (id.HasValue)
            {
                //devuelvo solamente la unidad academica a registrar
                registros = new List<UnidadAcademicaModel> {unidadAcademicaRules.GetUnidadAcademicaById(id.Value)};
            }
            else
            {
                //busco todas las unidades academicas disponibles para registrar
                registros = unidadAcademicaRules.GetUnidadesAcademicasParaRegistrarConfiguracionAsignaturaEspecial(empresa.Id);
            }

            if (sord == "asc")
                registros = registros.OrderBy(funcOrden).ToList();
            else
                registros = registros.OrderByDescending(funcOrden).ToList();
            
            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();

            var jsonData = new
            {
                total = 1,
                page = page,
                records = totalRegistros,
                rows = from a in registros
                       select new
                       {
                           cell = new string[] {
                            a.Id.ToString(), 
                            // Respetar el orden en que se mostrarán las columnas
                            /****************************** INICIO AREA EDITABLE ******************************/
                            a.DetalleAsignatura.FirstOrDefault().Asignatura.AsignaturaNombre,
                            a.DetalleAsignatura.FirstOrDefault().HorasSemanales.ToString(),
                            a.DivisionAsignEspecial
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, int? filtroAsignatura, int? filtroCargaHoraria, string filtroDivision)
        {
            Func<UnidadAcademicaModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Division" ? x => x.DiagramacionCurso.Division :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<UnidadAcademicaModel, IComparable>)(x => x.Id);

            //    // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = unidadAcademicaRules.GetUnidadesAcademicasByConfiguracionAsignaturaEspecial(empresa.Id, null, filtroAsignatura, filtroCargaHoraria, filtroDivision);
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
                            a.DetalleAsignatura.FirstOrDefault().Asignatura.AsignaturaNombre,
                            a.DetalleAsignatura.FirstOrDefault().HorasSemanales.ToString(),
                            a.DivisionAsignEspecial
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCombinacionDiagramacionCursoByUnidadAcademica(int? id, int? turno)
        {
            var configuracionesAsignaturaEspecial = new List<ConfiguracionAsigEspecialModel>();

            if (id.HasValue && turno.HasValue)
            {
                configuracionesAsignaturaEspecial = unidadAcademicaRules.GetConfiguracionesAsignaturaEspecialByUnidadAcademicaRegistro(turno.Value, empresa.Id, id.Value);
                Session["ConfiguracionAsigEspecial"] = configuracionesAsignaturaEspecial;
            }

            // Construyo el json con los valores que se mostraran en la grilla
            var resultado = new
            {
                total = 1,
                page = 1,
                records = configuracionesAsignaturaEspecial.Count,
                rows = from a in configuracionesAsignaturaEspecial
                       select new
                       {
                           cell = new string[] {
                            a.Id.ToString(), 
                            // Respetar el orden en que se mostrarán las columnas
                            /****************************** INICIO AREA EDITABLE ******************************/
                            a.GradoAñoNombre,
                            a.Division.ToString(),
                            a.Sexo.ToString()
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegistrarConfiguracion(List<int> configuracionesId, int unidadAcademica)
        {
            try
            {
                var configuracionesAsignaturaEspecialPrevias =
                    (List<ConfiguracionAsigEspecialModel>) Session["ConfiguracionAsigEspecial"];
                Session.Remove("ConfiguracionAsigEspecial");

                //Limpio la lista, y dejo solo aquellas seleccionado por el usuario
                configuracionesAsignaturaEspecialPrevias = (from cae in configuracionesAsignaturaEspecialPrevias
                                                            where configuracionesId.Contains(cae.Id)
                                                            select cae).ToList();

                unidadAcademicaRules.ConfiguracionAsignaturaEspecialSave(unidadAcademica, configuracionesAsignaturaEspecialPrevias);

                return Json(new {status = true});
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                    e = e.InnerException;

                var error =
                    new
                    {
                        status = false,
                        details = new List<string> { e.Message }
                    };

                return Json(error, JsonRequestBehavior.AllowGet);
            }
        }

        public override ActionResult Index()
        {
            ViewData["ComboTurno"] = new SelectList(ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnoByEscuelaLogueada(), "Id", "Nombre");
            return base.Index();
        }

        public override ActionResult Registrar()
        {
            ViewData["ComboTurno"] = new SelectList(ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnoByEscuelaLogueada(), "Id", "Nombre");
            return base.Registrar();
        }

        public override ActionResult Editar(int id)
        {
            ViewData["IdUnidadAcademica"] = id;
            ViewData["ComboTurno"] = new SelectList(ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnoByEscuelaLogueada(), "Id", "Nombre");
            return base.Editar(id);
        }

        public override ActionResult Ver(int id)
        {
            ViewData["IdUnidadAcademica"] = id;
            var unidadAcademica = unidadAcademicaRules.GetUnidadAcademicaById(id);
            var diagramacionCursoId = unidadAcademica.ConfAsigEspecial.First().DiagramacionCursoId;
            var turnoId = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetDiagramacionCursoById(diagramacionCursoId).Turno;
            ViewData["ComboTurno"] = new SelectList(ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnoByEscuelaLogueada(), "Id", "Nombre", turnoId);
            return base.Ver(id);
        }

        public JsonResult GetDiagramacionesParaVer(int id)
        {
            var configuracionesAsignaturaEspecial = unidadAcademicaRules.GetUnidadAcademicaById(id).ConfAsigEspecial;
            Session["ConfiguracionAsigEspecial"] = configuracionesAsignaturaEspecial;

            // Construyo el json con los valores que se mostraran en la grilla
            var resultado = new
            {
                total = 1,
                page = 1,
                records = configuracionesAsignaturaEspecial.Count,
                rows = from a in configuracionesAsignaturaEspecial
                       select new
                       {
                           cell = new string[] {
                            a.Id.ToString(), 
                            // Respetar el orden en que se mostrarán las columnas
                            /****************************** INICIO AREA EDITABLE ******************************/
                            a.GradoAñoNombre,
                            a.Division.ToString(),
                            a.Sexo.ToString()
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
    }
}
