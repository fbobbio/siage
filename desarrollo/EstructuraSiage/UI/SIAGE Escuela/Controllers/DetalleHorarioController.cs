using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;


namespace SIAGE_Escuela.Controllers
{
    public class DetalleController : AjaxAbmcDetalleController<DetalleHorarioModel, IDetalleHorarioRules>
    {
        private IUnidadAcademicaRules unidadAcademicaRules;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Rule = ServiceLocator.Current.GetInstance<IDetalleHorarioRules>();
            AbmcView = "HorarioEscuelaEditor";
            unidadAcademicaRules = ServiceLocator.Current.GetInstance<IUnidadAcademicaRules>();
        }

        public override void RegistrarPost(DetalleHorarioModel model, int idPadre)
        {
            var unidad = unidadAcademicaRules.GetUnidadAcademicaById(idPadre);
            unidad.DetalleHorario.Add(model);

            unidadAcademicaRules.UnidadAcademicaSave(unidad);
        }

        public override void EditarPost(DetalleHorarioModel model, int idPadre)
        {
            var unidad = unidadAcademicaRules.GetUnidadAcademicaById(idPadre);
            unidad.DetalleHorario = unidad.DetalleHorario.FindAll(x => x.Id != model.Id);
            unidad.DetalleHorario.Add(model);

            unidadAcademicaRules.UnidadAcademicaSave(unidad);
        }

        public override void EliminarPost(DetalleHorarioModel model, int idPadre)
        {
            var unidad = unidadAcademicaRules.GetUnidadAcademicaById(idPadre);
            unidad.DetalleHorario = unidad.DetalleHorario.FindAll(x => x.Id != model.Id);

            unidadAcademicaRules.UnidadAcademicaSave(unidad);
        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, TurnoEnum? filtroTurno,
              GradoAñoEnum? filtroGradoAño, DivisionEnum? filtroDivision, int? filtroIdAsignatura)
        {
            Func<UnidadAcademicaModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Turno" ? x => x.DiagramacionCurso.Turno :
                
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<UnidadAcademicaModel, IComparable>)(x => x.Id);
            //    // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = unidadAcademicaRules.GetHorarioEscuelaByFiltros( filtroTurno, filtroGradoAño, filtroDivision, filtroIdAsignatura);
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
                            a.DiagramacionCurso.Turno.ToString(),
                            a.DiagramacionCurso.GradoAño.ToString(),
                            a.DiagramacionCurso.Division.ToString(),
                            a.AsignaturaModel.Nombre.ToString()


                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

      
    }
}
