using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class InformeInasistenciaController : Controller
    {
        #region Atributos / Propiedades

        private int _idEscuela;
        private int _idNivel;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
        }

        public ActionResult Index()
        {
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(_idEscuela) && _idNivel != (int)NivelEducativoNombreEnum.SUPERIOR) // validar si estoy logueado como escuela de nivel superior
            {
                ViewData["NivelId"] = _idNivel = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetNivelEducativoByEscuela(_idEscuela).Id;
                ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoPorNivelEducativoDeEscuela(_idEscuela));
                ViewData.Add(ViewDataKey.TURNO.ToString(), ServiceLocator.Current.GetInstance<ITurnoRules>().GetTurnosByEscuela(_idEscuela));
                return View();
            }
            TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela de nivel inicial, primario o medio.";
            return RedirectToAction("Error", "Home");
            
        }
        #endregion


        #region Procesamiento Busquedas
        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id,
             int filtroTurno, int filtroGrado, DivisionEnum filtroDivision, int? filtroAsignatura)
        {
            // Construyo la funcion de ordenamiento
            Func<InasistenciaRegistrarModel, IComparable> funcOrden =
                sidx == "Apellido" ? x => x.Apellido :
                sidx == "Nombre" ? x => x.Nombre :
                (Func<InasistenciaRegistrarModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = ServiceLocator.Current.GetInstance<IAsistenciaRules>().VerificarInasistencias(_idEscuela, filtroTurno, filtroGrado, filtroDivision, filtroAsignatura);

            // Ordeno los registros
            registros = sord == "asc" ? registros.OrderBy(funcOrden).ToList() : registros.OrderByDescending(funcOrden).ToList();

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
                        a.Documento,
                        a.Ausencias.ToString(),
                        a.AusenciasJustificadas.ToString()
                    }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Soporte

        public JsonResult GetTurnosByGradoAnio(int idGradoAnio)
        {
            var turnos = ServiceLocator.Current.GetInstance<IInscripcionRules>().GetTurnosByGradoAnio(idGradoAnio, _idEscuela).OrderBy(x => x.Nombre);
            return Json(new SelectList(turnos, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEspecialidadesEscuelaAsociadasAGradoAnio(int idGradoAnio, int idTurno, DivisionEnum idDivision)
        {
            var especialidades = ServiceLocator.Current.GetInstance<IAsistenciaRules>().GetAsignaturasEspecialesByDiagramacionCurso(idGradoAnio, idTurno, idDivision, _idEscuela).OrderBy(x => x.AsignaturaNombre);
            return Json(new SelectList(especialidades, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDivisionesEscuelaByGradoAnioTurno(int idGradoAnio, int idTurno)
        {
            List<DivisionEnum> divisiones = ServiceLocator.Current.GetInstance<IInscripcionRules>().GetDivisionesEscuelaByGradoAnioTurno(idGradoAnio, idTurno, _idEscuela);
            var items = new SelectList(
                (divisiones.Cast<object>().Select(
                    item => new { Text = item.ToString(), Value = item.ToString() })), "Value", "Text");

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
