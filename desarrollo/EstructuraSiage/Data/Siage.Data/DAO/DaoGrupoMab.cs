using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using Siage.Base;
using Siage.Core.Domain;
using Siage.Core.DaoInterfaces;

namespace Siage.Data.DAO
{
    public class DaoGrupoMab : DaoBase<GrupoMab, int>, IDaoGrupoMab


    {


        public List<GrupoMab> GetGrupoMabByFiltros(TipoGrupoMabEnum? tipoGrupo, int? numeroGrupo, string codigoMovimiento)
        {
            var query = Session.QueryOver<CodigoMovimientoMab>().Where(x=>x.GrupoMab!=null);
            var queryAllGrupos = Session.QueryOver<GrupoMab>();
            if (!string.IsNullOrEmpty(codigoMovimiento))
            {
                query.And(x => x.Codigo == codigoMovimiento);
                queryAllGrupos.And(x => x.TipoGrupo == tipoGrupo);
            }

           
            if (numeroGrupo.HasValue && tipoGrupo.HasValue)
            {
                query.JoinQueryOver<GrupoMab>(x => x.GrupoMab).And(x => x.NumeroGrupo == numeroGrupo.Value).And(
                    x => x.TipoGrupo == tipoGrupo.Value);
                queryAllGrupos.And(x => x.NumeroGrupo == numeroGrupo);

            }
            else if (numeroGrupo.HasValue)
                query.JoinQueryOver<GrupoMab>(x => x.GrupoMab).And(x => x.NumeroGrupo == numeroGrupo.Value);
            else if (tipoGrupo.HasValue)
                query.JoinQueryOver<GrupoMab>(x => x.GrupoMab).And(x => x.TipoGrupo == tipoGrupo.Value);

            var lista = (List<GrupoMab>)query.Select(Projections.Group<CodigoMovimientoMab>(p => p.GrupoMab)).List<GrupoMab>();

            if (string.IsNullOrEmpty(codigoMovimiento))
            {
                foreach (var grupoMab in (List<GrupoMab>) queryAllGrupos.List())
                    if (lista.FirstOrDefault(x => x.Id == grupoMab.Id) == null)
                        lista.Add(grupoMab);
            }
            if (numeroGrupo.HasValue && tipoGrupo.HasValue)
                lista = lista.Where(x => x.NumeroGrupo == numeroGrupo.Value && x.TipoGrupo == tipoGrupo.Value).ToList();
            else if (tipoGrupo.HasValue)
                lista = lista.Where(x => x.TipoGrupo == tipoGrupo.Value).ToList();
            else if (numeroGrupo.HasValue)
                lista = lista.Where(x => x.NumeroGrupo == numeroGrupo.Value).ToList();
            return lista.OrderBy(x => x.NumeroGrupo).ToList();
        }



        public int GetUltimoNumeroCorrelativo()
        {
            //var query = Session.QueryOver<GrupoMab>();
            //return query.Future().Count()+1;

            var criterioMaximoNumeroGrupoMab =
                QueryOver.Of<GrupoMab>().SelectList(p => p.SelectMax(e => e.NumeroGrupo));
            var grupoMabConMaximoNumeroGrupoMab =
                Session.QueryOver<GrupoMab>().Where(
                    Subqueries.WhereProperty<GrupoMab>(c => c.NumeroGrupo).Eq(criterioMaximoNumeroGrupoMab)).
                    List();

            return grupoMabConMaximoNumeroGrupoMab != null && grupoMabConMaximoNumeroGrupoMab.Count > 0 ? grupoMabConMaximoNumeroGrupoMab.First().NumeroGrupo + 1 : 1;
        }

     
    }
}
