using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Siage.Core.Domain;
using Siage.Services.Core.Models;

namespace Siage.UCControllers.AutoMapperResolvers
{
    public class DireccionDeNivelNivelesEducativosPorTipoEducacion : ValueResolver<DireccionDeNivel, List<NivelEducativoPorTipoEducacionModel>>
    {
        protected override List<NivelEducativoPorTipoEducacionModel> ResolveCore(DireccionDeNivel source)
        {
            List<NivelEducativoPorTipoEducacionModel> listado = new List<NivelEducativoPorTipoEducacionModel>();
            NivelEducativoPorTipoEducacionModel item;
            if (source.NivelesEducativos != null && source.NivelesEducativos.Count > 0 )
            {
                var tipoEducacion = source.TipoEducacion;
                foreach (var nivelEducativo in source.NivelesEducativos)
                {
                    item = new NivelEducativoPorTipoEducacionModel();
                    item.TipoEducacion = tipoEducacion;
                    item.NivelEducativo = Mapper.Map<NivelEducativo, NivelEducativoModel>(nivelEducativo);
                    listado.Add(item);
                }
            }
            return listado;
        }
    }
}
