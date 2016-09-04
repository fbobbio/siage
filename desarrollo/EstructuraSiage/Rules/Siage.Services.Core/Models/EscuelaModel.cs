using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;

namespace Siage.Services.Core.Models
{
    public class EscuelaModel : EmpresaModel
    {
        public virtual int NumeroEscuela { get; set; }
        public virtual int NumeroAnexo { get; set; }
        public virtual bool Religioso { get; set; }
        public virtual bool Arancelado { get; set; }
        public virtual bool Albergue { get; set; }
        public virtual string CUE { get; set; }
        public virtual int CUEAnexo { get; set; }
        public virtual string HorarioDeFuncionamiento { get; set; }
        public virtual string Colectivos { get; set; }
        public virtual bool EstructuraDefinitiva { get; set; }
        public virtual bool ContextoDeEncierro { get; set; }
        public virtual bool Hospitalaria { get; set; }
        public virtual bool Privado { get; set; }
        public virtual EscuelaModel EscuelaMadre { get; set; }
        public virtual bool EsRaiz { get; set; }
        public virtual IList<EscuelaPlanModel> EscuelaPlan { get; set; }
        public virtual EscuelaModel EscuelaRaiz { get; set; }
        public virtual EscuelaPrivadaModel EscuelaPrivada { get; set; }
        public virtual TipoCooperadoraEnum? TipoCooperadora { get; set; }
        public virtual CategoriaEscuelaEnum TipoCategoria { get; set; }
        public virtual ZonaDesfavorableModel ZonaDesfavorable { get; set; }
        public virtual AmbitoEscuelaEnum? Ambito { get; set; }
        public virtual DependenciaEnum? Dependencia { get; set; }
        public virtual int NivelEducativo { get; set; }
        public virtual string NivelEducativoNombre { get; set; }
        public virtual TipoEscuelaModel TipoEscuela { get; set; }
        public virtual TipoEducacionEnum TipoEducacion { get; set; }
        public virtual ModalidadJornadaModel ModalidadJornada { get; set; }
        //public virtual IList<EscuelaPlanModel> PlanesEscuela { get; set; }
        public virtual int PeriodoLectivo { get; set; }
        public virtual IList<TurnoModel> Turnos { get; set; }

    
    }
}
