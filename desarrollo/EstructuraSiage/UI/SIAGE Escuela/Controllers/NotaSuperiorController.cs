using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    /** 
    * <summary> Clase de implementación del controlador NotaSuperiorController
    *	
    * </summary>
    * <remarks>
    *		Autor: COMPU
    *		Fecha: 11/8/2011 7:07:31 PM
    * </remarks>
    */
    public class NotaSuperiorController : Controller
    {

        /** Región para la declaración de atributos de clase */
        #region Atributos
        private INotaSuperiorRules Rule;
        #endregion

        /** Región para declarar la inicialización del controller */
        #region Superiorización
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            Rule = ServiceLocator.Current.GetInstance<INotaSuperiorRules>();

            // Validar si estoy logueado como escuela
            var idEscuela = (int)Session[Siage.Base.ConstantesSession.EMPRESA.ToString()];
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(idEscuela))
            {
                ViewData["GradoAnioList"] = ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoPorNivelEducativoDeEscuela(idEscuela);
                ViewData["CarrerasList"] = ServiceLocator.Current.GetInstance<ICarreraRules>().GetCarrerasByEscuela(idEscuela);
            }
        }
        #endregion

        public ActionResult Index()
        {
            var idEscuela = (int)Session[Siage.Base.ConstantesSession.EMPRESA.ToString()];
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(idEscuela)) // validar si estoy logueado como escuela
            {
                var escuela = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetEscuelaDtoById(idEscuela);
                if (escuela.NivelEducativo.Id == 4) //Solo acceden nivel superior 
                    return View();

                TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela nivel superior.";
            }
            else
                TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela.";
            return RedirectToAction("Error", "Home");
        }

        public JsonResult GetAsignaturas(string carrera, string anio)
        {
            //var idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];

            var asignaturas = Rule.GetAsignaturas(int.Parse(carrera), int.Parse(anio));

            //var asignaturas = Rule.GetAsignaturas(idEscuela, anio, turno, division);
            var json = asignaturas.Select(d => new {d.Id, Nombre = d.AsignaturaNombre });
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSistemaNota(int asignatura)
        {
            var valor = Rule.GetSistemaNotaByAsignatura(asignatura);
            return Json(valor, JsonRequestBehavior.AllowGet);
        }
       
        /** Región para declarar los métodos POST (Agregar, Editar y Eliminar) */
        #region Post
        public void RegistrarPost(NotaSuperiorModel model)
        {
            Rule.Registrar(model);
        }
        #endregion

        
        /** Región para declarar métodos de procesamiento que devuelvan JsonResults*/
        #region Procesamiento Busquedas

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, string filtroAnio, string filtroTurno, string filtroDivision, string filtroCarrera, string filtroAsignatura)
        {

            Func<InscripcionNotasModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Nombre" ? x => x.Nombre :
                sidx == "Apellido" ? x => x.Apellido :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<InscripcionNotasModel, IComparable>)(x => x.Id);
            //    // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var idEscuela = (int)Session[Siage.Base.ConstantesSession.EMPRESA.ToString()];

            var registros = Rule.GetInscripciones(idEscuela, filtroAnio, filtroTurno, filtroDivision, filtroCarrera, filtroAsignatura);
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
                            a.Apellido,
                            a.Nombre,
                            a.Nota1
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        /** Región para la declaración de métodos de validación y soporte en general */
        #region Soporte
        #endregion
    }
}