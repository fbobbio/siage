using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using Siage.Base.Dto;
using Siage.Core.Domain;
using Siage.Core.DaoInterfaces;
using Siage.Base;

namespace Siage.Data.DAO
{
    /** 
    * <summary> Clase DaoMab
    *	
    * </summary>
    * <remarks>
    *		Autor: fede.bobbio
    *		Fecha: 6/13/2011 1:33:17 PM
    * </remarks>
    */
    public class DaoMab : DaoBase<Mab, int>, IDaoMab
    {
        #region Constantes
        #endregion

        #region Atributos
        #endregion

        #region Constructores
        #endregion

        #region Métodos Públicos

        public int GetUltimoNumeroMab()
        {
            //return (int) Session.CreateCriteria(typeof(Mab)).SetProjection( Projections.Max("Id") ).UniqueResult();
            var criterioMaximoNumeroMab =
                QueryOver.Of<Mab>().SelectList(p => p.SelectMax(e => e.Id));
            var mabConMaximoNumeroId =
                Session.QueryOver<Mab>().Where(
                    Subqueries.WhereProperty<Mab>(c => c.Id).Eq(criterioMaximoNumeroMab)).
                    List();

            return mabConMaximoNumeroId != null && mabConMaximoNumeroId.Count > 0 ? mabConMaximoNumeroId.First().Id + 1 : 1;
        }

        public bool TieneAsociadoCodigoMovimiento(int idCodigoMovimiento)
        {
            var codigo = Session.QueryOver<CodigoMovimientoMab>().Where(x => x.Id == idCodigoMovimiento).SingleOrDefault();
            var Mabs =(List<Mab>) Session.QueryOver<Mab>().Where(y => y.CodigoMovimiento == codigo).List();

            return Mabs.Count > 0 ? true : false; 
        }


        public List<Mab> GetByTipoNovedad(int idTipoNovedad)
        {
            var query = Session.QueryOver<Mab>();
            query.And(x => x.TipoNovedad.Id == idTipoNovedad);
            return (List<Mab>)query.List<Mab>();

            //throw new NotImplementedException();
        }

        public List<Mab> GetByCodigoBarra(string codigo)
        {
            var query = Session.QueryOver<Mab>();
            if (!String.IsNullOrEmpty(codigo))
                query.And(x => x.CodigoBarra.IsLike(codigo));
            return (List<Mab>)query.List<Mab>();
        }

        public List<Mab> GetByAgente(int idAgente, DateTime fechaInicial, DateTime fechaFinal)
        {
            var query = Session.QueryOver<Mab>();
            var agente = new Agente();
            query.JoinQueryOver<Agente>(x => x.AgenteMab, () => agente);
            query.And(x => agente.Id == idAgente);
            query.And(x => x.FechaConfeccion >= fechaInicial);
            query.And(x => x.FechaConfeccion <= fechaFinal);
            return (List<Mab>)query.List<Mab>();

            //throw new NotImplementedException();
        }

        public bool AgenteTieneMabAlta(int agenteId)
        {
            var query = Session.QueryOver<Mab>();
            var agente = new Agente();
            query.JoinQueryOver(x => x.AgenteMab, () => agente);
            query.And(x => agente.Id == agenteId);
            query.And(x => x.TipoNovedad.Id == (int) TipoNovedadEnum.ALTA);
            return query.RowCount() > 0;
        }

        /// <summary>
        /// Trae todos los mabs de ausentismo de la empresa pasada por parametro con fecha desde o fecha hasta = fecha actual
        /// </summary>
        /// <param name="idEmpresa">id empresa</param>
        /// <returns>lista con los mabs.</returns>
        public List<DtoGestionAsignacionPorMab> GetByIdEmpresa(int idEmpresa)
        {
            var query = Session.QueryOver<Mab>();
            Asignacion asignacion = null;
            Agente agente = null;
            PersonaFisica personaFisica = null;
            TipoDocumento tipoDocumento = null;
            PuestoDeTrabajo puestoDeTrabajo = null;
            TipoCargo tipoCargo = null;
            EmpresaBase empresaBase = null;
            TipoNovedad tipoNovedad = null;
            CodigoMovimientoMab codigoMovimientoMab = null;
            DtoGestionAsignacionPorMab dtoGestionAsignacionPorMab = null;

            query.JoinAlias(x => x.TipoNovedad, () => tipoNovedad);
            query.JoinAlias(x => x.CodigoMovimiento, () => codigoMovimientoMab);
            query.JoinAlias(x => x.Asignacion, () => asignacion);
            query.JoinAlias(x => asignacion.Agente, () => agente);
            query.JoinAlias(x => asignacion.PuestoDeTrabajo, () => puestoDeTrabajo);
            query.JoinAlias(x => puestoDeTrabajo.TipoCargo, () => tipoCargo);
            query.JoinAlias(x => puestoDeTrabajo.Empresas[0], () => empresaBase);
            query.JoinAlias(x => agente.Persona, () => personaFisica);
            query.JoinAlias(x => personaFisica.TipoDocumento, () => tipoDocumento);

            query.And(x => empresaBase.Id == idEmpresa);
            query.And(x => tipoNovedad.Tipo == TipoNovedadEnum.AUSENTISMO.ToString());
            query.And(x => x.FechaDesde == DateTime.Now || x.FechaHasta == DateTime.Now);

            return query.SelectList(list => list
                 .Select(x => x.Id).WithAlias(() => dtoGestionAsignacionPorMab.Id)
                 .Select(x => tipoNovedad.Tipo).WithAlias(() => dtoGestionAsignacionPorMab.TipoNovedad)
                 .Select(x => tipoDocumento.Nombre).WithAlias(() => dtoGestionAsignacionPorMab.TipoDocumentoAgente)
                 .Select(x => personaFisica.NumeroDocumento).WithAlias(() => dtoGestionAsignacionPorMab.NumeroDocumentoAgente)
                 .Select(x => personaFisica.Apellido).WithAlias(() => dtoGestionAsignacionPorMab.ApellidoAgente)
                 .Select(x => personaFisica.Nombre).WithAlias(() => dtoGestionAsignacionPorMab.NombreAgente)
                 .Select(x => x.FechaDesde).WithAlias(() => dtoGestionAsignacionPorMab.FechaDesde)
                 .Select(x => x.FechaHasta).WithAlias(() => dtoGestionAsignacionPorMab.FechaHasta)
                 .Select(x => codigoMovimientoMab.Descripcion).WithAlias(() => dtoGestionAsignacionPorMab.CodigoMovimiento)
                 .Select(x => empresaBase.Nombre).WithAlias(() => dtoGestionAsignacionPorMab.NombreEmpresa)
                 .Select(x => empresaBase.CodigoEmpresa).WithAlias(() => dtoGestionAsignacionPorMab.CodigoEmpresa)
                 .Select(x => tipoCargo.Nombre).WithAlias(() => dtoGestionAsignacionPorMab.Cargo))
                 .TransformUsing(Transformers.AliasToBean<DtoGestionAsignacionPorMab>()).List<DtoGestionAsignacionPorMab>().ToList();
        }

        #endregion

        #region Métodos Privados
        #endregion

        #region Overrides

        public String ToString()
        {
            return base.ToString();
        }

        public int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
