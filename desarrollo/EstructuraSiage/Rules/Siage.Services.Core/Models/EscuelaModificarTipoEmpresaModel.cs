using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Siage.Base;
using System.ComponentModel;

namespace Siage.Services.Core.Models
{
    public class EscuelaModificarTipoEmpresaModel
    {
        public int Id { get; set; }
        [Required, DisplayName("Nombre")]
        public string Nombre { get; set; }
        [DisplayName("Tipo empresa")]
        public TipoEmpresaEnum TipoEmpresa { get; set; }
        [DisplayName("Es raíz")]
        public bool EsRaiz { get; set; }
        public int? IdEscuelaRaiz { get; set; }
        public int? IdEscuelaMadre { get; set; }

        [DisplayName("Número escuela (*)")]
        public int? NumeroEscuela { get; set; }
        [DisplayName("Número anexo (*)")]
        public int? NumeroAnexo { get; set; }
    }
}
