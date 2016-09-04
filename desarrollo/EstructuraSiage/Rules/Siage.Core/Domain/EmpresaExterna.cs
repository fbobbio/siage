using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Siage.Base;


namespace Siage.Core.Domain
{
  public class EmpresaExterna
  { 
	    public EmpresaExterna()
		{
			Comunicaciones=new List<Comunicacion>();
      	}
		public virtual int Id {get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }
		public virtual DateTime? FechaBaja {get; set;}
        public virtual string Observaciones { get; set; }
        public virtual DateTime FechaAlta { get; set; }
        public virtual DateTime FechaCreacion { get; set; }
        public virtual DateTime FechaUltimaActivacion { get; set; }
        public virtual bool Activo { get; set; }
        public virtual string Sucursal { get; set; }
        public virtual TipoEmpresaExternaEnum TipoEmpresaExterna { get; set; }
        public virtual PersonaJuridica PersonaJuridica { get; set; }
        public virtual PersonaFisica Referente { get; set; }
        public virtual string MotivoBaja { get; set; }
        public virtual CondicionIva CondicionIva { get; set; }
        public virtual int? NumeroAnses { get; set; }
        public virtual int? NumeroIngBrutos { get; set; }
        public virtual Domicilio Domicilio { get; set; }
        public virtual IList<Comunicacion> Comunicaciones { get; set; }
        
        #region No Mapeadas

        public virtual string Telefono
        {
            get
            {
                if (Comunicaciones != null && Comunicaciones.Count != 0)
                {
                    var value = Comunicaciones.FirstOrDefault(x => x.TipoComunicacion == TipoComunicacionEnum.TELEFONO_PRINCIPAL);
                    return (value != null) ? value.Valor : string.Empty;
                }
                else
                {
                    return String.Empty;
                }
            }
        }
        public virtual string Email
        {
            get
            {
                if (Comunicaciones != null && Comunicaciones.Count != 0)
                {
                    var value = Comunicaciones.FirstOrDefault(x => x.TipoComunicacion == TipoComunicacionEnum.DIRECCION_DE_CORREO);
                    return (value != null) ? value.Valor : string.Empty;
                }
                else
                {
                    return String.Empty;
                }
            }
        }
        public virtual string Fax
        {
            get
            {
                if (Comunicaciones != null && Comunicaciones.Count != 0)
                {
                    var value = Comunicaciones.FirstOrDefault(x => x.TipoComunicacion == TipoComunicacionEnum.FAX_PRINCIPAL);
                    return (value != null) ? value.Valor : string.Empty;
                }
                else
                {
                    return String.Empty;
                }
            }
        }
        #endregion

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
                    //Origen = System.Configuration.ConfigurationManager.AppSettings["Schema"] + ".T_ES_ESTUDIANTES"
                    Origen = "T_DO_EMP_EXTERNA"
                });
            }
        }

  }
}

