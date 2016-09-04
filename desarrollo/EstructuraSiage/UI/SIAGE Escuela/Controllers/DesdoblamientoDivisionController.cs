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
    public class DesdoblamientoDivisionController : Controller
    {
        #region Atributos / Propiedades

        private int idEmpresa;
        
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetCurrentEmpresa();
            idEmpresa = empresa.Id;
            ViewData["ComboVacio"] = new SelectList(new List<string>());
            ViewData["TurnoList"] = new SelectList(GetTurno(), "Id", "Nombre");
        }

        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(DesdoblamientoDivisionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var origen = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetDiagramacionCursoByFiltros(idEmpresa, model.IdTurno, model.GradoAño, model.ViejaDivision);
                    var destino = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetDiagramacionCursoByFiltros(idEmpresa, model.IdTurno, model.GradoAño, model.NuevaDivision);
                    ServiceLocator.Current.GetInstance<IDesdoblamientoDivisionRules>().DesdoblamientoDivisionSave(origen.Id, destino.Id, model.IdInscripciones);

                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new {status = false, details = new List<string> {"El modelo es invalido"}}, JsonRequestBehavior.AllowGet);
                }
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

        #endregion

        #region Procesamiento Busquedas

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, int? filtroTurno, int? filtroGrado, DivisionEnum? filtroDivision)
        {
            //Esto se verifica porque la grilla apenas se inicia, hace una llamada, con datos vacíos. Y los datos son obligatorios
            if (!filtroTurno.HasValue || !filtroGrado.HasValue || !filtroDivision.HasValue)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            // Construyo la funcion de ordenamiento
            Func<InscripcionModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Apellido" ? x => x.Estudiante.Persona.Apellido :
                sidx == "Nombre" ? x => x.Estudiante.Persona.Nombre :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<InscripcionModel, IComparable>)(x => x.Id);


            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = ServiceLocator.Current.GetInstance<IInscripcionRules>().GetInscripcionByFiltros(idEmpresa, filtroTurno, filtroGrado, filtroDivision);
            /******************************** FIN AREA EDITABLE *******************************/

            // Ordeno los registros);
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
                            a.Estudiante.Persona.Apellido,
                            a.Estudiante.Persona.Nombre
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Soporte

        public List<TurnoModel> GetTurno()
        {
            var turnos = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetTurnoByCarrera(idEmpresa, null);
            return turnos;
        }

        public JsonResult GetGradoByTurno(int? turno)
        {
            var json = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetGradosByCarreraTurno(idEmpresa, null, turno);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDivisionesByTurnoGrado(int turno, int grado)
        {
            var json = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetDivisionesByCarreraTurnoGrado(idEmpresa, null, turno, grado);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDivisionesParaDesdoblarByTurnoGrado(int turno, int grado)
        {
            var diagramaciones = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>().GetDiagramacionTurnoByHabilitadasYSinIncripcion(idEmpresa, turno, grado);
            var json = GetDivisionesParaDesdoblarByCarreraTurnoGrado(diagramaciones);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        private List<string> GetDivisionesParaDesdoblarByCarreraTurnoGrado(List<DiagramacionCursoModel> diagramaciones)
        {
            var divisiones = new List<string>();

            if (diagramaciones.Count > 0)
            {
                foreach (var item in diagramaciones)
                {
                    divisiones.Add(item.Division.ToString());
                }
            }

            return divisiones;
        }

        #endregion
    }
}
