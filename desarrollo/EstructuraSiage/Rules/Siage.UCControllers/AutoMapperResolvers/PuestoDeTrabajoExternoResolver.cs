using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Siage.Base;
using Siage.Core.Domain;
using Siage.Data.DAO;

namespace Siage.UCControllers.AutoMapperResolvers
{
    public class PuestoDeTrabajoExternoAgenteId : ValueResolver<PuestoDeTrabajo, int>
    {
        protected override int ResolveCore(PuestoDeTrabajo source)
        {
            var asignacion = source.Asignaciones.FirstOrDefault();
            return asignacion.Agente.Id;
        }
    }

    public class PuestoDeTrabajoExternoAgenteNombre : ValueResolver<PuestoDeTrabajo, string>
    {
        protected override string ResolveCore(PuestoDeTrabajo source)
        {
            var asignacion = source.Asignaciones.FirstOrDefault();
            return asignacion.Agente.Persona.Nombre + " " + asignacion.Agente.Persona.Apellido;
        }
    }

    public class PuestoDeTrabajoExternoFechaDesde : ValueResolver<PuestoDeTrabajo, DateTime>
    {
        protected override DateTime ResolveCore(PuestoDeTrabajo source)
        {
            var asignacion = source.Asignaciones.FirstOrDefault();
            return asignacion.FechaInicioEnPuesto;
        }
    }

    public class PuestoDeTrabajoExternoFechaHasta : ValueResolver<PuestoDeTrabajo, DateTime?>
    {
        protected override DateTime? ResolveCore(PuestoDeTrabajo source)
        {
            var asignacion = source.Asignaciones.FirstOrDefault();
            return asignacion.FechaFinEnPuesto;
        }
    }
}
