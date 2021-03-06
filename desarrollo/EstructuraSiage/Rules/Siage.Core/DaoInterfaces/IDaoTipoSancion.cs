﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Core.Domain;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoTipoSancion : IDao<TipoSancion, int>
    {
        List<TipoSancion>GetByNivelEducativo(int idNivel);
    }
}

