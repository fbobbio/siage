using System;
using System.Linq;
using System.Web.Mvc;
using Siage.Base;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using SIAGE.UI_Common.Content;

namespace SIAGE.UI_Common.Controllers
{
    public class EstudianteController : AjaxAbmcController<EstudianteModel, IEstudianteRules>
    {
        #region Atributos / Propiedades

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "EstudianteEditor";
            Rule = ServiceLocator.Current.GetInstance<IEstudianteRules>();
            
        }

        public override ActionResult Index()
        {
            ViewData.Add(ViewDataKey.SEXO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetSexoAll());
            return View();
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            CargarViewDataPersonaFisica();
            ViewData.Add(ViewDataKey.TIPO_VINCULO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetTiposVinculoAll());
        }

        private void CargarViewDataPersonaFisica()
        {
            var entidadesGeneralesRules = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();

            ViewData.Add(ViewDataKey.PAIS.ToString(), entidadesGeneralesRules.GetPaisAll());
            ViewData.Add(ViewDataKey.ESTADO_CIVIL.ToString(), entidadesGeneralesRules.GetEstadoCivilAll());
            ViewData.Add(ViewDataKey.SEXO.ToString(), entidadesGeneralesRules.GetSexoAll());
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), entidadesGeneralesRules.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.TIPO_CALLE.ToString(), entidadesGeneralesRules.GetTipoCalleAll());
            ViewData.Add(ViewDataKey.ORGANISMO_EMISOR.ToString(), entidadesGeneralesRules.GetOrganismoEmisorDocumentoAll());
            //ViewData["PaisesList"] = ViewData["PaisesList"] ?? entidades.GetPaisAll();
            //ViewData["TiposDocumentoList"] = ViewData["TipoDocumentoList"] ?? entidades.GetTipoDocumentoAll();
            //ViewData["EstadoCivilList"] = ViewData["EstadoCivilList"] ?? entidades.GetEstadoCivilAll();
            //ViewData["SexoList"] = ViewData["SexoList"] ?? entidades.GetSexoAll();
            //ViewData["TipoCalleList"] = ViewData["TipoCalleList"] ?? entidades.GetTipoCalleAll();
            //ViewData["OrganismoEmisorList"] = ViewData["OrganismoEmisorList"] ?? entidades.GetOrganismoEmisorDocumentoAll();
        }

        #endregion

        #region POST ConfiguracionTurno

        public override void RegistrarPost(EstudianteModel model)
        {
            var modelo = Rule.EstudianteSave(model);
            SessionEstudiante = modelo;
        }


        public override void EditarPost(EstudianteModel model)
        {
            Rule.EstudianteUpdate(model);
        }

        public override void EliminarPost(EstudianteModel model)
        {
            Rule.EstudianteDelete(model);
        }

        public EstudianteModel SessionEstudiante
        {
            get
            {
                return (EstudianteModel) Session["Estudiante"];
            }
            set
            {
                Session.Add("Estudiante", value);
            }
        }

        public JsonResult GetEstudianteSession()
        {
            var estudianteSession = SessionEstudiante;
            var jsonData = new
            {
               Persona = new
                             {
                                 estudianteSession.Persona.Id,
                                 estudianteSession.Persona.Nombre,
                                 estudianteSession.Persona.Apellido,
                                 Sexo = estudianteSession.Persona.SexoNombre.ToString(),
                                 estudianteSession.Persona.NumeroDocumento,
                                 EstaEnRCivil = estudianteSession.ExistePersonaEnRCivil
                } 
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
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
                sidx == "Sexo" ? x => x.Persona.SexoNombre :
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
                            a.Persona.SexoNombre.ToString(),
                            a.Persona.NumeroDocumento,
                            a.Persona.Sexo,
                            a.Persona.Id.ToString(),
                            a.Persona.TipoDocumento,
                            a.Persona.FechaNacimiento.ToString()
                            /******************************** FIN AREA EDITABLE *******************************/
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Soporte

        public JsonResult GetVinculosByPersonaId(int idPersona)
        {
            var rows = from DiagramacionCursoModel e in ServiceLocator.Current.GetInstance<IVinculoFamiliarRules>().GetVinculosByPersonaFisica(idPersona)
                         select new { cell = new string[] {e.Id.ToString(), e.Division.ToString(), e.TurnoNombre, e.Cupo.ToString(),  e.Inscripciones.Count().ToString()}};
            return Json(new {rows = rows}, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetBecasByEstudiante(string dniEstudiante)
        {
            var rows = from BecaModel e in ServiceLocator.Current.GetInstance<IBecaRules>().GetBecasByEstudiante(dniEstudiante)
                       select new { cell = new string[] { e.Id.ToString(), e.Nombre, e.TipoBeca.ToString(), e.FechaApertura.ToString() } };
            return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDocumentacionByEstudiante(string dniEstudiante)
        {
            var rows = from HistorialDocumentoModel e in ServiceLocator.Current.GetInstance<IHistorialDocumentoRules>().GetHistorialDocumentosByEstudiante(dniEstudiante)
                       select new { cell = new string[] { e.Id.ToString(), e.DocumentoRequerido.GradoAnio, e.DocumentoRequerido.Proceso.ToString(), e.DocumentoRequerido.Documento.Nombre, e.ObservacionValor, e.Fecha.ToString(), /*Aca va el dato de presentado*/ ""} };
            return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CalcularEdad(DateTime? fecha)
        {
            var retorno = ServiceLocator.Current.GetInstance<IEstudianteRules>().CalcularEdad(fecha);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }       
}
