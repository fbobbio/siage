using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Siage.Core.Domain
{
    /// <summary>
    /// Grupo:Docente
    /// </summary>
    public class EjecucionMab
    {
       
        public virtual int Id { get; set; }
        public virtual bool Liquidacion { get; set; }
        public virtual IList<EjecucionMABEstadosPuesto> EstadosPuestos { get; set; }
        public virtual EstadoAsignacion EstadoAsignacion { get; set; }
        public virtual bool GeneraVacante { get; set; }
        public virtual bool ModificaSitRev { get; set; }
        public virtual bool ModificaEstadoAsignacionEnPT { get; set; }
        public virtual bool ModificaEstadoAnteriorEnPT { get; set; }
        public virtual bool ModificaEstadoPosteriorEnPT { get; set; }
        public virtual bool ModificaEstadoAsignacionEnPTAnterior { get; set; }
        public virtual bool ModificaEstadoAnteriorEnPTAnterior { get; set; }
        public virtual bool ModificaEstadoPosteriorEnPTPosterior { get; set; }
        public virtual bool ImplicaCierreDePTP { get; set; }


        #region no mapeados
        public virtual List<EstadoPuesto> EstadosAnterioresPt
        {

            get
            {
                if (EstadosPuestos != null && EstadosPuestos.Count >0)
                {
                    return (from x in EstadosPuestos where !x.EsPosterior select x.EstadoPuesto).ToList<EstadoPuesto>();
                }
                return null;
            }
            
        }

        public virtual EstadoPuesto EstadoPosteriorPt
        {

            get
            {
                if (EstadosPuestos != null && EstadosPuestos.Count > 0)
                {
                    return (from x in EstadosPuestos where x.EsPosterior select x.EstadoPuesto).SingleOrDefault<EstadoPuesto>();
                }
                return null;
            }
        }


        #endregion
    }
}
