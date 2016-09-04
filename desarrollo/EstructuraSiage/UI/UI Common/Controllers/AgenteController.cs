using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using Microsoft.Practices.ServiceLocation;
using SIAGE.UI_Common.Content;


namespace SIAGE.UI_Common.Controllers
{
    public class AgenteController : AjaxAbmcController<AgenteModel, IAgenteRules>
    {
        private IEntidadesGeneralesRules _entidades;
        private ILocalidadRules _ruleLocalidad;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            _ruleLocalidad = ServiceLocator.Current.GetInstance<ILocalidadRules>();
            _entidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();

            Rule = ServiceLocator.Current.GetInstance<IAgenteRules>();
            AbmcView = "AgenteEditor";
        }

        public AgenteModel Agente
        {
            get { return (AgenteModel)Session["Agente"]; } 
            set { Session.Add("Agente", value); }
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            ViewData.Add(ViewDataKey.DIRECCION_NIVEL.ToString(), _entidades.GetDireccionDeNivelAll());
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), _entidades.GetTipoDocumentoAll());

            switch (estado)
            {
                case EstadoABMC.Consultar:
                    string idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");

                    ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(), _entidades.GetDepartamentoProvincialByProvincia(idProvincia));
                    ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), new List<LocalidadModel>());
                    ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), _entidades.GetNivelEducativoAll());
                    ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), _entidades.GetTipoCargoAll());
                    ViewData.Add(ViewDataKey.ASIGNATURA.ToString(), _entidades.GetAsignaturaAll());
                    ViewData.Add(ViewDataKey.TITULO.ToString(), _entidades.GetTituloAll());
                    ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), _entidades.GetSituacionRevistaAll());
                    break;

                default:
                    ViewData.Add(ViewDataKey.PAIS.ToString(), _entidades.GetPaisAll());
                    ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(), _entidades.GetEstadoCivilAll());
                    ViewData.Add(ViewDataKey.SEXO.ToString(), _entidades.GetSexoAll());
                    ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(), _entidades.GetTipoCalleAll());
                    ViewData.Add(ViewDataKey.SUCURSAL_BANCARIA.ToString(), _entidades.GetSucursalBancariaAll());
                    ViewData.Add(ViewDataKey.MOTIVO_BAJA_AGENTE.ToString(), _entidades.GetMotivoBajaAgenteAll());
                    ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(), _entidades.GetOrganismoEmisorDocumentoAll());
                    ViewData.Add(ViewDataKey.TIPO_VINCULO.ToString(), _entidades.GetTiposVinculoAll());
                    break;
            }
        }

        public override ActionResult Index()
        {
            CargarViewData(EstadoABMC.Consultar);
            return View();
        }

        [HttpGet]
        public PartialViewResult GetAgenteById(int? id)
        {
            var agente = new AgenteModel();
            if (id.HasValue)
            {
                agente = Rule.GetAgenteById(id.Value);
                agente.Persona.IdPaisNacionalidad = agente.Persona.IdPaisEmisorDocumento;
                agente.Persona.IdPaisOrigen = agente.Persona.IdPaisEmisorDocumento;
            }

            CargarViewData(EstadoABMC.Ver);

            return PartialView("AgenteSeleccionEditor", agente);
        }

        public override ActionResult ProcesarAbmGet(int? id, EstadoABMC estado)
        {
            ViewData[AjaxAbmc.EstadoText] = estado.ToString();
            ViewData[AjaxAbmc.EstadoId] = (int)estado;
            
            CargarViewData(estado);
            
            Agente = Activator.CreateInstance<AgenteModel>();
            if (id.HasValue)
                Agente = Util.ReflectionUtil.GetById<AgenteModel>(Rule, id.Value);

            return PartialView(AbmcView, Agente);
        }

        public List<int> ListaVinculos
        {
            get { return Session["Vinculos"] as List<int>; }
            private set
            {
                Session["Vinculos"] = value;
            }
        }

        public byte[] ImagenAgente
        {
            get { return (byte[])ViewData["ImagenAgente"]; }
            private set
            {
                ViewData["ImagenAgente"] = value;
            }
        }

        [HttpGet]
        public ContentResult GetAgenteByIdEditar(int idAgente)
        {
            if(Agente == null)
                Agente = Rule.GetAgenteById(idAgente);
            if (Agente.TipoAgente != null && Agente.TipoAgente.Count > 0)
            {
                foreach (var denominacion in
                    Agente.TipoAgente.Where(denominacion => denominacion != null && denominacion.Tipo != null))
                {
                    switch (denominacion.Tipo.Id)
                    {
                        case (int) TipoAgenteEnum.DOCENTE:
                            Agente.EsDocente = true;
                            break;
                        case (int) TipoAgenteEnum.NO_DOCENTE:
                            Agente.EsNoDocente = true;
                            break;
                        case (int) TipoAgenteEnum.ESPECIAL:
                            Agente.EsEspecial = true;
                            break;
                    }
                    denominacion.Agente = null;
                }
            }
            if (Agente.Titulos != null && Agente.Titulos.Count > 0)
            {
                foreach (var titulo in Agente.Titulos)
                {
                    titulo.Agente = null;
                }
            }
            if (Agente.Persona.Id != null && Agente.Persona.Id > 0)
                Agente.Persona.Vinculos = ServiceLocator.Current.GetInstance<IVinculoFamiliarRules>().GetVinculosByPersonaFisica(Agente.Persona.Id.Value);
            if (Agente.Persona.Vinculos != null && Agente.Persona.Vinculos.Count > 0)
            {
                ListaVinculos = new List<int>();
                foreach (var vinculo in Agente.Persona.Vinculos)
                {
                    vinculo.Persona = null;
                    vinculo.Pariente = null;
                    ListaVinculos.Add(vinculo.Id);
                }
                Agente.Vinculos = Agente.Persona.Vinculos;
            }

            var titulos = Agente.Titulos != null ? Agente.Titulos.Select(a => new
                                                         {
                                                             a.Id,
                                                             TipoTitulo = a.TipoTitulo.ToString(), a.NroRegTitulo
                                                         }): null;

            var contacto = Agente.TelefonosEmail != null ? Agente.TelefonosEmail.Select(a => new
                                                                 {
                                                                     TipoComunicacion = a.TipoComunicacion.ToString(), a.Valor
                                                                 }): null;

            /********************************* Persona ********************************/
            var persona = new object();
            if (Agente.Persona != null)
            {
                var localidad = _ruleLocalidad.GetLocalidadById(Agente.Persona.LocalidadNacimiento);
                var idPais = localidad.DepartamentoProvincial.Provincia.Pais.Id;
                persona = new
                {
                    Agente.Persona.Id,
                    Agente.Persona.Nombre,
                    Agente.Persona.Apellido,
                    FechaNacimiento = Agente.Persona.FechaNacimiento != null ? ((DateTime)Agente.Persona.FechaNacimiento).ToString("dd/MM/yyyy") : string.Empty,
                    Agente.Persona.TipoDocumento,
                    Agente.Persona.NumeroDocumento,
                    Agente.Persona.Sexo,
                    Agente.Persona.EstadoCivil,
                    Agente.Persona.Observaciones,
                    /** TODO David quitar hardcode **/
                    Agente.Persona.OrganismoEmisorDocumento,
                    //OrganismoEmisorDocumento = "ANSES",
                    //Clase = "ACTOR",
                    Agente.Persona.IdPaisEmisorDocumento,
                    //IdPaisEmisorDocumento = idPais,
                    //IdPaisNacionalidad = idPais,
                    Agente.Persona.IdPaisNacionalidad,
                    /** Fin hardcode **/
                    Provincia = new { localidad.DepartamentoProvincial.Provincia.Nombre, localidad.DepartamentoProvincial.Provincia.Id },
                    DepartamentoProvincial = new { localidad.DepartamentoProvincial.Nombre, Id = localidad.DepartamentoProvincial.Id.ToString() },
                    Localidad = new { localidad.Nombre, Id = localidad.Id.ToString() },
                    IdPaisOrigen = idPais,
                    Agente.Persona.Domicilio
                };
            }
            var json = new { persona, agente = Agente, contacto, titulos };
            var serializer = new JavaScriptSerializer {MaxJsonLength = Int32.MaxValue};
            return new ContentResult { Content = serializer.Serialize(json), ContentType = "application/json" };
        }

        public JsonResult ValidarAgenteByPersona(string nroDocumento, string tipoDocumento)
        {
            var validacion = Rule.ValidarAgenteByPersona(tipoDocumento, nroDocumento);
            return Json(validacion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarDomicilio(int idAgente)
        {
            if(Agente == null)
                Agente = Rule.GetAgenteById(idAgente);
            var domicilio = Agente.Persona.Domicilio;
            var localidad = ServiceLocator.Current.GetInstance<ILocalidadRules>().GetLocalidadById(domicilio.IdLocalidad);

            var calles = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetCalleByFiltro(null, domicilio.IdLocalidad, domicilio.IdTipoCalle).OrderBy(x => x.Nombre);
            var barrios = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetBarrioByLocalidad(domicilio.IdLocalidad).OrderBy(x => x.Nombre);
            var localidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetLocalidadByProvincia(domicilio.IdProvincia);
            var departamentos = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetDepartamentoProvincialByProvincia(domicilio.IdProvincia).OrderBy(x => x.Nombre);
            var provincias =
                ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetProvinciabyPais(
                    localidad.Provincia.Pais.Id);

            var jsonProvincias = (provincias.Select(d => new {d.Id, d.Nombre })).ToList();
            var jsonDepartamentos = (departamentos.Select(d => new {d.Id, d.Nombre })).ToList();
            var jsonLocalidades = (localidades.Select(d => new {d.Id, d.Nombre })).ToList();
            var jsonBarrios = (barrios.Select(d => new {d.Id, d.Nombre })).ToList();
            var jsonCalles = (calles.Select(d => new {d.Id, d.Nombre })).ToList();

            var datos = new
            { 
                IdPais = localidad.Provincia.Pais.Id,
                Provincias = jsonProvincias,
                IdProvinicia = domicilio.IdProvincia,
                Departamentos = jsonDepartamentos,
                IdDepartamento = domicilio.IdDepartamentoProvincial,
                Localidades = jsonLocalidades, domicilio.IdLocalidad,
                Barrios = jsonBarrios, domicilio.IdBarrio,
                Calles = jsonCalles, domicilio.IdCalle
            };
            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        public override void RegistrarPost(AgenteModel model)
        {
            if (!string.IsNullOrEmpty(model.IdImagen))
            {
                var fotoAgente = model.IdImagen + ".jpg";
                string path = Server.MapPath("~/Content/FicherosSubidos/") + fotoAgente;
                var fs = new FileStream(path, FileMode.Open);
                var bytes = new byte[(int) fs.Length];
                fs.Read(bytes, 0, (int) fs.Length);
                fs.Close();
                model.Fotografia = bytes;
                GuardarAgente(model);
                var file = new FileInfo(path);
                file.Delete();
            }
            else
            {
                GuardarAgente(model);
            }
        }

        [HttpGet]
        public JsonResult RegistroReciente()
        {
            return Json(CustomJson<AgenteModel>(Agente), JsonRequestBehavior.AllowGet);
        }

        private void GuardarAgente(AgenteModel model)
        {
            DomicilioModel domicilio = null;
            if (model.Persona != null && model.Persona.Domicilio != null)
            {
                domicilio = model.Persona.Domicilio;
                model.Persona.Domicilio = null;
            }
            if(model.Id == 0)
            model = Rule.AgenteSave(model);
            else
            {
                model = Rule.AgenteUpdate(model, ListaVinculos);
            }
            if (domicilio != null && model.Persona.Id.HasValue)
            {
                ServiceLocator.Current.GetInstance<IDomicilioRules>().DomicilioPersonaFisicaSaveOUpdate(
                           domicilio,
                            model.Persona.Id.Value,
                            OrigenEnum.T_PER_FISICA);
            }
            Agente = model;
        }

        [HttpPost]
        public Guid GuardarImagen()
        {
            var nombreImagen = Guid.NewGuid();
            try
            {
                var fotoAgente = Request.Files[0];
                if (fotoAgente != null)
                {
                    var mimeType = fotoAgente.ContentType;
                    var fileStream = fotoAgente.InputStream;
                    string fileName = Path.GetFileName(fotoAgente.FileName);
                    int fileLength = fotoAgente.ContentLength;
                    var imagen = new byte[fileLength];
                    if(!Directory.Exists(Server.MapPath("~/Content/FicherosSubidos/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/FicherosSubidos/"));
                    }
                    fotoAgente.SaveAs(Server.MapPath("~/Content/FicherosSubidos/") + nombreImagen + ".jpg");
                    if (Agente == null)
                        Agente = new AgenteModel();
                    Agente.Fotografia = imagen;
                }
            }
            catch (Exception ex)
            {

            }
            return nombreImagen;
        }

        public override void EditarPost(AgenteModel model)
        {
            model.UsuarioAlta = Agente.UsuarioAlta;
            model.TipoAgente = Agente.TipoAgente;
            if (!string.IsNullOrEmpty(model.IdImagen))
            {
                var fotoAgente = model.IdImagen + ".jpg";
                var path = Server.MapPath("~/Content/FicherosSubidos/") + fotoAgente;
                var fs = new FileStream(path, FileMode.Open);
                var bytes = new byte[(int) fs.Length];
                fs.Read(bytes, 0, (int) fs.Length);
                fs.Close();
                model.Fotografia = bytes;
                GuardarAgente(model);
                //model = Rule.AgenteUpdate(model, ListaVinculos);
                Agente = model;
                var file = new FileInfo(path);
                file.Delete();
            }
            else
            {
                if (Agente.Fotografia != null)
                    model.Fotografia = Agente.Fotografia;
                GuardarAgente(model);
            }
        }

        public ActionResult Imagen(string fotoAgente)
        {
            if (!string.IsNullOrEmpty(fotoAgente))
            {
                var path = Server.MapPath("~/Content/FicherosSubidos/") + fotoAgente + ".jpg";
                var fs = new FileStream(path, FileMode.Open);
                var bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                fs.Close();
                return File(bytes, "image/jpg");
            }
            if (Agente != null && Agente.Fotografia != null)
            {
                return File(Agente.Fotografia, "image/jpg");
            }

            return File(new byte[0], "image/jpg"); ;
        }

        public override void EliminarPost(AgenteModel model)
        {
            model.UsuarioAlta = Agente.UsuarioAlta;
            Rule.AgenteDelete(model);
        }

        public JsonResult ProcesarBusquedaSeleccion(string sidx, string sord, int page, int rows, int? id, string FiltroNumeroDocumento, string FiltroTipoDocumento, SexoEnum? FiltroSexo, string FiltroApellido, string FiltroNombre, int? FiltroDepartamentoProvincial, int? FiltroLocalidad, string FiltroNroLegajoSiage, string FiltroNroLegajoMedia, string FiltroNroLegajoInicial, string FiltroNombreEmpresa, int? FiltroNivelEducativo, int? FiltroDireccionNivel, int? FiltroSituacionRevista, int? FiltroTipoCargo, int? FiltroAsignatura, DateTime? FiltroFechaDesdeAlta, DateTime? FiltroFechaHastaAlta, DateTime? FiltroFechaDesdeBaja, DateTime? FiltroFechaHastaBaja, int? FiltroTitulo, TipoAgenteEnum? FiltroTipoAgente,int? FitroEmpresaId)
        {
            // Construyo la funcion de ordenamiento
            Func<AgenteModel, IComparable> funcOrden =
                sidx == "IdentificadorEdificio" ? x => x.Id :
                (Func<AgenteModel, IComparable>)(x => x.Id);
            
            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetAgenteByFilters(FiltroNumeroDocumento, FiltroTipoDocumento, FiltroSexo, FiltroApellido, FiltroNombre, FiltroDepartamentoProvincial, FiltroLocalidad, FiltroNroLegajoSiage, FiltroNroLegajoMedia, FiltroNroLegajoInicial, FiltroNombreEmpresa, FiltroNivelEducativo, FiltroDireccionNivel, FiltroSituacionRevista, FiltroTipoCargo, FiltroAsignatura, FiltroFechaDesdeAlta, FiltroFechaHastaAlta, FiltroFechaDesdeBaja, FiltroFechaHastaBaja, FiltroTitulo, FiltroTipoAgente, FitroEmpresaId);

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
                            cell = new string[] 
                            {
                                a.Id.ToString(), 
                                a.Persona != null? a.Persona.TipoDocumento: string.Empty,
                                a.Persona != null? a.Persona.NumeroDocumento: string.Empty,
                                a.NumLegajoSiage,
                                a.Persona != null? a.Persona.Nombre: string.Empty,
                                a.Persona != null? a.Persona.Apellido: string.Empty,
                                a.Persona != null? a.Persona.SexoNombre.Value.ToString(): string.Empty,
                                a.FechaBaja != null? a.FechaBaja.Value.ToString("dd/MM/yyyy"): string.Empty
                            }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarLocalidadByDepartamentoProvincial(int? idDepartamento)
        {
            var localidad =
                ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetLocalidadByDepartamentoProvincial(
                    idDepartamento.Value).OrderBy(x => x.Nombre);
            return Json(new SelectList(localidad, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarEliminarAgente(int idAgente)
        {
            bool validacion = Rule.ValidarAgenteDelete(idAgente);
            return Json(validacion, JsonRequestBehavior.AllowGet);
        }
    }
}