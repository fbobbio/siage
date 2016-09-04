using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.UCControllers.AutoMapperResolvers
{
    public class PlanEstudioAsignacionesResolver : ValueResolver<PlanEstudio, List<int>>
    {
        protected override List<int> ResolveCore(PlanEstudio source)
        {
            if (source.AsignacionesInstrumentoLegal == null) return null;
            List<int> asignaciones = new List<int>();            
            foreach (var asignacion in source.AsignacionesInstrumentoLegal)
            {
                asignaciones.Add(asignacion.Id);
            }
            return asignaciones;
        }
    }
}
