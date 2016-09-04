using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoSolicitudCreacionPuestoDeTrabajo : IDao<SolicitudCreacionPuestoDeTrabajo, int>
    {
        List<SolicitudCreacionPuestoDeTrabajo> GetByFilter(string cue, int? cueAnexo, string codigoEmpresa, int? numeroSolicitud, DateTime? fechaDesde, DateTime? fechaHasta, EstadoSolicitudCreacionPuestoDeTrabajoEnum? estado, string codigoPN);
    }
}

