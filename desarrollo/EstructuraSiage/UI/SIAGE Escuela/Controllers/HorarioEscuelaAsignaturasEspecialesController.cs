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
    public class HorarioEscuelaAsignaturasEspecialesController : AjaxAbmcController<HorarioModel, IDetalleHorarioRules>
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

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "HorarioEscuelaAsignaturasEspecialesEditor";

            ViewData["ComboTurno"] = new SelectList(ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnoByEscuelaLogueada(), "Id", "Nombre");

            ViewData["ComboVacio"] = new SelectList(new List<string> {});
        }

        public JsonResult GetDetalleHorasTurno(int? turno)
        {
            var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();
            var configuracionTurno = ServiceLocator.Current.GetInstance<IConfiguracionTurnoRules>().GetConfiguracionTurnoVigente(null, turno, empresa.Id);

            //Variable a devolver, que será un JSON con valores HoraInicio y HoraFin para armar la grilla
            var horarios = new List<object>();

            //Aca armo la variable "Horarios" declarada arriba
            foreach (var idDetalle in configuracionTurno.DetalleHoras)
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

        public JsonResult GetAsignaturasEspecialesByTurno(int? turno, string sidx, string sord, int page, int rows)
        {
            var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();

            Func<UnidadAcademicaModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Division" ? x => x.DiagramacionCurso.Division :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<UnidadAcademicaModel, IComparable>)(x => x.Id);

            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = new List<UnidadAcademicaModel>();
            if (turno.HasValue)
            {
                registros = unidadAcademicaRules.GetUnidadesAcademicasByConfiguracionAsignaturaEspecial(empresa.Id, turno, null, null, null);
            }
            //    /******************************** FIN AREA EDITABLE *******************************/

            // Ordeno los registros));
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

        //Falta
        public JsonResult GetHorario(int id)
        {
            var model = unidadAcademicaRules.GetDetalleHorarioByUnidadAcademica(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //Listo (Pero esperar el cambio de la DB, para ver si sigue escribiendo: "VIERNES" o su respectivo valor de Enum
        [HttpPost]
        public override ActionResult Editar(HorarioModel model)
        {
            try
            {
                var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();
                var modeloFinal = ProcesarModeloHorario(model);
                ServiceLocator.Current.GetInstance<IUnidadAcademicaRules>().UpdateHorarioEscuelaByUnidadAcademica(modeloFinal, empresa.Id);
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

            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
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

            var modeloFinal = new HorarioModel();
            modeloFinal.TurnoId = modelo.TurnoId;
            modeloFinal.GradoAñoId = modelo.GradoAñoId;
            modeloFinal.Division = modelo.Division;
            modeloFinal.DetalleHorarioModels = new List<DetalleHorarioModel>();

            var anterior = new DetalleHorarioModel();

            //Recorremos el modelo nuevo
            foreach (DetalleHorarioModel item in modelo.DetalleHorarioModels)
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
