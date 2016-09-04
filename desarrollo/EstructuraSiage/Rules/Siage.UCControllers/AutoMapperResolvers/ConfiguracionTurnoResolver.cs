using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Siage.Base;
using Siage.Core.Domain;
using Siage.Services.Core.Models;

namespace Siage.UCControllers.AutoMapperResolvers
{
    public class DetalleHorasResolver : ValueResolver<ConfiguracionTurno, List<int>>
    {
        protected override List<int> ResolveCore(ConfiguracionTurno source)
        {
            List<int> detalleHoras = new List<int>();
            foreach (var detalleHora in source.DetalleHoras)
                detalleHoras.Add(detalleHora.Id);
            return detalleHoras;
        }
    }

    public class DetalleHorasModelResolver : ValueResolver<ConfiguracionTurno, List<DetalleHoraTurnoModel>>
    {
        protected override List<DetalleHoraTurnoModel> ResolveCore(ConfiguracionTurno source)
        {
            List<DetalleHoraTurnoModel> detalleHoras = new List<DetalleHoraTurnoModel>();
            foreach (var detalleHora in source.DetalleHoras)
                detalleHoras.Add(Mapper.Map<DetalleHoraTurno, DetalleHoraTurnoModel>(detalleHora));
            return detalleHoras;
        }
    }
}
