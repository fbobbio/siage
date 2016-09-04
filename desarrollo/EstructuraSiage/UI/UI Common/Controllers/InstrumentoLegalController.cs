using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;

namespace SIAGE.UI_Common.Controllers
{
    public class InstrumentoLegalController : AjaxAbmcController<InstrumentoLegalModel, IInstrumentoLegalRules>
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "InstrumentoLegalEditor";
            Rule = ServiceLocator.Current.GetInstance<IInstrumentoLegalRules>();
        }

        public override void CargarViewData(Content.EstadoABMC estado)
        {
            var entidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            var tiposCargo = ServiceLocator.Current.GetInstance<ITipoCargoRules>();
            string idProvincia = ConfigurationManager.AppSettings.Get("IdProvincia");

            ViewData.Add(ViewDataKey.ASIGNATURA.ToString(), entidades.GetAsignaturaAll());
            ViewData.Add(ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString(), entidades.GetDepartamentoProvincialByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.DIRECCION_NIVEL.ToString(), entidades.GetDireccionDeNivelAll());
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), entidades.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.LOCALIDAD.ToString(), entidades.GetLocalidadByProvincia(idProvincia));
            ViewData.Add(ViewDataKey.TIPO_CARGO.ToString(), tiposCargo.FiltroBusquedaTipoCargo(null, null, null, null, null, EstadoTipoCargoEnum.VIGENTE));
            ViewData.Add(ViewDataKey.TIPO_INSTRUMENTO_LEGAL.ToString(), entidades.GetTipoInstrumentoLegalAll());
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), entidades.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.TITULO.ToString(), entidades.GetTituloAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), entidades.GetSituacionRevistaAll());
        }

        public List<InstrumentoLegalModel> ListaInstrumentoLegal
        {
            get
            {
                if (Session["ListaInstrumentoLegal"] == null)
                    Session["ListaInstrumentoLegal"] = new List<InstrumentoLegalModel>();
                return (List<InstrumentoLegalModel>)Session["ListaInstrumentoLegal"];
            }
            set
            {
                Session.Add("ListaInstrumentoLegal", value);
            }
        }

        #region GET

        public override void RegistrarPost(InstrumentoLegalModel model)
        {
            model = Rule.InstrumentoLegalSave(model);
        }

        public override void EditarPost(InstrumentoLegalModel model)
        {
            Rule.InstrumentoLegalSave(model);
        }

        public override void EliminarPost(InstrumentoLegalModel model)
        {
            Rule.InstrumentoLegalDelete(model);
        }

        #endregion

        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, string filtroNumeroDeInstrumentoLegal)
        {
            // Construyo la funcion de ordenamiento
            Func<InstrumentoLegalModel, IComparable> funcOrden =
                sidx == "NroInstrumentoLegal" ? x => x.NroInstrumentoLegal :
                sidx == "FechaEmision" ? x => x.FechaEmision :
                sidx == "EmisorInstrumentoLegal" ? x => x.EmisorInstrumentoLegal :
                sidx == "IdTipoInstrumentoLegal" ? x => GetTipoInstrumentoLegal(x.IdTipoInstrumentoLegal.Value) :
                sidx == "Observaciones" ? x => x.Observaciones :
                (Func<InstrumentoLegalModel, IComparable>)(x => x.Id);
            
            // Obtengo los registros filtrados segun los criterios ingresados
            var registros = Rule.GetInstrumentoLegalByFiltros(filtroNumeroDeInstrumentoLegal);

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
                rows = from a in registros select new {
                    cell = new string[] {
                        a.Id.ToString(), 
                        a.NroInstrumentoLegal,
                        a.FechaEmision.ToString(),
                        a.EmisorInstrumentoLegal.ToString(),
                        a.IdTipoInstrumentoLegal != null? GetTipoInstrumentoLegal(a.IdTipoInstrumentoLegal.Value): "-",
                        a.Observaciones
                    }
                }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private string GetTipoInstrumentoLegal(int tipo)
        {
            return ServiceLocator.Current.GetInstance<ITipoInstrumentoLegalRules>().GetTipoInstrumentoLegalById(tipo).Nombre;
        }

        public JsonResult GetInstrumentoLegalByFilters(string numero, int? id)
        {
            InstrumentoLegalModel instrumento = null;
            if (id.HasValue)
            {
                if (ListaInstrumentoLegal.Count > 0)
                {
                    instrumento = ListaInstrumentoLegal.FirstOrDefault(x => x.Id == id.Value);
                }
                if (instrumento == null)
                {
                    instrumento = Rule.GetInstrumentoLegalById(id.Value);
                    if (instrumento != null)
                        ListaInstrumentoLegal.Add(instrumento);
                }
            }
            if (!string.IsNullOrEmpty(numero))
                numero = numero.ToUpper();
            if (!string.IsNullOrEmpty(numero) && numero != "NULL" && instrumento == null)
            {
                if (ListaInstrumentoLegal.Count > 0)
                {
                    instrumento = ListaInstrumentoLegal.FirstOrDefault(x => x.NroInstrumentoLegal == numero);
                }
                if (instrumento == null)
                {
                    instrumento = Rule.GetInstrumentoLegalByNumeroDeInstrumento(numero);
                    if (instrumento != null)
                        ListaInstrumentoLegal.Add(instrumento);
                }

            }

            if (instrumento == null)
                return null;
            
            var jsonRetorno = new
            {
                Id = instrumento.Id,
                NroInstrumentoLegal = instrumento.NroInstrumentoLegal,
                FechaEmision = ((DateTime)instrumento.FechaEmision).ToString("dd/MM/yyyy"),
                FechaAlta = ((DateTime)instrumento.FechaAlta).ToString("dd/MM/yyyy"),
                EmisorInstrumentoLegal = instrumento.EmisorInstrumentoLegal.ToString(),
                IdTipoInstrumentoLegal = instrumento.IdTipoInstrumentoLegal,
                Observaciones = instrumento.Observaciones,
                RegistrarExpediente = instrumento.RegistrarExpediente,
                Expediente = instrumento.Expediente != null ? instrumento.Expediente.Id : null
            };

            return Json(jsonRetorno, JsonRequestBehavior.AllowGet);
        }
    }
}
