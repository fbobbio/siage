using System;
using System.ComponentModel.DataAnnotations;
using Siage.Base;

namespace Siage.Services.Core.Models
{
    public class EmpresaModel
    {
        public int Id { get; set; }

        [Required]
        public virtual string CodigoEmpresa { get; set; }
        [Required]
        public string Nombre { get; set; }
        //[Required]
        //public string Descripcion { get; set; }
        [Required]
        public virtual DateTime FechaAlta { get; set; }
        //public virtual DateTime? FechaBaja { get; set; }
        public virtual string Observaciones { get; set; }
        public virtual string Telefono { get; set; }
        [Required]
        public virtual TipoEmpresaEnum TipoEmpresa { get; set; }
        [Required]
        public virtual UsuarioModel UsuarioAlta { get; set; }
        public virtual EmpresaModel EmpresaPadreOrganigrama { get; set; }
        public virtual UsuarioModel UsuarioModificacion { get; set; }
        //public virtual IList<EdificioModel> Edificio { get; set; }
        public virtual EstadoEmpresaEnum EstadoEmpresa { get; set; }
        //public virtual IList<HistorialEstadoEmpresaModel> HistorialEstadoEmpresa { get; set; }
        public virtual DateTime? FechaInicioActividad { get; set; }
        public virtual DateTime? FechaUltimaModificacion { get; set; }
        public virtual OrdenDePagoModel OrdenDePago { get; set; }
        public virtual DomicilioModel Domicilio { get; set; }
        //public virtual ServicioEntidadModel Servicio { get; set; }
        public virtual ProgramaPresupuestarioModel ProgramaPresupuestario { get; set; }
    }
}