using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoComunicacion : IDao<Comunicacion, int>
    {
        List<Comunicacion> GetByPersona(string idEntidad, string tablaOrigen);

        Comunicacion DeleteComunicacion(Comunicacion comunicacion);
        Comunicacion SaveComunicacion(Comunicacion comunicacion);
    }
}

