using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using NHibernate.Criterion;
using NHibernate.Transform;
using Siage.Base.Dto;
using Siage.Core.Domain;
using Siage.Core.DaoInterfaces;
using Siage.Base;
using NHibernate.Linq;
using Siage.Base.Dto;

namespace Siage.Data.DAO
{
    public class DaoEmpresaBase : DaoBase<EmpresaBase, int>, IDaoEmpresaBase
    {
        public DaoEmpresaBase()
        {

        }

        public new EmpresaBase GetById(int id)
        {
            var empresa = base.GetById(id);
            if (empresa != null)
                empresa.Domicilio = new DaoDomicilio().GetByEntidad(empresa.VinculoDomicilio, empresa.Id);

            return empresa;
        }

        #region IDaoEmpresa Members

        /// <summary>
        /// Obtiene una lista de empresas segun filtro básico
        /// </summary>
        /// <param name="codigoEmpresa">Codigo de empresa</param>
        /// <param name="nombreEmpresa">Nombre de empresa</param>
        /// <param name="idLocalidad">Pertenece al domicilio</param>
        /// <param name="barrio">Pertenece al domicilio</param>
        /// <param name="calle">Pertenece al domicilio</param>
        /// <param name="altura">Pertenece al domicilio</param>
        /// <param name="estadoEmpresaEnum">Estado de la empresa</param>
        /// <returns>Devuelve una lista de empresas que cumplan con los parametros de busqueda. </returns>
        public List<EmpresaBase> GetByFiltrosBasico(string codigoEmpresa, string nombreEmpresa, int? idLocalidad, string barrio, string calle, int? altura, List<EstadoEmpresaEnum> estadoEmpresaEnum, List<TipoEmpresaFiltroBusquedaEnum> empresaFiltro, int? idDireccionDeNivelActual, int? idEmpresaUsuarioLogueado)
        {
            var listaTipoEmpresaPermitidos = new List<TipoEmpresaEnum>();
            var empresa = new EmpresaBase();
            var domicilio = new Domicilio();
            var direccionDeNivel = new DireccionDeNivel();
            var empresaRegistro = new EmpresaBase();
            var query = Session.QueryOver<EmpresaBase>(() => empresa);
            query.AndNot(
                x =>
                x.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE || x.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO);

            if (idEmpresaUsuarioLogueado.HasValue){
            query.JoinQueryOver(x => x.EmpresaRegistro, () => empresaRegistro);
            query.And(x => empresaRegistro.Id == idEmpresaUsuarioLogueado);
            }
            if (empresaFiltro != null && empresaFiltro.Count > 0)
            {
                if (!empresaFiltro.Contains(TipoEmpresaFiltroBusquedaEnum.TODAS))
                {
                    if (empresaFiltro.Contains(TipoEmpresaFiltroBusquedaEnum.MINISTERIO))
                        listaTipoEmpresaPermitidos.Add(TipoEmpresaEnum.MINISTERIO);
                    if (empresaFiltro.Contains(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_NIVEL))
                        listaTipoEmpresaPermitidos.Add(TipoEmpresaEnum.DIRECCION_DE_NIVEL);
                    if (empresaFiltro.Contains(TipoEmpresaFiltroBusquedaEnum.ESCUELA_MADRE))
                        listaTipoEmpresaPermitidos.Add(TipoEmpresaEnum.ESCUELA_MADRE);
                    if (empresaFiltro.Contains(TipoEmpresaFiltroBusquedaEnum.ESCUELA_ANEXO))
                        listaTipoEmpresaPermitidos.Add(TipoEmpresaEnum.ESCUELA_ANEXO);
                    if (empresaFiltro.Contains(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_INFRAESTRUCTURA))
                        listaTipoEmpresaPermitidos.Add(TipoEmpresaEnum.DIRECCION_DE_INFRAESTRUCTURA);
                    if (empresaFiltro.Contains(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_RECURSOS_HUMANOS))
                        listaTipoEmpresaPermitidos.Add(TipoEmpresaEnum.DIRECCION_DE_RECURSOS_HUMANOS);
                    if (empresaFiltro.Contains(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_SISTEMAS))
                        listaTipoEmpresaPermitidos.Add(TipoEmpresaEnum.DIRECCION_DE_SISTEMAS);
                    if (empresaFiltro.Contains(TipoEmpresaFiltroBusquedaEnum.DIRECCION_DE_TESORERIA))
                        listaTipoEmpresaPermitidos.Add(TipoEmpresaEnum.DIRECCION_DE_TESORERIA);
                    if (empresaFiltro.Contains(TipoEmpresaFiltroBusquedaEnum.SECRETARIA))
                        listaTipoEmpresaPermitidos.Add(TipoEmpresaEnum.SECRETARIA);

                    

                    query.And(x => x.TipoEmpresa.IsIn(listaTipoEmpresaPermitidos));
                }
            }

            if (!string.IsNullOrEmpty(nombreEmpresa))
                query.And(x => x.Nombre.IsLike(nombreEmpresa));

            if (!string.IsNullOrEmpty(codigoEmpresa))
                query.And(x => x.CodigoEmpresa.IsLike(codigoEmpresa));
            if (idDireccionDeNivelActual.HasValue)
            {
                query.JoinQueryOver<DireccionDeNivel>(x => x.TipoDireccionNivel, () => direccionDeNivel);
                query.And(() => direccionDeNivel.Id == idDireccionDeNivelActual);
            }

            var empresaBases = (List<EmpresaBase>)query.List<EmpresaBase>();

            //TODO mejorar el filtrado por domicilio; hasta ahora se obtiene el listado de empresa y se filtra recien ahi (pesimo!)
            if (idLocalidad.HasValue || !string.IsNullOrEmpty(barrio) || !string.IsNullOrEmpty(calle) || altura.HasValue)
            {
                barrio = !string.IsNullOrEmpty(barrio) ? barrio.ToUpper() : string.Empty;
                calle = !string.IsNullOrEmpty(calle) ? calle.ToUpper() : string.Empty;

                foreach (EmpresaBase emp in empresaBases)
                {
                    emp.Domicilio = new DaoDomicilio().GetByEntidad(emp.VinculoDomicilio, emp.Id);
                }
                empresaBases = (List<EmpresaBase>)empresaBases.Where(x => x.Domicilio.Localidad.Id == idLocalidad &&
                                                               (string.IsNullOrEmpty(barrio) ||
                                                                x.Domicilio.Barrio.Nombre.Contains(barrio)) &&
                                                               (string.IsNullOrEmpty(calle) ||
                                                                x.Domicilio.Calle.Nombre.Contains(calle))
                                                               && (!altura.HasValue || x.Domicilio.Altura == altura))
                                               .ToList();
            }

            if (estadoEmpresaEnum != null && estadoEmpresaEnum.Count > 0)
                return empresaBases.Where(x => estadoEmpresaEnum.Any(p => p == x.EstadoEmpresa)).ToList();

            return empresaBases;
        }
       
        public DtoTipoEmpresa GetTipoEmpresaById(int idEmpresa)
        {
            DtoTipoEmpresa tipoEmpresa = null;
            var query = Session.QueryOver<EmpresaBase>();
            query.And(x => x.Id == idEmpresa);
            return query.SelectList(list => list
                 .Select(x => x.Id).WithAlias(() => tipoEmpresa.Id)
                 .Select(x => x.TipoEmpresa).WithAlias(() => tipoEmpresa.TipoEmpresa))
                 .TransformUsing(Transformers.AliasToBean<DtoTipoEmpresa>()).SingleOrDefault<DtoTipoEmpresa>();
        }
       public DtoEstadoEmpresa GetEstadoByid(int idEscuela)
        {
            DtoEstadoEmpresa estadoEmpresa = null;
            var query = Session.QueryOver<EmpresaBase>();
            query.And(x => x.Id == idEscuela);
            return query.SelectList(list => list
                 .Select(x => x.Id).WithAlias(() => estadoEmpresa.Id)
                 .Select(x => x.Estado).WithAlias(() => estadoEmpresa.Estado))
                 .TransformUsing(Transformers.AliasToBean<DtoEstadoEmpresa>()).SingleOrDefault<DtoEstadoEmpresa>();
        }


       public List<DtoEmpresaInspeccionadaPorInspeccion> GetEmpresasInspeccionadasPorInspeccionId(int idInspeccion)
       {

           var query = Session.CreateSQLQuery(@"select 
    empresa.id_empresa AS CodigoEmpresa,
    empresa.n_empresa AS NombreEmpresa,
    tipoEmpresa.n_tipo_empresa AS TipoEmpresa,
    tipoInspeccion.n_tipo_inspeccion AS TipoInspeccion,
    estadoEmpresa.n_estado AS EstadoEmpresa,
    nivelEducativo.n_nivel_educativo AS NivelEducativo,
    domicilio.LOCALIDAD AS Localidad,
    domicilio.DEPARTAMENTO AS Departamento,
    estadoAsignacion.n_easig_insp_esc AS EstadoAsignacion,
    supervisor.id_empresa As NombreInspeccion

    from t_em_empresas empresa
    inner join t_em_tempresa tipoEmpresa on empresa.id_seq_tipo_empresa = tipoEmpresa.id_seq_tipo_empresa
    inner join t_em_estados estadoEmpresa on empresa.id_seq_estado = estadoEmpresa.id_seq_estado
    left join t_em_tinspeccion tipoInspeccion on empresa.id_seq_tipo_inspeccion = tipoInspeccion.id_seq_tipo_inspeccion
     
    left join t_em_empresas supervisor on empresa.ID_SEQ_INSPECCION_SUPERIOR = supervisor.id_seq_empresa
       
    left join T_EM_ASIG_INSPECCION_ESC asignacion on empresa.id_seq_empresa = asignacion.id_seq_empresa_esc
    left join t_em_empresas supervisorEscuela on asignacion.id_seq_empresa_insp = supervisorEscuela.id_seq_empresa
    left join t_em_easig_insp_esc estadoAsignacion on asignacion.id_seq_easig_insp_esc = estadoAsignacion.id_seq_easig_insp_esc
  
    left join T_EM_EMPR_X_TED_NIV tipoEducacion on empresa.id_seq_empresa = tipoEducacion.id_seq_empresa
    left join T_EM_TEDUC_X_NIV_EDUCAT nivelTipoEducacion on tipoEducacion.id_seq_teduc_x_niv_educat = nivelTipoEducacion.id_seq_teduc_x_niv_educat
    left join T_EM_NIVELES_EDUCATIVO nivelEducativo on nivelTipoEducacion.id_seq_nivel_educativo = nivelEducativo.id_seq_nivel_educativo
    
    left join DOM_MANAGER.DOM_VINCULACION domicilio on empresa.id_vin = domicilio.id_vin
  
where 
    ((supervisor.id_seq_empresa = :idInspeccion AND supervisor.id_seq_tipo_inspeccion = 1 AND (empresa.id_seq_tipo_inspeccion = 2 OR empresa.id_seq_tipo_inspeccion = 3)) OR    
    (supervisor.id_seq_empresa = :idInspeccion AND supervisor.id_seq_tipo_inspeccion = 3 AND empresa.id_seq_tipo_inspeccion = 2) OR     
    (supervisorEscuela.id_seq_tipo_inspeccion = 2 AND supervisorEscuela.id_seq_empresa = :idInspeccion))");

           query.SetInt32("idInspeccion", idInspeccion);

           var resultado = query.SetResultTransformer(Transformers.AliasToBean<DtoEmpresaInspeccionadaPorInspeccion>()).List<DtoEmpresaInspeccionadaPorInspeccion>();
           
           return (List<DtoEmpresaInspeccionadaPorInspeccion>)resultado;

       }

        /// <summary>
       /// Método que valida la fecha de inicio de actividades contra los puestos de trabajo 
       /// </summary>
       /// <param name="empresa">La empresa a validar</param>
       /// <returns>True si la fecha es válida, false si no lo es </returns>
        public bool FechaInicioActividadesValidaPuestoTrabajo(EmpresaBase empresa)
       {
           EmpresaBase empr = null;
            var query = Session.QueryOver<PuestoDeTrabajo>();
            query.JoinQueryOver<EmpresaBase>(x => x.Empresas, () => empr);
            query.And(x => empr.Id == empresa.Id)
                .And(x => x.FechaInicioPuestoTrabajo < empresa.FechaInicioActividad);

            var cantidad = query.RowCount();
            return cantidad == 0;
        }
            
       /// <summary>
        /// Método que valida la fecha de inicio de actividades contra las asignaciones a planes de estudio si es escuela
       /// </summary>
       /// <param name="empresa">La empresa a validar</param>
       /// <returns>True si la fecha es válida, false si no lo es </returns>
        public bool FechaInicioActividadesValidaPlanDeEstudio(EmpresaBase empresa)
        {
            var query = Session.QueryOver<EscuelaPlan>();
            query.And(x => x.Escuela.Id == empresa.Id)
                .And(x => x.FechaAsignacion < empresa.FechaInicioActividad);

            var asignaciones = query.List();
            return asignaciones.Count == 0;
        }

        public List<EmpresaBase> GetEmpresasDependientes(EmpresaBase entidad)
        {
            var empresa = (from m in Session.Query<EmpresaBase>()
                           where m.EmpresaPadreOrganigrama.CodigoEmpresa == entidad.CodigoEmpresa
                           select m).ToList();
            return empresa;
        }

        /// <summary>
        /// Busca en base al id de una empresa, busca las empresas dependientes
        /// </summary>
        /// <param name="idEmpresaPadre">id empresa padre</param>
        /// <param name="codigoEmpresa">codigo empresa padre</param>
        /// <param name="nombreEmpresa">nombre empresa padre</param>
        /// <returns>Lita de empresas dependientes</returns>
        
        public List<EmpresaBase> GetEmpresasDependientes(int idEmpresaPadre, string codigoEmpresa, string nombreEmpresa)
        {
            codigoEmpresa = codigoEmpresa.ToUpper();
            nombreEmpresa = nombreEmpresa.ToUpper();
            string sql = @"
select * from t_em_empresas
where ID_EMPRESA like :codigo || '%' and N_EMPRESA like :nombre || '%'  and id_seq_empresa <> :idEmpresa
start with id_seq_empresa = :idEmpresa
connect by prior id_seq_empresa = id_seq_empresa_padre
";
            var query = Session.CreateSQLQuery(sql).AddEntity(typeof(EmpresaBase))
                .SetInt32("idEmpresa", idEmpresaPadre)
                .SetString("codigo", codigoEmpresa)
                .SetString("nombre", nombreEmpresa);

            return (List<EmpresaBase>)query.List<EmpresaBase>();
        }

        /// <summary>
        /// Obtiene una lista de empresas segun filtro avanzado
        /// </summary>
        /// <param name="fechaAltaDesde">Fecha de alta (desde)</param>
        /// <param name="fechaAltaHasta">Fecha de alta (hasta)</param>
        /// <param name="fechaInicioActividadDesde">Fecha de inicio de actividades (desde)</param>
        /// <param name="fechaInicioActividadHasta">Fecha de inicio de actividades (hasta)</param>
        /// <param name="tipoEmpresaEnum">Tipo de empresa</param>
        /// <param name="idProgramaPresupuestario">Programa presupuestario</param>
        /// <param name="estadoEmpresaEnum">Estado de la empresa</param>
        /// <param name="idLocalidad">Pertenece al domicilio</param>
        /// <param name="barrio">Pertenece al domicilio</param>
        /// <param name="calle">Pertenece al domicilio</param>
        /// <param name="altura">Pertenece al domicilio</param>
        /// <returns>Devuelve una lista de empresas que cumplan con los parametros de busqueda</returns>
        public List<EmpresaBase> GetByFiltroAvanzado(DateTime? fechaAltaDesde, DateTime? fechaAltaHasta, DateTime? fechaInicioActividadDesde, DateTime? fechaInicioActividadHasta, TipoEmpresaEnum? tipoEmpresaEnum, int? idProgramaPresupuestario, List<EstadoEmpresaEnum> estadoEmpresaEnum, int? idLocalidad, string barrio, string calle, int? altura, string nombre, DateTime? fechaDesdeNotificacion, DateTime? fechaHastaNotificacion, int? idEmpresaUsuarioLogueado)
        {
            var empresa = new EmpresaBase();

            var programaPresupuestario = new ProgramaPresupuestario();
            var query = Session.QueryOver<EmpresaBase>(() => empresa);
            query.And(
                x =>
                x.TipoEmpresa != TipoEmpresaEnum.ESCUELA_MADRE && x.TipoEmpresa != TipoEmpresaEnum.ESCUELA_ANEXO &&
                x.TipoEmpresa != TipoEmpresaEnum.INSPECCION &&
                x.TipoEmpresa != TipoEmpresaEnum.DIRECCION_DE_NIVEL);

            if(idEmpresaUsuarioLogueado.HasValue){
            var empresaRegistro = new EmpresaBase();
            query.JoinQueryOver(x => x.EmpresaRegistro, () => empresaRegistro);
            query.And(x => empresaRegistro.Id == idEmpresaUsuarioLogueado);
                }
            if (!string.IsNullOrEmpty(nombre))
                query.And(x => x.Nombre.IsLike(nombre));
            if (fechaDesdeNotificacion.HasValue)
                query.And(x => x.FechaNotificacion >= fechaDesdeNotificacion);
            if (fechaHastaNotificacion.HasValue)
                query.And(x => x.FechaNotificacion <= fechaHastaNotificacion);
            if (fechaAltaDesde.HasValue)
                query.And(x => x.FechaAlta >= fechaAltaDesde);
            if (fechaAltaHasta.HasValue)
                query.And(x => x.FechaAlta <= fechaAltaHasta);
            if (fechaInicioActividadDesde.HasValue)
                query.And(x => x.FechaInicioActividad >= fechaInicioActividadDesde);
            if (fechaInicioActividadHasta.HasValue)
                query.And(x => x.FechaInicioActividad <= fechaInicioActividadHasta);
            if (tipoEmpresaEnum.HasValue)
                query.And(x => x.TipoEmpresa == tipoEmpresaEnum);
            if (idProgramaPresupuestario.HasValue)
            {
                query.JoinQueryOver<ProgramaPresupuestario>(x => x.ProgramaPresupuestario, () => programaPresupuestario);
                query.Where(() => programaPresupuestario.Id == idProgramaPresupuestario);
            }

            var empresaBases = (List<EmpresaBase>)query.List<EmpresaBase>();



            if (idLocalidad.HasValue || !string.IsNullOrEmpty(barrio) || !string.IsNullOrEmpty(calle) ||
                altura.HasValue)
            {
                barrio = barrio != string.Empty ? barrio.ToUpper() : string.Empty;
                calle = calle != string.Empty ? calle.ToUpper() : string.Empty;

                foreach (var emp in empresaBases)
                {
                    emp.Domicilio = new DaoDomicilio().GetByEntidad(empresa.VinculoDomicilio, empresa.Id);
                }
                empresaBases = (List<EmpresaBase>)empresaBases.Where(x => x.Domicilio.Localidad.Id == idLocalidad &&
                    x.Domicilio.Barrio.Nombre.Contains(barrio) && x.Domicilio.Calle.Nombre.Contains(calle)
                    && x.Domicilio.Altura == altura);

            }


            if (estadoEmpresaEnum != null && estadoEmpresaEnum.Count > 0)
                return empresaBases.Where(x => estadoEmpresaEnum.Any(p => p == x.EstadoEmpresa)).ToList();

            return empresaBases;
        }

        //TODO AGUSTIN: verificar metodo
        public List<string> GetCamposConInstrumentoLegal()
        {
            return new List<string> { "Nombre", "TipoEmpresa", "TipoGestion", "Observaciones" };
        }

        public void CambiarTipoEmpresa(int idEmpresa, TipoEmpresaEnum tipoEmpresaNueva, string seters)
        {
            var queryString = new StringBuilder("UPDATE T_EM_EMPRESAS\n");

            queryString.AppendLine(" SET ID_SEQ_TIPO_EMPRESA = " + ((int)tipoEmpresaNueva) + ", ");
            queryString.AppendLine(seters);
            queryString.AppendLine("WHERE ID_SEQ_EMPRESA = " + idEmpresa);

            var query = Session.CreateSQLQuery(queryString.ToString());
            query.ExecuteUpdate();
        }

        /// <summary>
        /// Valida que no se graban empresas con nombre repetido
        /// </summary>
        /// <param name="codigoEmpresa">El nombre a validar</param>
        /// <param name="id">es el id de la empresaBase a persistir en la base</param>
        /// <returns></returns>
        public bool NombreRepetido(string nombre, int id)
        {
            var functionResultValue = false;
            var query = Session.QueryOver<EmpresaBase>();
            query.And(x => x.Nombre == nombre);

            var empresas = query.List<EmpresaBase>();

            foreach (var empresa in empresas)
            {
                if (id != 0 ? id != empresa.Id && empresa.EstadoEmpresa != EstadoEmpresaEnum.RECHAZADA
                                              : empresa.EstadoEmpresa != EstadoEmpresaEnum.RECHAZADA)
                {
                    return true;
                }
            }

            return functionResultValue;
        }

        /// <summary>
        /// Valida que no se graban empresas con codigo repetido
        /// </summary>
        /// <param name="nombre">El codigo a validar</param>
        /// <param name="id">es el id de la empresaBase a persistir en la base</param>
        /// <returns></returns>
        public bool CodigoRepetido(string codigoEmpresa, int id)
        {
            var functionResultValue = false;
            var query = Session.QueryOver<EmpresaBase>();
            query.And(x => x.CodigoEmpresa == codigoEmpresa);
            var empresa = query.SingleOrDefault<EmpresaBase>();
            if (empresa != null)
                functionResultValue = id != 0
                                          ? id != empresa.Id && empresa.EstadoEmpresa != EstadoEmpresaEnum.RECHAZADA
                                          : empresa.EstadoEmpresa != EstadoEmpresaEnum.RECHAZADA;
            return functionResultValue;
        }

        public bool ValidarEstructuraDefinitiva(int escuelaId)
        {
            var queryString = new StringBuilder(" SELECT COUNT (EM.ID_SEQ_EMPRESA) ");

            queryString.AppendLine(" FROM T_EM_EMPRESAS EM ");
            queryString.AppendLine(" JOIN T_EM_TEMPRESA TE ON TE.ID_SEQ_TIPO_EMPRESA = EM.ID_SEQ_TIPO_EMPRESA ");
            queryString.AppendLine(" WHERE EM.ESTRUCTURA_DEFINITIVA = 'Y' ");
            queryString.AppendLine(" AND (TE.ID_SEQ_TIPO_EMPRESA = 11 OR TE.ID_SEQ_TIPO_EMPRESA = 12) ");
            queryString.AppendLine(" AND EM.ID_SEQ_EMPRESA = " + escuelaId);

            var query = Session.CreateSQLQuery(queryString.ToString());
            var resultado = Convert.ToInt32(query.UniqueResult());
            
            return resultado > 0 ? true : false;
        }

        public List<VinculoEmpresaEdificio> GetVinculosCompletos(int idEmpresa)
        {
            var daoProvider = new DaoProvider();
            var empresa = GetById(idEmpresa);
            foreach (var vin in empresa.VinculoEmpresaEdificio)
            {
                vin.Edificio = daoProvider.GetDaoEdificio().GetById(vin.Edificio.Id);
            }
            return empresa.VinculoEmpresaEdificio.ToList();
        }

        public bool EsEscuela(int idEmpresa)
        {
            var query = Session.QueryOver<EmpresaBase>();
            query.And(x => x.Id == idEmpresa);
            query.And(x => x.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE || x.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO);
            var resultado = query.SingleOrDefault();
            return resultado != null ? true : false;
        }
        public bool EsDireccionDeNivel(int idEmpresa)
        {
            var query = Session.QueryOver<EmpresaBase>();
            query.And(x => x.Id == idEmpresa);
            query.And(x => x.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL);
            var resultado = query.SingleOrDefault();
            return resultado != null ? true : false;
        }
        #endregion

        #region Override

        public override EmpresaBase Save(EmpresaBase entity)
        {
            Session.SaveOrUpdate(entity);
            entity = Session.Get<EmpresaBase>(entity.Id);
            return entity;
        }

        public override EmpresaBase SaveOrUpdate(EmpresaBase entity)
        {
            Session.SaveOrUpdate(entity);
            entity = Session.Get<EmpresaBase>(entity.Id);
            return entity;
        }

        #endregion

        public List<DtoEmpresaConsultaEdificio> GetEmpresasByEdificios(List<int> listaEdificios)
        {
            //todo javier revisar el futuro cercano
            var res = new List<DtoEmpresaConsultaEdificio>();
            Edificio edificio = null;
            VinculoEmpresaEdificio vinculos = null;
            var empresa = new DtoEmpresaConsultaEdificio();

            var query1 = Session.QueryOver<EmpresaBase>()
                .And(x => x.TipoEmpresa != TipoEmpresaEnum.ESCUELA_MADRE && x.TipoEmpresa != TipoEmpresaEnum.ESCUELA_ANEXO)
                .JoinQueryOver<VinculoEmpresaEdificio>(x => x.VinculoEmpresaEdificio, () => vinculos)
                
                .And(() => vinculos.Edificio.Id.IsIn(listaEdificios))
                .SelectList(list => list
                                        .Select(x => x.Id).WithAlias(() => empresa.Id)
                                        .Select(x => x.Estado).WithAlias(() => empresa.EstadoEmpresa)
                                        .Select(x => x.Nombre).WithAlias(() => empresa.Nombre)
                                        .Select(x => x.TipoEmpresa).WithAlias(() => empresa.TipoEmpresa)
                                        .Select(x => x.CodigoEmpresa).WithAlias(() => empresa.CodigoEmpresa))
                .TransformUsing(Transformers.AliasToBean<DtoEmpresaConsultaEdificio>());

            var query2 = Session.QueryOver<Escuela>()
                .JoinQueryOver<VinculoEmpresaEdificio>(x => x.VinculoEmpresaEdificio, () => vinculos)
                .And(() => vinculos.Edificio.Id.IsIn(listaEdificios))
                .SelectList(list => list
                                        .Select(x => x.Id).WithAlias(() => empresa.Id)
                                        .Select(x => x.Estado).WithAlias(() => empresa.EstadoEmpresa)
                                        .Select(x => x.Nombre).WithAlias(() => empresa.Nombre)
                                        .Select(x => x.TipoEmpresa).WithAlias(() => empresa.TipoEmpresa)
                                        .Select(x => x.CodigoEmpresa).WithAlias(() => empresa.CodigoEmpresa)
                                        .Select(x => x.CUE).WithAlias(() => empresa.CUE))
                .TransformUsing(Transformers.AliasToBean<DtoEmpresaConsultaEdificio>());

            var query3 = Session.QueryOver<EscuelaAnexo>()
                .JoinQueryOver<VinculoEmpresaEdificio>(x => x.VinculoEmpresaEdificio, () => vinculos)
                .And(() => vinculos.Edificio.Id.IsIn(listaEdificios))
                .SelectList(list => list
                                        .Select(x => x.Id).WithAlias(() => empresa.Id)
                                        .Select(x => x.Estado).WithAlias(() => empresa.EstadoEmpresa)
                                        .Select(x => x.Nombre).WithAlias(() => empresa.Nombre)
                                        .Select(x => x.TipoEmpresa).WithAlias(() => empresa.TipoEmpresa)
                                        .Select(x => x.CodigoEmpresa).WithAlias(() => empresa.CodigoEmpresa)
                                        .Select(x => x.CUE).WithAlias(() => empresa.CUE))
                .TransformUsing(Transformers.AliasToBean<DtoEmpresaConsultaEdificio>());

            res.AddRange((List<DtoEmpresaConsultaEdificio>)query1.List<DtoEmpresaConsultaEdificio>());
            res.AddRange((List<DtoEmpresaConsultaEdificio>)query2.List<DtoEmpresaConsultaEdificio>());
            res.AddRange((List<DtoEmpresaConsultaEdificio>)query3.List<DtoEmpresaConsultaEdificio>());

            return res.Distinct().ToList<DtoEmpresaConsultaEdificio>();
        }

        public bool EsDireccionDeNivelSuperior(int idEmpresa)
        {
            NivelEducativoPorTipoEducacion nete = null;
            var query = Session.QueryOver<DireccionDeNivel>();
            query.JoinQueryOver(x => x.NivelesEducativoXTE, () => nete);
            query.And(x => x.Id == idEmpresa);
            query.And(x => nete.NivelEducativo.Id == (int) NivelEducativoNombreEnum.SUPERIOR);
            return query.RowCount() > 0 ? true : false;   
        }

        public void ValidarCodigoInspeccionRepetido(int idEmpresa, string codigoInspeccion)
        {
            var query = Session.QueryOver<EscuelaAnexo>();
            query.And(x => x.CodigoInspeccion == codigoInspeccion);
            query.And(x => x.Id != idEmpresa);

            var query2 = Session.QueryOver<Escuela>();
            query2.And(x => x.CodigoInspeccion == codigoInspeccion);
            query2.And(x => x.Id != idEmpresa);

            if (query.List().Count > 0 || query2.List().Count > 0)
            {
                throw new BaseException("El código de inspección " + codigoInspeccion + " ya existe para otra escuela");
            }
        }

        public bool PoseeEmpresasHijasNoCerradasORechazadas(int id)
        {
            var query = Session.QueryOver<EmpresaBase>();
            query.And(x => x.EmpresaPadreOrganigrama.Id == id);
            query.And(x => x.Estado != EstadoEmpresaEnum.CERRADA);
            query.And(x => x.Estado != EstadoEmpresaEnum.RECHAZADA);

            return query.List().Count > 0;
        }
    }
}