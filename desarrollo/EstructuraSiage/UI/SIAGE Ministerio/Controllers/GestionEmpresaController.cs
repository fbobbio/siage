using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using SIAGE.UI_Common.Controllers;
using SIAGE_Ministerio.Content;
using Siage.Base;
using System.Collections;
using SIAGE_Ministerio.Content.resources;
using SIAGE.UI_Common.Content;

namespace SIAGE_Ministerio.Controllers
{
    public class GestionEmpresaController : AjaxAbmcController<EmpresaRegistrarModel, IEmpresaRules>
    {
        private IUsuarioRules RuleUsuario;
        private IEntidadesGeneralesRules entidadesGenerales;
        private ITipoInstrumentoLegalRules tipoInstrumentoLegalRule;

        public override ActionResult Index()
        {
            // validar si estoy logueado como direccion de nivel o ministerio
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsDireccionDeNivel(GetIdEmpresaUsuarioLogueado()) || GetEmpresaUsuarioLogueado().TipoEmpresa == TipoEmpresaEnum.MINISTERIO)
            {
                //CargarViewData(EstadoABMC.Consultar);
                ViewData[Constantes.VistaEmpresa] = Request.QueryString["vista"];
                CargarViewData(EstadoABMC.Consultar);
                return View();
            }

            TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como dirección de nivel o ministerio.";
            
            return RedirectToAction("Error", "Home");
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Rule = ServiceLocator.Current.GetInstance<IEmpresaRules>();
            entidadesGenerales = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            tipoInstrumentoLegalRule = ServiceLocator.Current.GetInstance<ITipoInstrumentoLegalRules>();
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            //EMPRESA
            string idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            ViewData.Add(ViewDataKey.TIPO_ESCUELA.ToString(), entidadesGenerales.GetTipoEscuelaAll());
            ViewData.Add(ViewDataKey.PERIODO_LECTIVO.ToString(), entidadesGenerales.GetPeriodoLectivoAll());
            ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(), entidadesGenerales.GetDepartamentoProvincialByProvincia(idProvincia));

            ViewData.Add(ViewDataKey.INSPECCION_INTERMEDIA.ToString(), Rule.GetInspeccionIntermediaByDireccionDeNivelActual());
            //ViewData.Add(ViewDataKey.INSPECCION_INTERMEDIA.ToString(),ViewData[ViewDataKey.INSPECCION_INTERMEDIA.ToString()] ??
            //                                       new SelectList(
            //                                           Rule.GetInspeccionIntermediaByDireccionDeNivelActual(), "Id",
            //                                           "Descripcion"));
            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), new List<LocalidadModel>());
            ViewData.Add(ViewDataKey.ZONA_DESFAVORABLE.ToString(), entidadesGenerales.GetZonaDesfavorableAll());
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), entidadesGenerales.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.OBRA_SOCIAL.ToString(), entidadesGenerales.GetObraSocialAll());
            ViewData.Add(ViewDataKey.TURNO.ToString(), entidadesGenerales.GetTurnoAll());
            ViewData.Add(ViewDataKey.MODALIDAD_JORNADA.ToString(), entidadesGenerales.GetModalidadJornadaAll());
            ViewData.Add(ViewDataKey.PROGRAMA_PRESUPUESTARIO.ToString(), entidadesGenerales.GetProgramaPresupuestarioAll());

            //INSTRUMENTO LEGAL
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), entidadesGenerales.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.DIRECCION_NIVEL.ToString(), entidadesGenerales.GetDireccionDeNivelAll());
            ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), entidadesGenerales.GetTipoCargoAll());
            ViewData.Add(ViewDataKey.ASIGNATURA.ToString(), entidadesGenerales.GetAsignaturaAll());
            ViewData.Add(ViewDataKey.TITULO.ToString(), entidadesGenerales.GetTituloAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), entidadesGenerales.GetSituacionRevistaAll());
            ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(), entidadesGenerales.GetEstadoCivilAll());
            ViewData.Add(ViewDataKey.SEXO.ToString(), entidadesGenerales.GetSexoAll());
            ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(), entidadesGenerales.GetOrganismoEmisorDocumentoAll());
            ViewData.Add(ViewDataKey.PAIS.ToString(), entidadesGenerales.GetPaisAll());
            ViewData.Add(ViewDataKey.TIPO_INSTRUMENTO_LEGAL.ToString(), ServiceLocator.Current.GetInstance<ITipoInstrumentoLegalRules>().GetAll());
            ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(), entidadesGenerales.GetTipoCalleAll());

            ViewData.Add(ViewDataKey.TIPO_JORNADA.ToString(), entidadesGenerales.GetTipoJornadaAll());
            ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), entidadesGenerales.GetGradoAñoAll());
            ViewData.Add(ViewDataKey.CARRERA.ToString(), entidadesGenerales.GetCarreraAll());
            ViewData.Add(ViewDataKey.ORDEN_DE_PAGO.ToString(), entidadesGenerales.GetOrdenDePagoAll());
            ViewData.Add(ViewDataKey.SUCURSAL_BANCARIA.ToString(), entidadesGenerales.GetSucursalBancariaAll());
            ViewData.Add(ViewDataKey.FUNCION_EDIFICIO.ToString(), entidadesGenerales.GetFuncionEdificioAll());
            ViewData.Add(ViewDataKey.TIPO_ESCUELA_PERMITIDA_EMPRESA.ToString(), Rule.GetTiposEscuelasPermitidos());
            ViewData.Add(ViewDataKey.JERARQUIA_DE_INSPECCION_IGUAL_A_ORGANIGRAMA.ToString(), ParametroJerarquiaSigueOrganigrama());
            ViewData.Add(ViewDataKey.TIPO_EMPRESA_USUARIO_LOGUEADO.ToString(), GetEmpresaUsuarioLogueado().TipoEmpresa);
        }

        private EmpresaConsultarModel GetEmpresaUsuarioLogueado()
        {
            return Rule.GetEmpresaConsultaById(GetIdEmpresaUsuarioLogueado());
        }

        [HttpGet]
        public JsonResult AccionesVisadoAQuitar()
        {
            var acciones = new List<string>();

            var entidadesGeneralesRules = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            bool soloVisado = entidadesGeneralesRules.GetValorParametroBooleano(ParametroEnum.SOLO_VISADO_EN_CREACIÓN_O_REACTIVACIÓN_DE_EMPRESA);

            //Devuelvo el caso contrario para quitarlas del combo de la enumeración
            if (!soloVisado)
            {
                acciones.Add(AccionVisadoActivacionEmpresaEnum.VISADA.ToString());
            }
            else
            {
                acciones.Add(AccionVisadoActivacionEmpresaEnum.AUTORIZAR.ToString());
                acciones.Add(AccionVisadoActivacionEmpresaEnum.RECHAZAR.ToString());
            }
            return Json(acciones, JsonRequestBehavior.AllowGet);
        }

        #region Procesar Seleccion Vistas
        /// <summary>
        /// De acuerdo a la vista que se use, nos retorna esa vista.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vistaActiva"></param>
        /// <returns>Vista Parcial</returns>
        [HttpGet]
        public ActionResult ProcesarSeleccion(int? id, VistaEmpresa vistaActiva, string tipoEmpresa)
        {
            CargarViewDataConsultarEmpresa();
            switch (vistaActiva)
            {
                case VistaEmpresa.EmitirResolucionEmpresaEditor:
                    return PartialView(vistaActiva.ToString());

                //case VistaEmpresa.ModificarAsignacionEscuelaAInspeccionEditor:
                //    return PartialView(vistaActiva.ToString());

                case VistaEmpresa.ModificarTipoEmpresaEditor:
                    return PartialView(vistaActiva.ToString());

                case VistaEmpresa.ConsultarSolicitudesDesactivacionEmpresasEditor:
                    return PartialView(vistaActiva.ToString());

                case VistaEmpresa.ActivacionCodigoEmpresaEditor:
                    ActivacionCodigoEmpresaModel modelActivacion = new ActivacionCodigoEmpresaModel();
                    if (id.HasValue)
                    {
                        modelActivacion = Rule.GetEmpresaActivarCodigoById(id.Value);
                    }
                    return PartialView(vistaActiva.ToString(), modelActivacion);

                case VistaEmpresa.CerrarEmpresaEditor:
                    CargarViewDataCierreEmpresa();
                    return PartialView(vistaActiva.ToString());

                case VistaEmpresa.ReactivarEmpresaEditor:
                    EmpresaReactivacionModel modelReactivar = new EmpresaReactivacionModel();
                    if (id != null && id.HasValue)
                    {
                        modelReactivar = Rule.GetEmpresaReactivacionById(id.Value);
                    }
                    CargarViewDataReactivarEmpresa();
                    return PartialView(vistaActiva.ToString(), modelReactivar);

                case VistaEmpresa.VisarActivacionEmpresaEditor:
                    EmpresaVisarModel modelVisarActivacion = new EmpresaVisarModel();
                    if (id.HasValue)
                        modelVisarActivacion = Rule.GetEmpresaVisarById(id.Value);

                    return PartialView(vistaActiva.ToString(), modelVisarActivacion);

                case VistaEmpresa.VisarCierreEmpresaEditor:
                    EmpresaVisarModel modelVisarCierre = new EmpresaVisarModel();
                    if (id.HasValue)
                    {
                        //TODO Usar la regla para traer el modelo en base al ID
                        modelVisarCierre = null;
                    }
                    return PartialView(vistaActiva.ToString(), modelVisarCierre);

                case VistaEmpresa.SolicitudDePuestoDeTrabajo:
                case VistaEmpresa.SinVista:
                    return null;

                case VistaEmpresa.Seccion:
                    return View("Index");

                case VistaEmpresa.ConsultarEstructuraEditor:
                    return PartialView("EstructuraEscuelaConsultarEditor");

                case VistaEmpresa.AsignacionPlan:
                    return PartialView("AsignacionPlanEstudioEditor");

                default: //Nunca deberia llegar aca. Pero sino, no compila
                    return null;
                //TODO volver a return View("Index");
            }
        }

        private void CargarViewDataConsultarEmpresa()
        {
            string idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");
            ViewData.Add(ViewDataKey.TIPO_ESCUELA.ToString(), entidadesGenerales.GetTipoEscuelaAll());
            ViewData.Add(ViewDataKey.PERIODO_LECTIVO.ToString(), entidadesGenerales.GetPeriodoLectivoAll());
            ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(), entidadesGenerales.GetDepartamentoProvincialByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.INSPECCION_INTERMEDIA.ToString(), Rule.GetInspeccionIntermediaByDireccionDeNivelActual());
            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), new List<LocalidadModel>());
            ViewData.Add(ViewDataKey.ZONA_DESFAVORABLE.ToString(), entidadesGenerales.GetZonaDesfavorableAll());
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), entidadesGenerales.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.OBRA_SOCIAL.ToString(), entidadesGenerales.GetObraSocialAll());
            ViewData.Add(ViewDataKey.TURNO.ToString(), entidadesGenerales.GetTurnoAll());
            ViewData.Add(ViewDataKey.MODALIDAD_JORNADA.ToString(), entidadesGenerales.GetModalidadJornadaAll());
            ViewData.Add(ViewDataKey.PROGRAMA_PRESUPUESTARIO.ToString(),
                         entidadesGenerales.GetProgramaPresupuestarioAll());
        }

        public void CargarViewDataCierreEmpresa()
        {
            //CargarViewDataConsultarEmpresa();
            //INSTRUMENTO LEGAL
            ViewData.Add(ViewDataKey.TIPO_INSTRUMENTO_LEGAL.ToString(), ServiceLocator.Current.GetInstance<ITipoInstrumentoLegalRules>().GetAll());
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), entidadesGenerales.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.DIRECCION_NIVEL.ToString(), entidadesGenerales.GetDireccionDeNivelAll());
            ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), entidadesGenerales.GetTipoCargoAll());
            ViewData.Add(ViewDataKey.ASIGNATURA.ToString(), entidadesGenerales.GetAsignaturaAll());
            ViewData.Add(ViewDataKey.TITULO.ToString(), entidadesGenerales.GetTituloAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), entidadesGenerales.GetSituacionRevistaAll());
            ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(), entidadesGenerales.GetEstadoCivilAll());
            ViewData.Add(ViewDataKey.SEXO.ToString(), entidadesGenerales.GetSexoAll());
            ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(), entidadesGenerales.GetOrganismoEmisorDocumentoAll());
            ViewData.Add(ViewDataKey.PAIS.ToString(), entidadesGenerales.GetPaisAll());
            ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(), entidadesGenerales.GetTipoCalleAll());
        }

        public void CargarViewDataReactivarEmpresa()
        {
            ViewData.Add(ViewDataKey.ORDEN_DE_PAGO.ToString(), entidadesGenerales.GetOrdenDePagoAll());
            ViewData.Add(ViewDataKey.FUNCION_EDIFICIO.ToString(), entidadesGenerales.GetFuncionEdificioAll());
            ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), entidadesGenerales.GetGradoAñoAll());
            ViewData.Add(ViewDataKey.CARRERA.ToString(), entidadesGenerales.GetCarreraAll());
            //INSTRUMENTO LEGAL
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), entidadesGenerales.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.DIRECCION_NIVEL.ToString(), entidadesGenerales.GetDireccionDeNivelAll());
            ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), entidadesGenerales.GetTipoCargoAll());
            ViewData.Add(ViewDataKey.ASIGNATURA.ToString(), entidadesGenerales.GetAsignaturaAll());
            ViewData.Add(ViewDataKey.TITULO.ToString(), entidadesGenerales.GetTituloAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), entidadesGenerales.GetSituacionRevistaAll());
            ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(), entidadesGenerales.GetEstadoCivilAll());
            ViewData.Add(ViewDataKey.SEXO.ToString(), entidadesGenerales.GetSexoAll());
            ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(), entidadesGenerales.GetOrganismoEmisorDocumentoAll());
            ViewData.Add(ViewDataKey.PAIS.ToString(), entidadesGenerales.GetPaisAll());
            ViewData.Add(ViewDataKey.TIPO_INSTRUMENTO_LEGAL.ToString(), ServiceLocator.Current.GetInstance<ITipoInstrumentoLegalRules>().GetAll());
            ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(), entidadesGenerales.GetTipoCalleAll());
        }

        #endregion

        #region Registrar empresa 2

        public List<TipoEmpresaEnum> TipoEmpresasQuePuedeCrearDeACuerdoAlTipoDeEmpresaDelUsuarioLogeado(TipoEmpresaEnum tipoEmpresa)
        {
            var empresasPermitidas = new List<TipoEmpresaEnum>();
            if (tipoEmpresa == TipoEmpresaEnum.MINISTERIO)
            {
                empresasPermitidas.Add(TipoEmpresaEnum.DIRECCION_DE_NIVEL);
                empresasPermitidas.Add(TipoEmpresaEnum.SUBSECRETARIA);
                empresasPermitidas.Add(TipoEmpresaEnum.SECRETARIA);
                empresasPermitidas.Add(TipoEmpresaEnum.MINISTERIO);
                empresasPermitidas.Add(TipoEmpresaEnum.APOYO_ADMINISTRATIVO);
                empresasPermitidas.Add(TipoEmpresaEnum.DIRECCION_DE_INFRAESTRUCTURA);
                empresasPermitidas.Add(TipoEmpresaEnum.DIRECCION_DE_RECURSOS_HUMANOS);
                empresasPermitidas.Add(TipoEmpresaEnum.DIRECCION_DE_SISTEMAS);
                empresasPermitidas.Add(TipoEmpresaEnum.DIRECCION_DE_TESORERIA);
                empresasPermitidas.Add(TipoEmpresaEnum.APOYO_ADMINISTRATIVO);

            }
            else
            {
                //si soy una direccion de nivel
                empresasPermitidas.Add(TipoEmpresaEnum.INSPECCION);
                empresasPermitidas.Add(TipoEmpresaEnum.ESCUELA_MADRE);
                empresasPermitidas.Add(TipoEmpresaEnum.ESCUELA_ANEXO);
            }
            return empresasPermitidas;
        }

        [HttpGet]
        public PartialViewResult SeleccionTipoEmpresaAbmc(TipoEmpresaEnum tipo, int? id, EstadoABMC estado)
        {
            ViewData.Add(ViewDataKey.TIPO_EMPRESA.ToString(), tipo);
            //CargarViewDataRegistrarEmpresa(tipo);
            CargarViewData(estado);
            var model = new EmpresaRegistrarModel();
            if (!id.HasValue || id.Value <= 0) // Caso en el que es un registro
            {
                model.TipoEmpresa = tipo;
                ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Registrar.ToString();
                ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Registrar;
            }
            else // Caso en el que sea un editar o ver
            {
                model = Rule.GetEmpresaById(id.Value);
                //CargarViewDataRegistrarEmpresa(tipo);
                ViewData[AjaxAbmc.EstadoText] = estado.ToString();
                ViewData[AjaxAbmc.EstadoId] = (int)estado;
            }

            return PartialView("RegistrarEmpresaEditor", model);
        }

        [HttpGet]
        public JsonResult GetVinculosEmpresaEdificio(int empresaId)
        {
            var model = Rule.GetEmpresaById(empresaId);
            var vinculosYdomicilios = new List<object>();
            var vin = new List<object>();
            //tengo q mostrar solo los vinculos q sean != INACTIVOS
            foreach (var vinculo in model.VinculoEmpresaEdificio.Where(x => x.Estado != EstadoVinculoEmpresaEdificioEnum.INACTIVO))
            {
                vin.Add(
                    new
                        {
                            vinculo.Id,
                            IdEdificio = vinculo.Edificio.Id,
                            vinculo.Edificio.IdentificadorEdificio,
                            IdTipoEstructuraEdilicia =
                                vinculo.Edificio.IdTipoEstructuraEdilicia == 1 ? "ESCOLAR" : "NO_ESCOLAR",
                            FechaDesde = vinculo.FechaDesde.ToShortDateString(),
                            vinculo.Observacion,
                            vinculo.DeterminaDomicilio,
                            EstadoVinculo = vinculo.Estado.ToString()

                        });
            }
            var domicilio =
                ServiceLocator.Current.GetInstance<IDomicilioRules>().GetDomicilioById(model.DomicilioId.Value);
            var localidad = Rule.GetLocalidadToStringById(domicilio.IdLocalidad);
            var dpto = string.Empty;
            if (domicilio != null && domicilio.IdDepartamentoProvincial.HasValue)
                dpto = Rule.GetDepartamentoProvincialById(domicilio.IdDepartamentoProvincial.Value);
            var domJson = new { Id = domicilio.Id, Calle = domicilio.CalleNueva, Altura = domicilio.Altura, Barrio = domicilio.BarrioNuevo, Localidad = localidad, DepartamentoProvincial = dpto };
            vinculosYdomicilios.Add(new { vin, domJson });

            return Json(vinculosYdomicilios, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpresasInspeccionadasPorInspeccionId(int idInspeccion)
        {
            return Json(Rule.GetEmpresasInspeccionadasPorInspeccionId(idInspeccion), JsonRequestBehavior.AllowGet);
        }


        #region Vinculo empresa-edificio
        [HttpGet]
        public JsonResult GetCallesPredioByIdEdificio(int idEdificio)
        {
            var callesPredioModel = Rule.GetCallesPredioByEdificioId(idEdificio);
            return Json(callesPredioModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Registrar Empresa

        [HttpGet]
        public override ActionResult Registrar()
        {

            ViewData.Add(ViewDataKey.NOMBRE_EMPRESA_HABILITADO.ToString(),
                         entidadesGenerales.GetValorParametroBooleano(ParametroEnum.MODIFICAR_NOMBRE_SUGERIDO_DE_EMPRESA));
            AbmcView = "RegistroEmpresa/SeleccionTipoEmpresaEditor";
            return base.ProcesarAbmGet(null, EstadoABMC.Registrar);
        }

        [HttpGet]
        public override ActionResult Ver(int id)
        {
            //ViewData.Add(ViewDataKey.TIPO_ESCUELA_PERMITIDA_EMPRESA.ToString(), Rule.GetTiposEscuelasPermitidos());
            AbmcView = "RegistroEmpresa/SeleccionTipoEmpresaEditor";
            return ProcesarAbmGet(id, EstadoABMC.Ver);
        }

        [HttpGet]
        public ActionResult VerDatosGenerales(int id)
        {
            var model = Rule.GetEmpresaById(id);
            return PartialView("DatosGeneralesEmpresa", model);
        }


        [HttpGet]
        public override ActionResult Editar(int id)
        {
            AbmcView = "RegistroEmpresa/SeleccionTipoEmpresaEditor";
            return ProcesarAbmGet(id, EstadoABMC.Editar);
        }

        [HttpPost]
        public override void EditarPost(EmpresaRegistrarModel model)
        {
            Rule.EmpresaSave(model);
            if (model.DomicilioId.HasValue)
                Rule.DomicilioSave(model.DomicilioId.Value, model);
        }

        [HttpGet]
        public bool ParametroEstructura()
        {
            return entidadesGenerales.GetValorParametroBooleano(ParametroEnum.ESTRUCTURA_ESCOLAR_EN_CREACIÓN_EMPRESA);
            //return true;
        }

        [HttpGet]
        public bool ParametroJerarquiaSigueOrganigrama()
        {
            return entidadesGenerales.GetValorParametroBooleano(ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA);
            //return true;
        }

        [HttpGet]
        public bool CargarComboTipoInspeccion()
        {
            return entidadesGenerales.GetValorParametroBooleano(ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA);
        }

        [HttpGet]
        public bool CargarComboAccionVisado()
        {
            return entidadesGenerales.GetValorParametroBooleano(ParametroEnum.SOLO_VISADO_EN_CREACIÓN_O_REACTIVACIÓN_DE_EMPRESA);
        }

        [HttpGet]
        public JsonResult SePuedeEditarEmpresa(int id)
        {
            var entidad = Rule.GetEmpresaById(id);
            return entidad.EstadoEmpresa == EstadoEmpresaEnum.AUTORIZADA ? Json(new { status = true, JsonRequestBehavior.AllowGet }) : Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTiposEducacionByDireccionDeNivelUsuario()
        {
            var empresa = Rule.GetCurrentEmpresa();
            var dir = Rule.GetEmpresaById(empresa.Id);
            var json = new List<object>();
            //si es una DN devolver solo el valor del tipo educacion q tiene asignado
            if (dir.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL)
            {
                json.Add(new { Id = dir.TipoEducacion, TipoEducacion = dir.TipoEducacion.Value.ToString() });
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            //si es otro tipo de empresa (MINISTERIO) devolver toda la enumeracion
            var valores = typeof(TipoEducacionEnum).GetEnumNames();
            for (int i = 0; i < valores.Length; i++)
            {
                json.Add(new { Id = i + 1, TipoEducacion = valores[i] });
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarTipoEmpresa(string tipoGestion)
        {
            switch (tipoGestion)
            {
                case ("ESCUELA"):
                    var values = from TipoEmpresaEnum e in Enum.GetValues(typeof(TipoEmpresaEnum))
                                 where e.ToString() == TipoEmpresaEnum.ESCUELA_ANEXO.ToString()
                                 || e.ToString() == TipoEmpresaEnum.ESCUELA_MADRE.ToString()
                                 orderby e.ToString()
                                 select new { Id = e.ToString(), Nombre = e.ToString() };
                    return Json(new SelectList(values, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
                case ("GESTION_EDUCATIVA"):
                    var values1 = from TipoEmpresaEnum e in Enum.GetValues(typeof(TipoEmpresaEnum))
                                  where e.ToString() == TipoEmpresaEnum.DIRECCION_DE_NIVEL.ToString()
                                  || e.ToString() == TipoEmpresaEnum.INSPECCION.ToString()
                                  orderby e.ToString()
                                  select new { Id = e.ToString(), Nombre = e.ToString() };
                    return Json(new SelectList(values1, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
                case ("GESTION_ADMINISTRATIVA"):
                    var values2 = from TipoEmpresaEnum e in Enum.GetValues(typeof(TipoEmpresaEnum))
                                  where e.ToString() == TipoEmpresaEnum.APOYO_ADMINISTRATIVO.ToString()
                                  || e.ToString() == TipoEmpresaEnum.DIRECCION_DE_INFRAESTRUCTURA.ToString()
                                  || e.ToString() == TipoEmpresaEnum.DIRECCION_DE_RECURSOS_HUMANOS.ToString()
                                  || e.ToString() == TipoEmpresaEnum.DIRECCION_DE_SISTEMAS.ToString()
                                  || e.ToString() == TipoEmpresaEnum.DIRECCION_DE_TESORERIA.ToString()
                                  || e.ToString() == TipoEmpresaEnum.MINISTERIO.ToString()
                                  || e.ToString() == TipoEmpresaEnum.SECRETARIA.ToString()
                                  || e.ToString() == TipoEmpresaEnum.SUBSECRETARIA.ToString()
                                  orderby e.ToString()
                                  select new { Id = e.ToString(), Nombre = e.ToString() };
                    return Json(new SelectList(values2, "Id", "Nombre"), JsonRequestBehavior.AllowGet);

            }
            var rowsVacio = new
            {
                Id = 0,
                Nombre = "SELECCIONE"
            };
            return Json(rowsVacio);

        }

        private TipoGestionEnum CargarTipoGestion(string tipoEmpresa)
        {
            switch (tipoEmpresa)
            {
                case "APOYO_ADMINISTRATIVO":
                case "DIRECCION_DE_INFRAESTRUCTURA":
                case "DIRECCION_DE_RECURSOS_HUMANOS":
                case "DIRECCION_DE_SISTEMAS":
                case "DIRECCION_DE_TESORERIA":
                case "MINISTERIO":
                case "SECRETARIA":
                case "SUBSECRETARIA":
                    return TipoGestionEnum.GESTION_ADMINISTRATIVA;
                case "DIRECCION_DE_NIVEL":
                case "INSPECCION":
                    return TipoGestionEnum.GESTION_EDUCATIVA;
                case "ESCUELA_MADRE":
                case "ESCUELA_ANEXO":
                    return TipoGestionEnum.ESCUELA;
            }
            return TipoGestionEnum.GESTION_EDUCATIVA;
        }

        [HttpGet]
        public JsonResult GetPeriodosLectivosPorEscuela(int escuelaId)
        {

            var json = new ArrayList();
            var i = 0;
            foreach (var periodosLectivo in Rule.GetEmpresaById(escuelaId).PeriodosLectivos)
            {
                json.Add(new
                             {
                                 id = i,
                                 PeriodoLectivoId = periodosLectivo.Id,
                                 PeriodoLectivoText = periodosLectivo.Nombre

                             });
                i++;
            }
            return Json(json, JsonRequestBehavior.AllowGet);

            //var empresaEscuela = Rule.GetEmpresaById(escuelaId);
            //var listadoPeriodos = new List<PeriodoLectivoModel>();
            //if (empresaEscuela != null)
            //    listadoPeriodos = empresaEscuela.PeriodosLectivos.OrderBy(x => x.Id).ToList();
            //return Json((listadoPeriodos.Select(p => new { Id = p.Id, PeriodoLectivoText = p.Nombre,PeriodoLectivoId=p.Id })).ToList(), JsonRequestBehavior.AllowGet); 

        }

        [HttpGet]
        public JsonResult GetUsuarioDeCierreEmpresa(int empresaId)
        {
            var usuarioYFecha = Rule.GetUsuarioDeCierre(empresaId);
            if (usuarioYFecha != null)
            {
                var json = new
                               {
                                   UsuarioCierre = usuarioYFecha.UsuarioCierre,
                                   FechaCierre = usuarioYFecha.FechaCierre.ToShortDateString()
                               };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public override ActionResult Registrar(EmpresaRegistrarModel model)
        {
            if (model.ZonaDesfavorableId.HasValue && model.ZonaDesfavorableId.Value == (int)ZonaDesfavorableEnum.A)
            {
                model.AsignacionInstrumentoLegalZonaDesfavorable = null;
            }
            try
            {

                if (ModelState.IsValid)
                {
                    string msj = string.Empty;
                    model = Rule.EmpresaSave(model);
                    if (model.DomicilioId.HasValue)
                        Rule.DomicilioSave(model.DomicilioId.Value, model);
                    bool resultado = true;// Rule.EnviarMail(OpcionEnvioMailEnum.ACTIVACION, model.Id, out msj);
                    return Json(new { status = true, mail = resultado, mensaje = msj });
                }
            }
            catch (Exception e)
            {
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

            return Json(new { status = (ModelState.IsValid), details = errores.ToArray() });
        }

        [HttpPost]
        public ActionResult GetCampos(EmpresaRegistrarModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Session["EmpresaModificada"] = model;
                    List<string> campos = Rule.VerificarCampos(model);
                    //si no hay campos que validar
                    if (campos.Count == 0)
                    {
                        Rule.EmpresaSave(model);
                        return Ver(model.Id);
                        //return PartialView(VistaEmpresa.RegistrarEmpresaEditor.ToString(), model);
                    }

                    var instrumentosLegales = new Dictionary<string, AsignacionInstrumentoLegalModel>();

                    foreach (var campo in campos)
                        instrumentosLegales[campo] = new AsignacionInstrumentoLegalModel();
                    ViewData[AjaxAbmc.EstadoText] = "Registrar";
                    ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Registrar;
                    return View("ModificarEmpresaInstrumentoLegal", instrumentosLegales);
                }
            }
            catch (Exception e)
            {
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

            return Json(new { status = (ModelState.IsValid), details = errores.ToArray() });

        }

        //[HttpPost]
        //public ActionResult Editar(Dictionary<string, AsignacionInstrumentoLegalModel> InstrumentosLegalesEnEdicion)
        //{
        //    var idModel=0;
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            EmpresaRegistrarModel model = (EmpresaRegistrarModel)Session["EmpresaModificada"];
        //            model.AsignacionesIntrumentosLegales = new List<AsignacionInstrumentoLegalModel>();
        //            foreach (var asignacion in InstrumentosLegalesEnEdicion)
        //            {
        //                model.AsignacionesIntrumentosLegales.Add(asignacion.Value);
        //            }

        //            model = Rule.EmpresaSave(model);
        //           idModel = model.Id;
        //            return ProcesarGet(model.Id, EstadoABMC.Ver);
        //            // return View("RegistrarEmpresaEditor", model);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        while (e.InnerException != null)
        //            e = e.InnerException;

        //        ModelState.AddModelError(string.Empty, e.Message);
        //    }
        //    var errores = new List<string>();
        //    for (int i = 0; i < ModelState.Values.Count; i++)
        //    {
        //        var propiedad = ModelState.Values.ElementAt(i);
        //        if (propiedad.Errors.Count != 0)
        //        {
        //            errores.AddRange(propiedad.Errors.Select(item => string.IsNullOrEmpty(item.ErrorMessage) ? item.Exception.Message : item.ErrorMessage));
        //        }
        //    }

        //    return Json(new { status = (ModelState.IsValid),id=idModel,details = errores.Count>0 ?errores.ToArray(): new string[1] });
        //    //return Json(new { status = true });
        //}

        public JsonResult CargarTipoEducacion(string nivelEducativoId)
        {
            var resultado = Rule.GetTiposEducacionByNivelEducativoId(int.Parse(nivelEducativoId));
            return Json(new SelectList(resultado), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarNivelEducativo(TipoEducacionEnum? tipoEducacion)
        {
            var empresaActual = Rule.GetCurrentEmpresa();
            var direccionActual = Rule.GetEmpresaById(empresaActual.Id);
            var resultado = new List<NivelEducativoModel>();
            //tiene q mostrar los niveles educativos q tenga la DN del usuario logueado
            if (direccionActual.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL)
            {
                foreach (var nete in direccionActual.NivelEducativoPorTipoEducacion)
                {
                    resultado.Add(nete.NivelEducativo);
                }
                return Json(new SelectList(resultado, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
            }
            //si es otro tipo de empresa, devolver todos los niveles educativos
            //var listado2 = ServiceLocator.Current.GetInstance<INivelEducativoRules>().GetAllNivelEducativo();
            var listado = Rule.GetNivelesEducativosPorTipoEducacion((TipoEducacionEnum)tipoEducacion);
            return Json(new SelectList(listado, "IdNivelEducativo", "Nombre"), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllGradoAñoByNivelEducativo(int idNivel, TipoEducacionEnum idTipoEd)
        {
            var values = ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoByNivelEducativoTipoEducacion(idNivel, idTipoEd);
            return Json(new SelectList(values, "Id", "Nombre"), JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllGradoAñoByNivelEducativoByEscuelaId(int escuelaId)
        {
            var escuela = Rule.GetEmpresaById(escuelaId);
            if (escuela.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO || escuela.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
            {
                var listadoGradoAnios =
                    ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoByNivelEducativoTipoEducacion(
                        escuela.NivelEducativoId.Value, (escuela.TipoEducacion.Value));
                return Json(new SelectList(listadoGradoAnios, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
            }
            return Json(new SelectList("Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTurnosByEscuelaId(int escuelaId)
        {
            var empresaEscuela = Rule.GetEmpresaById(escuelaId);
            var listadoTurnos = new List<TurnoModel>();
            if (empresaEscuela != null)
                listadoTurnos = empresaEscuela.Turnos.OrderBy(x => x.Id).ToList();
            return Json((listadoTurnos.Select(p => new { id = p.Id, turnos = p.Nombre })).ToList(), JsonRequestBehavior.AllowGet);
            //return Json(new SelectList(listadoTurnos, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNETEByEscuelaId(int escuelaId)
        {
            var empresaEscuela = Rule.GetEmpresaById(escuelaId);
            var listadoNETE = new List<NivelEducativoPorTipoEducacionModel>();
            if (empresaEscuela != null)
                listadoNETE = empresaEscuela.NivelEducativoPorTipoEducacion;
            return Json((listadoNETE.Select(p => new { id = p.Id, nivelEducativo = p.NivelEducativo.Nombre, tipoEducacion = p.TipoEducacion })).ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca el parametro booleano de Jerarquia Inspeccion Organigrama
        /// </summary>
        /// <returns>json con el valor del parametro</returns>
        public JsonResult GetParametroJerarquiaInspeccionOrganigrama()
        {
            var valorParametro = Rule.GetParametroBooleanoJerarquiaInspeccionOrganigrama();
            if (valorParametro != null)
                return Json(valorParametro.Valor, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiposEscuelaByEmpresaId(int empresaId)
        {
            var empresaEscuela = Rule.GetEmpresaById(empresaId);
            var listadoTE = new List<TipoEscuelaModel>();
            if (empresaEscuela != null)
                listadoTE = empresaEscuela.TiposEscuelas;
            return Json((listadoTE.Select(p => new { id = p.Id, tipoEscuela = p.Nombre })).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVinculoEdificio(int empresaId)
        {
            var empresaEscuela = Rule.GetEmpresaById(empresaId);
            var listadoVinculo = new List<VinculoEmpresaEdificioModel>();
            if (empresaEscuela != null)
                listadoVinculo = empresaEscuela.VinculoEmpresaEdificio;
            if (listadoVinculo != null)
                return
                    Json(
                        (listadoVinculo.Select(
                            p =>
                            new
                                {
                                    Id = p.Id,
                                    IdEdificio = p.Edificio.Id,
                                    IdentificadorEdificio = p.Edificio.IdentificadorEdificio,
                                    IdTipoEstructuraEdilicia = p.Edificio.TipoEstructuraEdiliciaNombre,
                                    FechaDesde = p.FechaDesde.ToShortDateString(),
                                    Observacion = p.Observacion,
                                    DeterminaDomicilio = p.DeterminaDomicilio
                                })).ToList(), JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInstrumentosLegalesByEmpresaId(int empresaId)
        {
            var empresa = Rule.GetEmpresaById(empresaId);
            var listadoAsignacionesInstrumentosLegales = new List<AsignacionInstrumentoLegalModel>();
            if (empresa != null)
            {
                listadoAsignacionesInstrumentosLegales = empresa.AsignacionesIntrumentosLegales;
                listadoAsignacionesInstrumentosLegales = listadoAsignacionesInstrumentosLegales.OrderBy(x => x.FecNotificacion).ToList();
                if (empresa.AsignacionInstrumentoLegalZonaDesfavorable != null)
                    listadoAsignacionesInstrumentosLegales.AddRange(empresa.AsignacionesIntrumentosLegales);
            }


            var jsonData = new
                               {
                                   rows = from ail in listadoAsignacionesInstrumentosLegales
                                          select new
                                                     {
                                                         cell = new string[]
                                                                    {
                                                                        ail.Id.ToString(),
                                                                        ail.InstrumentoLegal.NroInstrumentoLegal,
                                                                        ail.InstrumentoLegal.EmisorInstrumentoLegal.
                                                                            ToString(),
                                                                        ail.InstrumentoLegal.FechaEmision.HasValue
                                                                            ? ail.InstrumentoLegal.FechaEmision.ToString
                                                                                  ()
                                                                            : string.Empty,
                                                                        ail.Observaciones,
                                                                        ail.InstrumentoLegal.IdTipoInstrumentoLegal.
                                                                            HasValue
                                                                            ? tipoInstrumentoLegalRule.
                                                                                  GetTipoInstrumentoLegalById(
                                                                                      ail.InstrumentoLegal.
                                                                                          IdTipoInstrumentoLegal.Value).
                                                                                  Nombre
                                                                            : string.Empty,
                                                                        Rule.GetTipoMovimientoAsginacionInstrumentoLegal
                                                                            (ail.IdTipoMovimientoInstrumentoLegal),
                                                                        ail.FecNotificacion.HasValue
                                                                            ? ail.FecNotificacion.Value.
                                                                                  ToShortDateString()
                                                                            : string.Empty
                                                                    }
                                                     }
                               };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //if (listadoAsignacionesInstrumentosLegales != null)
            //    return
            //        Json(
            //            (listadoAsignacionesInstrumentosLegales.Select(
            //                ail =>
            //                new
            //                    {
            //                        Id = ail.Id,
            //                        Numero = ail.InstrumentoLegal.NroInstrumentoLegal,
            //                        Emisor = ail.InstrumentoLegal.EmisorInstrumentoLegal.ToString(),
            //                        FechaNotificacion = ail.FecNotificacion.HasValue?ail.FecNotificacion.Value.ToShortDateString():string.Empty,
            //                        Expediente = ail.InstrumentoLegal.Expediente!=null?ail.InstrumentoLegal.Expediente.Numero:string.Empty
            //                    })).ToList(), JsonRequestBehavior.AllowGet);
            //else
            //    return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpresaById(int idEmpresa)
        {
            var empresa = Rule.GetEmpresaById(idEmpresa);
            return Json(empresa, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmpresaConsultaById(int idEmpresa)
        {
            var empresa = Rule.GetEmpresaConsultaById(idEmpresa);
            var ret = new
                          {
                              Id = empresa.Id,
                              CodigoEmpresa = empresa.CodigoEmpresa,
                              NombreEmpresa = empresa.Nombre,
                              TipoEducacion = empresa.TipoEducacion.ToString(),
                              NivelEducativo = empresa.NivelEducativo,
                              TipoEmpresa = empresa.TipoEmpresa.ToString(),
                              EstadoEmpresa = empresa.EstadoEmpresa.ToString(),
                              //armo el cue como un string, luego desde js lo divido y muestro ambas partes
                              CUE = empresa.CUE + "-" + empresa.CUEAnexo
                          };
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCodigoEmpresaById(int idEmpresa)
        {
            var codigoEmpresa = Rule.GetCodigoEmpresaById(idEmpresa);
            return Json(codigoEmpresa, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Devuelve la estructura escolar asociada a la empresa
        /// </summary>
        /// <param name="escuelaId"></param>
        /// <returns></returns>
        public JsonResult GetEstructuraEscolar(int escuelaId)
        {
            var listadoEstructuraEscolar = Rule.GetDiagramacionCursoByEscuelaId(escuelaId);
            if (listadoEstructuraEscolar.Count > 0)
            {
                //foreach (var diagramacionCursoModel in listadoEstructuraEscolar)
                //{
                //    diagramacionCursoModel.FechaApertura = diagramacionCursoModel.FechaApertura.ToShortDateString();
                //}
                //return Json(listadoEstructuraEscolar, JsonRequestBehavior.AllowGet);
                foreach (var diagramacionCursoModel in listadoEstructuraEscolar)
                    diagramacionCursoModel.Inscripciones = null;

                return CustomJson<List<DiagramacionCursoModel>>(listadoEstructuraEscolar);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ProcesarHistorial

        public void CargarTurnosModificados(int id)
        {
            //TODO: falta implementacion
        }

        public void CargarNivelesEducativoModificados(int id)
        {
            //TODO: falta implementacion
        }

        public void CargarTipoEscuelasModificadas(int id)
        {
            //TODO: falta implementacion
        }

        public ActionResult MostrarVistaHistorial()
        {
            return PartialView("ContenedorHistorialesEditor", null);
        }

        public ActionResult VerDetalleHistorial(int id)
        {
            ViewData[AjaxAbmc.EstadoText] = EstadoABMC.Historial.ToString();
            ViewData[AjaxAbmc.EstadoId] = (int)EstadoABMC.Historial;
            HistorialesEmpresaModel model = new HistorialesEmpresaModel();
            model = Rule.GetHistorialById(id);
            return PartialView("HistorialesEmpresaEditor", model);
        }

        public JsonResult ProcesarHistorial(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString, int id)
        {
            // Construyo la funcion de ordenamiento
            Func<HistorialesEmpresaModel, IComparable> funcOrden =
                /****************************** INICIO AREA EDITABLE ******************************/
                sidx == "Nombre" ? x => x.NombreEmpresa :
                /******************************** FIN AREA EDITABLE *******************************/
                (Func<HistorialesEmpresaModel, IComparable>)(x => x.Id);


            // Obtengo los registros filtrados segun los criterios ingresados
            /****************************** INICIO AREA EDITABLE ******************************/
            List<HistorialesEmpresaModel> registros = new List<HistorialesEmpresaModel>();

            registros = Rule.ProcesarHistorial(id);

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
                           cell = new string[] {
                            a.Id.ToString(), 
                            // Respetar el orden en que se mostrarán las columnas
                            /****************************** INICIO AREA EDITABLE ******************************/
                            a.NombreEmpresa,
                            a.NombreAgenteModificacion,
                            a.FechaDesde.ToString()
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Sugerir Nombre

        /// <summary>
        /// Sugiere un nombre para las escuelas
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        public ActionResult SugerirNombre(long? domicilioId, int? tipoEscuelaId, int? escuelaRaizId, int? escuelaMadreId, TipoEmpresaEnum? tipoEmpresa, int? numeroEscuelaAnexo)
        {
            try
            {
                return Json(new { status = true, NombreEscuela = Rule.SugerirNombreEscuelas(domicilioId, tipoEscuelaId, escuelaRaizId, escuelaMadreId, tipoEmpresa, numeroEscuelaAnexo) });
            }
            catch (Exception e)
            {
                return
                    Json(
                        new { status = false, details = new string[] { e.Message } });
            }

            //var errores = new List<string>();
            //for (int i = 0; i < ModelState.Values.Count; i++)
            //{
            //    var propiedad = ModelState.Values.ElementAt(i);
            //    if (propiedad.Errors.Count != 0)
            //    {
            //        errores.AddRange(propiedad.Errors.Select(item => string.IsNullOrEmpty(item.ErrorMessage) ? item.Exception.Message : item.ErrorMessage));
            //    }
            //}

            //return Json(new { status = (ModelState.IsValid), details = errores.ToArray() });

        }
        /// <summary>
        /// Sugiere un nombre para las escuelas
        /// </summary>
        /// <param name="idEmpresa"></param>
        public JsonResult SugerirNombre(int idEmpresa)
        {
            return Json(new { status = true, nombre = Rule.NombreSugeridoParaEscuelas(idEmpresa) },
                        JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CerrarEmpresa

        [HttpPost]
        public ActionResult CerrarEmpresa(EmpresaCierreModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Rule.EmpresaCerrar(model);
                    return Json(new { status = true });
                }
            }
            catch (Exception e)
            {
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

            return Json(new { status = (ModelState.IsValid), details = errores.ToArray() });
        }

        #endregion

        #region Activar Codigo Empresa
        /// <summary>
        /// Activar el codigo de la empresa
        /// </summary>
        /// <param name="id">Id de la empresa</param>
        [HttpPost]
        public ActionResult ActivarCodigoEmpresa(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Rule.ActivarCodigoEmpresa(id);
                    return Json(new { status = true });
                }
            }
            catch (Exception e)
            {
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

            return Json(new { status = (ModelState.IsValid), details = errores.ToArray() });
        }
        #endregion

        #region ModificarTipoEmpresaEditor

        [HttpPost]
        public ActionResult ModificarTipoEmpresa(EscuelaModificarTipoEmpresaModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Rule.ModificarTipoEmpresa(model);
                    return Json(new { status = true });
                }
            }
            catch (Exception e)
            {
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

            return Json(new { status = (ModelState.IsValid), details = errores.ToArray() });
        }

        #endregion

        #region Reactivacion Empresa

        /** Método que devuelve el ID de la empresa a la que está asociado el usuario logueado */
        private int GetIdEmpresaUsuarioLogueado()
        {
            //var empresaUsuarioLogueado = ServiceLocator.Current.GetInstance<IUsuarioRules>().GetCurrentUser().RolActual.EmpresaId;
            return (int)Session[ConstantesSession.EMPRESA.ToString()];
        }

        /*public JsonResult GetGradoAnioByEscuela(int idEscuela)
        {
            
            return Json(new SelectList(values, "Id", "Nombre"),JsonRequestBehavior.AllowGet);
        }*/

        /* Método que checkea si la empresa es una escuela, si es madre, o si su madre está cerrada*/
        public JsonResult CheckEscuela(int idEmpresa)
        {
            bool escuela = Rule.EmpresaEsEscuela(idEmpresa), madre = Rule.EmpresaEsMadre(idEmpresa);
            bool nivelSuperior = false;
            bool escuelaMadre = false;
            if (escuela) // Si es escuela
            {
                var escuelaEmpr = Rule.GetEscuelaById(idEmpresa);
                var values = ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoByNivelEducativoTipoEducacion(escuelaEmpr.NivelEducativo, escuelaEmpr.TipoEducacion);
                ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), new SelectList(values, "Id", "Nombre"));

                //Checkeo si la empresa es de nivel superior
                nivelSuperior = Rule.IsEscuelaNivelSuperior(idEmpresa);
                if (!madre) // Si no es madre, traigo la madre y guardo true si está cerrada, o false si no está cerrada
                {
                    escuelaMadre = Rule.GetEscuelaMadreCerrada(idEmpresa);
                }
            }
            var resultado = new { EsEscuela = escuela, EsMadre = madre, MadreCerrada = escuelaMadre, Superior = nivelSuperior };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmpresaHasVinculosActivos(int idEmpresa)
        {
            var ret = Rule.EmpresaHasVinculosActivos(idEmpresa);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindDomicilioDeEdificio(int idEdificio)
        {
            var ret = Rule.FindDomicilioDeEdificio(idEdificio);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReactivarEmpresa(EmpresaReactivacionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Rule.EmpresaReactivar(model, GetIdEmpresaUsuarioLogueado());
                    return Json(new { status = true });
                }
            }
            catch (Exception e)
            {
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

            return Json(new { status = (ModelState.IsValid), details = errores.ToArray() });
        }

        /// <summary>
        /// Metodo que recibe desde la vista todos los parametros necesarios para la obtención de los registros a mostrar, filtrarlos y paginados.
        /// A partir del parámetro id (sin incluirlo), los parámetros siguientes son opcionales y dependientes del caso de uso.
        /// </summary>
        /// <param name="sidx">Campo por el cual se ordenan los registros</param>
        /// <param name="sord">Dirección de ordenamiento (Ascendente/Descendente)</param>
        /// <param name="page">Número de página a mostrar</param>
        /// <param name="rows">Cantidad de registros por página</param>
        /// <param name="id">Valor de filtrado por ID</param>
        /// <returns>Objeto JSON que representa la matriz de datos a ser mostrados en la grilla</returns>
        public JsonResult ProcesarBusquedaEstructuraEscolar(string sidx, string sord, int page, int rows, int id, int? filtroTurno, int? filtroGrado, int? filtroCarrera, int? filtroCicloEducativo)
        {
            Func<DiagramacionCursoModel, IComparable> funcOrden = null;
            var diagramacionCursoRules = ServiceLocator.Current.GetInstance<IDiagramacionCursoRules>();
            var nivel = diagramacionCursoRules.GetNivelByEscuela(id);

            if (nivel.Id == (int)NivelEducativoNombreEnum.SUPERIOR)
            {
                funcOrden =
                    sidx == "Carrera" ? x => x.CarreraNombre :
                    sidx == "Turno" ? x => x.TurnoNombre :
                    sidx == "Grado" ? x => x.GradoAnioNombre :
                    sidx == "Division" ? x => x.Division :
                    sidx == "Cupo" ? x => x.Cupo :
                    sidx == "FechaApertura" ? x => x.FechaApertura :
                (Func<DiagramacionCursoModel, IComparable>)(x => x.Id);
            }
            else
            {
                funcOrden =
                   sidx == "Turno" ? x => x.TurnoNombre :
                   sidx == "Grado" ? x => x.GradoAnioNombre :
                   sidx == "Division" ? x => x.Division :
                   sidx == "Cupo" ? x => x.Cupo :
                   sidx == "FechaApertura" ? x => x.FechaApertura :
                (Func<DiagramacionCursoModel, IComparable>)(x => x.Id);

            }
            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = diagramacionCursoRules.GetDiagramacionCursoByFiltros(filtroTurno, filtroGrado, filtroCarrera, filtroCicloEducativo, id);

            // Ordeno los registros
            registros = sord == "asc" ? registros.OrderBy(funcOrden).ToList() : registros.OrderByDescending(funcOrden).ToList();

            // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
            int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            registros = registros.Skip((page - 1) * rows).Take(rows).ToList();

            // Construyo el json con los valores que se mostraran en la grilla
            var jsonData = new object();

            if (nivel.Id == (int)NivelEducativoNombreEnum.SUPERIOR)
            {
                jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRegistros,
                    rows = from a in registros
                           select new
                           {
                               cell = new string[] {
                                a.Id.ToString(),
                                a.CarreraNombre,
                                a.TurnoNombre,
                                a.GradoAnioNombre,
                                a.Division.ToString(),
                                a.Cupo.ToString(),
                                a.FechaApertura.ToString("dd/MM/yyyy")
                                }
                           }
                };
            }
            else
            {
                jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRegistros,
                    rows = from a in registros
                           select new
                           {
                               cell = new string[] {
                                    a.Id.ToString(), 
                                    a.TurnoNombre,
                                    a.GradoAnioNombre,
                                    a.Division.ToString(),
                                    a.Cupo.ToString(),
                                    a.FechaApertura.ToString("dd/MM/yyyy")
                                }
                           }
                };
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Activacion Empresa

        [HttpPost]
        public ActionResult VisarActivacion(EmpresaVisarModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Rule.VisarActivacion(model);
                    return Json(new { status = true });
                }
            }
            catch (Exception e)
            {
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

            return Json(new { status = (ModelState.IsValid), details = errores.ToArray() });
        }

        #endregion

        #region Emitir Resolucion Empresa

        [HttpPost]
        public ActionResult EmitirResolucionEmpresa(ResolucionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model = Rule.RegistrarResolucionVinculadaAEmpresa(model);
                    return Json(new { status = true });
                }
            }
            catch (Exception e)
            {
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

            return Json(new { status = (ModelState.IsValid), details = errores.ToArray() });
        }

        #endregion

        #region Búsquedas


        /************************************     COMBOS EN CASCADA     ***********************************/

        public JsonResult CargarLocalidades(int idDepartamento)
        {
            var values = from LocalidadModel e in entidadesGenerales.GetLocalidadByDepartamentoProvincial(idDepartamento)
                         orderby e.Nombre
                         select new { Id = e.Id, Nombre = e.Nombre };
            return Json(new SelectList(values, "Id", "Nombre"));
        }
        /************************************     COMBOS EN CASCADA     ***********************************/


        public JsonResult ProcesarBusquedaAsignacionEscuela(string sidx, string sord, int page, int rows)
        {
            var registros = Rule.GetByFiltroBasico(null, null, null, null, null, null, null, null, null,
                new List<TipoEmpresaFiltroBusquedaEnum>() { TipoEmpresaFiltroBusquedaEnum.TODAS }, GetIdEmpresaUsuarioLogueado(), null, GetEmpresaUsuarioLogueado().TipoEmpresa, false);

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
                                a.FechaAlta.ToString("dd/MM/yyyy"),
                                a.CodigoEmpresa,
                                (!a.CUEAnexo.HasValue || a.CUEAnexo.Value == 0) ? string.Empty : a.CUE + "-" + a.CUEAnexo,
                                a.Nombre,
                                a.TipoEmpresa.ToString(),
                                a.EstadoEmpresa.ToString(),
                                a.NivelEducativo.ToString(),
                                a.TipoEducacion.HasValue ? a.TipoEducacion.Value.ToString() : string.Empty
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /************************************     PROCESAR BUSQUEDA BASICO     ***********************************/
        // nivle educativo, tipo de educacion
        public JsonResult ProcesarBusquedaBasico(string sidx, string sord, int page, int rows, string fltCUE, int? fltCUEAnexo,
            string fltNombreEmpresaBasico, string fltCodigoEmpresa, VistaEmpresa vista, int? fltDepartamentoProvincialBasico,
            int? fltLocalidadBasico, string fltBarrioBasico, string fltCalleBasico, int? fltAlturaBasico, int? idEmpresaDependientePadre, bool seConsultaDesdeRegistrarEmpresa)
        {
            List<EmpresaConsultarModel> registros;
            var tiposEmpresasFiltro = ArmarListadoTiposEmpresa(vista);

            registros = Rule.GetByFiltroBasico(fltCUE, fltCUEAnexo, fltCodigoEmpresa, fltNombreEmpresaBasico, ArmarListadoEstados(vista, null), //fltDepartamentoProvincialBasico,
                    fltLocalidadBasico, fltBarrioBasico, fltCalleBasico, fltAlturaBasico, tiposEmpresasFiltro, GetIdEmpresaUsuarioLogueado(), idEmpresaDependientePadre, GetEmpresaUsuarioLogueado().TipoEmpresa, seConsultaDesdeRegistrarEmpresa);

            return ProcesarBusqueda(sidx, sord, page, rows, registros);
        }

        private List<TipoEmpresaFiltroBusquedaEnum> ArmarListadoTiposEmpresa(VistaEmpresa vista)
        {
            List<TipoEmpresaFiltroBusquedaEnum> tiposEmpresasFiltro = new List<TipoEmpresaFiltroBusquedaEnum>();
            tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.TODAS);

            switch (vista)
            {
                case (VistaEmpresa.ModificarTipoEmpresaEditor):
                case (VistaEmpresa.ModificarAsignacionEscuelaAInspeccionEditor):
                case (VistaEmpresa.ConsultarEstructuraEditor):
                case (VistaEmpresa.Seccion):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.ESCUELA_MADRE);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.ESCUELA_ANEXO);
                    break;

                case (VistaEmpresa.SinVista):
                    break;

                case (VistaEmpresa.BuscarInspecciones):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.INSPECCION);
                    break;

                case (VistaEmpresa.BuscarInspeccionesZonal):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.INSPECCION_ZONAL);
                    break;

                case (VistaEmpresa.SoloMinisterio):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.MINISTERIO);
                    break;

                case (VistaEmpresa.BusquedaPorSecretaria):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.MINISTERIO);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_INFRAESTRUCTURA);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_RECURSOS_HUMANOS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_SISTEMAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_TESORERIA);
                    break;

                case (VistaEmpresa.BusquedaPorSubSecretaria):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.SECRETARIA);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_NIVEL);
                    break;

                case (VistaEmpresa.BusquedaPorApoyoAdm):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_NIVEL);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.INSPECCION);
                    break;

                case (VistaEmpresa.BusquedaPorInspeccion):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_NIVEL);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.INSPECCION);
                    break;

                case (VistaEmpresa.BusquedaPorInspeccionUnica):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.INSPECCION);
                    break;

                case (VistaEmpresa.BusquedaPorEscuelaMadre):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    //TODO: En el caso de uso dice que solo se muestran Direcciones de Nivel
                    //if (entidades.GetValorParametroBooleano(ParametroEnum.JERARQUIA_INSPECCION_ORGANIGRAMA))
                    //    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.INSPECCION_ZONAL);
                    //else
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_NIVEL);
                    break;

                case (VistaEmpresa.BusquedaPorEscuelaAnexo):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    //TODO: En el caso de uso dice que solo se muestra la escuela madre
                    //if (entidades.GetValorParametroBooleano(ParametroEnum.JERARQUIA_INSPECCION_ORGANIGRAMA))
                    //    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.INSPECCION_ZONAL);
                    //else
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.ESCUELA_MADRE);
                    break;

                case (VistaEmpresa.BusquedaPorEscuelaRaiz):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.ESCUELA_MADRE_RAIZ);
                    break;

                case (VistaEmpresa.DireccionDeNivel):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_NIVEL);
                    break;

                case (VistaEmpresa.CualquierInspeccionNoZonalPertenecienteADireccionDeNivelDelUsuarioLogueado):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.INSPECCION_NO_ZONAL_QUE_PERTENECE_A_DIRECCION_DE_NIVEL_DEL_USUARIO_ACTUAL);
                    break;

                case (VistaEmpresa.CualquierInspeccionDeDireccionDeNivelDelUsuarioLogueado):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(
                        TipoEmpresaFiltroBusquedaEnum.INSPECCION_QUE_PERTENECE_A_DIRECCION_DE_NIVEL_DEL_USUARIO_ACTUAL);
                    break;

                case (VistaEmpresa.SoloEscuelas):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.ESCUELA_MADRE);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.ESCUELA_ANEXO);
                    break;
                case (VistaEmpresa.AsignacionPlan):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.ESCUELA_MADRE);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.ESCUELA_ANEXO);
                    break;
                case (VistaEmpresa.DireccionDeNivelDelUsuarioLogeado):
                    tiposEmpresasFiltro.Remove(TipoEmpresaFiltroBusquedaEnum.TODAS);
                    tiposEmpresasFiltro.Add(TipoEmpresaFiltroBusquedaEnum.DIRECCION_NIVEL_USUARIO_LOGUEADO);
                    break;
                default:
                    break;
            }
            return tiposEmpresasFiltro;
        }

        private List<EstadoEmpresaEnum> ArmarListadoEstados(VistaEmpresa vista, EstadoEmpresaEnum? filtro)
        {
            List<EstadoEmpresaEnum> estadoEmpresaEnum = new List<EstadoEmpresaEnum>();
            switch (vista)
            {
                //case (VistaEmpresa.ModificarAsignacionEscuelaAInspeccionEditor):
                //    if(filtro.HasValue)
                //        estadoEmpresaEnum.Add(filtro.Value);
                //    break;
                case (VistaEmpresa.ModificarTipoEmpresaEditor):
                    break;
                case (VistaEmpresa.ActivacionCodigoEmpresaEditor):
                    estadoEmpresaEnum.Add(EstadoEmpresaEnum.ACTIVA);
                    break;
                case (VistaEmpresa.EmitirResolucionEmpresaEditor):
                    break;
                case (VistaEmpresa.ReactivarEmpresaEditor):
                    estadoEmpresaEnum.Add(EstadoEmpresaEnum.CERRADA);
                    break;
                case (VistaEmpresa.RegistrarEmpresaEditor):
                    break;
                case (VistaEmpresa.SinVista):
                    break;
                case (VistaEmpresa.VisarActivacionEmpresaEditor):
                    estadoEmpresaEnum.Add(EstadoEmpresaEnum.GENERADA);
                    estadoEmpresaEnum.Add(EstadoEmpresaEnum.EN_PROCESO_DE_REACTIVACION);
                    break;
                case (VistaEmpresa.VisarCierreEmpresaEditor):
                    estadoEmpresaEnum.Add(EstadoEmpresaEnum.EN_PROCESO_DE_CIERRE);
                    estadoEmpresaEnum.Add(EstadoEmpresaEnum.CERRADA_SIN_VISADO);
                    break;
                case (VistaEmpresa.CerrarEmpresaEditor):
                case (VistaEmpresa.SoloMinisterio):
                case (VistaEmpresa.BuscarInspecciones):
                case (VistaEmpresa.BuscarInspeccionesZonal):
                case (VistaEmpresa.BusquedaPorSecretaria):
                case (VistaEmpresa.BusquedaPorSubSecretaria):
                case (VistaEmpresa.BusquedaPorApoyoAdm):
                case (VistaEmpresa.BusquedaPorInspeccion):
                case (VistaEmpresa.BusquedaPorEscuelaMadre):
                case (VistaEmpresa.BusquedaPorEscuelaAnexo):
                case (VistaEmpresa.BusquedaPorEscuelaRaiz):
                case (VistaEmpresa.DireccionDeNivel):
                case (VistaEmpresa.CualquierInspeccionNoZonalPertenecienteADireccionDeNivelDelUsuarioLogueado):
                case (VistaEmpresa.CualquierInspeccionDeDireccionDeNivelDelUsuarioLogueado):
                case (VistaEmpresa.SolicitudDePuestoDeTrabajo):
                    estadoEmpresaEnum.Add(EstadoEmpresaEnum.AUTORIZADA);
                    break;
            }

            //Si tiene filtro de estado 1.valido que el estado filtrado este permitido para la operación 2.Solo envío ese estado
            if (filtro.HasValue)
            {
                if (estadoEmpresaEnum.Count > 0)
                {
                    if (estadoEmpresaEnum.FindAll(x => x == filtro).Count > 0)
                    {
                        estadoEmpresaEnum.Clear();
                        estadoEmpresaEnum.Add(filtro.Value);
                    }
                    else
                    {
                        //TODO FEDE: MENSAJE QUE ESE ESTADO NO ESTA CONTEMPLADO DENTRO DE LA OPERACIÓN QUE DESEA REALIZAR
                    }
                }
                else
                    estadoEmpresaEnum.Add(filtro.Value);
            }
            return estadoEmpresaEnum;
        }

        /************************************      PROCESAR BUSQUEDA EMPRESA      ***********************************/

        //TODO el filtroEmpresa esta declarado como int pero recibe un string. Esto se soluciona cuando se cambie la enumeracion por clase
        public JsonResult ProcesarBusquedaAvanzadaEmpresa(string sidx, string sord, int page, int rows,
            DateTime? fltFechaDesdeEmpresa, DateTime? fltFechaHastaEmpresa, DateTime? fltFechaDesdeInicioActividad,
            DateTime? fltFechaHastaInicioActividad, TipoEmpresaEnum fltTipoEmpresa,
            int? fltProgramaPresupuestarioEmpresa, EstadoEmpresaEnum? fltEstadoEmpresa, VistaEmpresa vista,
            int? fltDepartamentoProvincialEmpresa, int? fltLocalidadEmpresa, string fltBarrioEmpresa,
            string fltCalleEmpresa, int? fltAlturaEmpresa, int? fltObraSocial, TipoEducacionEnum? fltTipoEducacionEmpresa,
            int? fltNivelEducativoEmpresa, int? fltTurno, string fltNombreEmpresa, DateTime? fltFechaDesdeNotificacion, DateTime? fltFechaHastaNotificacion, int? fltTipoInspeccionIntermedia, TipoInspeccionEnum? fltTipoInspeccion, TipoEmpresaEnum? tipoEmpresaUsuarioLogueado, bool? seConsultaDesdeRegistrarEmpresa)
        {
            //Sirve para filtrar solo por inspeccion ZONAL, utilizado en el modificar asignacion escuela a inspeccion.
            TipoInspeccionEnum? tipoInspeccion = null;
            if (vista == VistaEmpresa.ModificarAsignacionEscuelaAInspeccionEditor && fltTipoEmpresa == TipoEmpresaEnum.INSPECCION)
                tipoInspeccion = TipoInspeccionEnum.ZONAL;
            var filtroTiposEmpresa = ArmarListadoTiposEmpresa(vista);
            var registros = Rule.GetByFiltroAvanzado(fltFechaDesdeEmpresa, fltFechaHastaEmpresa, fltFechaDesdeInicioActividad, fltFechaHastaInicioActividad,
                fltTipoEmpresa, null, ArmarListadoEstados(vista, fltEstadoEmpresa), null, null, fltProgramaPresupuestarioEmpresa,
                null, fltTipoEducacionEmpresa, fltNivelEducativoEmpresa, null, null, null, null, fltTipoInspeccion, fltLocalidadEmpresa, fltBarrioEmpresa,
                fltCalleEmpresa, fltAlturaEmpresa, null, null, null, fltNombreEmpresa, fltFechaDesdeNotificacion, fltFechaHastaNotificacion, fltTipoInspeccionIntermedia, filtroTiposEmpresa, GetIdEmpresaUsuarioLogueado(), GetEmpresaUsuarioLogueado().TipoEmpresa, seConsultaDesdeRegistrarEmpresa.Value, string.Empty);

            return ProcesarBusqueda(sidx, sord, page, rows, registros);
        }

        /************************************      PROCESAR BUSQUEDA ESCUELA      ***********************************/

        public JsonResult ProcesarBusquedaAvanzadaEscuela(string sidx, string sord, int page, int rows, int? fltNumeroEscuela,
            DateTime? fltFechaDesdeEscuela, DateTime? fltFechaHastaEscuela, DateTime? fltFechaDesdeInicioActividadEscuela,
            DateTime? fltFechaHastaInicioActividadEscuela, int? fltTipoEscuela, int? fltProgramaPresupuestarioEscuela,
            CategoriaEscuelaEnum? fltTipoCategoria, TipoEducacionEnum? fltTipoEducacion, int? fltProgramaAdministrativo,
            int? fltNivelEducativo, DependenciaEnum? fltDependencia,
            AmbitoEscuelaEnum? fltAmbito, NoSiEnum? fltReligioso, NoSiEnum? fltArancelado,
            TipoInspeccionEnum? fltTipoInspeccion, EstadoEmpresaEnum? fltEstadoEmpresaEscuela,
            TipoEmpresaEnum fltTipoEmpresa, VistaEmpresa vista, int? fltDepartamentoProvincialEscuela,
            int? fltLocalidadEscuela, string fltBarrioEscuela, string fltCalleEscuela, int? fltAlturaEscuela, int? fltObraSocial,
            int? fltPeriodoLectivo, int? fltTurno, string fltNombreEscuela, DateTime? fltFechaDesdeNotificacionEscuela, DateTime? fltFechaHastaNotificacionEscuela, int? idEmpresa, bool seConsultaDesdeRegistrarEmpresa, string fltCodigoInspeccion)
        {
            var registros = Rule.GetByFiltroAvanzado(fltFechaDesdeEscuela, fltFechaHastaEscuela,
                                                     fltFechaDesdeInicioActividadEscuela,
                                                     fltFechaHastaInicioActividadEscuela,
                                                     fltTipoEmpresa, fltProgramaAdministrativo,
                                                     ArmarListadoEstados(vista, fltEstadoEmpresaEscuela),
                                                     fltNumeroEscuela, fltTipoEscuela, fltProgramaPresupuestarioEscuela,
                                                     fltTipoCategoria, fltTipoEducacion,
                                                     fltNivelEducativo, fltDependencia, fltAmbito, fltReligioso,
                                                     fltArancelado, fltTipoInspeccion,
                                                     fltLocalidadEscuela, fltBarrioEscuela, fltCalleEscuela,
                                                     fltAlturaEscuela, fltObraSocial, fltPeriodoLectivo,
                                                     fltTurno, fltNombreEscuela, fltFechaDesdeNotificacionEscuela, fltFechaHastaNotificacionEscuela, null, null,
                                                     GetIdEmpresaUsuarioLogueado(), GetEmpresaUsuarioLogueado().TipoEmpresa, seConsultaDesdeRegistrarEmpresa, fltCodigoInspeccion);
            return ProcesarBusqueda(sidx, sord, page, rows, registros);
        }

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, List<EmpresaConsultarModel> registros)
        {
            // Construyo la funcion de ordenamiento
            Func<EmpresaConsultarModel, IComparable> funcOrden =
                sidx == "FechaAlta" ? x => x.FechaAlta.ToLongDateString() :
                sidx == "FechaHasta" ? x => x.FechaAlta.ToLongDateString() :
                sidx == "CodigoEmpresa" ? x => x.CodigoEmpresa :
                sidx == "CUE" ? x => x.CUE :
                sidx == "Nombre" ? x => x.Nombre :
                sidx == "TipoEmpresa" ? x => x.TipoEmpresa.ToString() :
                sidx == "Estado" ? x => x.EstadoEmpresa :
                sidx == "NivelEducativo" ? x => x.NivelEducativo :
                sidx == "TipoEducacion" ? x => (x.TipoEducacion.HasValue ? x.TipoEducacion.Value.ToString() : string.Empty) :
                (Func<EmpresaConsultarModel, IComparable>)(x => x.Id);

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
                                a.FechaAlta.ToString("dd/MM/yyyy"), //Noel cambió esto para eliminar la HORA. Fue pedido en el ticket #748. Antes estaba así: a.FechaAlta.ToString(),
                                a.CodigoEmpresa,
                                string.IsNullOrEmpty(a.CUE) &&
                                (!a.CUEAnexo.HasValue || a.CUEAnexo.Value == 0) ? string.Empty : a.CUE + "-" + a.CUEAnexo,
                                a.Nombre,
                                a.TipoEmpresa.ToString(),
                                a.EstadoEmpresa.ToString(),
                                a.NivelEducativo,
                                a.TipoEducacion.HasValue? a.TipoEducacion.Value.ToString():string.Empty
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarBusquedaDomicilio(string sidx, string sord, int page, int rows, int? id)
        {
            // Construyo la funcion de ordenamiento
            Func<DomicilioEdificioModel, IComparable> funcOrden =
               sidx == "Calle" ? x => x.Calle :
               sidx == "Altura" ? x => x.Altura :
               sidx == "Barrio" ? x => x.Barrio :
               sidx == "Localidad" ? x => x.Localidad :
            (Func<DomicilioEdificioModel, IComparable>)(x => x.Id);

            // Obtengo los registros filtrados segun los criterios ingresados
            if (id.HasValue && id.Value > 0)
            {
                var registros = Rule.GetDomiciliosDeEdificiosVinculadosAEmpresa(id.Value);

                // Ordeno los registros
                registros = sord == "asc" ? registros.OrderBy(funcOrden).ToList() : registros.OrderByDescending(funcOrden).ToList();

                // Selecciono los registros segun el numero de pagina y cantidad de registros por pagina
                /*int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            registros = registros.Skip((page - 1) * rows).Take(rows).ToList();*/

                // Construyo el json con los valores que se mostraran en la grilla
                var jsonData = new
                {
                    //total = totalPages,
                    rows = from a in registros
                           select new
                           {
                               cell = new string[] {
                                    a.Id.ToString(), a.EntidadId, a.Calle, a.Altura, a.Barrio, Rule.GetLocalidadToStringById(a.Localidad)
                                }
                           }
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public JsonResult GetLocalidadString(int idLocalidad)
        {
            return Json(Rule.GetLocalidadToStringById(idLocalidad), JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetFiltroEmpresaPadreInspeccion(string tipoInspeccion)
        //{
        //    var filtro = string.Empty;
        //    var parametro = entidadesGenerales.GetValorParametroBooleano(ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA);



        //    if(parametro)
        //    {
        //        if (tipoInspeccion == "GENERAL")
        //            filtro =VistaEmpresa.DireccionDeNivel.ToString();
        //        else
        //            filtro = VistaEmpresa.CualquierInspeccionNoZonalPertenecienteADireccionDeNivelDelUsuarioLogueado.ToString();
        //    }
        //    else
        //        filtro =VistaEmpresa.CualquierInspeccionDeDireccionDeNivelDelUsuarioLogueado.ToString();
        //    return Json(filtro, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetFiltroEmpresaPadreInspeccion(string tipoInspeccion)
        {
            var filtro = string.Empty;
            var parametro = entidadesGenerales.GetValorParametroBooleano(ParametroEnum.JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA);
            if (tipoInspeccion != "SELECCIONE")
            {
                if (parametro)
                {
                    if (tipoInspeccion != "GENERAL")
                        filtro =
                            VistaEmpresa.CualquierInspeccionNoZonalPertenecienteADireccionDeNivelDelUsuarioLogueado.ToString
                                ();

                }
                else filtro = VistaEmpresa.CualquierInspeccionDeDireccionDeNivelDelUsuarioLogueado.ToString();

                if (tipoInspeccion == "GENERAL")
                    filtro = VistaEmpresa.DireccionDeNivelDelUsuarioLogeado.ToString();
                else if (tipoInspeccion != "ZONAL")
                    filtro = VistaEmpresa.CualquierInspeccionDeDireccionDeNivelDelUsuarioLogueado.ToString();
            }

            return Json(filtro, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatosEscuelaPrivada(int empresaId)
        {
            var empresa = Rule.GetEmpresaById(empresaId);
            object json = null;
            if (empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE || empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
                if (empresa.Privado)
                {
                    var localidadDir = ServiceLocator.Current.GetInstance<ILocalidadRules>().GetLocalidadById(empresa.Director.LocalidadNacimiento);
                    var localidadRep = ServiceLocator.Current.GetInstance<ILocalidadRules>().GetLocalidadById(empresa.RepresentanteLegal.LocalidadNacimiento);
                    var idPaisDir = localidadDir.DepartamentoProvincial.Provincia.Pais.Id;
                    var idPaisRep = localidadRep.DepartamentoProvincial.Provincia.Pais.Id;
                    var director = new
                                       {

                                           Id = empresa.Director.Id,
                                           Nombre = empresa.Director.Nombre,
                                           Apellido = empresa.Director.Apellido,
                                           FechaNacimiento =
                                               empresa.Director.FechaNacimiento != null
                                                   ? ((DateTime)empresa.Director.FechaNacimiento).ToString("dd/MM/yyyy")
                                                   : string.Empty,
                                           TipoDocumento = empresa.Director.TipoDocumento,
                                           NumeroDocumento = empresa.Director.NumeroDocumento,
                                           Sexo = empresa.Director.Sexo,
                                           EstadoCivil = empresa.Director.EstadoCivil,
                                           Observaciones = empresa.Director.Observaciones,
                                           OrganismoEmisorDocumento = empresa.Director.OrganismoEmisorDocumento,
                                           //Clase = empresa.Director. "ACTOR",
                                           IdPaisEmisorDocumento = idPaisDir,
                                           IdPaisNacionalidad = idPaisDir,
                                           Provincia =
                                               new
                                                   {
                                                       Nombre = localidadDir.DepartamentoProvincial.Provincia.Nombre,
                                                       Id = localidadDir.DepartamentoProvincial.Provincia.Id
                                                   },
                                           DepartamentoProvincial =
                                               new
                                                   {
                                                       Nombre = localidadDir.DepartamentoProvincial.Nombre,
                                                       Id = localidadDir.DepartamentoProvincial.Id.ToString()
                                                   },
                                           Localidad = new { Nombre = localidadDir.Nombre, Id = localidadDir.Id.ToString() },
                                           IdPaisOrigen = idPaisDir,
                                           Domicilio = empresa.Director.Domicilio
                                       };
                    var representante = new
                                            {

                                                Id = empresa.RepresentanteLegal.Id,
                                                Nombre = empresa.RepresentanteLegal.Nombre,
                                                Apellido = empresa.RepresentanteLegal.Apellido,
                                                FechaNacimiento =
                                                    empresa.RepresentanteLegal.FechaNacimiento != null
                                                        ? ((DateTime)empresa.RepresentanteLegal.FechaNacimiento).ToString(
                                                            "dd/MM/yyyy")
                                                        : string.Empty,
                                                TipoDocumento = empresa.RepresentanteLegal.TipoDocumento,
                                                NumeroDocumento = empresa.RepresentanteLegal.NumeroDocumento,
                                                Sexo = empresa.RepresentanteLegal.Sexo,
                                                EstadoCivil = empresa.RepresentanteLegal.EstadoCivil,
                                                Observaciones = empresa.RepresentanteLegal.Observaciones,
                                                OrganismoEmisorDocumento = empresa.RepresentanteLegal.OrganismoEmisorDocumento,
                                                //Clase = empresa.Director. "ACTOR",
                                                IdPaisEmisorDocumento = idPaisRep,
                                                IdPaisNacionalidad = idPaisRep,
                                                Provincia =
                                                    new
                                                        {
                                                            Nombre = localidadRep.DepartamentoProvincial.Provincia.Nombre,
                                                            Id = localidadRep.DepartamentoProvincial.Provincia.Id
                                                        },
                                                DepartamentoProvincial =
                                                    new
                                                        {
                                                            Nombre = localidadRep.DepartamentoProvincial.Nombre,
                                                            Id = localidadDir.DepartamentoProvincial.Id.ToString()
                                                        },
                                                Localidad =
                                                    new { Nombre = localidadRep.Nombre, Id = localidadDir.Id.ToString() },
                                                IdPaisOrigen = idPaisRep,
                                                Domicilio = empresa.RepresentanteLegal.Domicilio
                                            };
                    json = new { director, representante };

                }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca los datos de la empresa que registró dicha empresa. Además, si la empresa se encuentra en estado "CERRADA" nos devuelve la fecha de cierre.
        /// </summary>
        /// <param name="idEmpresa">id Empresa</param>
        /// <returns>Json con los datos de la empresa que registro, y la fecha cierre</returns>
        public JsonResult GetDatosNecesariosVerEditarEmpresa(int idEmpresa)
        {
            var empresa = Rule.GetEmpresaById(idEmpresa);
            var fechaCierre = new Object();
            if (empresa.EstadoEmpresa == EstadoEmpresaEnum.CERRADA && empresa.FechaBaja.HasValue)
            {
                fechaCierre = empresa.FechaBaja.Value;
            }
            else
            {
                fechaCierre = null;
            }
            var empresaRegistro = Rule.GetEmpresaById(empresa.EmpresaRegistro);
            var json = new
                           {
                               CodigoEmpresa = empresaRegistro.CodigoEmpresa,
                               NombreEmpresa = empresaRegistro.Nombre,
                               FechaCierre = fechaCierre,
                               IdEmpresaPadre = empresa.EmpresaPadreOrganigramaId
                           };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private

        private ActionResult ProcesarGet(int? id, EstadoABMC estado)
        {
            ViewData[AjaxAbmc.EstadoText] = estado.ToString();
            ViewData[AjaxAbmc.EstadoId] = (int)estado;
            ViewData[Constantes.VistaEmpresa] = VistaEmpresa.RegistrarEmpresaEditor.ToString();
            //RuleUsuario = ServiceLocator.Current.GetInstance<IUsuarioRules>();
            //TODO VANESA: FALTA OBTENER DATO DEL TiPO USUARIO LOGUEADO
            //ViewData[Constantes.TipoUsuarioLogueado] = RuleUsuario.GetCurrentUser().r
            ViewData[Constantes.TipoUsuarioLogueado] = "PERSONA";
            EmpresaRegistrarModel model = new EmpresaRegistrarModel();
            if (id.HasValue)
            {
                model = Rule.GetEmpresaById(id.Value);
                model.TipoGestion = CargarTipoGestion(model.TipoEmpresa.ToString());
                //if (tipoGestion == TipoGestionEnum.ESCUELA.ToString())
                //{
                //    model.TipoGestion = TipoGestionEnum.ESCUELA;

                //}
                //if (tipoGestion == TipoGestionEnum.GESTION_ADMINISTRATIVA.ToString())
                //{
                //    model.TipoGestion = TipoGestionEnum.GESTION_ADMINISTRATIVA;
                //}
                //if (tipoGestion == TipoGestionEnum.GESTION_EDUCATIVA.ToString())
                //{
                //    model.TipoGestion = TipoGestionEnum.GESTION_EDUCATIVA;
                //}


            }
            return PartialView(VistaEmpresa.RegistrarEmpresaEditor.ToString(), model);
        }

        #endregion

        #region PROVISORIO HASTA QUE ESTEN LS CONTROLES DE BUSQUEDA DE PERSONA PARA DIRECTOR Y REPRESENTANTE LEGAL

        public JsonResult GetDirectorRepresentanteLegal()
        {
            var directorModel = entidadesGenerales.GetPersonaFisicaById(2);
            var representanteLegalModel = entidadesGenerales.GetPersonaFisicaById(2);
            var director = new
            {
                Nombre = directorModel.Nombre,
                Apellido = directorModel.Apellido,
                TipoDocumento = directorModel.TipoDocumento,
                NumeroDocumento = directorModel.NumeroDocumento,
                Sexo = directorModel.Sexo
            };
            var representante = new
            {
                Nombre = representanteLegalModel.Nombre,
                Apellido = representanteLegalModel.Apellido,
                TipoDocumento = representanteLegalModel.TipoDocumento,
                NumeroDocumento = representanteLegalModel.NumeroDocumento,
                Sexo = representanteLegalModel.Sexo
            };

            return Json(new { director = director, representante = representante }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OrdenDePagoProgramaPresupuestarioFromDireccionDeNivelActual()
        {
            var empresaCurrent = Rule.GetCurrentEmpresa();
            var esDireccionDeNivel = empresaCurrent.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL;
            return Json(new
            {
                programaPresupuestarioId = esDireccionDeNivel ? empresaCurrent.ProgramaPresupuestario.Id.ToString() : string.Empty,
                ordenDePagoId = esDireccionDeNivel ? empresaCurrent.OrdenDePago.Id.ToString() : string.Empty
            },
                JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
