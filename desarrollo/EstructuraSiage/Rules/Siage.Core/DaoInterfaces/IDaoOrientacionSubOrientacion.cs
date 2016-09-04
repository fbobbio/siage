using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoOrientacionSubOrientacion : IDao<OrientacionSubOrientacion, int>
    {
        OrientacionSubOrientacion ValidarEspecialidadEnSubOrientacion(string nombre, int subOrientacion);
        OrientacionSubOrientacion GetByOrientacionSubOrientacion(int idOrientacion, int idSubOrientacion);
    }
}
