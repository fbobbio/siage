using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Siage.Base;
using System.ComponentModel;

namespace Siage.Services.Core.Models
{
    public class EmpresaVisarModel
    {
        public int Id { get; set; }
        [DisplayName("Observaciones:")]
        public string ObservacionesVisarActivacion { get; set; }
        [Required, DisplayName("Accion:")]
        public AccionVisadoActivacionEmpresaEnum Accion { get; set; }
    }
}