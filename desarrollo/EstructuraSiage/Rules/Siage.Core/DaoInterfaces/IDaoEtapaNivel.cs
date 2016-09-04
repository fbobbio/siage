using System;
using System.Collections.Generic;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.Domain;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoEtapaNivel : IDao<EtapaNivel, int>
    {
        List<EtapaNivel> GetEtapasNivelByNivelEducativo(int idNivel);
    }
}
