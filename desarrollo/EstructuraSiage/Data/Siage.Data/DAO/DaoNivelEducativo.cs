using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Siage.Base.Dto;
using Siage.Core.Domain;
using Siage.Core.DaoInterfaces;
using Siage.Base;
using NHibernate.Linq;
using NHibernate.Criterion;
using NHibernate.Transform;
using Siage.Data.MockEntities;
 
namespace Siage.Data.DAO
{
    public class DaoNivelEducativo : DaoBase<NivelEducativo, int>, IDaoNivelEducativo
    {
        #region IDaoNivelEducativo Members
            
        public List<NivelEducativo> GetNivelEducativoPorDireccionNivel(int direccionNivelId)
        {
            var dirNivel = (from d in Session.Query<DireccionDeNivel>()
                            where (d.Id == direccionNivelId)
                            select d).SingleOrDefault<DireccionDeNivel>();
            return dirNivel.NivelesEducativos.ToList<NivelEducativo>();
        }

        public List<CicloEducativo> GetCiclosEducativosByNivelId(int nivelEducativoId)
        {
            NivelEducativoPorTipoEducacion nete = null;
            var query = Session.QueryOver<GradoAnioPorTipoNivelEducativo>();
            query.JoinQueryOver(x => x.NivelEducativoPorTipoEducacion, () => nete);
            query.And(x => nete.NivelEducativo.Id == nivelEducativoId);
            query.Select(x => x.CicloEducativo);
            return query.List<CicloEducativo>().Distinct().ToList();
        }

        public int GetIdNivelEducativoByEscuela(int idEscuela)
        {
            NivelEducativoPorTipoEducacion nivelEducativo = null;
            var query = Session.QueryOver<EmpresaBase>();
            query.And(x => x.Id == idEscuela);
            query.Select(x => x.TipoEmpresa);
            var tipoEmpresa = query.SingleOrDefault<TipoEmpresaEnum>();
            if(tipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
            {
                var query2 = Session.QueryOver<Escuela>();
                query2.And(x => x.Id == idEscuela);
                query2.JoinQueryOver(x => x.NivelesEducativo, () => nivelEducativo);
                query2.Select(x => nivelEducativo.NivelEducativo.Id);
                return query2.SingleOrDefault<int>();
            }
            else
            {

                var query2 = Session.QueryOver<EscuelaAnexo>();
                query2.And(x => x.Id == idEscuela);
                query2.JoinQueryOver(x => x.NivelesEducativo, () => nivelEducativo);
                query2.Select(x => nivelEducativo.NivelEducativo.Id);
                return query2.SingleOrDefault<int>();
            }
        }

        public NivelEducativo GetNivelEducativoByGradoAnio(int idGradoAnio)
        {
            var query = Session.QueryOver<NivelEducativo>();
            NivelEducativoPorTipoEducacion nivelEducativoPorTipoEducacion = null;
            GradoAnioPorTipoNivelEducativo gradoAnioPorTipoNivelEducativo = null;
            query.JoinQueryOver(x => x.NivelEducativoPorTipoEducacion, () => nivelEducativoPorTipoEducacion);
            query.JoinQueryOver(x => nivelEducativoPorTipoEducacion.GradosAnioTNE, () => gradoAnioPorTipoNivelEducativo);
            query.Where(x => gradoAnioPorTipoNivelEducativo.GradoAño.Id == idGradoAnio);
            var rdo= query.List();
            return rdo.FirstOrDefault();
        }

        public DtoConsultaNivelEducativo GetDtoNivelEducativoByGradoAnio(int idGradoAnio)
        {
            DtoConsultaNivelEducativo  dtoConsultaNivelEducativo = null;
            NivelEducativo nivelEducativo = null;
            var query = Session.QueryOver<NivelEducativo>();
            NivelEducativoPorTipoEducacion nivelEducativoPorTipoEducacion = null;
            GradoAnioPorTipoNivelEducativo gradoAnioPorTipoNivelEducativo = null;

            query.JoinAlias(x => x.NivelEducativoPorTipoEducacion, () => nivelEducativoPorTipoEducacion);
            query.JoinAlias(x => nivelEducativoPorTipoEducacion.GradosAnioTNE, () => gradoAnioPorTipoNivelEducativo);
            //query.JoinQueryOver(x => x.NivelEducativoPorTipoEducacion, () => nivelEducativoPorTipoEducacion);
            //query.JoinQueryOver(x => nivelEducativoPorTipoEducacion.GradosAnioTNE, () => gradoAnioPorTipoNivelEducativo);


            query.Where(x => gradoAnioPorTipoNivelEducativo.GradoAño.Id == idGradoAnio);

            var resultado = query.List();
            var entidad = resultado.FirstOrDefault();
            if(entidad != null && entidad.NivelEducativoPorTipoEducacion != null && entidad.NivelEducativoPorTipoEducacion.Count > 0)
            {
                var nivelEducativoAux = entidad.NivelEducativoPorTipoEducacion.FirstOrDefault().NivelEducativo;
                return new DtoConsultaNivelEducativo {Id = nivelEducativoAux.Id, Nombre = nivelEducativoAux.Nombre};
            }
            return null;

            /* NO ME SALIO EL DTO :) ME TIRA EXCEPCION EN ESTA LINEA
            return query.SelectList(list => list
                 .Select(x => nivelEducativo.Id).WithAlias(() => dtoConsultaNivelEducativo.Id)
                 .Select(x => nivelEducativo.Nombre).WithAlias(() => dtoConsultaNivelEducativo.Nombre))
                 .TransformUsing(Transformers.AliasToBean<DtoConsultaNivelEducativo>()).List<DtoConsultaNivelEducativo>().FirstOrDefault();*/
        }

        #endregion
        
        public NivelEducativo GetFiltroNombre(string nombre)
        {
            var query = Session.QueryOver<NivelEducativo>();
            query.And(x => x.Nombre.IsLike(nombre));

            return (NivelEducativo)query.List<NivelEducativo>(); 
        }
    }
}
