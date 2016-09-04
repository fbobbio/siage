using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;
using Siage.Base.Dto;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoDocenteActividadEspecial : IDao<DocenteActividadEspecial, int>
    {
        List<DocenteActividadEspecial> GetDocenteActividadEspecialByAgenteIdYRango(int agenteId, DateTime fechaDesde, DateTime fechaHasta);
    }
}
