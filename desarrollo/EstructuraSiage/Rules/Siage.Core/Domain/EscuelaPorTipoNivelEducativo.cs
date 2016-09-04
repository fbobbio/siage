using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;

namespace Siage.Core.Domain
{
    public class EscuelaPorTipoNivelEducativo
    {

        public virtual NivelEducativoPorTipoEducacion NivelEducativoPorTipoEducacion { get; set; }
        
        public virtual int EscuelaId { get; set; }
        //public virtual int TipoEducacionId { get; set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Comunicacion;
            if (compareTo == null)
                return false;
            return this.GetHashCode() == compareTo.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.NivelEducativoPorTipoEducacion.GetHashCode() ^ this.EscuelaId.GetHashCode();
        }


        #region nomapeados
        public virtual TipoEducacionEnum TipoEducacion
        {
            get { return  NivelEducativoPorTipoEducacion.TipoEducacion; }
            set { NivelEducativoPorTipoEducacion.TipoEducacion = value; }
        }

        public virtual NivelEducativo NivelEducativo
        {
            get { return NivelEducativoPorTipoEducacion.NivelEducativo; }
            set { NivelEducativoPorTipoEducacion.NivelEducativo = value; }
        }

        #endregion
    }
}
