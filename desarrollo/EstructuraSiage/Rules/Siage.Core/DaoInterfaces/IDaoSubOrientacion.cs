using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoSubOrientacion : IDao<SubOrientacion, int>
    {
        List<SubOrientacion> GetByIdOrientacion(int id, bool dadosDeBaja);
        bool TieneEspecialidad(int idSubOrientacion);
    }
}

