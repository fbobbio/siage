using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Siage.Base;
using System.ComponentModel;
using Siage.Core.Domain;

namespace Siage.Services.Core.Models
{
    public class EmpresaExternaModel
    {
        public virtual int Id { get; set; }
        [Required]
        public virtual string Nombre { get; set; }
        public virtual string RazonSocial { get; set; }
        public virtual string Cuil { get; set; }
        public virtual string Cuit { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual DateTime? FechaBaja { get; set; }
        [StringLength(100, ErrorMessage = "Se ha superado el máximo de 100 caracteres para el campo Observaciones")]
        public virtual string Observaciones { get; set; }
        public virtual DateTime? FechaAlta { get; set; }
        [Required, DisplayName("Fecha de creación")]
        public virtual DateTime? FechaCreacion { get; set; }
        public virtual DateTime FechaUltimaActivacion { get; set; }
        public virtual bool Activo { get; set; }
        public virtual string Sucursal { get; set; }
        [Required, DisplayName("Tipo de Empresa")]
        public virtual TipoEmpresaExternaEnum TipoEmpresaExterna { get; set; }
        [UIHint("PersonaJuridicaEditor")]
        public virtual PersonaJuridicaModel PersonaJuridica { get; set; }
        public virtual string Fax { get; set;}
        public virtual string Telefono { get; set; }
        public virtual string Email { get; set; }
        [UIHint("PersonaFisicaEditor")]
        public virtual PersonaFisicaModel Referente { get; set; }
        [UIHint("PersonaFisicaEditor")]
        public virtual PersonaFisicaModel ReferenteEmpresa { get; set; }
        [DisplayName("Motivo de baja")]
        public virtual string MotivoBaja { get; set; }
        [DisplayName("Condicion Iva")]
        public virtual string CondicionIva { get; set; }
        [DisplayName("Numero de Anses")]
        public virtual int? NumeroAnses { get; set; }
        [DisplayName("Numero de Ingresos")]
        public virtual int? NumeroIngBrutos { get; set; }
        [UIHint("DomicilioEditor")]
        public virtual DomicilioModel Domicilio { get;  set; }
        public virtual int VinculoDomicilio { get;  set; }
        [DisplayName("Asignar Domicilio de persona Fisica")]
        public virtual bool AsignarDomiPersonaF{ get; set; }
        [DisplayName("Asignar Domicilio de persona Jurídica")]
        public virtual bool AsignarDomiPersonaJ { get; set; }
        [DisplayName("Registrar Domicilio a Empresa Externa")]
        public virtual bool RegistarDomicilioEmpresa { get; set; }


    }
}

       
       
		
       