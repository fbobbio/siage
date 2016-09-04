using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoLocalidad : IDao<Localidad, int>
    {
        List<Localidad> GetByFiltros();
        List<Localidad> GetAllByProvincia(string idProvincia);
        List<Localidad> GetAllByDepartamentoProvincial(int idDepartamentoProvincial);

        List<Localidad> GetAllByProvinciaConsulta(string idProvincia);
    }
}

