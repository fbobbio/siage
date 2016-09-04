using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Controllers;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    
    public class NotaPrimarioMedioController : Controller
    {

        /** Región para la declaración de atributos de clase */
        #region Atributos
            private INotaPrimarioMedioRules Rule;
        #endregion

        /** Región para declarar la inicialización del controller */
        #region Inicialización
            protected override void Initialize(System.Web.Routing.RequestContext requestContext)
            {
                base.Initialize(requestContext);

                Rule = ServiceLocator.Current.GetInstance<INotaPrimarioMedioRules>();

                // Validar si estoy logueado como escuela
                var idEscuela = (int)Session[Siage.Base.ConstantesSession.EMPRESA.ToString()];
                if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(idEscuela)) 
                {
                    ViewData["GradoAnioList"] =
                        ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoPorNivelEducativoDeEscuela(
                            idEscuela);

                    //ViewData["EtapaList"] = 
                }
            }
        

        #endregion

        public ActionResult Index()
        {
            var idEscuela = (int)Session[Siage.Base.ConstantesSession.EMPRESA.ToString()];
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(idEscuela)) // validar si estoy logueado como escuela
            {
                var escuela = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetEscuelaDtoById(idEscuela);
                if (escuela.NivelEducativo.Id == 3)//Solo acceden nivel medio == 3
                    return View();

                TempData[Constantes.ErrorVista] =
                    "Opción válida unicamente para usuarios logueados como escuela nivel primario o medio.";
            }
            else
                TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela.";
            return RedirectToAction("Error", "Home");
        }

        /** Región para declarar los métodos POST (Agregar, Editar y Eliminar) */
        #region Post
            public void RegistrarPost(NotaPrimarioMedioModel model)
            {
                Rule.Registrar(model);
            }
        #endregion
        
        /** Región para declarar métodos de procesamiento que devuelvan JsonResults*/
        #region Procesamiento Busquedas
            public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, string filtroAnio, string filtroTurno, string filtroDivision)
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
                var registros = Rule.GetInscripciones(idEscuela, filtroAnio, filtroTurno, filtroDivision);
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
                            a.Nota1,
                            a.Nota2,
                            a.Nota3,
                            a.Nota4,
                            a.Nota5,
                            a.Nota6,
                            a.Nota7
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                           }
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        #endregion

        public JsonResult GetAsignaturas(string anio, string division, string turno)
        {
            var idEscuela = (int)Session[Siage.Base.ConstantesSession.EMPRESA.ToString()];
            var asignaturas = Rule.GetAsignaturas(idEscuela, anio, turno, division);
            var json = asignaturas.Select(d => new {Id = d.Id, Nombre = d.AsignaturaNombre});
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public bool ValidarEtapa(string etapa, string gradoAnio)
        {
            EtapasEnum etapaEnum = (EtapasEnum)Enum.Parse(typeof (EtapasEnum), etapa);
            return Rule.ValidarEtapa(etapaEnum, gradoAnio);
        }

        public JsonResult GetSistemaNota(int asignatura)
        {
            var valor = Rule.GetSistemaNotaByAsignatura(asignatura);
            return Json(valor, JsonRequestBehavior.AllowGet);
        }

        public void GetById(int id)
        {
            id = 44;
            var resultado = Rule.GetSistemaNotaById(id);
        }

        /** Región para la declaración de métodos de validación y soporte en general */
        #region Soporte
        #endregion
    }
}