using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;

namespace Siage.Core.Domain
{
    /// <summary>
    /// Grupo: Docente
    /// </summary>
   public class Mab
    {
       public Mab (){}

        public virtual int Id { get; set; }
        public  virtual DateTime FechaConfeccion {get; set; }
        public virtual DateTime? FechaDesde{ get; set; }
        public virtual DateTime? FechaHasta { get; set; }
        public virtual TipoNovedad TipoNovedad { get; set; }
        public virtual Agente AgenteMab { get; set; }
        public virtual Asignacion  AsignacionAgenteOrigen { get; set; }
        public virtual AsignacionInstrumentoLegal  ActoAdministrativo { get; set; }
        public virtual Agente AgenteAutorizado { get; set; }
        public virtual CodigoMovimientoMab CodigoMovimiento { get; set; }
        public virtual PuestoDeTrabajo PuestoDeTrabajo { get; set; }
        public virtual Asignacion Asignacion { get; set; }
        public virtual SituacionDeRevista SituacionDeRevista { get; set; }
        public virtual string Observaciones { get; set; }
        public virtual string CodigoBarra { get; set; }
        public virtual DateTime? FechaAutorizacionCargo { get; set; }
        public virtual Asignacion PuestoDeTrabajoAnterior { get; set; }
        public virtual ModalidadMab Modalidad { get; set; }
        public virtual Usuario UsuarioAlta { get; set;}
        public virtual Usuario UsuarioModif { get; set; }
        public virtual DateTime FechaUltModif { get; set; }
        


    }
}
