using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using SIAGE.UI_Common.Controllers;
using SIAGE.UI_Common.Content;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class HorarioEscuelaController : AjaxAbmcController<HorarioModel, IDetalleHorarioRules>
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

        private int idEscuela;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "HorarioEscuelaEditor";
            idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
        }

        public override ActionResult Index()
        {
            var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetEmpresaById(idEscuela);
            if (empresa.TipoEmpresa != TipoEmpresaEnum.ESCUELA_MADRE && empresa.TipoEmpresa != TipoEmpresaEnum.ESCUELA_ANEXO)
            {
                TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela";
                return RedirectToAction("Error", "Home");
            }

            CargarCombos();
            return base.Index();
        }

        public override ActionResult Registrar()
        {
            CargarCombos();
            return base.Registrar();
        }

        public override ActionResult Eliminar(int id)
        {
            CargarCombos();
            return base.Eliminar(id);
        }

        public void CargarCombos()
        {
            var escuela = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetEscuelaById(idEscuela);
            ViewData["EsSuperior"] = (escuela.NivelEducativoNombre == NivelEducativoNombreEnum.SUPERIOR.ToString()) ? true : false;
            ViewData["ComboCarreras"] = new SelectList(ServiceLocator.Current.GetInstance<ICarreraRules>().GetCarrerasPorEscuelaLogueada(), "Id", "Nombre");
            ViewData["ComboTurno"] = new SelectList(ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnoByEscuelaLogueada(), "Id", "Nombre");
            ViewData["ComboVacio"] = new SelectList(new List<string>());
        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, int? filtroCarrera, int? filtroTurno, int? filtroGradoAño, DivisionEnum? filtroDivision)
        {
            Func<UnidadAcademicaModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Carrera" ? x => x.DiagramacionCurso.CarreraNombre :
                sidx == "Turno" ? x => x.DiagramacionCurso.TurnoNombre :
                sidx == "GradoAño" ? x => x.DiagramacionCurso.GradoAnio.ToString() :
                sidx == "Division" ? x => x.DiagramacionCurso.Division :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<UnidadAcademicaModel, IComparable>)(x => x.Id);

            //    // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();
            var registros = unidadAcademicaRules.GetHorarioEscuelaByFiltros(empresa.Id, filtroCarrera, filtroTurno, filtroGradoAño, filtroDivision);
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
                            a.DiagramacionCurso.CarreraNombre,
                            a.DetalleAsignatura.Count > 0 ?a.DetalleAsignatura[0].Asignatura.AsignaturaNombre: ":(",
                            a.DiagramacionCurso.TurnoNombre,
                            a.DiagramacionCurso.GradoAnioNombre,
                            a.DiagramacionCurso.Division.ToString(),
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTurnoByCarrera(int carrera)
        {
            var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();
            var turnos = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetTurnoByCarrera(empresa.Id, carrera);
            var json = new SelectList(turnos, "Id", "Nombre");
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGradoByTurno(int? carrera, int? turno)
        {
            try
            {
                var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();
                var grados = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetGradosByCarreraTurno(empresa.Id, carrera, turno);
                var json = new SelectList(grados, "Id", "Nombre");
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new SelectList(new List<string>()), JsonRequestBehavior.AllowGet);
            }
        }

        //Devuelve todas las divisiones. Se usa en el Index.
        public JsonResult GetDivisionesByTurnoGrado(int? carrera, int turno, int grado)
        {
            var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();
            var divisiones = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetDivisionesByCarreraTurnoGrado(empresa.Id, carrera, turno, grado);
            var divisionesModelo = (from d in divisiones
                                    select new { Id = d, Nombre = d }).ToList();

            var json = new SelectList(divisionesModelo, "Id", "Nombre");
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        //Devuelve divisiones. Solo las que NO tienen ya registrado un horario. Se usa en el Editor (Registrando).
        public JsonResult GetDivisionesByTurnoGradoRegistro(int? carrera, int turno, int grado)
        {
            var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();
            var divisiones = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetDivisionesByCarreraTurnoGradoRegistro(empresa.Id, carrera, turno, grado);
            var divisionesModelo = (from d in divisiones
                                    select new { Id = d, Nombre = d }).ToList();
            var json = new SelectList(divisionesModelo, "Id", "Nombre");
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAsignaturasByUnidadAcademica(int id)
        {
            var unidadAcademica = unidadAcademicaRules.GetUnidadAcademicaById(id);
            var carrera = unidadAcademica.DiagramacionCurso.Carrera;
            var turno = unidadAcademica.DiagramacionCurso.Turno;
            var grado = unidadAcademica.DiagramacionCurso.GradoAnio;
            var division = unidadAcademica.DiagramacionCurso.Division;

            return GetAsignaturas(carrera, turno, grado, division);
        }

        public JsonResult GetAsignaturas(int? carrera, int? turno, int? grado, DivisionEnum? division)
        {
            var escuela = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetEscuelaById(idEscuela);
            var nivelEducativo = escuela.NivelEducativoNombre;

            var asignaturas = new List<AsignaturaModel>();

            if (nivelEducativo == NivelEducativoNombreEnum.INICIAL.ToString())
            {
                //Comunes
                asignaturas = unidadAcademicaRules.GetAsignaturasEspecialesHorarioEscuela(turno, grado, division, idEscuela);
            }
            else if (nivelEducativo == NivelEducativoNombreEnum.PRIMARIO.ToString())
            {
                var asignaturasComunes = unidadAcademicaRules.GetAsignaturasHorarioEscuela(turno, grado, division, carrera, idEscuela);
                var asignaturasEspeciales = unidadAcademicaRules.GetAsignaturasEspecialesHorarioEscuela(turno, grado, division, idEscuela);
                var asignaturasDefinicionInstitucional = unidadAcademicaRules.GetAsignaturasDefinicionInsitucionalHorarioEscuela(turno, grado, division, carrera, idEscuela);

                if (asignaturasComunes != null)
                    asignaturas.AddRange(asignaturasComunes);

                if (asignaturasEspeciales != null)
                    asignaturas.AddRange(asignaturasEspeciales);

                if (asignaturasDefinicionInstitucional != null)
                    asignaturas.AddRange(asignaturasDefinicionInstitucional);
            }
            else if (nivelEducativo == NivelEducativoNombreEnum.MEDIO.ToString())
            {
                var asignaturasComunes = unidadAcademicaRules.GetAsignaturasHorarioEscuela(turno, grado, division, carrera, idEscuela);
                var asignaturasDefinicionInstitucional = unidadAcademicaRules.GetAsignaturasDefinicionInsitucionalHorarioEscuela(turno, grado, division, carrera, idEscuela);

                if (asignaturasComunes != null)
                    asignaturas.AddRange(asignaturasComunes);

                if (asignaturasDefinicionInstitucional != null)
                    asignaturas.AddRange(asignaturasDefinicionInstitucional);
            }
            else if (nivelEducativo == NivelEducativoNombreEnum.SUPERIOR.ToString())
            {
                //Comunes
                asignaturas = unidadAcademicaRules.GetAsignaturasHorarioEscuela(turno, grado, division, carrera, idEscuela);
            }

            var json = new List<object>();
            foreach (var item in asignaturas)
            {
                json.Add(new { Id = item.Id, Nombre = item.Nombre, HorasSemanales = item.HorasSemanales });
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetalleHorasTurno(int? id, int? carrera, int? turno)
        {
            var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();

            ConfiguracionTurnoModel configuracionTurno;
            if (id.HasValue)
            {
                var ua = ServiceLocator.Current.GetInstance<IUnidadAcademicaRules>().GetUnidadAcademicaById(id.Value);
                configuracionTurno = ServiceLocator.Current.GetInstance<IConfiguracionTurnoRules>().GetConfiguracionTurnoVigente(ua.DiagramacionCurso.Carrera, ua.DiagramacionCurso.Turno, empresa.Id);
            }
            else
            {
                configuracionTurno = ServiceLocator.Current.GetInstance<IConfiguracionTurnoRules>().GetConfiguracionTurnoVigente(carrera, turno, empresa.Id);
            }

            //Variable a devolver, que será un JSON con valores HoraInicio y HoraFin para armar la grilla
            var horarios = new List<object>();

            //Aca armo la variable "Horarios" declarada arriba
            foreach (int idDetalle in configuracionTurno.DetalleHoras)
            {
                //Aca se cargaria el modelo del detalle hora turno que tiene el id de la lista que tiene la configuracion turno
                var modulo = ServiceLocator.Current.GetInstance<IDetalleHoraTurnoRules>().GetDetalleHoraTurnoById(idDetalle);

                horarios.Add(new
                {
                    HoraInicio = String.Format("{0:00}:{1:00}", modulo.HoraInicio.Hours, modulo.HoraInicio.Minutes),
                    HoraFin = String.Format("{0:00}:{1:00}", modulo.HoraFin.Hours, modulo.HoraFin.Minutes)
                });
            }

            return Json(horarios, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHorario(int id)
        {
            var model = unidadAcademicaRules.GetDetalleHorarioByUnidadAcademica(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Ver(int id)
        {
            CargarCombos();

            ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Ver.ToString();
            ViewData[AjaxAbmc.EstadoId] = EstadoABMC.Ver;
            ViewData["IdHorario"] = id;

            var unidadAcademica = unidadAcademicaRules.GetUnidadAcademicaById(id);

            ViewData["Carrera"] = new SelectList(new List<DiagramacionCursoModel> { unidadAcademica.DiagramacionCurso }, "Carrera", "CarreraNombre");
            ViewData["Turno"] = new SelectList(new List<DiagramacionCursoModel> { unidadAcademica.DiagramacionCurso }, "Turno", "TurnoNombre");
            ViewData["GradoAño"] = new SelectList(new List<DiagramacionCursoModel> { unidadAcademica.DiagramacionCurso }, "GradoAño", "GradoAñoNombre");
            ViewData["Division"] = new SelectList(new List<DiagramacionCursoModel> { unidadAcademica.DiagramacionCurso }, "Division", "Division");

            return PartialView(AbmcView);
        }

        public override ActionResult Editar(int id)
        {
            CargarCombos();

            ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Editar.ToString();
            ViewData[AjaxAbmc.EstadoId] = EstadoABMC.Editar;
            ViewData["IdHorario"] = id;

            var unidadAcademica = unidadAcademicaRules.GetUnidadAcademicaById(id);

            ViewData["Carrera"] = new SelectList(new List<DiagramacionCursoModel> { unidadAcademica.DiagramacionCurso }, "Carrera", "CarreraNombre");
            ViewData["Turno"] = new SelectList(new List<DiagramacionCursoModel> { unidadAcademica.DiagramacionCurso }, "Turno", "TurnoNombre");
            ViewData["GradoAño"] = new SelectList(new List<DiagramacionCursoModel> { unidadAcademica.DiagramacionCurso }, "GradoAño", "GradoAñoNombre");
            ViewData["Division"] = new SelectList(new List<DiagramacionCursoModel> { unidadAcademica.DiagramacionCurso }, "Division", "Division");

            return PartialView(AbmcView);
        }

        [HttpPost]
        public override ActionResult Registrar(HorarioModel modelo)
        {
            try
            {
                var modeloFinal = ProcesarModeloHorario(modelo);
                var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();
                unidadAcademicaRules.UpdateHorarioEscuelaByUnidadAcademica(modeloFinal, empresa.Id);

                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public override ActionResult Editar(HorarioModel model)
        {
            try
            {
                var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();
                var modeloFinal = ProcesarModeloHorario(model);
                unidadAcademicaRules.UpdateHorarioEscuelaByUnidadAcademica(modeloFinal, empresa.Id);

                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
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


        #region "Funciones Soporte"

        /// <summary>
        /// Este metodo recibe un modelo que contiene DetallesHorarios "sucios" (), y devuelve un modelo de DetalleHorario "limpio".
        /// Sucio: 1 detalle horario POR CADA MODULO DE CADA asignatura.
        /// Limpio: 1 detalle horario POR MODULOS CONSECUTIVOS de asignatura.
        /// </summary>
        /// <param name="modelo">Modelo sucio a procesar</param>
        /// <returns>Modelo limpio</returns>
        private HorarioModel ProcesarModeloHorario(HorarioModel modelo)
        {
            modelo.DetalleHorarioModels = (from a in modelo.DetalleHorarioModels
                                           orderby a.DiaSemana, a.HoraDesde
                                           select a).ToList();

            var modeloFinal = new HorarioModel
                                  {
                                      TurnoId = modelo.TurnoId,
                                      GradoAñoId = modelo.GradoAñoId,
                                      Division = modelo.Division,
                                      DetalleHorarioModels = new List<DetalleHorarioModel>()
                                  };

            var anterior = new DetalleHorarioModel();

            //Recorremos el modelo nuevo
            foreach (var item in modelo.DetalleHorarioModels)
            {
                //Si NO es la 1º iteracion
                if (modeloFinal.DetalleHorarioModels.Count != 0)
                {
                    //Creamos un puntero (anterior) que apunta al ultimo item agregado de la lista
                    anterior = modeloFinal.DetalleHorarioModels[modeloFinal.DetalleHorarioModels.Count - 1];
                }

                //Si hay 2 asignaturas consecutivas (2 o mas modulos seguidos)
                if ((anterior.DiaSemana == item.DiaSemana) && (anterior.AsignaturaId == item.AsignaturaId) && ((anterior.HoraHasta + 1) == item.HoraDesde))
                {
                    //Añadimos 1 modulo mas a ANTERIOR
                    anterior.HoraHasta++;
                }
                else
                {
                    modeloFinal.DetalleHorarioModels.Add(new DetalleHorarioModel { AsignaturaId = item.AsignaturaId, DiaSemana = item.DiaSemana, HoraDesde = item.HoraDesde, HoraHasta = item.HoraDesde });
                }
            }

            return modeloFinal;
        }

        #endregion
    }
}