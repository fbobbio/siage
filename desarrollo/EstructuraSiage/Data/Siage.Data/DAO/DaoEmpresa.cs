using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;using Siage.Core.Domain;using Siage.Core.DaoInterfaces;
using Siage.Base;
using NHibernate.Linq;


namespace Siage.Data.DAO
{
    public class DaoEmpresa : DaoBase<EmpresaBase, int>, IDaoEmpresa
    {
        public DaoEmpresa()
        {

        }

        #region IDaoEmpresa Members
        //TODO AGUSTIN: verificar metodo
        public List<string> GetCamposConInstrumentoLegal()
        {

            return new List<string> {"NombreEmpresa", "TipoEmpresa", "TipoGestion", "Observaciones" };
        }
        //TODO VANESA: OJO DE INSPECCION, DN Y ESCUELA SE DEBE SACAR LA EMPRESA
        /// <summary>
        /// Obtiene una lista de empresas segun filtro básico
        /// </summary>
        /// <param name="codigoEmpresa">Codigo de empresa</param>
        /// <param name="nombreEmpresa">Nombre de empresa</param>
        /// <param name="idDepartamentoProvincial">Pertenece al domicilio</param>
        /// <param name="idLocalidad">Pertenece al domicilio</param>
        /// <param name="barrio">Pertenece al domicilio</param>
        /// <param name="calle">Pertenece al domicilio</param>
        /// <param name="altura">Pertenece al domicilio</param>
        /// <param name="estadoEmpresaEnum">Estado de la empresa</param>
        /// <returns>Devuelve una lista de empresas que cumplan con los parametros de busqueda. </returns>
        /// <summary>
        /// Obtiene una lista de empresas segun filtro básico
        /// </summary>
        /// <param name="codigoEmpresa">Codigo de empresa</param>
        /// <param name="nombreEmpresa">Nombre de empresa</param>
        /// <param name="idDepartamentoProvincial">Pertenece al domicilio</param>
        /// <param name="idLocalidad">Pertenece al domicilio</param>
        /// <param name="barrio">Pertenece al domicilio</param>
        /// <param name="calle">Pertenece al domicilio</param>
        /// <param name="altura">Pertenece al domicilio</param>
        /// <param name="estadoEmpresaEnum">Estado de la empresa</param>
        /// <returns>Devuelve una lista de empresas que cumplan con los parametros de busqueda. </returns>
        /// <summary>
        /// Obtiene una lista de empresas segun filtro básico
        /// </summary>
        /// <param name="codigoEmpresa">Codigo de empresa</param>
        /// <param name="nombreEmpresa">Nombre de empresa</param>
        /// <param name="idDepartamentoProvincial">Pertenece al domicilio</param>
        /// <param name="idLocalidad">Pertenece al domicilio</param>
        /// <param name="barrio">Pertenece al domicilio</param>
        /// <param name="calle">Pertenece al domicilio</param>
        /// <param name="altura">Pertenece al domicilio</param>
        /// <param name="estadoEmpresaEnum">Estado de la empresa</param>
        /// <returns>Devuelve una lista de empresas que cumplan con los parametros de busqueda. </returns>
        public List<EmpresaBase> GetByFiltrosBasico(string codigoEmpresa, string nombreEmpresa, int? idDepartamentoProvincial, int? idLocalidad, string barrio, string calle, string altura,List<EstadoEmpresaEnum> estadoEmpresaEnum)
        {
            var empresas = (from m in Session.Query<EmpresaBase>()
                            where m.TipoEmpresa != TipoEmpresaEnum.ESCUELA_MADRE && m.TipoEmpresa != TipoEmpresaEnum.ESCUELA_ANEXO
                            && (codigoEmpresa.Length == 0 || m.CodigoEmpresa.Contains(codigoEmpresa))
                            && (nombreEmpresa.Length == 0 || m.Nombre.Contains(nombreEmpresa))
                            && (idLocalidad == null || m.Domicilio.Localidad.Id == idLocalidad)
                            && (idDepartamentoProvincial == null || m.Domicilio.Localidad.DepartamentoProvincial.Id == idDepartamentoProvincial)
                            && (barrio.Length == 0 || m.Domicilio.Barrio.Contains(barrio))
                            && (calle.Length == 0 || m.Domicilio.Calle.Contains(calle))
                            && (altura.Length == 0 || m.Domicilio.Altura.Contains(altura))
                            && (estadoEmpresaEnum.Count == 0 || estadoEmpresaEnum.Contains(m.EstadoEmpresa))
                            orderby m.Nombre
                            select m).ToList<EmpresaBase>();
            return empresas;
            //StringBuilder hql = new StringBuilder("FROM Empresa e WHERE e.TipoEmpresa!=:EscuelaMadre AND e.TipoEmpresa!=:EscuelaAnexo");
            
            //if (!String.IsNullOrEmpty(codigoEmpresa))
            //{
            //    hql.Append(" AND e.CodigoEmpresa=:codigoEmpresa");
            //}
            //if (!String.IsNullOrEmpty(nombreEmpresa))
            //{
            //    hql.Append(" AND e.Nombre LIKE :nombreEmpresa");
            //}
            //if (idLocalidad.HasValue)
            //{
            //    hql.Append(" AND e.Domicilio.Localidad.Id=:idLocalidad");
            //}
            //if (idDepartamentoProvincial.HasValue)
            //{
            //    hql.Append(" AND e.Domicilio.Localidad.DepartamentoProvincial.Id=:idDepartamentoProvincial");
            //}
            //if (!String.IsNullOrEmpty(barrio))
            //{
            //    hql.Append(" AND e.Domicilio.Barrio LIKE :barrio");
            //}
            //if (!String.IsNullOrEmpty(calle))
            //{                
            //    hql.Append(" AND e.Domicilio.Calle LIKE :calle");
            //}
            //if (!String.IsNullOrEmpty(altura))
            //{
            //    hql.Append(" AND e.Domicilio.Altura LIKE :altura");
            //}
            //if (estadoEmpresaEnum.Count>0)
            //{
            //    hql.Append(" AND (");
            //    foreach (EstadoEmpresaEnum estado in estadoEmpresaEnum)
            //    {
            //        hql.Append(" e.EstadoEmpresa=:estado" + Convert.ToInt16(estado));
            //        hql.Append(" OR ");
            //    }
            //    hql.Remove(hql.Length - 4, 4);
            //    hql.Append(")");
            //}

            //IQuery query = Session.CreateQuery(hql.ToString());

            //if (!String.IsNullOrEmpty(codigoEmpresa))
            //{
            //    query.SetString("codigoEmpresa", codigoEmpresa);
            //}
            //if (!String.IsNullOrEmpty(nombreEmpresa))
            //{
            //    query.SetString("nombreEmpresa", nombreEmpresa);
            //}
            //if (idLocalidad.HasValue)
            //{
            //    query.SetInt32("idLocalidad", idLocalidad.Value);
            //}
            //if (idDepartamentoProvincial.HasValue)
            //{
            //    query.SetInt32("idDepartamentoProvincial", idDepartamentoProvincial.Value);
            //}
            //if (!String.IsNullOrEmpty(barrio))
            //{
            //    query.SetString("barrio", barrio);
            //}
            //if (!String.IsNullOrEmpty(calle))
            //{
            //    query.SetString("calle", calle);
            //}
            //if (!String.IsNullOrEmpty(altura))
            //{
            //    query.SetString("altura", altura);
            //}
            //if (estadoEmpresaEnum.Count > 0)
            //{
            //    foreach (EstadoEmpresaEnum estado in estadoEmpresaEnum)
            //    {
            //        query.SetString("estado" + Convert.ToInt16(estado), estado.ToString());
            //    }
            //}
            //List<EmpresaBase> lista = (List<EmpresaBase>)query.SetInt16("EscuelaMadre", (int)TipoEmpresaEnum.ESCUELA_MADRE).SetInt16("EscuelaAnexo", (int)TipoEmpresaEnum.ESCUELA_ANEXO).SetCacheable(true).List<EmpresaBase>();
            //return lista;
        }

        /// <summary>
        /// Obtiene una lista de empresas segun filtro avanzado
        /// </summary>
        /// <param name="fechaAltaDesde">Fecha de alta (desde)</param>
        /// <param name="fechaAltaHasta">Fecha de alta (hasta)</param>
        /// <param name="fechaInicioActividadDesde">Fecha de inicio de actividades (desde)</param>
        /// <param name="fechaInicioActividadHasta">Fecha de inicio de actividades (hasta)</param>
        /// <param name="TipoEmpresaEnum">Tipo de empresa</param>
        /// <param name="idProgramaPresupuestario">Programa presupuestario</param>
        /// <param name="estadoEmpresaEnum">Estado de la empresa</param>
        /// <param name="idDepartamentoProvincial">Pertenece al domicilio</param>
        /// <param name="idLocalidad">Pertenece al domicilio</param>
        /// <param name="barrio">Pertenece al domicilio</param>
        /// <param name="calle">Pertenece al domicilio</param>
        /// <param name="altura">Pertenece al domicilio</param>
        /// <returns>Devuelve una lista de empresas que cumplan con los parametros de busqueda</returns>
        public List<EmpresaBase> GetByFiltroAvanzado(DateTime? fechaAltaDesde, DateTime? fechaAltaHasta, DateTime? fechaInicioActividadDesde, DateTime? fechaInicioActividadHasta, TipoEmpresaEnum TipoEmpresaEnum, int? idProgramaPresupuestario, List<EstadoEmpresaEnum> estadoEmpresaEnum, int? idDepartamentoProvincial, int? idLocalidad, string barrio, string calle, string altura)
        {
            StringBuilder hql = new StringBuilder("FROM Empresa e WHERE e.TipoEmpresa!=:EscuelaMadre AND e.TipoEmpresa!=:EscuelaAnexo AND e.TipoEmpresa!=:Inspeccion");
            
            if (fechaAltaDesde.HasValue)
            {
                hql.Append(" AND e.FechaAlta>=:fechaAltaDesde");
            }
            if (fechaAltaHasta.HasValue)
            {
                hql.Append(" AND e.FechaAlta<=:fechaAltaHasta");
            }
            if (fechaInicioActividadDesde.HasValue)
            {
                hql.Append(" AND e.FechaInicioActividad>=:fechaInicioActividadDesde");
            }
            if (fechaInicioActividadHasta.HasValue)
            {
                hql.Append(" AND e.FechaInicioActividad<=:fechaInicioActividadHasta");
            }
            hql.Append(" AND e.TipoEmpresa=:idTipoEmpresa");

            if (idProgramaPresupuestario.HasValue)
            {
                hql.Append(" AND e.ProgramaPresupuestario.Id=:idProgramaPresupuestario");
            }
            if (estadoEmpresaEnum != null && estadoEmpresaEnum.Count > 0)
            {
                hql.Append(" AND (");
                foreach (EstadoEmpresaEnum estado in estadoEmpresaEnum)
                {
                    hql.Append(" e.EstadoEmpresa=:estado" + Convert.ToInt16(estado));
                    hql.Append(" OR ");
                }
                hql.Remove(hql.Length - 4, 4);
                hql.Append(")");
            }
            if (idLocalidad.HasValue)
            {
                hql.Append(" AND e.Domicilio.Localidad.Id=:idLocalidad");
            }
            if (idDepartamentoProvincial.HasValue)
            {
                hql.Append(" AND e.Domicilio.Localidad.DepartamentoProvincial.Id=:idDepartamentoProvincial");
            }
            if (!String.IsNullOrEmpty(barrio))
            {
                hql.Append(" AND e.Domicilio.Barrio LIKE :barrio");
            }
            if (!String.IsNullOrEmpty(calle))
            {
                hql.Append(" AND e.Domicilio.Calle LIKE :calle");
            }
            if (!String.IsNullOrEmpty(altura))
            {
                hql.Append(" AND e.Domicilio.Altura LIKE :altura");
            }
            
            IQuery query = Session.CreateQuery(hql.ToString());

            if (fechaAltaDesde.HasValue)
            {
                query.SetDateTime("fechaAltaDesde", fechaAltaDesde.Value);
            }
            if (fechaAltaHasta.HasValue)
            {
                query.SetDateTime("fechaAltaHasta", fechaAltaHasta.Value);
            }
            if (fechaInicioActividadDesde.HasValue)
            {
                query.SetDateTime("fechaInicioActividadDesde", fechaInicioActividadDesde.Value);
            }
            if (fechaInicioActividadHasta.HasValue)
            {
                query.SetDateTime("fechaInicioActividadHasta", fechaInicioActividadHasta.Value);
            }

            query.SetInt32("idTipoEmpresa", Convert.ToInt16(TipoEmpresaEnum));

            if (idProgramaPresupuestario.HasValue)
            {
                query.SetInt32("idProgramaPresupuestario", idProgramaPresupuestario.Value);
            }
            if (idLocalidad.HasValue)
            {
                query.SetInt32("idLocalidad", idLocalidad.Value);
            }
            if (idDepartamentoProvincial.HasValue)
            {
                query.SetInt32("idDepartamentoProvincial", idDepartamentoProvincial.Value);
            }
            if (!String.IsNullOrEmpty(barrio))
            {
                query.SetString("barrio", barrio);
            }
            if (!String.IsNullOrEmpty(calle))
            {
                query.SetString("calle", calle);
            }
            if (!String.IsNullOrEmpty(altura))
            {
                query.SetString("altura", altura);
            }
            if (estadoEmpresaEnum != null && estadoEmpresaEnum.Count > 0)
            {
                foreach (EstadoEmpresaEnum estado in estadoEmpresaEnum)
                {
                    query.SetString("estado" + Convert.ToInt16(estado), estado.ToString());
                }
            }
            return (List<EmpresaBase>)query.SetCacheable(true).SetInt16("EscuelaMadre", (int)TipoEmpresaEnum.ESCUELA_MADRE).SetInt16("EscuelaAnexo", (int)TipoEmpresaEnum.ESCUELA_ANEXO).SetInt16("Inspeccion", (int)TipoEmpresaEnum.INSPECCION).List<EmpresaBase>();
        }

        //public List<Empresa> GetByFiltroAvanzadoEscuela(TipoEmpresaEnum TipoEmpresaEnum, int? numeroEscuela, TipoEscuelaEnum? tipoEscuelaEnum, CategoriaEscuelaEnum? categoriaEscuelaEnum, TipoEducacionEnum? tipoEducacionEnum, int? nivelEducativoEnum, DependenciaEnum? dependenciaEnum, AmbitoEscuelaEnum? ambitoEscuelaEnum, bool? esReligioso, bool? esArancelado, TipoInspeccionEnum? tipoInspeccionEnum, int? idDepartamentoProvincial, int? idLocalidad, string barrio, string calle, string altura,List<EstadoEmpresaEnum> estadoEmpresaEnum)
        //{
        //    StringBuilder hql = new StringBuilder("FROM Escuela e WHERE e.Id>0");
        //    hql.Append(" AND e.TipoEmpresa=:idTipoEmpresa");
        //    if (numeroEscuela.HasValue)
        //    {
        //        hql.Append(" AND e.NumeroEscuela=:numeroEscuela");
        //    }
        //    if (tipoEscuelaEnum != null)
        //    {
        //        hql.Append(" AND e.TipoEscuela=:tipoEscuela");
        //    }
        //    if (categoriaEscuelaEnum != null)
        //    {
        //        hql.Append(" AND e.TipoCategoria=:tipoCategoria");
        //    }
        //    if (tipoEducacionEnum != null)
        //    {
        //        hql.Append(" AND e.TipoEducacion=:tipoEducacion");
        //    }
        //    if (nivelEducativoEnum != null)
        //    {
        //        hql.Append(" AND e.Nivel=:nivel");
        //    }
        //    if (dependenciaEnum != null)
        //    {
        //        hql.Append(" AND e.Dependencia=:dependencia");
        //    }
        //    if (ambitoEscuelaEnum != null)
        //    {
        //        hql.Append(" AND e.Ambito=:ambito");
        //    }
        //    if (esReligioso.HasValue)
        //    {
        //        hql.Append(" AND e.Religioso=:esReligioso");
        //    }
        //    if (esArancelado.HasValue)
        //    {
        //        hql.Append(" AND e.Arancelado=:esArancelado");
        //    }
        //    if (tipoInspeccionEnum != null)
        //    {
        //        hql.Append(" AND e.TipoInspeccion=:tipoInspeccion");
        //    }
        //    if (idLocalidad.HasValue)
        //    {
        //        hql.Append(" AND e.Domicilio.Localidad.Id=:idLocalidad");
        //    }
        //    if (idDepartamentoProvincial.HasValue)
        //    {
        //        hql.Append(" AND e.Domicilio.Localidad.DepartamentoProvincial.Id=:idDepartamentoProvincial");
        //    }
        //    if (!String.IsNullOrEmpty(barrio))
        //    {
        //        hql.Append(" AND e.Domicilio.Barrio LIKE :barrio");
        //    }
        //    if (!String.IsNullOrEmpty(calle))
        //    {
        //        hql.Append(" AND e.Domicilio.Calle LIKE :calle");
        //    }
        //    if (!String.IsNullOrEmpty(altura))
        //    {
        //        hql.Append(" AND e.Domicilio.Altura LIKE :altura");
        //    }
        //    if (estadoEmpresaEnum != null && estadoEmpresaEnum.Count > 0)
        //    {
        //        hql.Append(" AND (");
        //        foreach (EstadoEmpresaEnum estado in estadoEmpresaEnum)
        //        {
        //            hql.Append(" e.EstadoEmpresa=:estado" + Convert.ToInt16(estado));
        //            hql.Append(" OR ");
        //        }
        //        hql.Remove(hql.Length - 4, 4);
        //        hql.Append(")");
        //    }
        //    IQuery query = Session.CreateQuery(hql.ToString());
        //    query.SetInt32("idTipoEmpresa", Convert.ToInt16(TipoEmpresaEnum));
        //    if (numeroEscuela.HasValue)
        //    {
        //        query.SetInt32("numeroEscuela", numeroEscuela.Value);
        //    }
        //    if (tipoEscuelaEnum != null)
        //    {
        //        query.SetString("tipoEscuela", tipoEscuelaEnum.ToString());
        //    }
        //    if (categoriaEscuelaEnum != null)
        //    {
        //        query.SetString("tipoCategoria", categoriaEscuelaEnum.ToString());
        //    }
        //    if (tipoEducacionEnum != null)
        //    {
        //        query.SetString("tipoEducacion", tipoInspeccionEnum.ToString());
        //    }
        //    if (nivelEducativoEnum != null)
        //    {
        //        query.SetString("nivel", nivelEducativoEnum.ToString());
        //    }
        //    if (dependenciaEnum != null)
        //    {
        //        query.SetString("dependencia", dependenciaEnum.ToString());
        //    }
        //    if (ambitoEscuelaEnum != null)
        //    {
        //        query.SetString("ambito", ambitoEscuelaEnum.ToString());
        //    }
        //    if (esReligioso.HasValue)
        //    {
        //        query.SetBoolean("esReligioso", esReligioso.Value);
        //    }
        //    if (esArancelado.HasValue)
        //    {
        //        query.SetBoolean("esArancelado", esArancelado.Value);
        //    }
        //    if (tipoInspeccionEnum != null)
        //    {
        //        query.SetString("tipoInspeccion", tipoInspeccionEnum.ToString());
        //    }
        //    if (idLocalidad.HasValue)
        //    {
        //        query.SetInt32("idLocalidad", idLocalidad.Value);
        //    }
        //    if (idDepartamentoProvincial.HasValue)
        //    {
        //        query.SetInt32("idDepartamentoProvincial", idDepartamentoProvincial.Value);
        //    }
        //    if (!String.IsNullOrEmpty(barrio))
        //    {
        //        query.SetString("barrio", barrio);
        //    }
        //    if (!String.IsNullOrEmpty(calle))
        //    {
        //        query.SetString("calle", calle);
        //    }
        //    if (!String.IsNullOrEmpty(altura))
        //    {
        //        query.SetString("altura", altura);
        //    }
        //    if (estadoEmpresaEnum.Count > 0)
        //    {
        //        foreach (EstadoEmpresaEnum estado in estadoEmpresaEnum)
        //        {
        //            query.SetString("estado" + Convert.ToInt16(estado), estado.ToString());
        //        }
        //    }
        //    return (List<Empresa>)query.SetCacheable(true).List<Empresa>();
        //}

        #endregion        
    }
}

