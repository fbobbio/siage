using System.Collections.Generic;
using NHibernate.Transform;
using Siage.Core.Domain;
using Siage.Core.DaoInterfaces;
using Siage.Base;
using NHibernate.Criterion;
using Siage.Base.Dto;

namespace Siage.Data.DAO
{
    public class DaoEmpresaExterna : DaoBase<EmpresaExterna, int>, IDaoEmpresaExterna
    {
        
        #region IDaoEmpresaExterna Members

        public bool GetByFiltrosVerificarPersonaAsociadaAempresa(string cuil, string cuit)
        {
            var query = Session.QueryOver<EmpresaExterna>();

            if (!string.IsNullOrEmpty(cuil))
            {
                var personaFisica = new PersonaFisica();
                query.JoinQueryOver<PersonaFisica>(x => x.Referente, () => personaFisica).And(pf => pf.CUIL.IsLike(cuil));
            }

            if (!string.IsNullOrEmpty(cuit))
            {
                var personaJuridica = new PersonaJuridica();
                query.JoinQueryOver<PersonaJuridica>(x => x.PersonaJuridica,()=>personaJuridica).And(x => x.CUIT == cuit);
            }

            return query.List<EmpresaExterna>().Count>0;/*retorna true en caso de existir caso contrario false*/
        }

        public int VerificarPersonaYgetByPersonaId(string documentoPf,string cuitPj)
        {

            if (!string.IsNullOrEmpty(documentoPf))
            {
                var query = Session.QueryOver<PersonaFisica>();
                query.Where(x => x.NumeroDocumento == documentoPf);

                var persona = (PersonaFisica)query.SingleOrDefault();
                if (persona != null)
                    return persona.Id;
            }

            if (!string.IsNullOrEmpty(cuitPj))
            {
                var query = Session.QueryOver<PersonaJuridica>();
                query.Where(x => x.CUIT == cuitPj);

                var persona = (PersonaJuridica)query.SingleOrDefault();
                if (persona != null)
                    return persona.Id;
            }

            return 0;
        }

        public bool GetByFiltrosExisteEmpresa(string nombre)
        {
            var query = Session.QueryOver<EmpresaExterna>();

            if (!string.IsNullOrEmpty(nombre))
                query.Where(x => x.Nombre == nombre);

            return query.List().Count > 0;
        }

        public List<DtoEmpresaExternaConsulta> GetByFiltros(string nombre, string razonSocial, string cuil, string cuit, TipoEmpresaExternaEnum? tipoEmpresa, bool estado)
        {
            var query = Session.QueryOver<EmpresaExterna>();
            var personaFisica = new PersonaFisica();
            var personaJuridica = new PersonaJuridica();
            Domicilio domicilio = null;

            var dtoEmpresa = new DtoEmpresaExternaConsulta();

            query.Left.JoinQueryOver(x => x.PersonaJuridica, () => personaJuridica);
            query.Left.JoinQueryOver(x => x.Referente, () => personaFisica);
            query.Left.JoinQueryOver(x => x.Domicilio, () => domicilio);

            if (!string.IsNullOrEmpty(nombre))
                query.Where(x => x.Nombre.IsLike(nombre + "%"));

            if (!string.IsNullOrEmpty(razonSocial))
                query.Where(() => personaJuridica.RazonSocial.IsLike(razonSocial + "%"));

            if (!string.IsNullOrEmpty(cuil))
                query.Where(() => personaFisica.NumeroDocumento == cuil.Substring(2, 8));

            if (!string.IsNullOrEmpty(cuit))
                query.Where(() => personaJuridica.CUIT == cuit);

            if (tipoEmpresa.HasValue)
                query.Where(x => x.TipoEmpresaExterna == tipoEmpresa);

            if(!estado)
                query.Where(x => x.Activo);
           
            return (List<DtoEmpresaExternaConsulta>) query.SelectList(list => list
                .Select(x => x.Id).WithAlias(() => dtoEmpresa.Id)
                .Select(x => x.Nombre).WithAlias(() => dtoEmpresa.Nombre)
                .Select(x => personaJuridica.RazonSocial).WithAlias(() => dtoEmpresa.RazonSocial)
                .Select(x => personaFisica.CUIL).WithAlias(() => dtoEmpresa.Cuil)
                .Select(x => personaJuridica.CUIT).WithAlias(() => dtoEmpresa.Cuit)
                .Select(x => x.Activo).WithAlias(() => dtoEmpresa.Activo)
                .Select(x => x.TipoEmpresaExterna).WithAlias(() => dtoEmpresa.TipoEmpresa)
                .Select(x => domicilio.Altura).WithAlias(() => dtoEmpresa.Altura)
                .Select(x => domicilio.BarrioNuevo).WithAlias(() => dtoEmpresa.BarrioNuevo)
                .Select(x => domicilio.CalleNueva).WithAlias(() => dtoEmpresa.NombreCalle))
            .TransformUsing(Transformers.AliasToBean<DtoEmpresaExternaConsulta>())
            .List<DtoEmpresaExternaConsulta>();
        }

        public bool VerificarExistenciaEnPuestoTrabajo(int empresaExternaId)
        {
            var query = Session.QueryOver<PuestoDeTrabajo>();
            query.Where(x => x.EmpresaServicio.Id == empresaExternaId);
            query.JoinQueryOver(x => x.Estado).And(x => x.Valor == EstadoPuestoDeTrabajoEnum.CERRADO);

            return query.RowCount()>0;
        }

        #endregion
    }
}



/*
            if (!string.IsNullOrEmpty(nombre))
            {
                query.JoinAlias(() => personaFisica., () => personaFisica).Where(pf => pf.CUIL == cuit.Substring(2, 8));
                query.JoinQueryOver<PersonaJuridica>(x => x.PersonaJuridica, () => personaJuridica).Where(
                    pj => pj.CUIT == cuit.Substring(2, 8));
            }

            if (tipoEmpresa.HasValue)
            {
                query.Where(x => x.TipoEmpresaExterna == tipoEmpresa);
            }

            if (!string.IsNullOrEmpty(nombre))
            {
                query.Where(x => x.Nombre == nombre);
                query.Where(() => personaJuridica.RazonSocial == nombre);
            }

            return (List<DtoEmpresaExternaConsulta>) query.SelectList(list => list
                                                                                  .Select(x => x.Id).WithAlias(
                                                                                      () => dtoEmpresa.Id)
                                                                                  .Select(x => x.Nombre).WithAlias(
                                                                                      () => dtoEmpresa.Nombre)
                                                                                  .Select(
                                                                                      x => personaJuridica.RazonSocial)
                                                                                  .Select(x => personaFisica.CUIL)
                                                                                  .Select(x => personaJuridica.CUIT)
                                                                                  .Select(x => x.Activo)
                                                                                  .Select(x => x.TipoEmpresaExterna))

                                                         .TransformUsing(
                                                             Transformers.AliasToBean<DtoEmpresaExternaConsulta>()).List
                                                         <DtoEmpresaExternaConsulta>();*/

/*var sqlWhere = "";
            var  sql = @"
                SELECT Empresa.ID_SEQ_EMPRESA_EXTERNA as Id, Empresa.N_EMPRESA_EXTERNA as Nombre,PersonaJuridica.RAZON_SOCIAL_ALTERNATIVA as RazonSocial,PersonaFisica.CUIL as Cuil,PersonaJuridica.CUIT as Cuit ,Empresa.ACTIVO as Activo
                FROM 
                T_DO_EMP_EXTERNA Empresa
                 left join 
                    T_PER_JURIDICA PersonaJuridica on Empresa.ID_SEQ_PERSONA_JURIDICA = PersonaJuridica.ID_SEQ_PERSONA_JURIDICA 
                  left join 
                    VT_PERSONAS  PersonaFisica on PersonaFisica.ID_SEQ_PERSONA_FISICA=Empresa.ID_SEQ_PERSONA_FISICA  
                  inner join 
                    T_TEMPRESA_EXTERNA TipoEmpresa on TipoEmpresa.ID_SEQ_TIPO_EMPRESA_EXTERNA=Empresa.ID_SEQ_TIPO_EMPRESA_EXTERNA WHERE
                    WHERE";
                     
                if(!string.IsNullOrEmpty(nombre))
                {
                    sqlWhere = " and Empresa.N_EMPRESA_EXTERNA =" + nombre +"\n";
                }
                
                if(tipoEmpresa.HasValue)
                {
                    sqlWhere +=" and TipoEmpresa.N_TEMPRESA_EXTERNA=" +tipoEmpresa.Value+"\n";
                }
                
                if(!string.IsNullOrEmpty(cuil))
                {
                    sqlWhere +=" and  PersonaFisica.CUIL="+cuil+"\n";
                }

                if (string.IsNullOrEmpty(sqlWhere))
                {
                    sql.Trim();
                    sql.Replace("WHERE",String.Empty);

                }

            List<DtoEmpresaExternaConsulta> lista=new List<DtoEmpresaExternaConsulta>();
            var sentencia = (sql + sqlWhere).Trim();

            var result = Session.CreateSQLQuery(sentencia)
            .AddScalar("Id", NHibernateUtil.Int32)
            .AddScalar("Nombre", NHibernateUtil.String)
            .AddScalar("RazonSocial", NHibernateUtil.String)
            .AddScalar("Cuil", NHibernateUtil.String)
            .AddScalar("Cuit", NHibernateUtil.String)
            .AddScalar("Activo", NHibernateUtil.YesNo);
            lista.AddRange(result
            .SetResultTransformer(Transformers.AliasToBean<DtoEmpresaExternaConsulta>()).List
            <DtoEmpresaExternaConsulta>());

            return lista;*/