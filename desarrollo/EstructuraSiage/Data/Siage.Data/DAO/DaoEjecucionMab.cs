using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.DaoInterfaces;
using Siage.Core.Domain;
using NHibernate.Criterion;

namespace Siage.Data.DAO
{
    class DaoEjecucionMab :DaoBase<EjecucionMab,int>,IDaoEjecucionMab
    {
        public List<EstadoPuestoDeTrabajoEnum> GetEstadoPuestosPorEjecucionMabId(int ejecucionMabId)
        {
            

            EjecucionMABEstadosPuesto emep = null;
            EstadoPuesto ep = null;

            var query = Session.QueryOver<EjecucionMab>().Where(x => x.Id == ejecucionMabId)
                .JoinQueryOver(x => x.EstadosPuestos, () => emep)
                .And(Restrictions.Eq("EsPosterior", 'N'))
                //.And(x=>x.EsPosterior==false)
                .JoinQueryOver(x => x.EstadoPuesto, () => ep)
                .SelectList(list => list.Select(x => ep.Valor));
            
            var lista = ( List<EstadoPuestoDeTrabajoEnum>) query.List<EstadoPuestoDeTrabajoEnum>();

            return lista;

        }
    }
}
