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
    public class SuspensionActividadController : AjaxAbmcController<SuspensionActividadModel, ISuspensionActividadRules>
    {
        private int _idEscuela;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "SuspensionActividadEditor";
            _idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
            Rule = ServiceLocator.Current.GetInstance<ISuspensionActividadRules>();
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

        public override void RegistrarPost(SuspensionActividadModel model)
        {
            model.IdEscuela = _idEscuela;
            Rule.SuspensionActividadSave(model);
        }

        public override void EditarPost(SuspensionActividadModel model)
        {
            model.IdEscuela = _idEscuela;
            Rule.SuspensionActividadUpdate(model);
        }

        public override void EliminarPost(SuspensionActividadModel model)
        {
            model.IdEscuela = _idEscuela;
            Rule.SuspensionActividadDelete(model);
        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, 
            DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, string filtroNombreAgente, string filtroApellidoAgente,
            bool filtroDadosDeBaja)
        {
            Func<SuspensionActividadModel, IComparable> funcOrden =
                sidx == "FechaSuspension" ? x => x.FechaSuspensionActividad :
                sidx == "HoraDesde" ? x => x.HoraDesde :
                sidx == "HoraHasta" ? x => x.HoraHasta :
                sidx == "Motivo" ? x => x.Motivo :
                (Func<SuspensionActividadModel, IComparable>)(x => x.Id);

            var registros = Rule.GetByFiltros(_idEscuela, filtroFechaDesde, filtroFechaHasta, filtroNombreAgente, filtroApellidoAgente, filtroDadosDeBaja);
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
                                a.FechaSuspensionActividad.Value.ToString("dd/MM/yyyy"),
                                a.HoraDesde.ToString(),
                                a.HoraHasta.ToString(),
                                a.Motivo,
                                a.FechaBaja.HasValue? "INACTIVA": "ACTIVA",
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarAgentesAfectadosRegistradosByIdSuspension(int idSuspension)
        {
            return Json(Rule.GetAgentesByIdSuspensionActividad(idSuspension), JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarAgentesSoloByIdSuspension(int idSuspension)
        {
            var resultado = Rule.GetAgentesByIdSuspension(idSuspension);
            var jsonData = new
            {
                Agentes = from a in resultado
                          select new string[][]
                       {
                           new string[] {
                                a.IdAgente.ToString(),
                                string.IsNullOrEmpty(a.Nombre)? "-": a.Nombre,
                                string.IsNullOrEmpty(a.Apellido)? "-": a.Apellido,
                                string.IsNullOrEmpty(a.Cargo)? "-": a.Cargo,
                                string.IsNullOrEmpty(a.NumLegajoJuntaMedia)? "-": a.NumLegajoJuntaMedia,
                                string.IsNullOrEmpty(a.NumLegajoJuntaPrimaria)? "-": a.NumLegajoJuntaPrimaria,
                                string.IsNullOrEmpty(a.LegajoSiage)? "-": a.LegajoSiage
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarAgentesAfectadosByFechaSuspension(DateTime fechaSuspension, string fechaDesde, string fechaHasta)
        {
            int idEscuela = _idEscuela;

            var aux = fechaSuspension.ToShortDateString() + " ";
            DateTime fechaHoraDesde = DateTime.Parse(aux + fechaDesde);
            DateTime fechaHoraHasta = DateTime.Parse(aux + fechaHasta);

            try
            {
                var resultado = Rule.GetAgentesbyEscuelaHorario(idEscuela, fechaHoraDesde, fechaHoraHasta);
                var jsonData = new
                {
                    Agentes = from a in resultado
                              select new string[][]
                       {
                           new string[] {
                                a.IdAgente.ToString(),
                                string.IsNullOrEmpty(a.Nombre)? "-": a.Nombre,
                                string.IsNullOrEmpty(a.Apellido)? "-": a.Apellido,
                                string.IsNullOrEmpty(a.Cargo)? "-": a.Cargo,
                                string.IsNullOrEmpty(a.NumLegajoJuntaMedia)? "-": a.NumLegajoJuntaMedia,
                                string.IsNullOrEmpty(a.NumLegajoJuntaPrimaria)? "-": a.NumLegajoJuntaPrimaria,
                                string.IsNullOrEmpty(a.LegajoSiage)? "-": a.LegajoSiage
                            }
                       }
                };

                //var jsonData = JsonAgentesAfectados();

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return ProcesarError(e);
            }
        }

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

        #region ELIMINAR

        private List<SuspensionActividadModel> Harcode(bool filtroDadosDeBaja)
        {
            var retorno = new List<SuspensionActividadModel>();
            var activas = new List<SuspensionActividadModel>()
                              {
                                  new SuspensionActividadModel()
                                      {
                                          Id = 1,
                                          FechaSuspensionActividad = DateTime.Now,
                                            HoraDesde = new TimeSpan(8,0,0),
                                            HoraHasta = new TimeSpan(12,0,0),
                                          Motivo = "Corte de luz",
                                          FechaBaja = null,
                                          IdAgentesAfectados = new int[0]
                                      },
                                  new SuspensionActividadModel()
                                      {
                                          Id = 2,
                                          FechaSuspensionActividad = DateTime.Now.AddDays(11),
                                            HoraDesde = new TimeSpan(8,0,0),
                                            HoraHasta = new TimeSpan(12,0,0),
                                          Motivo = "Rotura de caño",
                                          FechaBaja = null,
                                          IdAgentesAfectados = new int[0]
                                      },
                                  new SuspensionActividadModel()
                                      {
                                          Id = 3,
                                          FechaSuspensionActividad = DateTime.Now.AddDays(-5),
                                            HoraDesde = new TimeSpan(8,0,0),
                                            HoraHasta = new TimeSpan(12,0,0),
                                          Motivo = "Corte de agua",
                                          FechaBaja = null,
                                          IdAgentesAfectados = new int[0]
                                      }
                              };
            var inactivas = new List<SuspensionActividadModel>()
                                {

                                    new SuspensionActividadModel()
                                        {
                                            Id = 4,
                                            FechaSuspensionActividad = DateTime.Now.AddDays(15),
                                            HoraDesde = new TimeSpan(8,0,0),
                                            HoraHasta = new TimeSpan(12,0,0),
                                            Motivo = "Motivo de prueba eliminada 2",
                                            FechaBaja = DateTime.Now,
                                            IdAgentesAfectados = new int[0]
                                        },
                                    new SuspensionActividadModel()
                                        {
                                            Id = 5,
                                            FechaSuspensionActividad = DateTime.Now.AddDays(-10),
                                            HoraDesde = new TimeSpan(8,0,0),
                                            HoraHasta = new TimeSpan(12,0,0),
                                            Motivo = "Motivo de prueba eliminada 2",
                                            FechaBaja = DateTime.Now,
                                            IdAgentesAfectados = new int[0]
                                        }
                                };

            if(filtroDadosDeBaja)
            {
                retorno.AddRange(inactivas);
            }
            retorno.AddRange(activas);
            return retorno;
        }

        private object JsonAgentesAfectadosYSeleccionados()
        {
            return new
            {
                Agentes = new string[][] {
                    new string[] {
                        "1",
                        "Nombre 1",
                        "Apellido 1",
                        "Cargo 1",
                        "NumLegJM 1",
                        "NumLegJP 1"
                    },
                    new string[] {
                        "2",
                        "Nombre 2",
                        "Apellido 2",
                        "Cargo 2",
                        "NumLegJM 2",
                        "NumLegJP 2"
                    },
                    new string[] {
                        "3",
                        "Nombre 3",
                        "Apellido 3",
                        "Cargo 3",
                        "NumLegJM 3",
                        "NumLegJP 3"
                    }
                },
                Seleccionados = new string[] { "2", "3" }
            };
        }

        private object JsonAgentesAfectados()
        {
            return new
            {
                Agentes = new string[][] {
                    new string[] {
                        "1",
                        "Nombre 1",
                        "Apellido 1",
                        "Cargo 1",
                        "NumLegJM 1",
                        "NumLegJP 1"
                    },
                    new string[] {
                        "2",
                        "Nombre 2",
                        "Apellido 2",
                        "Cargo 2",
                        "NumLegJM 2",
                        "NumLegJP 2"
                    },
                    new string[] {
                        "3",
                        "Nombre 3",
                        "Apellido 3",
                        "Cargo 3",
                        "NumLegJM 3",
                        "NumLegJP 3"
                    }
                }
            };
        }

        /*[HttpGet]
        public override ActionResult Editar(int id)
        {
            const EstadoABMC estado = EstadoABMC.Editar;
            ViewData[AjaxAbmc.EstadoText] = estado.ToString();
            ViewData[AjaxAbmc.EstadoId] = (int)estado;

            var model = new SuspensionActividadModel()
            {
                Id = 1,
                FechaSuspensionActividad = DateTime.Now,
                HoraDesde = new TimeSpan(8, 0, 0),
                HoraHasta = new TimeSpan(12, 0, 0),
                Motivo = "Corte de luz",
                FechaBaja = null,
                IdAgentesAfectados = new int[0]
            };

            return PartialView(AbmcView, model);
        }

        [HttpGet]
        public override ActionResult Eliminar(int id)
        {
            const EstadoABMC estado = EstadoABMC.Eliminar;
            ViewData[AjaxAbmc.EstadoText] = estado.ToString();
            ViewData[AjaxAbmc.EstadoId] = (int)estado;

            var model = new SuspensionActividadModel()
            {
                Id = 1,
                FechaSuspensionActividad = DateTime.Now,
                HoraDesde = new TimeSpan(8, 0, 0),
                HoraHasta = new TimeSpan(12, 0, 0),
                Motivo = "Corte de luz",
                FechaBaja = null,
                IdAgentesAfectados = new int[0]
            };

            return PartialView(AbmcView, model);
        }*/
        #endregion
    }
}

