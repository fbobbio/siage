using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;

namespace SIAGE.UI_Common.Controllers
{
    public class PuestoDeTrabajoController : Controller
    {
        public ViewResult Index()
        {
            var entidades = ServiceLocator.Current.GetInstance<IEntidadesGeneralesRules>();
            ViewData.Add(ViewDataKey.TIPO_DOCUMENTO.ToString(), entidades.GetTipoDocumentoAll());
            ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), entidades.GetNivelEducativoAll());
            ViewData.Add(ViewDataKey.SITUACION_REVISTA.ToString(), entidades.GetSituacionRevistaAll());
            ViewData.Add(ViewDataKey.TURNO.ToString(), entidades.GetTurnoAll());
            ViewData.Add(ViewDataKey.GRADO_ANIO.ToString(), entidades.GetGradoAñoAll());

            return View();
        }

        public JsonResult GetDetalle(int id)
        {
            var regla = ServiceLocator.Current.GetInstance<IPuestoDeTrabajoRules>();
            var pt = regla.GetDetallePuestoDeTrabajo(id);

            return Json(pt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarBusquedaBasico(string sidx, string sord, int page, int rows, int? codigoTipoCargo,
            string nombreTipoCargo, string codigoAgrupamiento, string codigoNivelCargo, EstadoPuestoDeTrabajoEnum? estadoPT,
            string nombreAsignatura, string tipoDocumento, string numeroDocumento, string tipoAgente, string codigoPosicionPN,
            int? agente, int? idEmpresa, int? situacionRevista, bool esPuestoActualMab, bool estadoEmpresaNoCerrada)
        {
            
            var regla = ServiceLocator.Current.GetInstance<IPuestoDeTrabajoRules>();
            var registros = regla.GetByFiltroBasico(codigoTipoCargo, nombreTipoCargo, codigoAgrupamiento,
                                                    codigoNivelCargo,
                                                    estadoPT, nombreAsignatura, tipoDocumento, numeroDocumento,
                                                    tipoAgente, codigoPosicionPN, agente, idEmpresa, situacionRevista,
                                                    esPuestoActualMab, estadoEmpresaNoCerrada);

            return ProcesarBusqueda(sidx, sord, page, rows, registros);
        }

        public JsonResult ProcesarBusquedaAvanzado(string sidx, string sord, int page, int rows,
            string cue, string codigoEmpresa, string nombreEmpresa, EstadoEmpresaEnum? estadoEmpresa, TipoEmpresaEnum? tipoEmpresa, NoSiEnum? escuela,
            int? nivelEducativo, string nombreProgPresup, NoSiEnum? presupuestado, NoSiEnum? liquidado, string tipoPuesto, EstadoAsignacionEnum? estadoAsignacion,
            int? situacionRevista, string tipoInspeccion, NoSiEnum? itinerante, string codigoAsignatura, NoSiEnum? materiaEspecial,
            int? gradoAnio, DivisionEnum? division, int? turno, string nombreCarrera, int? agente, int? idEmpresa, bool esPuestoActualMab, bool estadoEmpresaNoCerrada)
        {
            
            var regla = ServiceLocator.Current.GetInstance<IPuestoDeTrabajoRules>();
            var registros = regla.GetByFiltroAvanzado(cue, codigoEmpresa, nombreEmpresa, estadoEmpresa, tipoEmpresa,
                                                      escuela,
                                                      nivelEducativo, nombreProgPresup, presupuestado, liquidado,
                                                      tipoPuesto, estadoAsignacion, situacionRevista,
                                                      tipoInspeccion, itinerante, codigoAsignatura, materiaEspecial,
                                                      gradoAnio, division, turno, nombreCarrera, agente, idEmpresa,
                                                      esPuestoActualMab, estadoEmpresaNoCerrada);

            return ProcesarBusqueda(sidx, sord, page, rows, registros);
        }

        private JsonResult ProcesarBusqueda(string sidx, string sord, int page, int rows, List<DtoPuestoDeTrabajoControlConsulta> registros)
        {
            int totalRegistros = registros.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalRegistros / (decimal)rows);
            registros = registros.Skip((page - 1) * rows).Take(rows).ToList();

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
                            string.Empty,//GetEmpresaCue(a.Empresa),
                            a.CodigoEmpresa,
                            a.NombreEmpresa,
                            a.EstadoEmpresa.ToString(),
                            //a.ProgramaPresupuestario
                            a.CodigoPuesto,
                            a.CodigoTipoCargo.ToString(),
                            a.NombreTipoCargo,
                            a.NombreTipoCargoEspecial,
                            //a.Agrupamiento
                            a.NivelTipoCargo,
                            a.EstadoPuesto.ToString()
                            /*
                            tipo de documento, 
                            número de documento, 
                            apellidos y nombres del agente, 
                            estado de la asignación, 
                            situación de revista */
                        }
                       }
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public string GetEmpresaCue(EmpresaModel empresa)
        {
            if (empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE || empresa.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
            {
                var escuela = ServiceLocator.Current.GetInstance<IEmpresaRules>().GetEscuelaById(empresa.Id);
                return escuela.CUE;
            }
            return string.Empty;
        }
    }
}
