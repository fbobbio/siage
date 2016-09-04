using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class PrevisionAusenciaController : AjaxAbmcController<PrevisionAusenciaModel, IPrevisionAusenciaRules>
    {
        #region Atributos/Propiedades
        private IEntidadesGeneralesRules _entidadesGeneralesRules;
        private IAgenteRules _agenteRules;
        private int IdAgente;
        private int _idEscuela;
        #endregion

        #region GET/POST

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "AgenteEditor";
            _entidadesGeneralesRules = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            _agenteRules = ServiceLocator.Current.GetInstance<IAgenteRules>();
            Rule = ServiceLocator.Current.GetInstance<IPrevisionAusenciaRules>();
            IdAgente = ServiceLocator.Current.GetInstance<IUsuarioRules>().GetCurrentUser().RolActual.AgenteId.Value;
            try
            {
                _idEscuela = (int)Session[Siage.Base.ConstantesSession.EMPRESA.ToString()];
            }
            catch (Exception)
            {
                TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela.";
                RedirectToAction("Error", "Home");
                return;
            }
        }

        public override ActionResult Index()
        {
            CargarViewData(EstadoABMC.Consultar);
            return View();
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            var idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            ViewData.Add(ViewDataKey.PROVINCIA.ToString(), _entidadesGeneralesRules.GetProvinciabyPais("ARG"));
            //if (ViewData[ViewDataKey.TIPO_DOCUMENTO.ToString()] == null)
                ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), _entidadesGeneralesRules.GetTipoDocumentoAll());
            //if (ViewData[ViewDataKey.PAIS.ToString()] == null)
                ViewData.Add(ViewDataKey.PAIS.ToString(),
                            _entidadesGeneralesRules.GetPaisAll());
            //if (ViewData[ViewDataKey.TIPO_CALLE.ToString()] == null)
                ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(),
                             _entidadesGeneralesRules.GetTipoCalleAll());
        }

        public ActionResult RegistrarPrevision()
        {
            var prevision = new PrevisionAusenciaRegistrarModel();
            prevision.Agente = _agenteRules.GetAgenteConsultaById(IdAgente);

            return View(prevision);
        }

        public ActionResult EliminarPrevision()
        {
            CargarViewData(EstadoABMC.Eliminar);
            return View();
        }

        public ActionResult RegistrarInasistenciaPrevision()
        {
            CargarViewData(EstadoABMC.Registrar);
            return View();
        }

        public ActionResult RegistrarDecisionAutorizacion()
        {
            CargarViewData(EstadoABMC.Registrar);
            return View();
        }

        [HttpGet]
        public PartialViewResult GetPrevisionAusenciaById(int idPrevision)
        {
            var previsionAusencia = Rule.GetPrevisionAusenciaById(idPrevision);
            //var previsionAusencia = HarcodeDtoProvicionAusenciaMostrar();

            CargarViewData(EstadoABMC.Consultar);
            var vista = PartialView("../Shared/EditorTemplates/PrevisionAusenciaEditor", previsionAusencia);
            return vista;
        }

        public ActionResult VerPrevision(int idPrevision)
        {
            //var previsionAusencia = Rule.GetPrevisionAusenciaById(idPrevision);
            var previsionAusencia = HarcodeDtoProvicionAusenciaMostrar();
            var vista = PartialView("../Shared/EditorTemplates/PrevisionAusenciaEditor", previsionAusencia);
            return View(vista);
        }

        [HttpPost]
        public ActionResult RegistrarPrevisionPost(PrevisionAusenciaRegistrarModel model)
        {
            try
            {
                Rule.PrevisionAusenciaSave(model, IdAgente);

                return Json(new { status = true });
            }
            catch (Exception e)
            {
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

        [HttpPost]
        public ActionResult EliminarPrevision(int idPrevision)
        {
            try
            {
                Rule.PrevisionAusenciaDelete(idPrevision);

                return Json(new { status = true });
            }
            catch (Exception e)
            {
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

        [HttpPost]
        public ActionResult RegistrarInasistenciaPost(InasistenciaDocenteModel model)
        {
           
             try
            {
                Rule.RegistrarInasistenciaPrevisionAusencia(model);

                return Json(new { status = true });
            }
            catch (Exception e)
            {
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
        
        [HttpPost]
        public ActionResult RegistrarDecisionAutorizacionPost(PrevisionAusenciaModel model)
        {
            try
            {
                Rule.RegistrarDecisionAutorizacionPrevisionAusencia(model, _idEscuela);

                return Json(new { status = true });
            }
            catch (Exception e)
            {
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

        public JsonResult ObtenerPuestosTrabajo(string sidx, string sord, int page, int rows, DateTime fechaDesde, DateTime fechaHasta)
        {
            // Construyo la funcion de ordenamiento
            Func<PuestoDeTrabajoConsultaModel, IComparable> funcOrden =
                sidx == "NombreEmpresa" ? x => x.NombreEmpresa :
                sidx == "NombreCargo" ? x => x.NombreCargo :
                sidx == "Anio" ? x => x.Anio :
                sidx == "Division" ? x => x.Division :
                sidx == "Seccion" ? x => x.Seccion :                
                sidx == "Turno" ? x => x.Turno :
            (Func<PuestoDeTrabajoConsultaModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetPTAfectadosSinHorasCatedra(IdAgente, fechaDesde, fechaHasta);

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
                                a.NombreEmpresa,
                                a.NombreCargo,
                                a.Anio,
                                a.Division,
                                a.Seccion,
                                a.Turno
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerPuestosTrabajoConHorasCatedras(string sidx, string sord, int page, int rows, DateTime fechaDesde, DateTime fechaHasta)
        {
            // Construyo la funcion de ordenamiento
            Func<PuestoDeTrabajoConsultaModel, IComparable> funcOrden =
                sidx == "CodigoEmpresa" ? x => x.CodigoEmpresa :
                sidx == "NombreEmpresa" ? x => x.NombreEmpresa :
                sidx == "NombreCargo" ? x => x.NombreCargo :
                sidx == "Materia" ? x => x.Materia :
                sidx == "Division" ? x => x.Division :
                sidx == "Anio" ? x => x.Anio :
                sidx == "Seccion" ? x => x.Seccion :
                sidx == "Turno" ? x => x.Turno :
                sidx == "Fecha" ? x => x.Fecha :
                sidx == "DiaSemanal" ? x => x.DiaSemanal :
                sidx == "HoraDesde" ? x => x.HoraDesde :
                sidx == "HoraHasta" ? x => x.HoraHasta :
                (Func<PuestoDeTrabajoConsultaModel, IComparable>)(x => x.Fecha);


            // Obtengo los registros filtrados segun los criterios ingresados
            var registros =Rule.GetPTAfectadosConHorasCatedra(IdAgente,fechaDesde,fechaHasta);

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            registros = registros.Skip((page - 1) * rows).Take(rows).ToList();

            //Agrego id artificial para la grilla
            for (int i = 0; i < registros.Count(); i++)
            {
                registros[i].Id = i;

                //TODO borrar cuando christian cambie caso de uso de configuracion turno
                var horaDesde = new TimeSpan(long.Parse(registros[i].HoraDesde));
                var horaHasta = new TimeSpan(long.Parse(registros[i].HoraHasta));
                registros[i].HoraDesde = horaDesde.ToString("hh':'mm");
                registros[i].HoraHasta = horaHasta.ToString("hh':'mm");
            }

            // Ordeno los registros
            if (sord == "asc")
                registros = registros.OrderBy(funcOrden).ToList();
            else
                registros = registros.OrderByDescending(funcOrden).ToList();

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
                                a.Fecha.HasValue? a.Fecha.Value.ToString("dd/MM/yyyy"): "-",
                                a.DiaSemanal.HasValue? a.DiaSemanal.Value.ToString(): "-",
                                a.HoraDesde, 
                                a.HoraHasta,
                                a.NombreEmpresa,
                                a.NombreCargo,
                                a.Materia,
                                a.Anio,
                                a.Division,
                                a.Turno,
                                a.IdPuestoTrabajo.ToString()
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerPrevisionAusencia(string sidx, string sord, int page, int rows, int? id, DateTime? FiltroFechaDesde, DateTime? FiltroFechaHasta, string FiltroLegajo, string FiltroTipoDni, string FiltroSexo, string FiltroNumeroDocumento,
                                                   string FiltroNombre, string FiltroApellido, DateTime? FiltroFechaDesdeAutorizacion, DateTime? FiltroFechaHastaAutorizacion, EstadoPrevisionAusenciaEnum? FiltroEstado, string FiltroCodigoEmpresa,
                                                   string FiltroNombreEmpresa)
        {
            Func<DtoPrevisionAusenciaModel, IComparable> funcOrden =
                sidx == "CodigoEmpresa" ? x => x.CodigoEmpresa :
                sidx == "NombreEmpresa" ? x => x.NombreEmpresa :
                sidx == "Nombre" ? x => x.Nombre :
                sidx == "Apellido" ? x => x.Apellido :
                sidx == "NombrePuestoTrabajo" ? x => x.NombrePuestoTrabajo :
                sidx == "FechaPrevisionFechaDesde" ? x => x.FechaPrevisionFechaDesde :
                sidx == "Observaciones" ? x => x.Observaciones :
                sidx == "Asignatura" ? x => x.Asignatura :
                sidx == "Estado" ? x => x.Estado :
                (Func<DtoPrevisionAusenciaModel, IComparable>)(x => x.Id);

            var registros = Rule.GetPevisionSinHorasCatedra(FiltroFechaDesde, FiltroFechaHasta, FiltroLegajo, FiltroTipoDni, FiltroSexo, FiltroNumeroDocumento, FiltroNombre, FiltroApellido, FiltroFechaDesdeAutorizacion, FiltroFechaHastaAutorizacion, FiltroEstado, FiltroCodigoEmpresa); 
                
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
                          a.CodigoEmpresa,
                          a.NombreEmpresa,
                          a.Nombre,
                          a.Apellido,
                          a.NombrePuestoTrabajo,
                          a.FechaPrevisionFechaDesde.ToString("dd/MM/YYYY"),
                          a.Observaciones,
                          a.Asignatura,
                          a.Estado.ToString()
                         /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerPrevisionAusenciaConUnidadesAcademicas(string sidx, string sord, int page, int rows, int? id, DateTime? FiltroFechaDesde, DateTime? FiltroFechaHasta, string FiltroLegajo, string FiltroTipoDni, string FiltroSexo, string FiltroNumeroDocumento,
                                                  string FiltroNombre, string FiltroApellido, DateTime? FiltroFechaDesdeAutorizacion, DateTime? FiltroFechaHastaAutorizacion, EstadoPrevisionAusenciaEnum? FiltroEstado, string FiltroCodigoEmpresa,
                                                  string FiltroNombreEmpresa)
        {
            // Construyo la funcion de ordenamiento
            Func<DtoPrevisionAusenciaModel, IComparable> funcOrden =
                sidx == "CodigoEmpresa" ? x => x.CodigoEmpresa :
                sidx == "NombreEmpresa" ? x => x.NombreEmpresa :
                sidx == "Nombre" ? x => x.Nombre :
                sidx == "Apellido" ? x => x.Apellido :
                sidx == "NombrePuestoTrabajo" ? x => x.NombrePuestoTrabajo :
                sidx == "FechaPrevisionFechaDesde" ? x => x.FechaPrevisionFechaDesde :
                sidx == "Observaciones" ? x => x.Observaciones :
                sidx == "DetallePuesto" ? x => x.DetallePuesto :
                sidx == "Estado" ? x => x.Estado :
                (Func<DtoPrevisionAusenciaModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetPevisionConHorasCatedra(FiltroFechaDesde, FiltroFechaHasta, FiltroLegajo, FiltroTipoDni, FiltroSexo, FiltroNumeroDocumento, FiltroNombre, FiltroApellido, FiltroFechaDesdeAutorizacion, FiltroFechaHastaAutorizacion, FiltroEstado, FiltroCodigoEmpresa);
            
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
                            a.CodigoEmpresa,
                            a.NombreEmpresa,
                            a.Apellido,
                            a.Nombre,
                            a.NombrePuestoTrabajo,
                            a.FechaPrevisionFechaDesde.ToString("dd/MM/yyyy"),
                            a.Observaciones,
                            a.DetallePuesto = a.Asignatura + ",  "+ a.AnioGrado+" " + a.Division + " - " + 
                                            a.Turno + " - " + a.DiaSemanal + ", de " + a.HoraDesde + " a " + a.HoraHasta,
                            a.Estado.ToString(),
                        }
                    }    
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Hacodeo

        public List<PuestoDeTrabajoConsultaModel> Harcode()
        {

            List<PuestoDeTrabajoConsultaModel> lista;
            lista = new List<PuestoDeTrabajoConsultaModel>();
            lista.Add(new PuestoDeTrabajoConsultaModel()
            {
                Id = 1,
                CodigoEmpresa = "2",
                NombreEmpresa = "maxi",
                CodigoCargo = 6,
                NombreCargo = "lalal",
                TipoPuesto = "skdhsahd"
            });

            lista.Add(new PuestoDeTrabajoConsultaModel()
            {
                Id = 2,
                CodigoEmpresa = "3",
                NombreEmpresa = "sdasd",
                CodigoCargo = 56,
                NombreCargo = "rrrr",
                TipoPuesto = "rrrr"
            });


            return lista;

        }

        private List<DtoPrevisionAusenciaModel> harcodeDtoProvicionAusenciaConHorasCatedras()
        {
            var listadto = new List<DtoPrevisionAusenciaModel>();
            var dto = new DtoPrevisionAusenciaModel()
            {
                Id = 1,
                CodigoEmpresa = "20",
                NombreEmpresa = "lala",
                Apellido = "farino",
                Nombre = "maximi",
                AnioGrado = "holaaaaa"


            };
            listadto.Add(dto);
            return listadto;
        }

        public DtoPrevisionAusenciaDatosModel HarcodeDtoProvicionAusenciaMostrar()
        {

            var dto = new DtoPrevisionAusenciaDatosModel()
                          {
                              IdPrevision = 1,
                              NombreEmpresa = "jojo",
                              Legajo = "25",
                              Estado = EstadoPrevisionAusenciaEnum.CERRADA,
                              DomicilioEmpresa = new DomicilioModel()
                                                     {

                                                         Id = 5334,
                                                     },
                              DomicilioAgente = new DomicilioModel()
                              {

                                  Id = 5334,
                              }


                          };

            return dto;
        }

        public List<DtoPrevisionAusenciaModel> HarcodeDtoProvicionAusencia()
        {
            var lista = new List<DtoPrevisionAusenciaModel>();
            lista.Add(new DtoPrevisionAusenciaModel()
                          {
                              Id = 1,
                              Nombre = "maxi",
                              Apellido = "farino",
                              NombrePuestoTrabajo = "hola",
                              Motivo = "fdf",
                              Estado = EstadoPrevisionAusenciaEnum.ACTIVA
                          });



            lista.Add(new DtoPrevisionAusenciaModel()
            {
                Id = 2,
              Nombre = "ale",
                Apellido = "burgos",
                NombrePuestoTrabajo = "hi",
                Motivo = "",
                Estado = EstadoPrevisionAusenciaEnum.ACTIVA
            });


            return lista;
        }

        #endregion

        #region Soporte

        public JsonResult ValidarEliminarPrevision(int idPrevision)
        {
            var validacion = Rule.ValidarEliminarPrevision(idPrevision);
            return Json(validacion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarExistenciaInasistencia(DateTime fechaDesde,DateTime fechaHasta,string legAgente)
        {
            var existe = Rule.VerificarExistenciaInasistenciaDocente(fechaDesde, fechaHasta, legAgente);
            return Json(existe, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}