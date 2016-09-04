using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class ActividadEspecialController : AjaxAbmcController<ActividadEspecialModel, IActividadEspecialRules>
    {
        private int _idEscuela;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "ActividadEspecialEditor";
            Rule = ServiceLocator.Current.GetInstance<IActividadEspecialRules>();
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

        [HttpGet]
        public override ActionResult Index()
        {
            //Validacion de usuario logueado
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(_idEscuela))
            {
                return View();
            }
            TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela.";
            return RedirectToAction("Error", "Home");
        }

        public override void RegistrarPost(ActividadEspecialModel model)
        {
            model.idEscuela = _idEscuela;
            Rule.ActividadSave(model);
        }

        public JsonResult ValidarExistenciaDeSuspensionActividades(DateTime fecha, string horaDesde, string horaHasta)
        {
            string result = Rule.ValidarExistenciaDeSuspensionActividades(fecha, horaDesde, horaHasta, _idEscuela); //Devuelve el motivo de la suspension de actividad
            if (!string.IsNullOrEmpty(result))
                result = "Existe una suspensión de actividad registrada para la fecha ingresada: " + result + "<br/>";
            if (!Rule.ValidadFechaConCalendario(fecha, _idEscuela))
                result += "El día ingresado corresponde a una fecha no hábil. <br/>";

            if (!string.IsNullOrEmpty(result))
                result += "¿Desea continuar con la registración?";

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public override void EditarPost(ActividadEspecialModel model)
        {
            model.idEscuela = _idEscuela;
            Rule.ActividadUpdate(model);
        }

        public override void EliminarPost(ActividadEspecialModel model)
        {
            model.idEscuela = _idEscuela;
            Rule.ActividadDelete(model);
        }

        [HttpPost]
        public virtual ActionResult Asignar(ActividadEspecialModel model)
        {
            return GenericPost(AsignarPost, model, EstadoABMC.Editar);
        }
        
        public void AsignarPost(ActividadEspecialModel model)
        {
            model.idEscuela = _idEscuela;
            Rule.AsignarDocenteActividadSave(model);
        }

        [HttpPost]
        public virtual ActionResult RegistrarAsistencia(ActividadEspecialModel model)
        {
            return GenericPost(RegistrarAsistenciaPost, model, EstadoABMC.Editar);
        }

        public void RegistrarAsistenciaPost(ActividadEspecialModel model)
        {
            model.idEscuela = _idEscuela;
            Rule.RegistrarAsistenciaDocente(model);
        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id,
            TipoActividadEspecialEnum? filtroTipoActividad, DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, string filtroNombreDocente,
            string filtroApellidoDocente, bool filtroDadosDeBaja)
        {
            Func<ActividadEspecialModel, IComparable> funcOrden =
                sidx == "Fecha" ? x => x.Fecha :
                sidx == "HoraDesde" ? x => x.HoraDesde :
                sidx == "HoraHasta" ? x => x.HoraHasta :
                (Func<ActividadEspecialModel, IComparable>)(x => x.Id);

            var registros = Rule.ActividadGetByFiltros(filtroFechaDesde, filtroFechaHasta, filtroApellidoDocente, filtroNombreDocente, 
                filtroTipoActividad, filtroDadosDeBaja, _idEscuela);
            //var registros = Harcode(filtroDadosDeBaja);

            if (sord == "asc")
                registros = registros.OrderBy(funcOrden).ToList();
            else
                registros = registros.OrderByDescending(funcOrden).ToList();

            int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            registros = registros.Skip((page - 1) * rows).Take(rows).ToList();

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
                                a.Fecha.Value.ToString("dd/MM/yyyy"),
                                a.HoraDesde.ToString(),
                                a.HoraHasta.ToString(),
                                a.Descripcion,
                                a.Tipo.ToString().Replace('_', ' '),
                                a.FechaBaja.HasValue? a.FechaBaja.Value.ToString("dd/MM/yyyy"): "-",
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Devuelve Json con los datos de los docentes asignados a la actividad especial y el resto de docentes de la escuela que no fueron asignados
        /// </summary>
        public JsonResult BuscarDocentesAsignadosYNoAsignadosByIdActividad(int idActividad)
        {
            try
            {
                var resultado = Rule.DocentesAsignadosYNoAsignadosByIdEscuela(_idEscuela, idActividad);
                resultado.GroupBy(x => x.IdAgente);
                var seleccionados = Rule.GetDocentesAsignadosByIdActividad(idActividad);
                var jsonData = new
                {
                    Docentes = from a in resultado
                              select new string[][]
                       {
                           new string[] {
                                a.IdAgente.ToString(),
                                a.LegajoSiage,
                                a.Nombre,
                                a.Apellido,
                                a.Cargo,
                                a.Asistencia.HasValue?
                                    a.Asistencia.Value? "Si": "No"
                                    : "-"
                            }
                       },
                    Seleccionados = from a in seleccionados
                              select new string[][]
                       {
                           new string[] {
                                a.ToString()
                                //a. //De donde saco si asistio o no?
                            }
                       }
                };

                //var jsonData = JsonTodosLosDocentesYSeleccionados();

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return ProcesarError(e);
            }
        }

        public JsonResult BuscarSoloDocentesAsignadosByIdActividad(int idActividad)
        {
            try
            {
                var resultado = Rule.DocentesAsignadosByIdActividad(_idEscuela, idActividad);
                var jsonData = new
                {
                    Docentes = from a in resultado
                              select new string[][]
                       {
                           new string[] {
                                a.IdAgente.ToString(),
                                a.LegajoSiage,
                                a.Nombre,
                                a.Apellido,
                                a.Cargo,
                                a.Asistencia.HasValue?
                                    a.Asistencia.Value? "Si": "No"
                                    : "-"
                            }
                       }
                };

                //var jsonData = JsonSoloDocentesAsignados();

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return ProcesarError(e);
            }
        }

        /*public JsonResult BuscarAsistenciaDocenteByIdActividad(int idActividad)
        {
            try
            {
                var resultado = Rule.DocentesAsignadosByIdActividad(_idEscuela, idActividad);
                var jsonData = new
                {
                    Docentes = from a in resultado
                               select new string[][]
                       {
                           new string[] {
                                a.IdAgente.ToString(),
                                a.LegajoSiage,
                                a.Nombre,
                                a.Apellido,
                                a.Cargo,
                                a.Asistencia.HasValue?
                                    a.Asistencia.Value? "Si": "No"
                                    : "-"
                            }
                       }
                };

                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return ProcesarError(e);
            }
        }*/

        private JsonResult ProcesarError(Exception exception)
        {
            while (exception.InnerException != null)
                exception = exception.InnerException;

            ModelState.AddModelError(string.Empty, exception.Message);

            var errores = new List<string>();
            for (int i = 0; i < ModelState.Values.Count; i++)
            {
                var propiedad = ModelState.Values.ElementAt(i);
                if (propiedad.Errors.Count != 0)
                {
                    errores.AddRange(propiedad.Errors.Select(item => string.IsNullOrEmpty(item.ErrorMessage) ? item.Exception.Message : item.ErrorMessage));
                }
            }

            return Json(new { status = false, details = errores.ToArray() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Asignar(int id)
        {
            AbmcView = "ActividadEspecialEditor";
            return ProcesarAbmGet(id, EstadoABMC.Registrar);
        }

        [HttpGet]
        public ActionResult RegistrarAsistencia(int id)
        {
            AbmcView = "ActividadEspecialEditor";
            return ProcesarAbmGet(id, EstadoABMC.Registrar);
        }

        #region ELIMINAR
        /*
        private List<ActividadEspecialModel> Harcode(bool filtroDadosDeBaja)
        {
            var retorno = new List<ActividadEspecialModel>();
            var activas = new List<ActividadEspecialModel>()
                              {
                                  new ActividadEspecialModel()
                                      {
                                          Id = 1,
                                            Tipo = TipoActividadEspecialEnum.ACTO_ACADEMICO,
                                          Fecha = DateTime.Now,
                                            HoraDesde = new TimeSpan(8,0,0),
                                            HoraHasta = new TimeSpan(12,0,0),
                                            Descripcion = "Acto escolar",
                                          FechaBaja = null
                                      },
                                  new ActividadEspecialModel()
                                      {
                                          Id = 2,
                                            Tipo = TipoActividadEspecialEnum.ACTO_ACADEMICO,
                                          Fecha = DateTime.Now.AddDays(11),
                                            HoraDesde = new TimeSpan(8,0,0),
                                            HoraHasta = new TimeSpan(12,0,0),
                                          Descripcion = "Rotura de caño",
                                          FechaBaja = null
                                      },
                                  new ActividadEspecialModel()
                                      {
                                          Id = 3,
                                            Tipo = TipoActividadEspecialEnum.ACTO_ACADEMICO,
                                          Fecha = DateTime.Now.AddDays(-5),
                                            HoraDesde = new TimeSpan(8,0,0),
                                            HoraHasta = new TimeSpan(12,0,0),
                                          Descripcion = "Feria escolar",
                                          FechaBaja = null
                                      }
                              };
            var inactivas = new List<ActividadEspecialModel>()
                                {

                                    new ActividadEspecialModel()
                                        {
                                            Id = 4,
                                            Tipo = TipoActividadEspecialEnum.ACTO_ACADEMICO,
                                            Fecha = DateTime.Now.AddDays(15),
                                            HoraDesde = new TimeSpan(8,0,0),
                                            HoraHasta = new TimeSpan(12,0,0),
                                            Descripcion = "Descripcion de prueba eliminada 2",
                                            FechaBaja = DateTime.Now
                                        },
                                    new ActividadEspecialModel()
                                        {
                                            Id = 5,
                                            Tipo = TipoActividadEspecialEnum.ACTO_ACADEMICO,
                                            Fecha = DateTime.Now.AddDays(-10),
                                            HoraDesde = new TimeSpan(8,0,0),
                                            HoraHasta = new TimeSpan(12,0,0),
                                            Descripcion = "Descripcion de prueba eliminada 2",
                                            FechaBaja = DateTime.Now
                                        }
                                };

            if (filtroDadosDeBaja)
            {
                retorno.AddRange(inactivas);
            }
            retorno.AddRange(activas);
            return retorno;
        }

        [HttpGet]
        public override ActionResult Editar(int id)
        {
            const EstadoABMC estado = EstadoABMC.Editar;
            ViewData[AjaxAbmc.EstadoText] = estado.ToString();
            ViewData[AjaxAbmc.EstadoId] = (int)estado;

            var model = new ActividadEspecialModel()
            {
                Id = 1,
                Tipo = TipoActividadEspecialEnum.ACTO_ACADEMICO,
                Fecha = DateTime.Now,
                HoraDesde = new TimeSpan(8, 0, 0),
                HoraHasta = new TimeSpan(12, 0, 0),
                Descripcion = "Acto escolar",
                FechaBaja = null
            };

            return PartialView(AbmcView, model);
        }

        [HttpGet]
        public override ActionResult Eliminar(int id)
        {
            const EstadoABMC estado = EstadoABMC.Eliminar;
            ViewData[AjaxAbmc.EstadoText] = estado.ToString();
            ViewData[AjaxAbmc.EstadoId] = (int)estado;

            var model = new ActividadEspecialModel()
            {
                Id = 1,
                Tipo = TipoActividadEspecialEnum.ACTO_ACADEMICO,
                Fecha = DateTime.Now,
                HoraDesde = new TimeSpan(8, 0, 0),
                HoraHasta = new TimeSpan(12, 0, 0),
                Descripcion = "Acto escolar",
                FechaBaja = DateTime.Today
            };

            return PartialView(AbmcView, model);
        }

        [HttpGet]
        public override ActionResult Ver(int id)
        {
            const EstadoABMC estado = EstadoABMC.Ver;
            ViewData[AjaxAbmc.EstadoText] = estado.ToString();
            ViewData[AjaxAbmc.EstadoId] = (int)estado;

            var model = new ActividadEspecialModel()
            {
                Id = 1,
                Tipo = TipoActividadEspecialEnum.ACTO_ACADEMICO,
                Fecha = DateTime.Now,
                HoraDesde = new TimeSpan(8, 0, 0),
                HoraHasta = new TimeSpan(12, 0, 0),
                Descripcion = "Acto escolar",
                FechaBaja = null
            };

            return PartialView(AbmcView, model);
        }

        private object JsonTodosLosDocentesYSeleccionados()
        {
            return new
            {
                Docentes = new string[][] {
                    new string[] {
                        "1",
                        "52670",
                        "Nombre 1",
                        "Apellido 1",
                        "Cargo 1",
                        "Si"
                    },
                    new string[] {
                        "2",
                        "52671",
                        "Nombre 2",
                        "Apellido 2",
                        "Cargo 2",
                        "Si"
                    },
                    new string[] {
                        "3",
                        "52672",
                        "Nombre 3",
                        "Apellido 3",
                        "Cargo 3",
                        "No"
                    }
                },
                Seleccionados = new string[] { "2", "3" }
            };
        }

        private object JsonSoloDocentesAsignados()
        {
            return new
            {
                Docentes = new string[][] {
                    new string[] {
                        "1",
                        "52670",
                        "Nombre 1",
                        "Apellido 1",
                        "Cargo 1",
                        "Si"
                    },
                    new string[] {
                        "2",
                        "52671",
                        "Nombre 2",
                        "Apellido 2",
                        "Cargo 2",
                        "Si"
                    },
                    new string[] {
                        "3",
                        "52672",
                        "Nombre 3",
                        "Apellido 3",
                        "Cargo 3",
                        "No"
                    }
                }
            };
        }
        */
        #endregion
    }
}
