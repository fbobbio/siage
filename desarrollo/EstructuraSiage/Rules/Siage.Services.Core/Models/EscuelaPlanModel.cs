using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Siage.Base;

namespace Siage.Services.Core.Models
{
    public class EscuelaPlanModel
    {

        public EscuelaPlanModel()
        {
            UnidadesAcademicas = new List<UnidadAcademicaModel>();
        }
        public virtual int Id { get; set; }
        public virtual CarreraModel Carrera { get; set; }
        public virtual EscuelaModel Escuela { get; set; }
        public virtual PlanEstudioModel PlanEstudio { get; set; }
        public virtual DateTime FechaAsignacion { get; set; }
        public virtual DateTime? FechaFinAsignacion { get; set; }
        public virtual DateTime? FechaCierreMatricula { get; set; }
        public virtual IList<UnidadAcademicaModel> UnidadesAcademicas { get; set; }
        public virtual AsignacionInstrumentoLegalModel AsignacionInstrumentoLegal { get; set; }
		
    }
}
