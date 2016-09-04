using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;

namespace Siage.Data.MockEntities
{
    public static class NivelesEducativos
    {
        public static NivelEducativo Primario {
            get
            {
                return new NivelEducativo { Id = 1, Nombre = "Primario" };
            }
        }
    }
}
