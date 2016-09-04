using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siage.Services.Core.Models;
using Siage.Services.Core.InterfacesUC;
using Microsoft.Practices.ServiceLocation;
using Siage.UCControllers;
using Siage.Base;
using SIAGE.UI_Common.Controllers;
using SIAGE.UI_Common.Content;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class ClaustroDocenteController : AjaxAbmcController<ClaustroDocenteModel, IClaustroDocenteRules>
    {
        #region Atributos / Propiedades
        private int idEscuela;

        public List<DocentesYAsignaturasAsignadasModel> ListaDocentes
        {
            get { return Session[StateBagKeys.Docentes.ToString()] as List<DocentesYAsignaturasAsignadasModel>; }
            private set
            {
                Session[StateBagKeys.Docentes.ToString()] = value;
            }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            AbmcView = "ClaustroDocenteEditor";
            Rule = ServiceLocator.Current.GetInstance<IClaustroDocenteRules>();
            idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];
        }

        public override ActionResult Index()
        {
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(idEscuela)) // validar si estoy logueado como escuela
            {
                var nivel = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetNivelEducativoByEscuela(idEscuela);
                if(nivel.Id == (int)NivelEducativoNombreEnum.MEDIO)
                {
                    ViewData.Add(ViewDataKey.SEXO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetSexoAll());
                    ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoPorNivelEducativoDeEscuela(idEscuela));
                    return View();
                }
                TempData[Constantes.ErrorVista] = "Opción válida únicamente para escuelas de nivel educativo medio.";
                return RedirectToAction("Error", "Home");
            }

            TempData[Constantes.ErrorVista] = "Opción válida únicamente para usuarios logueados como escuela.";
            return RedirectToAction("Error", "Home");
        }

        public override void CargarViewData(EstadoABMC estado)
        {
            var entidadesRules = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            
            ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), ServiceLocator.Current.GetInstance<IGradoAñoRules>().GetAllGradoAñoPorNivelEducativoDeEscuela(idEscuela));
            ViewData.Add(ViewDataKey.SEXO.ToString(), ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>().GetSexoAll());

            //ViewData de Sancion
            ViewData.Add(ViewDataKey.MOTIVO_BAJA.ToString(), entidadesRules.GetMotivoBajaSancionAll());
            ViewData.Add(ViewDataKey.MOTIVO_INCORPORACION.ToString(), entidadesRules.GetMotivoSancionAll());
            ViewData.Add(ViewDataKey.TIPO_SANCION.ToString(), ServiceLocator.Current.GetInstance<ISancionRules>().GetTipoSancionByNivelEducativoEscuela(idEscuela));
        }


        #endregion

        #region POST EstructuraEscuela

        public override void RegistrarPost(ClaustroDocenteModel model)
        {
            Rule.ClaustroDocenteSave(model);
        }

        public override void EditarPost(ClaustroDocenteModel model)
        {
            Rule.ClaustroDocenteUpdate(model);
        }

        #endregion

        #region Procesamiento Busquedas

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
        public JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, int? id, string filtroNroDocumentoEstudiante, string filtroSexoEstudiante, string filtroNombre, string filtroApellido, int? filtroGradoAnio, int? filtroTurno, DivisionEnum? filtroDivision, MotivoClaustroEnum? filtroMotivo, DateTime? filtroFechaCreacion)
        {
            Func<ClaustroDocenteConsultaModel, IComparable> funcOrden = 
                    sidx == "MotivoClaustro" ? x => x.Motivo :
                (Func<ClaustroDocenteConsultaModel, IComparable>)(x => x.Id);
            
            /****************************** INICIO AREA EDITABLE ******************************/
            var registros = Rule.GetByFiltros(idEscuela, filtroTurno, filtroGradoAnio, filtroDivision, filtroNroDocumentoEstudiante, filtroSexoEstudiante, filtroNombre, filtroApellido, filtroMotivo, filtroFechaCreacion);
            /******************************** FIN AREA EDITABLE *******************************/

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
                    page,
                    records = totalRegistros,
                    rows = from a in registros
                           select new
                           {
                               cell = new string[] {
                                a.Id.ToString(),
                                a.TipoDoc,
                                a.NroDoc,
                                a.Nombre,
                                a.Apellido,
                                a.Turno,
                                a.GradoAnio,
                                a.Division.ToString(),
                                a.Fecha.ToString(),
                                a.Motivo.ToString(),
                                a.DiagramacionId.ToString(),
                                a.EstudianteId.ToString(),
                                a.Sexo.ToString()
                                }
                           }
                };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Soporte

        public JsonResult DocentesInvolucrados(int idDiagramacion)
        {
            var resultado = ServiceLocator.Current.GetInstance<IClaustroDocenteRules>().DocentesInvolucrados(idDiagramacion).Distinct().ToList();
            ListaDocentes = resultado;
            var rows = from DocentesYAsignaturasAsignadasModel e in resultado
                       select new { cell = new string[] { e.IdAgente.ToString(), e.NombreDocente, e.ApellidoDocente } };
            return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarBusquedaSubgrillaDocentes(int id)
        {
            List<DocentesYAsignaturasAsignadasModel> registros = ListaDocentes.Where(x => x.IdAgente == id).ToList();

            // Construyo el json con los valores que se mostraran en la subgrilla
            var jsonData = new
            {
                rows = from a in registros
                       select new
                       {
                           cell = new string[] {
                            a.CodigoAsignatura,
                            a.NombreAsignatura
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIdDocentesInvolucrados(int idClaustro)
        {
            var lista = Rule.GetIdDocentesInvolucrados(idClaustro);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
