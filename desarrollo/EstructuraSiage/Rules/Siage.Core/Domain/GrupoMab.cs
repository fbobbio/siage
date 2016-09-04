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
    public class GrupoMab
    {

        public  virtual int Id { get; set; }
        public virtual int NumeroGrupo { get; set; }
        public virtual EjecucionMab Enpt { get; set; }
        public virtual EjecucionMab EnPTanterior { get; set; }
        public virtual bool  GeneraPtp { get; set; }
        public virtual TipoGrupoMabEnum TipoGrupo { get; set; }

    }
}
