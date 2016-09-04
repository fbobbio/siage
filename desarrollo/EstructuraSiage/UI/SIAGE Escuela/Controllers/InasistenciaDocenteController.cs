using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class InasistenciaDocenteController : AjaxAbmcController<InasistenciaDocenteModel, IInasistenciaDocenteRules>
    {
        //
        // GET: /InasistenciaDocente/
        private IEntidadesGeneralesRules _entidadesGeneralesRules;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "InasistenciaDocenteEditor";
            _entidadesGeneralesRules = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            Rule = ServiceLocator.Current.GetInstance<IInasistenciaDocenteRules>();
        }

        public override ActionResult Index()
        {
            var idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(idEscuela)) // validar si estoy logueado como escuela
            {
                CargarViewData(EstadoABMC.Consultar);
                return View();
            }
            TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela.";
            return RedirectToAction("Error", "Home");
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            //Agente
            var idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            ViewData.Add(ViewDataKey.PAIS.ToString(), _entidadesGeneralesRules.GetPaisAll());
            ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(), _entidadesGeneralesRules.GetEstadoCivilAll());
            ViewData.Add(ViewDataKey.SEXO.ToString(), _entidadesGeneralesRules.GetSexoAll());
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), _entidadesGeneralesRules.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(), _entidadesGeneralesRules.GetTipoCalleAll());
            ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(), _entidadesGeneralesRules.GetOrganismoEmisorDocumentoAll());
            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), new List<LocalidadModel>());
            ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(), _entidadesGeneralesRules.GetDepartamentoProvincialByProvincia(idProvincia));

            //Puesto de trabajo
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), _entidadesGeneralesRules.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), _entidadesGeneralesRules.GetSituacionRevistaAll());
            ViewData.Add(ViewDataKey.TURNO.ToString(), _entidadesGeneralesRules.GetTurnoAll());
            ViewData.Add(ViewDataKey.ID_EMPRESA_USUARIO_LOGUEADO.ToString(), GetIdEmpresaUsuarioLogueado());

            ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), _entidadesGeneralesRules.GetGradoAñoAll());

            //Inasistencia
            ViewData.Add(ViewDataKey.DIRECCION_NIVEL.ToString(), _entidadesGeneralesRules.GetDireccionDeNivelAll());
            ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), _entidadesGeneralesRules.GetTipoCargoAll());
            ViewData.Add(ViewDataKey.ASIGNATURA.ToString(), _entidadesGeneralesRules.GetAsignaturaAll());
            ViewData.Add(ViewDataKey.TITULO.ToString(), _entidadesGeneralesRules.GetTituloAll());
            ViewData.Add(ViewDataKey.SUCURSAL_BANCARIA.ToString(), _entidadesGeneralesRules.GetSucursalBancariaAll());
            ViewData.Add(ViewDataKey.MOTIVO_BAJA_AGENTE.ToString(), _entidadesGeneralesRules.GetMotivoBajaAgenteAll());
            
        }


        /** Método que devuelve el ID de la empresa a la que está asociado el usuario logueado */
        private int GetIdEmpresaUsuarioLogueado()
        {
            var empresaUsuarioLogueado = Session[ConstantesSession.EMPRESA.ToString()];
            return (int)empresaUsuarioLogueado;
        }

        public override void RegistrarPost(InasistenciaDocenteModel model)
        {
            model.IdEmpresaUsuarioLogueado = (int)Session[ConstantesSession.EMPRESA.ToString()];
            
                Rule.InasistenciaDocenteSave(model);    

            
            
        }

        public override void EditarPost(InasistenciaDocenteModel model)
        {
            Rule.InasistenciaDocenteUpdate(model);
        }

        public override void EliminarPost(InasistenciaDocenteModel model)
        {
            Rule.InasistenciaDocenteDelete(model);
        }

        public JsonResult GetCantidadDiasDeAusencia(DateTime fechaDesde,DateTime fechaHasta, int idAgente, TipoMotivoInasistenciaDocenteEnum motivo)
        {
            var idEmpresaUsuarioLogueado = (int)Session[ConstantesSession.EMPRESA.ToString()];
            return Json(Rule.CalcularCantidadDiasAusencia(fechaDesde,fechaHasta, idEmpresaUsuarioLogueado, idAgente, motivo),JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows,int? idInasistencia, DateTime? filtroFechaDesde, 
            DateTime? filtroFechaHasta, string filtroLegajoAgente, bool? filtroAusenciaAnticipada,
            EstadoInasistenciaDocenteEnum? filtroEstadoInasistencia, TipoMotivoInasistenciaDocenteEnum? filtroTipoMotivoInasistencia)
        {
            // Construyo la funcion de ordenamiento
            Func<InasistenciaDocenteConsultaModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Id" ? x => x.Id :
                sidx == "LegajoAgente" ? x => x.LegajoAgenteInasistencia :
                sidx == "NombreAgenteInasistencia" ? x => x.NombreAgenteInasistencia :
                sidx == "ApellidoAgenteInasistencia" ? x => x.ApellidoAgenteInasistencia :
                sidx == "FechaDesdeInasistencia" ? x => x.FechaDesdeInasistencia :
                sidx == "FechaHastaInasistencia" ? x => x.FechaHastaInasistencia :
                sidx == "TipoMotivoInasistencia" ? x => x.TipoMotivoInasistencia :
                sidx == "EstadoInasistencia" ? x => x.EstadoInasistencia :
                /******************************** FIN AREA EDITABLE *******************************/
            (Func<InasistenciaDocenteConsultaModel, IComparable>)(x => x.Id);


            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetInasistenciasDocenteModel(idInasistencia,filtroFechaDesde, filtroFechaHasta, filtroLegajoAgente,
                                                              filtroAusenciaAnticipada, filtroEstadoInasistencia,
                                                              filtroTipoMotivoInasistencia);
            /******************************** FIN AREA EDITABLE *******************************/

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
                           cell = new string[]
                                                                    {
                                                                        a.Id.ToString(),
                                                                        // Respetar el orden en que se mostrarán las columnas
                                                                        /****************************** INICIO AREA EDITABLE ******************************/
                                                                        a.LegajoAgenteInasistencia,
                                                                        a.NombreAgenteInasistencia,
                                                                        a.ApellidoAgenteInasistencia,
                                                                        a.FechaDesdeInasistencia != null ? a.FechaDesdeInasistencia.ToShortDateString() : string.Empty,
                                                                        a.FechaHastaInasistencia!= null ? a.FechaHastaInasistencia.ToShortDateString() : string.Empty,
                                                                        a.TipoMotivoInasistencia.ToString(),
                                                                        a.EstadoInasistencia.ToString()
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


        public JsonResult ProcesarBusquedaAsignacionesAgentesAdheridoAlParo(string sidx, int? filtroNumeroDocumento, string filtroTipoDocumento,
            int? filtroSexo, string filtroApellido, string filtroNombre, DateTime filtroFechaDesde, DateTime filtroFechaHasta)
        {
            var idEmpresa = GetIdEmpresaUsuarioLogueado();
            var asignacionRules = ServiceLocator.Current.GetInstance<IAsignacionRules>();
            
            var registros = asignacionRules.GetAsignacionAgenteByFiltros(filtroNumeroDocumento, filtroTipoDocumento,
                                                                         filtroSexo, filtroApellido, filtroNombre,
                                                                         filtroFechaDesde, filtroFechaHasta, idEmpresa);

            return Json(registros,JsonRequestBehavior.AllowGet);
        }

        public JsonResult CalcularPorcentageParoAcatamiento(List<int> idAsignaciones)
        {
            List<PersonalPorTurnoModel> listaHardCodeada = new List<PersonalPorTurnoModel>();
            listaHardCodeada.Add(new PersonalPorTurnoModel()
                                     {
                                         NombreTurno = "MAÑANA",
                                         Porcentaje = 30
                                     });

            listaHardCodeada.Add(new PersonalPorTurnoModel()
                                     {
                                         NombreTurno = "TARDE",
                                         Porcentaje = 20 
                                     });

            listaHardCodeada.Add(new PersonalPorTurnoModel()
                                     {
                                         NombreTurno = "VESPERTINO",
                                         Porcentaje = 50 
                                     });

            listaHardCodeada.Add(new PersonalPorTurnoModel()
                                     {
                                         NombreTurno = "NOCHE",
                                         Porcentaje = 40 
                                     });
            listaHardCodeada.Add(new PersonalPorTurnoModel()
                                     {
                                         NombreTurno = "TOTAL",
                                         Porcentaje = 47 
                                     });
            //return Json(Rule.CalcularPorcentajeParoAcatamiento(idAsignaciones),JsonRequestBehavior.AllowGet);
            return Json(listaHardCodeada, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Devuelve la prevision correspondiente al puesto y agente pasados por parametro
        /// </summary>
        /// <param name="idPuesto">id puesto</param>
        /// <param name="idAgente">id agente</param>
        /// <returns>json con la prevision</returns>
        public JsonResult GetPrevisionesAusenciaDocenteConHorasCatedras(int idPuesto, int idAgente)
        {
            var registro = Rule.GetPrevisionAusenciaDocente(idAgente, idPuesto);
            if (registro != null)
            {
                var jsonData = new
                                   {
                                       Id = registro.Id.ToString(),
                                       FechaPrevision =
                                           registro.FechaPrevision.HasValue
                                               ? registro.FechaPrevision.Value.ToShortDateString()
                                               : string.Empty,
                                       ObservacionMotivo = registro.ObservacionMotivo,
                                       DiaSemana =
                                           registro.DiaSemanaEnum.HasValue
                                               ? registro.DiaSemanaEnum.ToString()
                                               : string.Empty,
                                       HoraDesde = registro.HoraDesde,
                                       HoraHasta = registro.HoraHasta
                                   };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Devuelve la prevision correspondiente al puesto y agente pasados por parametro
        /// </summary>
        /// <param name="idAgente">id agente</param>
        /// <param name="idPuesto">id puesto</param>
        /// <returns>json con la prevision</returns>
        public JsonResult GetPrevisionesAusenciaDocenteSinHorasCatedra(int idAgente, int idPuesto)
        {
            var registro = Rule.GetPrevisionAusenciaDocente(idAgente, idPuesto);
            if (registro != null)
            {
                var jsonData = new
                                   {
                                       Id = registro.Id.ToString(),
                                       FechaDesdeAutorizacion =
                                           registro.FechaDesdeAutorizacion.HasValue
                                               ? registro.FechaDesdeAutorizacion.Value.ToShortDateString()
                                               : string.Empty,
                                       FechaHastaAutorizacion =
                                           registro.FechaHastaAutorizacion.HasValue
                                               ? registro.FechaHastaAutorizacion.ToString()
                                               : string.Empty,
                                       ObservacionMotivo = registro.ObservacionMotivo,
                                   };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet); 
        }
        public JsonResult GetEstadoPuestoById(int id)
        {
            var estado = ServiceLocator.Current.GetInstance<IPuestoDeTrabajoRules>().GetEstadoPuestoById(id);
            return Json(estado.ToString(), JsonRequestBehavior.AllowGet);
        }

    }
}
