using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;

namespace SIAGE_Escuela.Controllers
{
    public class ConfiguracionTurnoController : AjaxAbmcController<ConfiguracionTurnoModel, IConfiguracionTurnoRules>
    {
        #region Atributos / Propiedades

        private ICarreraRules carreraRules;
        private int idEscuela;
        private IDetalleHoraTurnoRules _detalleHoraTurnoRules;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Rule = ServiceLocator.Current.GetInstance<IConfiguracionTurnoRules>();
            AbmcView = "ConfiguracionTurnoEditor";
            _detalleHoraTurnoRules = ServiceLocator.Current.GetInstance<IDetalleHoraTurnoRules>();
            idEscuela = (int) Session[ConstantesSession.EMPRESA.ToString()];
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            ViewData.Add(ViewDataKey.TURNO.ToString(), ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnosByEscuela(idEscuela).OrderBy(x => x.Id).ToList());
            var nivel = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetNivelEducativoByEscuela(idEscuela);
            ViewData["NivelEducativo"] = nivel != null ? nivel.Nombre : string.Empty;
            ViewData.Add(ViewDataKey.CARRERA.ToString(), ServiceLocator.Current.GetInstance<ICarreraRules>().GetCarrerasByEscuela(idEscuela));
            
        }

        public override ActionResult Index()
        {
            CargarViewData(EstadoABMC.Consultar);
            return View();
        }

        #endregion

        #region POST ConfiguracionTurno

        public override void RegistrarPost(ConfiguracionTurnoModel model)
        {
            model.Escuela = idEscuela;
            Rule.ConfiguracionTurnoSave(model);
        }
        public override void EditarPost(ConfiguracionTurnoModel model)
        {
            model.Escuela = idEscuela;
            Rule.ConfiguracionTurnoUpdate(model);
        }

        public override void EliminarPost(ConfiguracionTurnoModel model)
        {
            Rule.ConfiguracionTurnoDelete(model);
        }

        #endregion

        #region Procesamiento Busquedas

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, int? filtroTurno, int? filtroCarrera)
        {
            Func<ConfiguracionTurnoModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Turno" ? x => x.Turno  :
                sidx == "DuracionHora" ? x => x.DuracionHora :
                sidx == "FechaInicioVigencia" ? x => x.FechaInicioVigencia :
                sidx == "TotalHorasTurno" ? x => x.TotalHorasTurno :
                
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<ConfiguracionTurnoModel, IComparable>)(x => x.Id);
            //    // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetConfiguracionTurnoByFiltros(filtroCarrera, filtroTurno,  idEscuela);  
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
                            //a.DiagramacionCurso.Carrera.Descripcion.ToString(),
                            a.TurnoNombre,
                            a.DuracionHora.ToString(),
                            a.FechaInicioVigencia.ToString(),
                            a.TotalHorasTurno.ToString()
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarDetalle(string sidx, string sord, int page, int rows, int id)
        {
            Func<DetalleHoraTurnoModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "OrdenHora" ? x => x.OrdenHora :
                sidx == "HoraInicio" ? x => x.HoraInicio :
                sidx == "HoraFin" ? x => x.HoraFin :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<DetalleHoraTurnoModel, IComparable>)(x => x.Id);
            //    // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = _detalleHoraTurnoRules.GetDetallesHoraTurnoByIdConfiguracionTurno(id);
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
                            a.OrdenHora.ToString(),
                            a.HoraInicio.ToString(),
                            a.HoraFin.ToString()
                            
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetalleHoraTurnoByConfiguracionTurno(string sidx, string sord, int page, int rows, int id)
        {
            sidx = "OrdenHora";
            sord = "asc";
            Func<DetalleHoraTurnoModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "OrdenHora" ? x => x.OrdenHora :
                sidx == "HoraInicio" ? x => x.HoraInicio :
                sidx == "HoraFin" ? x => x.HoraFin :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<DetalleHoraTurnoModel, IComparable>)(x => x.Id);

            var registros = Rule.GetDetalleHoraTurnoByConfiguracionTurnoId(id);
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
                            a.OrdenHora.ToString(),
                            (a.HoraInicio.Hours.ToString().Length == 1 ? "0" + a.HoraInicio.Hours : a.HoraInicio.Hours.ToString()) + ":" + (a.HoraInicio.Minutes.ToString().Length == 1 ? "0" + a.HoraInicio.Minutes : a.HoraInicio.Minutes.ToString()),
                            (a.HoraFin.Hours.ToString().Length == 1 ? "0" + a.HoraFin.Hours : a.HoraFin.Hours.ToString()) + ":" + (a.HoraFin.Minutes.ToString().Length == 1 ? "0" + a.HoraFin.Minutes : a.HoraFin.Minutes.ToString()),
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Soporte

        public ActionResult ActualizacionHoras(int duracionHoraNuevo, int duracionHoraAnterior, List<DetalleHoraTurnoModel> detalles)
        {
            if (duracionHoraAnterior != duracionHoraNuevo && detalles.Count != 0)
            {
                int diferencia = 0;
                if (duracionHoraAnterior > duracionHoraNuevo)
                {
                    diferencia = duracionHoraAnterior - duracionHoraNuevo;
                    detalles = Rule.ActualizarHoras(diferencia, false, detalles);
                }
                else
                {
                    diferencia = duracionHoraNuevo - duracionHoraAnterior;
                    detalles = Rule.ActualizarHoras(diferencia, true, detalles);
                }
            }

            var jsonData = new
            {
                rows = from a in detalles
                       select new
                       {
                           cell = new string[] {
                            a.Id.ToString(), 
                            a.OrdenHora.ToString(),
                            (a.HoraInicio.Hours.ToString().Length == 1 ? "0" + a.HoraInicio.Hours : a.HoraInicio.Hours.ToString()) + ":" + (a.HoraInicio.Minutes.ToString().Length == 1 ? "0" + a.HoraInicio.Minutes : a.HoraInicio.Minutes.ToString()),
                            (a.HoraFin.Hours.ToString().Length == 1 ? "0" + a.HoraFin.Hours : a.HoraFin.Hours.ToString()) + ":" + (a.HoraFin.Minutes.ToString().Length == 1 ? "0" + a.HoraFin.Minutes : a.HoraFin.Minutes.ToString()),
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }       
}
