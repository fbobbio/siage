using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
  public  interface IDaoNivelCargo:IDao<NivelCargo,int>
  {

     
      List<NivelCargo> GetByFiltro();
      List<NivelCargo> GetByFiltroVerificarExistencia(string idnivel,string nombrenivelcargo);
      List<NivelCargo> GetByFiltro(string identificador, string idNivelcargo,DateTime? fechadesde,DateTime? fechahasta);
      List<NivelCargo> GetFiltroNivelCargoVigente();//metodo utilizado en TipoCargo

  }
}
