using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using SIAGE.UI_Common.Content;
using SIAGE.UI_Common.Controllers;
using SIAGE_Escuela.Content.resources;

namespace SIAGE_Escuela.Controllers
{
    public class CronogramaEscolarController : Controller
    {
        private int _idEscuela;
        private string _nivelEducativo;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            try
            {
                _idEscuela = (int)Session[ConstantesSession.EMPRESA.ToString()];

                ViewData.Add(ViewDataKey.NIVEL_EDUCATIVO.ToString(), 
                    ServiceLocator.Current.GetInstance<ICronogramaEscolarRules>().GetIdNivelEducativoByEscuela(_idEscuela));
            }
            catch (Exception)
            {
                return;
            }
        }

        public JsonResult DevolverEventos(int mes, int anio)
        {
            //Obtengo los eventos de la escuela
            List<CronogramaEscolarModel> eventos = ServiceLocator.Current.GetInstance<ICronogramaEscolarRules>().GetEventosEscolares(_idEscuela, mes, anio);
            //List<CronogramaEscolarModel> eventos = Hardcode(_idEscuela, mes, anio);

            //Transformo esos eventos de manera que se puedan cargar en el calendario
            var auxiliar = new List<object>();
            for (int i = 0; i < eventos.Count; i++)
            {
                auxiliar.Add(new
                {
                    title = eventos[i].Nombre,
                    start = ToUnixTimespan(eventos[i].Dia),
                    tipo = (int)eventos[i].Tipo,
                    horaDesde = eventos[i].HoraDesde,
                    horaHasta = eventos[i].HoraHasta
                });
            }

            var jsonData = new
                {
                    feriados = 
                        from a in eventos
                        where a.Tipo == TipoEventoEscolarEnum.FERIADO
                        select new
                        {
                            title = a.Nombre,
                            start = ToUnixTimespan(a.Dia),
                            tipo = (int) a.Tipo
                        },
                    actividadesEspeciales = 
                        from a in eventos
                        where a.Tipo == TipoEventoEscolarEnum.ACTIVIDAD_ESPECIAL
                        select new
                        {
                            title = a.Nombre,
                            start = ToUnixTimespan(a.Dia),
                            tipo = (int)a.Tipo,
                            horaDesde = a.HoraDesde,
                            horaHasta = a.HoraHasta
                        },
                    suspensionActividad = 
                        from a in eventos
                        where a.Tipo == TipoEventoEscolarEnum.SUSPENSION_ACTIVIDAD
                        select new
                        {
                            title = a.Nombre,
                            start = ToUnixTimespan(a.Dia),
                            tipo = (int)a.Tipo,
                            horaDesde = a.HoraDesde,
                            horaHasta = a.HoraHasta
                        },
                };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Index()
        {
            //Validacion de usuario logueado
            if (ServiceLocator.Current.GetInstance<IEmpresaRules>().EsEscuela(_idEscuela))
            {
                return View();
            }
            TempData[Constantes.ErrorVista] = "Opción válida unicamente para usuarios logueados como escuela.";
            return RedirectToAction("Error", "Home");
        }

        #region SOPORTE

        private long ToUnixTimespan(DateTime date)
        {
            TimeSpan tspan = date.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0));

            return (long)Math.Truncate(tspan.TotalSeconds);
        }

        #endregion

        #region ELIMINAR

        private List<CronogramaEscolarModel> Hardcode(int idEscuela, int idMes, int anio)
        {
            return new List<CronogramaEscolarModel>()
            {
                new CronogramaEscolarModel()
                    {
                        Dia = new DateTime(anio, idMes, 1),
                        Nombre = "Actividad especial",
                        Tipo = TipoEventoEscolarEnum.ACTIVIDAD_ESPECIAL
                    },
                new CronogramaEscolarModel()
                    {
                        Dia = new DateTime(anio, idMes, 2),
                        Nombre = "Suspension actividad",
                        Tipo = TipoEventoEscolarEnum.SUSPENSION_ACTIVIDAD
                    },         
                new CronogramaEscolarModel()
                    {
                        Dia = new DateTime(anio, idMes, 3),
                        Nombre = "Feriado",
                        Tipo = TipoEventoEscolarEnum.FERIADO
                    },           
                new CronogramaEscolarModel()
                    {
                        Dia = new DateTime(anio, idMes, 3),
                        Nombre = "Feriado",
                        Tipo = TipoEventoEscolarEnum.FERIADO
                    },      
                new CronogramaEscolarModel()
                    {
                        Dia = new DateTime(anio, 5, 26),
                        Nombre = "Feriado",
                        Tipo = TipoEventoEscolarEnum.FERIADO
                    },         
                new CronogramaEscolarModel()
                    {
                        Dia = new DateTime(anio, 9, 20),
                        Nombre = "Feriado",
                        Tipo = TipoEventoEscolarEnum.FERIADO
                    },         
                new CronogramaEscolarModel()
                    {
                        Dia = new DateTime(anio, 7, 12),
                        Nombre = "Feriado",
                        Tipo = TipoEventoEscolarEnum.FERIADO
                    },         
                new CronogramaEscolarModel()
                    {
                        Dia = new DateTime(anio, 9, 6),
                        Nombre = "Feriado",
                        Tipo = TipoEventoEscolarEnum.FERIADO
                    },         
                new CronogramaEscolarModel()
                    {
                        Dia = new DateTime(anio, 12, 2),
                        Nombre = "Feriado",
                        Tipo = TipoEventoEscolarEnum.FERIADO
                    }
            };
        }
        #endregion
    }
}
