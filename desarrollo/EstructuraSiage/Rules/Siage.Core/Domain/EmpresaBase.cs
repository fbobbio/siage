using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Siage.Base;
using Siage.Core.DomainServiceInterfaces;
using Microsoft.Practices.ServiceLocation;

namespace Siage.Core.Domain
{
  /// <summary>
    /// Unidad de organización dentro del Ministerio de Educación. 
    /// Es cada nivel de una escuela o un área del ministerio y se identifica con un código de empresa. 
    /// Cada empresa tiene su propia POF.
  /// </summary>
    public class EmpresaBase
  { 
	    public EmpresaBase()
		{			
		    HistorialEstados = new List<HistorialEstadoEmpresa>();
            VinculoEmpresaEdificio = new List<VinculoEmpresaEdificio>();
            Comunicaciones = new List<Comunicacion>();
      	}

		public virtual int Id {get; set; }

        public virtual string CodigoEmpresa { get; set; } 
		public virtual string Nombre { get; set; }
        public virtual DateTime FechaAlta { get; set; }
        public virtual DateTime FechaInicioActividad { get; set; } 
		public virtual string Observaciones { get; set; } 
		 
		public virtual DateTime? FechaUltimaModificacion { get; set; }
        public virtual DateTime? FechaCierre { get; set; }
        public virtual DateTime? FechaNotificacion { get; set; }
		public virtual Usuario UsuarioAlta {get; set;}
		public virtual EmpresaBase EmpresaPadreOrganigrama {get; set;}
        public virtual EmpresaBase EmpresaRegistro { get; set; }
        public virtual DireccionDeNivel TipoDireccionNivel { get; set; }
		public virtual OrdenDePago OrdenDePago {get; set;}
		//public virtual IList<Domicilio> Domicilios {get; private set;}
        public virtual int VinculoDomicilio { get; private set; }
		public virtual Usuario UsuarioModificacion {get; set;}
		public virtual IList<VinculoEmpresaEdificio > VinculoEmpresaEdificio {get; set;}
        public virtual TipoEmpresaEnum TipoEmpresa { get; set; }
        public virtual EstadoEmpresaEnum Estado { get; set; }
		public virtual IList<HistorialEstadoEmpresa> HistorialEstados {get; set;}
        public virtual ProgramaPresupuestario ProgramaPresupuestario { get; set; }
        public virtual string EmpresaPadreCodigo { get; set; }
        public virtual string DireccionDeNivelCodigo { get; set; }
        public virtual IList<TurnoPorEscuela> TurnosXEscuela { get; set; }
        public virtual IList<Comunicacion> Comunicaciones { get; set; }

        public virtual void AddVinculoEdificio(VinculoEmpresaEdificio entidad)
		{
            VinculoEmpresaEdificio.Add(entidad);
			entidad.Empresa = this;
		}

       

        #region No Mapeadas

        public virtual Domicilio Domicilio { get; set; }

        //devuelve el estado de la empresa vigente
        public virtual EstadoEmpresaEnum EstadoEmpresa
        {
            get
            {
                return Estado;
            }
        }

        public virtual string Telefono
        {
            get
            {
                Comunicacion com = Comunicaciones == null
                                       ? null
                                       : Comunicaciones.FirstOrDefault(
                                           x => x.TipoComunicacion == TipoComunicacionEnum.TELEFONO_PRINCIPAL);
                if (com != null)
                {
                    return com.Valor;
                }

                return null;
            }
            //set
            //{
            //    Comunicacion com = Comunicaciones== null? null : Comunicaciones.FirstOrDefault(x => x.TipoComunicacion == TipoComunicacionEnum.TELEFONO_PRINCIPAL);
            //    if (com != null)
            //    {
            //        com.Valor = value;
            //    }
            //    else
            //    {
            //        com = new Comunicacion();
            //        com.Entidad = Id.ToString();
            //        com.Valor = value;
            //        com.TipoComunicacion = TipoComunicacionEnum.TELEFONO_PRINCIPAL;
            //        com.Origen = "T_EM_EMPRESAS";
            //        if (Comunicaciones == null) Comunicaciones = new List<Comunicacion>();
            //        Comunicaciones.Add(com);
            //    }
  
            //}
        }

        public virtual void AddComunicacion(string valor, TipoComunicacionEnum tipoComunicacion)
        {
            if (string.IsNullOrEmpty(valor))
                return;

            var comunicacion = Comunicaciones.FirstOrDefault(x => x.TipoComunicacion == tipoComunicacion);
            if (comunicacion != null)
                comunicacion.Valor = valor;
            else
            {
                Comunicaciones.Add(new Comunicacion
                {
                    Entidad = Id.ToString(),
                    Valor = valor,
                    TipoComunicacion = tipoComunicacion,
                    //Origen = System.Configuration.ConfigurationManager.AppSettings["Schema"] + ".T_EM_EMPRESAS"
                    Origen = "T_EM_EMPRESAS"
                });
            }
        }
        #endregion
  }
}

