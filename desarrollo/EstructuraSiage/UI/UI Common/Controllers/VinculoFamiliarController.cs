using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using SIAGE.UI_Common.Controllers;



namespace SIAGE_Escuela.Controllers
{
    public class VinculoFamiliarController : AjaxAbmcController<VinculoFamiliarModel, IVinculoFamiliarRules>
    {
        #region Atributos / Propiedades

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Rule = ServiceLocator.Current.GetInstance<IVinculoFamiliarRules>();
            AbmcView = "VinculoFamiliarEditor";
        }

        #endregion

        #region POST VinculoFamiliar

        public override void RegistrarPost(VinculoFamiliarModel model)
        {
            Rule.VinculoFamiliarSave(model);
        }
        public override void EditarPost(VinculoFamiliarModel model)
        {
            Rule.VinculoFamiliarUpdate(model);
        }

        public override void EliminarPost(VinculoFamiliarModel model)
        {
            Rule.VinculoFamiliarDelete(model);
        }

        #endregion

        #region Procesamiento Busquedas

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, int idPersona)
        {
            Func<VinculoFamiliarModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "TipoDocumento" ? x => x.Pariente.TipoDocumento :
                sidx == "NumeroDocumento" ? x => x.Pariente.NumeroDocumento :
                sidx == "Nombre" ? x => x.Pariente.Nombre :
                sidx == "Apellido" ? x => x.Pariente.Apellido :
                sidx == "Sexo" ? x => x.Pariente.Sexo :
                sidx == "VinculoNombre" ? x => x.VinculoNombre:
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<VinculoFamiliarModel, IComparable>)(x => x.Id);
            //    // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetVinculosByPersonaFisica(idPersona);  
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
                            a.Pariente.TipoDocumento,
                            a.Pariente.NumeroDocumento,
                            a.Pariente.Nombre,
                            a.Pariente.Apellido,
                            a.Pariente.Sexo,
                            a.VinculoId,
                            a.VinculoNombre
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Soporte
        public JsonResult GetVinculosByPersona(int? id, int idPersona)
        {
            
            var registros = Rule.GetVinculosByPersonaFisica(idPersona);


            var jsonData = new
            {
                rows = from a in registros
                       select new
                       {
                            a.Id, 
                            // Respetar el orden en que se mostrarán las columnas
                            /****************************** INICIO AREA EDITABLE ******************************/
                            Pariente = new {
                                a.Pariente.Id,
                                a.Pariente.TipoDocumento,
                                a.Pariente.NumeroDocumento,
                                a.Pariente.Nombre,
                                a.Pariente.Apellido,
                                FechaNacimiento = a.Pariente.FechaNacimiento.ToString(),
                                a.Pariente.EstadoCivil,
                                a.Pariente.Observaciones,
                                //a.Pariente.Clase,
                                a.Pariente.OrganismoEmisorDocumento,
                                IdSexo = a.Pariente.Sexo,
                                Sexo = a.Pariente.SexoNombre,
                                a.Pariente.IdPaisEmisorDocumento,
                                a.Pariente.IdPaisNacionalidad,
                                a.Pariente.IdPaisOrigen,
                                a.Pariente.ProvinciaNacimiento,
                                a.Pariente.DepartamentoProvincialNacimiento,
                                a.Pariente.LocalidadNacimiento,
                            },
                            a.VinculoId,
                            a.VinculoNombre,
                            a.Ocupacion,
                            a.Telefono,
                            a.Vive,
                            a.PermisoRetiro
                            /******************************** FIN AREA EDITABLE *******************************/
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }       
}
