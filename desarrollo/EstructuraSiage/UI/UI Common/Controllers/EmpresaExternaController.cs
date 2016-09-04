using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Siage.Base;
using SIAGE.UI_Common.Content;

namespace SIAGE.UI_Common.Controllers
{
    public class EmpresaExternaController : AjaxAbmcController<EmpresaExternaModel, IEmpresaExternaRules>
    {
        private IEntidadesGeneralesRules entidades;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Rule = ServiceLocator.Current.GetInstance<IEmpresaExternaRules>();
            AbmcView = "EmpresaExternaEditor";
             entidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();

        }
        public override ActionResult Index()
        {
            CargarViewData(EstadoABMC.Consultar);
            return base.Index();
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            if (ViewData[ViewDataKey.PAIS.ToString()] == null)
                ViewData.Add(ViewDataKey.PAIS.ToString(),
                            entidades.GetPaisAll());
            if (ViewData[ViewDataKey.TIPO_CALLE.ToString()] == null)
                ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(),
                             entidades.GetTipoCalleAll());
            if (ViewData[ViewDataKey.ESTADO_CIVIL.ToString()] == null)
                ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(),
                            entidades.GetEstadoCivilAll());
            if (ViewData[ViewDataKey.TIPO_DOCUMENTO.ToString()] == null)
                ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(),
                            entidades.GetTipoDocumentoAll());
            if (ViewData[ViewDataKey.SEXO.ToString()] == null)
                ViewData.Add(ViewDataKey.SEXO.ToString(),
                             entidades.GetSexoAll());
            if (ViewData[ViewDataKey.ORGANISMO_EMISOR.ToString()] == null)
                ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(),
                            entidades.GetOrganismoEmisorDocumentoAll());
            if (ViewData[ViewDataKey.CONDICION_IVA.ToString()] == null)
                ViewData.Add(ViewDataKey.CONDICION_IVA.ToString(),
                             ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetCondicioIvaAll());

        }
        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, string filtroNombreEmpresa,
                                            string filtroNombreRazonSocial, string filtroCuil, string filtroCuit, TipoEmpresaExternaEnum? filtroTipoEmpresa,
                                            bool chkBusquedaEmpresaExternasEliminadas)
        {
            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetFiltroBusquedaEmpresaExterna(filtroNombreEmpresa, filtroNombreRazonSocial, filtroCuil, filtroCuit, filtroTipoEmpresa, chkBusquedaEmpresaExternasEliminadas);

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
                                                                        a.Nombre,
                                                                        a.RazonSocial,
                                                                        MascaraCuilCuit(a.Cuil),
                                                                        MascaraCuilCuit(a.Cuit),
                                                                        a.Activo ? "Activo" : "Inactivo",
                                                                        
                                                                    }
                                                     }
                               };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public  string MascaraCuilCuit(string valor)
        {
            string valorNuevo = "";
            
            if(!string.IsNullOrEmpty(valor))
            {
                int longuitudCadena = valor.Length;
                for (int i = 0; i < longuitudCadena; i++)
                {
                    valorNuevo += valor[i].ToString();
                    if (i == 1 || i == 9)
                    {
                        valorNuevo +="-";
                    }
                }
            }
            return valorNuevo;
        }

        public JsonResult ValidarExistenciaPersonaFisicaJuridica(string cuil, string cuit)
        {

            var existe = Rule.VerificarExistenciaEmpresaXpersona(cuil, cuit);
            var idPersonaF = 0;
            var idPersonaJ = 0;
              
            if (!existe)
            {

                idPersonaF = !string.IsNullOrEmpty(cuil) ? Rule.GetIdPersona(cuil, cuit) : -1;
                idPersonaJ = !string.IsNullOrEmpty(cuit) ? Rule.GetIdPersona(cuil, cuit) : -1;
               

            }
            var json = new
                           {
                               existeEmpresaXpersona = existe,
                               idPersonaFisica = idPersonaF,
                               idPersonaJuridica = idPersonaJ,
                               cuilPersonaFisicaTiene=(idPersonaF != 0 && idPersonaF != -1) ? Rule.ConsultarExistenciaCuil(idPersonaF) : false,
                               cuitPersonaJuridicaTiene = (idPersonaJ != 0 && idPersonaJ != -1) ? Rule.ConsultarExistenciaCuit(idPersonaJ) : false,

                           };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MostrarEmpresaExterna(int id)
        {
            var empresaExternaEntidad = Rule.TomarIdDeEmpresa(id);
            
            var idReferente = empresaExternaEntidad.Referente.Id;
            var idPersonaJuridica = empresaExternaEntidad.PersonaJuridica!=null?empresaExternaEntidad.PersonaJuridica.Id:0;
            var idReferenteEmpresa=empresaExternaEntidad.PersonaJuridica!=null?idReferente:0;
            
            var json = new
                           {
                               PersonaFisicaId = idReferente,
                               PersonaReferenteEmpresaId =idReferenteEmpresa,
                               PersonaJuridicaId =idPersonaJuridica,
                               DomicilioEmpresaId =
                                   empresaExternaEntidad.Domicilio != null ? empresaExternaEntidad.Domicilio.Id : 0
                           };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpresaExterna(int empresa)
        {
            var model = Rule.GetEmpresaExternaById(empresa);
            var json = new
            {
                Nombre = model.Nombre,
                Telefono = model.Telefono,
                Email = model.Email,
                TipoEmpresaExterna = model.TipoEmpresaExterna.ToString(),
                Estado = model.Activo ? "ACTIVA" : "INACTIVA"
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public override void RegistrarPost(EmpresaExternaModel model)
        {
            DomicilioModel domicilio = null;
            bool domicilioReferente = false;
            bool domicilioPJ = false;
            if (model.Domicilio == null && (model.AsignarDomiPersonaF == false) && (model.AsignarDomiPersonaJ == false))
            {
                throw new ApplicationException("es necesario asignar un domicilio a la empresa externa");
            }

            if (model.Domicilio != null)
            {
                domicilio = model.Domicilio;
            }
                
            else if (model.AsignarDomiPersonaF && model.Referente.Domicilio != null)
            {
                domicilio = model.Referente.Domicilio;
                model.Referente.Domicilio = null;
                domicilioReferente = true;
            }
            else if (model.AsignarDomiPersonaJ && model.PersonaJuridica.Domicilio != null)
            {
                domicilio = model.PersonaJuridica.Domicilio;
                model.PersonaJuridica.Domicilio = null;
                domicilioPJ = true;
            }

            var empresa = Rule.EmpresaExternaSave(model);

            Rule.DomicilioSave(domicilio, empresa.Id);
            if (domicilioReferente && empresa.Referente.Id.HasValue)
                Rule.DomicilioSave(domicilio, empresa.Referente.Id.Value);
            if(domicilioPJ && empresa.PersonaJuridica.Id.HasValue)
                Rule.DomicilioSave(domicilio, empresa.PersonaJuridica.Id.Value);
        }

        public override void EditarPost(EmpresaExternaModel model)
        {
            DomicilioModel domicilio = null;
            domicilio = model.Domicilio;
            var empresa = Rule.EmpresaExternaUpdateOrSave(model);
            
            if (domicilio != null)
            {
                Rule.DomicilioSave(domicilio, empresa.Id);
            }

        }

        public override void EliminarPost(EmpresaExternaModel model)
        {
          Rule.DeleteEmpresaExterna(model);
        }

        public override void ReactivarPost(EmpresaExternaModel model)
        {
            Rule.ReactivarEmpresaExterna(model);
        }

        public PersonaFisicaModel GetPersonaFisicaByFiltros(int id)
        {
            return Rule.TomarPersonaFisicaById(id);
        }
    }

}
