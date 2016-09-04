using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;
using Siage.Base;

namespace SIAGE_Escuela.Controllers
{
    public class RetrasoAgenteController : AjaxAbmcController<RetrasoAgenteModel, IRetrasoAgenteRules>
    {
        //
        // GET: /RetrasoAgente/
        private IEntidadesGeneralesRules entidades;
        private IEmpresaRules empresaRules;

        public override ActionResult Index()
        {
            CargarViewData(EstadoABMC.Consultar);
            return View();
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "RetrasoAgenteEditor";
            Rule = ServiceLocator.Current.GetInstance<IRetrasoAgenteRules>();
            entidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            string idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(),
                         entidades.GetDepartamentoProvincialByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), new List<LocalidadModel>());
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), entidades.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), entidades.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.DIRECCION_NIVEL.ToString(), entidades.GetDireccionDeNivelAll());
            ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), entidades.GetTipoCargoAll());
            ViewData.Add(ViewDataKey.ASIGNATURA.ToString(), entidades.GetAsignaturaAll());
            ViewData.Add(ViewDataKey.TITULO.ToString(), entidades.GetTituloAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), entidades.GetSituacionRevistaAll());
            ViewData.Add(ViewDataKey.SUCURSAL_BANCARIA.ToString(), entidades.GetSucursalBancariaAll());
            ViewData.Add(ViewDataKey.MOTIVO_BAJA_AGENTE.ToString(), entidades.GetMotivoBajaAgenteAll());
            ViewData.Add(ViewDataKey.PAIS.ToString(), entidades.GetPaisAll());
            ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(), entidades.GetTipoCalleAll());
            ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(), entidades.GetEstadoCivilAll());
            ViewData.Add(ViewDataKey.SEXO.ToString(), entidades.GetSexoAll());
            ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(), entidades.GetOrganismoEmisorDocumentoAll());
            ViewData.Add(ViewDataKey.TURNO.ToString(), entidades.GetTurnoAll());
            ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), entidades.GetGradoAñoAll());
        }

        public ActionResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, string filtroApellido,
                                                string filtroNombre, EstadoRetrasoEnum? filtroEstadoRetraso)
        {
            // Construyo la funcion de ordenamiento
            Func<RetrasoAgenteConsultaModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Id" ? x => x.Id :
                /******************************** FIN AREA EDITABLE *******************************/
            (Func<RetrasoAgenteConsultaModel, IComparable>)(x => x.Id);


            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetRetrasoAgenteByFiltros(id,filtroFechaDesde, filtroFechaHasta, filtroApellido,
                                                         filtroNombre, filtroEstadoRetraso);

             
            /******************************** FIN AREA EDITABLE *******************************/

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
                           cell = new string[]
                                                                    {
                                                                        
                                                                     a.Id.ToString(),
                                                                     a.CodigoEmpresa,
                                                                     a.NombreEmpresa,
                                                                     a.Barrio + " - " + a.Calle + " - " + a.Altura,
                                                                     a.Legajo,
                                                                     a.NombreAgente,
                                                                     a.ApellidoAgente,
                                                                     a.TipoDocumento,
                                                                     a.NumeroDocumento,
                                                                     a.TipoAgente,
                                                                     a.CargoAgente,
                                                                     a.AntiguedadAgente,
                                                                     a.BarrioAgente + " - " + a.CalleAgente + " - " + a.AlturaAgente,
                                                                     a.FechaRetraso.ToShortDateString(),
                                                                     a.MotivoRetraso.ToString(),
                                                                     a.CantidadMinutos.ToString(),
                                                                     a.Estado.ToString(),
                                                                     a.FechaBaja.ToString(),
                                                                     a.NombreAgenteBaja

                                                                        // Respetar el orden en que se mostrarán las columnas
                                                                        /****************************** INICIO AREA EDITABLE ******************************/
                                                                        /******************************** FIN AREA EDITABLE *******************************/
                                                                    }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca tipo y numero de documento del agente
        /// </summary>
        /// <param name="idAgente">id agente</param>
        /// <returns>Json con los datos del agente</returns>
        public JsonResult GetDniAgenteByIdAgente(int idAgente)
        {
            //TODO: Si hace falta crear la instancia en el initialize
            var ruleAccidenteLaboral = ServiceLocator.Current.GetInstance<IAccidenteLaboralRules>();
            var datosAgente = ruleAccidenteLaboral.GetDatosAgenteByIdAgente(idAgente);
            var json = new
            {
                NumeroDocumento = datosAgente.NumeroDocumento,
                TipoDocumento = datosAgente.TipoDocumento
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Determina si se registra o no una nueva inasistencia para el agente seleccionado y fecha ingresada
        /// </summary>
        /// <param name="idAgente">id agente</param>
        /// <param name="fechaRetraso">fecha de retraso</param>
        /// <returns>bool si se registra o no una inasistencia</returns>
        public JsonResult DeterminarSiSeRegistraInasistencia(int idAgente, DateTime fechaRetraso)
        {
            var registraNuevaInasistencia = Rule.DeterminarSiSeRegistraInasistencia(idAgente, fechaRetraso);

            return Json(registraNuevaInasistencia, JsonRequestBehavior.AllowGet);
        }

        //public override ActionResult Registrar()
        //{
        //    ViewData["FechaActual"] = DateTime.Today;
        //    return base.Registrar();
        //}

        //public JsonResult GetFechaActual()
        //{
        //    var fecha = DateTime.Today;
        //    return Json(fecha, JsonRequestBehavior.AllowGet);
        //}

        //public override void RegistrarPost(RetrasoAgenteModel model)
        //{
        //    Rule.RetrasoAgenteSave(model);
        //}

        public override ActionResult Registrar(RetrasoAgenteModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var modelReturned = Rule.RetrasoAgenteSave(model);
                    ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Registrar.ToString();
                    ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Registrar;
                    
                    
                    return Json(new
                    {
                        status = true,
                        model = ProcesarBusqueda(string.Empty,string.Empty,0,10,model.Id,null,null,string.Empty,string.Empty,null)
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Registrar.ToString();
                    ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Registrar;
                }
            }
            catch (Exception e)
            {
                ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Registrar.ToString();
                ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Registrar;

                while (e.InnerException != null)
                    e = e.InnerException;

                ModelState.AddModelError(string.Empty, e.Message);
            }

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

        public override void EliminarPost(RetrasoAgenteModel model)
        {
            Rule.RetrasoAgenteDelete(model);
        }
    }
}
