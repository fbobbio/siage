using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.UCControllers.AutoMapperResolvers
{
    public class SolicitudPTConsultaModelModelEscuelaCUE : ValueResolver<SolicitudCreacionPuestoDeTrabajo, string>
    {
        protected override string ResolveCore(SolicitudCreacionPuestoDeTrabajo source)
        {
            Escuela escuela = source.Empresa as Escuela;
            if (escuela != null)
                return escuela.CUE;
            return string.Empty;
        }
    }
}
