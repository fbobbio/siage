using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SIAGE.UI_Common.Controllers;
using Siage.Services.Core.Models;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using System.Configuration;
using SIAGE.UI_Common.Content;

namespace SIAGE_Ministerio.Controllers
{
    public class PuestoDeTrabajoProvisorioController : AjaxAbmcController<PuestoDeTrabajoProvisorioModel, IPuestoDeTrabajoRules>
    {

        /** Región para la declaración de atributos de clase */
        #region Atributos
        private IEntidadesGeneralesRules _entidadesGeneralesRules;

        #endregion

        /** Región para declarar la inicialización del controller */
        #region Inicialización
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _entidadesGeneralesRules = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();

            Rule = ServiceLocator.Current.GetInstance<IPuestoDeTrabajoRules>();
        }
        public override ActionResult Index()
        {
            InicializadorConsultaPT();
            return View();
        }

        
        private void InicializadorConsultaPT()
        {
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), _entidadesGeneralesRules.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), _entidadesGeneralesRules.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), _entidadesGeneralesRules.GetSituacionRevistaAll());
            ViewData.Add(ViewDataKey.TURNO.ToString(), _entidadesGeneralesRules.GetTurnoAll());
            ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), _entidadesGeneralesRules.GetGradoAñoAll());
        }

        private void InicializadorConsultaAgente()
        {
            var idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), _entidadesGeneralesRules.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), _entidadesGeneralesRules.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), _entidadesGeneralesRules.GetSituacionRevistaAll());
            ViewData.Add(ViewDataKey.DIRECCION_NIVEL.ToString(), _entidadesGeneralesRules.GetDireccionDeNivelAll());
            ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(), _entidadesGeneralesRules.GetDepartamentoProvincialByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), new List<LocalidadModel>());
            ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), _entidadesGeneralesRules.GetTipoCargoAll());
            ViewData.Add(ViewDataKey.ASIGNATURA.ToString(), _entidadesGeneralesRules.GetAsignaturaAll());
            ViewData.Add(ViewDataKey.TITULO.ToString(), _entidadesGeneralesRules.GetTituloAll());
            ViewData.Add(ViewDataKey.PAIS.ToString(), _entidadesGeneralesRules.GetPaisAll());
            ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(), _entidadesGeneralesRules.GetEstadoCivilAll());
            ViewData.Add(ViewDataKey.SEXO.ToString(), _entidadesGeneralesRules.GetSexoAll());
            ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(), _entidadesGeneralesRules.GetTipoCalleAll());
            ViewData.Add(ViewDataKey.SUCURSAL_BANCARIA.ToString(), _entidadesGeneralesRules.GetSucursalBancariaAll());
            ViewData.Add(ViewDataKey.MOTIVO_BAJA_AGENTE.ToString(), _entidadesGeneralesRules.GetMotivoBajaAgenteAll());
            ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(), _entidadesGeneralesRules.GetOrganismoEmisorDocumentoAll());
            ViewData.Add(ViewDataKey.TIPO_VINCULO.ToString(), _entidadesGeneralesRules.GetTiposVinculoAll());
        }

        private void InicializadorConsultaEmpresa()
        {
            string idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            ViewData.Add(ViewDataKey.INSPECCION_INTERMEDIA.ToString(), ServiceLocator.Current.GetInstance<IEmpresaRules>().GetInspeccionIntermediaByDireccionDeNivelActual());
            ViewData.Add(ViewDataKey.TIPO_ESCUELA.ToString(), _entidadesGeneralesRules.GetTipoEscuelaAll());
            ViewData.Add(ViewDataKey.PERIODO_LECTIVO.ToString(), _entidadesGeneralesRules.GetPeriodoLectivoAll());
            ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(), _entidadesGeneralesRules.GetDepartamentoProvincialByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), new List<LocalidadModel>());
            ViewData.Add(ViewDataKey.ZONA_DESFAVORABLE.ToString(), _entidadesGeneralesRules.GetZonaDesfavorableAll());
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), _entidadesGeneralesRules.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.OBRA_SOCIAL.ToString(), _entidadesGeneralesRules.GetObraSocialAll());
            ViewData.Add(ViewDataKey.TURNO.ToString(), _entidadesGeneralesRules.GetTurnoAll());
            ViewData.Add(ViewDataKey.MODALIDAD_JORNADA.ToString(), _entidadesGeneralesRules.GetModalidadJornadaAll());
            ViewData.Add(ViewDataKey.PROGRAMA_PRESUPUESTARIO.ToString(), _entidadesGeneralesRules.GetProgramaPresupuestarioAll());
        }
        
        private void InicializadorConsultaInscripcion()
        {
            var entidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            var empresa = ServiceLocator.Current.GetInstance<IEmpresaRules>();
            var idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");

            // Empresa
            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), entidades.GetLocalidadByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(), entidades.GetDepartamentoProvincialByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.INSPECCION_INTERMEDIA.ToString(), empresa.GetInspeccionIntermediaByDireccionDeNivelActual());
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), entidades.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.MODALIDAD_JORNADA.ToString(), entidades.GetModalidadJornadaAll());
            ViewData.Add(ViewDataKey.OBRA_SOCIAL.ToString(), entidades.GetObraSocialAll());
            ViewData.Add(ViewDataKey.PERIODO_LECTIVO.ToString(), entidades.GetPeriodoLectivoAll());
            ViewData.Add(ViewDataKey.PROGRAMA_PRESUPUESTARIO.ToString(), entidades.GetProgramaPresupuestarioAll());
            ViewData.Add(ViewDataKey.TIPO_ESCUELA.ToString(), entidades.GetTipoEscuelaAll());
            ViewData.Add(ViewDataKey.TURNO.ToString(), entidades.GetTurnoAll());
            ViewData.Add(ViewDataKey.ZONA_DESFAVORABLE.ToString(), entidades.GetZonaDesfavorableAll());

            //Inscripcion
            ViewData.Add(ViewDataKey.SEXO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetSexoAll());
        }
        
        #endregion

        #region Get
        [HttpGet]
        public override ActionResult Registrar()
        {
            //Registro general
            AbmcView = "PuestoDeTrabajoProvisorioEditor";
            InicializadorConsultaAgente();
            return ProcesarAbmGet(null, EstadoABMC.Registrar);
        }

        [HttpGet]
        public override ActionResult Eliminar(int id)
        {
            //Registro general
            AbmcView = "PuestoDeTrabajoProvisorioEliminarEditor";
            var model = Rule.GetPuestoDeTrabajoProvisorioById(id);
            return PartialView(AbmcView, model);
        }

        [HttpGet]
        public ActionResult RegistrarTareasPasivas(int? id)
        {
            AbmcView = "PTProvisorioMinisterioEditor";
            InicializadorConsultaEmpresa();
            return ProcesarAbmGet(id,EstadoABMC.Registrar);
        }

        [HttpGet]
        public ActionResult RegistrarItinerante(int? id)
        {
            AbmcView = "PTProvisorioItineranteEditor";
            InicializadorConsultaEmpresa();
            return ProcesarAbmGet(id, EstadoABMC.Registrar);
        }

        [HttpGet]
        public ActionResult RegistrarOtroMinisterio(int? id)
        {
            AbmcView = "PTProvisorioOtroMinisterioEditor";
            
            InicializadorConsultaEmpresa();
            //ViewData de tipos cargos
            ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), _entidadesGeneralesRules.GetTipoCargoAll().FindAll(x => x.Estado == EstadoTipoCargoEnum.VIGENTE));
            ViewData.Add(ViewDataKey.TIPO_CARGO_ESPECIAL.ToString(), _entidadesGeneralesRules.GetTipoCargoEspecialAll());

            return ProcesarAbmGet(null, EstadoABMC.Registrar);
        }

        [HttpGet]
        public ActionResult RegistrarMaestraIntegradora(int? id)
        {
            AbmcView = "PTProvisorioMaestraIntegradoraEditor";
            InicializadorConsultaInscripcion();
            return ProcesarAbmGet(null, EstadoABMC.Registrar);
        }
       
        #endregion

        /** Región para declarar los métodos POST (Agregar, Editar y Eliminar) */
        #region Post

        [HttpPost]
        public ActionResult RegistrarMinisterio(PuestoDeTrabajoProvisorioModel model)
        {
            try
            {
                Rule.PuestoDeTrabajoProvisorioTareaPasivaSave(model);

                ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Revisar.ToString();
                ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Revisar;

                return Json(new { status = true });
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

            return Json(new { status = false, details = errores.ToArray() });
        }

        [HttpPost]
        public ActionResult RegistrarItinerante(PuestoDeTrabajoProvisorioModel model)
        {
            try
            {
                Rule.PuestoDeTrabajoProvisorioItineranteSave(model);

                ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Revisar.ToString();
                ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Revisar;

                return Json(new { status = true });
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

            return Json(new { status = false, details = errores.ToArray() });
        }

        [HttpPost]
        public ActionResult RegistrarOtroMinisterio(PuestoDeTrabajoProvisorioModel model)
        {
             try
            {

                Rule.PuestoDeTrabajoProvisorioOtroMinisterioSave(model);

                ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Revisar.ToString();
                ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Revisar;

                return Json(new { status = true });
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

            return Json(new { status = false, details = errores.ToArray() });
        }

        [HttpPost]
        public ActionResult RegistrarMaestraIntegradora(PuestoDeTrabajoProvisorioModel model)
        {
            try
            {
                Rule.PuestoDeTrabajoProvisorioMaestraIntSave(model);

                ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Revisar.ToString();
                ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Revisar;

                return Json(new { status = true });
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

            return Json(new { status = false, details = errores.ToArray() });
        }

        public override void EliminarPost(PuestoDeTrabajoProvisorioModel model)
        {
            Rule.PuestoDeTrabajoProvisorioDelete(model);
        }

        #endregion
        #region Soporte
        public JsonResult GetPTOrigenByAgente(int agenteId)
        {
            var registros = Rule.GetPTByAgente(agenteId);
            var jsonData = new
            {
                rows = from a in registros
                       select new
                       {
                           cell = new string[]
                            {
                                a.Id.ToString(), 
                                a.CodigoPuesto,
                                a.CodigoCargo.ToString(),
                                a.NombreCargo,
                                String.Empty ,
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPTVacantesByEmpresa(int? empresaId)
        {
            var registros = new List<PuestoDeTrabajoModel>();
            if (empresaId.HasValue)
            {
                registros = Rule.GetPTVacantesByEmpresa(empresaId.Value);
            }
            var jsonData = new
            {
                rows = from a in registros
                        select new
                        {
                            cell = new string[]
                            {
                            a.Id.ToString(), 
                            a.CodigoPosicionPn,
                            a.TipoCargo.CodigoPn.ToString(),
                            a.TipoCargo.Nombre,
                            a.TipoCargo.NivelCargoNombre,
                            }
                        }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetalleAgente(int id)
        {
            var regla = ServiceLocator.Current.GetInstance<IAgenteRules>();
            var agente = regla.GetAgenteById (id);

            return Json(new
            {
                Id = agente.Id,
                Legajo = agente.NumLegajoSiage,
                Nombre = agente.Persona.Nombre  ,
                Apellido = agente.Persona.Apellido ,
                Sexo = agente.Persona.SexoNombre.ToString() ,
                TipoAgente= agente.TipoAgente.Count >0 ?  agente.TipoAgente[0].Tipo.Nombre.ToString() : String.Empty ,
                TipoDocumento = agente.Persona.TipoDocumento ,
                NroDocumento = agente.Persona.NumeroDocumento 
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetDetalleTipoCargo(int id)
        {
            var regla = ServiceLocator.Current.GetInstance<ITipoCargoRules>();
            var tc = regla.GetTipoCargoById(id);

            return Json(new
            {
                Horas = tc.CantidadHoras.ToString() 
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Cancelar(int id)
        {
            //Registro general
            InicializadorConsultaAgente();
            return View("PuestoDeTrabajoProvisorioEditor");
 
        }
        #endregion
    }
}
