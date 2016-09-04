using System;
using System.Linq;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using SIAGE.UI_Common.Controllers;

namespace SIAGE_Escuela.Controllers
{
    public class EstudianteConsultarController : AjaxAbmcController<EstudianteModel, IEstudianteRules>
    {
        #region Atributos / Propiedades

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "EstudianteEditor";
            Rule = ServiceLocator.Current.GetInstance<IEstudianteRules>();
            
            //CargarViewData();
        }
        
        #endregion

        #region POST ConfiguracionTurno

        public override void RegistrarPost(EstudianteModel model)
        {
            Rule.EstudianteSave(model);

        }
        public override void EditarPost(EstudianteModel model)
        {
            Rule.EstudianteUpdate(model);
        }

        public override void EliminarPost(EstudianteModel model)
        {
            Rule.EstudianteDelete(model);
        }

        #endregion

        #region Procesamiento Busquedas

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, string filtroSexo, string filtroDni, string filtroNombre, string filtroApellido)
        {
            Func<EstudianteModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Nombre" ? x => x.Persona.Nombre :
                sidx == "Apellido" ? x => x.Persona.Apellido :
                sidx == "Dni" ? x => x.Persona.NumeroDocumento :
                sidx == "Sexo" ? x => x.Persona.Sexo :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<EstudianteModel, IComparable>)(x => x.Id);
            //    // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetEstudianteByFiltros(filtroSexo, filtroDni, filtroNombre, filtroApellido);
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
                            a.Persona.Nombre,
                            a.Persona.Apellido,
                            a.Persona.Sexo,
                            a.Persona.NumeroDocumento
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
