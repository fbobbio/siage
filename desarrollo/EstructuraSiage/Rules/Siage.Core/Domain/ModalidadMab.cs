using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Siage.Core.Domain
{
    /// <summary>
    /// Grupo: Docente
    /// </summary>
   public class ModalidadMab
    {
       public virtual int Id { get; set; }
       public virtual string Codigo { get; set; }
       public virtual string Descripcion{ get; set; }
 
    }
}
