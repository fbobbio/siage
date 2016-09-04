using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Siage.Base;
using System.ComponentModel;

namespace Siage.Services.Core.Models
{
    public class EmpresaConsultarModel
    {
        public virtual int Id { get; set; }

        [DisplayName("Código Empresa"), ValidationAttributes.CaracteresEspeciales("|°¬#$%&='´¨+*~^`-_¡!¿?(){}[]<>"), StringLength(9)]
        public virtual string CodigoEmpresa { get; set; }

        [DisplayName("CUE"), ValidationAttributes.CaracteresEspeciales("|°¬#$%&='´¨+*~^`-_¡!¿?(){}[]<>")]
        public virtual string CUE { get; set; }

        [DisplayName("CUE Anexo")]
        public virtual int? CUEAnexo { get; set; }

        [DisplayName("Nombre"), ValidationAttributes.CaracteresEspeciales("|°¬#$%&='´¨+*~^`-_¡!¿?(){}[]<>")]
        public virtual string Nombre { get; set; }

        [DisplayName("Descripción"), ValidationAttributes.CaracteresEspeciales("|°¬#$%&='´¨+*~^`-_¡!¿?(){}[]<>")]
        public virtual string Descripcion { get; set; }

        [DisplayName("Fecha Alta")]
        public virtual DateTime FechaAlta { get; set; }

        [DisplayName("Fecha Baja")]
        public virtual DateTime? FechaBaja { get; set; }
        
        [DisplayName("Fecha Inicio Actividad")]
        public virtual DateTime FechaInicioActividad { get; set; }

        [DisplayName("Observaciones"), ValidationAttributes.CaracteresEspeciales("|°¬#$%&='´¨+*~^`-_¡!¿?(){}[]<>")]
        public virtual string Observaciones { get; set; }

        [DisplayName("Teléfono"), ValidationAttributes.CaracteresEspeciales("|°¬#$%&='´¨+*~^`_¡!¿?(){}[]<>")]
        public virtual string Telefono { get; set; }

        [Required, DisplayName("Tipo Empresa")]
        public virtual TipoEmpresaEnum TipoEmpresa { get; set; }

        [DisplayName("Estado Empresa")]
        public virtual EstadoEmpresaEnum EstadoEmpresa { get; set; }

        [DisplayName("Nivel Educativo")]
        public virtual string NivelEducativo { get; set; }

        [DisplayName("Tipo Educación")]
        public virtual TipoEducacionEnum? TipoEducacion { get; set; }

        [DisplayName("Turno")]
        public virtual TurnoModel Turno { get; set; }
    }
}