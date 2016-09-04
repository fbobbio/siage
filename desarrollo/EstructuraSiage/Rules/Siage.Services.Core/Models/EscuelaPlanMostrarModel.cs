using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;

namespace Siage.Services.Core.Models
{
    public class EscuelaPlanMostrarModel
    {
        public virtual string CodigoEmpresa { get; set; }
        public virtual string CodigoPlan { get; set; }
        public virtual string NombreEscuela { get; set; }
        public virtual string NivelEducativo { get; set; }
        public virtual TipoEducacionEnum TipoEducacion { get; set; }
        public virtual TipoEmpresaEnum TipoEmpresa { get; set; }
        public virtual EstadoEmpresaEnum EstadoEmpresa { get; set; }
        public virtual AmbitoEscuelaEnum AmbitoEmpresa { get; set; }
        public virtual string TituloPlan { get; set; }
        public virtual string EstadoPlan { get; set; }
        public virtual int CicloPlan { get; set; }
        public virtual int EscuelaId { get; set; }
        public virtual int PlanEstudioId { get; set; }
        public virtual DateTime FechaAsignacionEscuelaPlan { get; set; }
    }
}
