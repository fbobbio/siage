using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Testing.Values;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Mapping;
using NHibernate.Transform;
using NHibernate.Type;
using Siage.Base.Dto;
using Siage.Core.Domain;
using Siage.Core.DaoInterfaces;
using Siage.Base;
using NHibernate.Linq;
using Property = NHibernate.Criterion.Property;


namespace Siage.Data.DAO
{
    public class DaoEscuela : DaoBase<Escuela, int>, IDaoEscuela
    {
        public DaoEscuela()
        {

        }

        #region IDaoEscuela Members

        public bool CueRepetido(string cue,int cueAnexo,int id)
        {
            
            var query = Session.QueryOver<Escuela>();
            query.And(x => x.CUE == cue);
            query.And(x => x.CUEAnexo == cueAnexo);
            
            var cantidadEmpresasConCueYCueAnexo = query.RowCount();


            if (cantidadEmpresasConCueYCueAnexo >= 1)
                return true;
            return false;



        }
        public int GetIdByEscuelaPlan(int filtroEscuelaPlan)
        {
            //TODO NICO:Hacer bien el dao!!!
            var query = Session.QueryOver<EscuelaPlan>();
            query.And(x => x.Id == filtroEscuelaPlan);
            var escuelaPlan = query.SingleOrDefault();
            return escuelaPlan.Escuela.Id;
        }

         public DtoAmbitoEmpresa GetAmbitoByEscuelaMadreId(int idEmpresa)
        {
            DtoAmbitoEmpresa ambitoEmpresa = null;
            var query = Session.QueryOver<Escuela>();
            query.And(x => x.Id == idEmpresa);
            return query.SelectList(list => list
                 .Select(x => x.Id).WithAlias(() => ambitoEmpresa.Id)
                 .Select(x => x.EstructuraDefinitiva).WithAlias(() => ambitoEmpresa.EstructuraDefinitiva)
                 .Select(x => x.Ambito).WithAlias(() => ambitoEmpresa.Ambito))
                 .TransformUsing(Transformers.AliasToBean<DtoAmbitoEmpresa>()).SingleOrDefault<DtoAmbitoEmpresa>();   
        }

        public bool ValidarNivelEmpresaSeleccionada(int idEscuela, int idEmpresaLogueado)
        {
            var query = Session.QueryOver<Escuela>();
            query.And(x => x.Id == idEscuela);
            var escuela = query.SingleOrDefault();
            var nivel = escuela.NivelEducativo.Id;

            var query2 = Session.QueryOver<DireccionDeNivel>();
            query2.And(x => x.Id == idEmpresaLogueado);
            var direccionNivel = query2.SingleOrDefault();
            var nivelDireccionNivel = direccionNivel.NivelesEducativos;

            foreach (var nivelEducativo in nivelDireccionNivel)
            {
                if (nivel == nivelEducativo.Id)
                    return true;

            }
            return false;
        }

        public List<CicloEducativo> CicloEducativoPorEscuela(int escuelaId)
        {
            NivelEducativoPorTipoEducacion nete = null;
            NivelEducativoPorTipoEducacion nete2 = null;
            GradoAnioPorTipoNivelEducativo gatne = null;
            var query = Session.QueryOver<EmpresaBase>();
            query.And(x => x.Id == escuelaId);
            query.Select(x => x.TipoEmpresa);
            var tipoEmpresa = query.SingleOrDefault<TipoEmpresaEnum>();
            if(tipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
            {
                var query2 = Session.QueryOver<Escuela>();
                query2.And(x => x.Id == escuelaId);
                query2.JoinQueryOver(x => x.NivelesEducativo, () => nete);
                query2.Select(x => nete.TipoEducacion);
                var tipoEducacion = query2.SingleOrDefault<TipoEducacionEnum>();
                query2.JoinQueryOver(x => nete.GradosAnioTNE, () => gatne);
                query2.JoinQueryOver(x => gatne.NivelEducativoPorTipoEducacion, () => nete2);
                query2.And(x => nete2.TipoEducacion == tipoEducacion);
                query2.Select(x => gatne.CicloEducativo);
                return query2.List<CicloEducativo>().Distinct().ToList();
            }
            else
            {
                var query2 = Session.QueryOver<EscuelaAnexo>();
                query2.And(x => x.Id == escuelaId);
                query2.JoinQueryOver(x => x.NivelesEducativo, () => nete);
                query2.Select(x => nete.TipoEducacion);
                var tipoEducacion = query2.SingleOrDefault<TipoEducacionEnum>();
                query2.JoinQueryOver(x => nete.GradosAnioTNE, () => gatne);
                query2.JoinQueryOver(x => gatne.NivelEducativoPorTipoEducacion, () => nete2);
                query2.And(x => nete2.TipoEducacion == tipoEducacion);
                query2.Select(x => gatne.CicloEducativo);
                return query2.List<CicloEducativo>().Distinct().ToList();
            }
        }
          
        public IList<Escuela> GetEscuelaByTipo(TipoEmpresaEnum tipo)
        {
            var escuelas = (from m in Session.Query<Escuela>()
                             where (m.TipoEmpresa == tipo)
                             orderby m.Nombre
                             select m).ToList<Escuela>();
            return escuelas.Where(escuela => escuela.EstadoEmpresa != EstadoEmpresaEnum.CERRADA).ToList();
        }



        /// <summary>
        /// Obtiene una lista de escuelas segun filtro básico
        /// </summary>
        /// <param name="cue">CUE</param>
        /// <param name="codigoEmpresa">Codigo de la empresa</param>
        /// <param name="nombreEmpresa">Nombre de la empresa</param>
        /// <param name="idDepartamentoProvincial">Pertenece al domicilio</param>
        /// <param name="idLocalidad">Pertenece al domicilio</param>
        /// <param name="barrio">Pertenece al domicilio</param>
        /// <param name="calle">Pertenece al domicilio</param>
        /// <param name="altura">Pertenece al domicilio</param>
        /// <param name="estadoEmpresaEnum">Estado de la empresa</param>
        /// <returns>Devuelve una lista de escuelas que cumplan con los parametros de búsqueda</returns>
        public List<Escuela> GetByFiltrosBasico(string cue, int? cueAnexo, string codigoEmpresa, string nombreEmpresa, int? idLocalidad, string barrio, string calle, int? altura, List<EstadoEmpresaEnum> estadoEmpresaEnum, bool? esRaiz, int? idEmpresaUsuarioLogueado,int? idEmpresaDependintePadre)
        {
            //idEmpresaDependintePadre es el id de la empresa el cual le estamos modificando o la empresa inspeccion/padre/madre etc
            var escuela = new Escuela();
            
            var domicilio = new Domicilio();
            var query = Session.QueryOver<Escuela>(() => escuela);
            
            var empresaRegistro = new EmpresaBase();
            if(idEmpresaUsuarioLogueado.HasValue){
            query.JoinQueryOver(x => x.EmpresaRegistro, () => empresaRegistro);
            query.And(x=>empresaRegistro.Id==idEmpresaUsuarioLogueado);
            }
            if (!string.IsNullOrEmpty(cue))
                query.And(x => x.CUE.IsLike(cue));
            if (cueAnexo.HasValue)
                query.And(x => x.CUEAnexo == cueAnexo);
            if (!string.IsNullOrEmpty(codigoEmpresa))
                query.And(x => x.CodigoEmpresa.IsLike(codigoEmpresa));
            if (!string.IsNullOrEmpty(nombreEmpresa))
                query.And(x => x.Nombre.IsLike(nombreEmpresa));
            if (esRaiz.HasValue)
                query.And(Restrictions.Eq("EsRaiz", esRaiz.Value ? 'Y' : 'N'));
            

            if(esRaiz.HasValue && esRaiz.Value && idEmpresaDependintePadre.HasValue)
                query.And(x => x.Id != idEmpresaDependintePadre);
            


            var escuelas = (List<Escuela>)query.List<Escuela>();
            //TODO mejorar el filtrado por domicilio; hasta ahora se obtiene el listado de empresa y se filtra recien ahi (pesimo!)
            if (idLocalidad.HasValue || !string.IsNullOrEmpty(barrio) || !string.IsNullOrEmpty(calle) ||
                altura.HasValue)
            {
                barrio = barrio != string.Empty ? barrio.ToUpper() : string.Empty;
                calle = calle != string.Empty ? calle.ToUpper() : string.Empty;

                foreach (Escuela emp in escuelas)
                {
                    emp.Domicilio = new DaoDomicilio().GetByEntidad(emp.VinculoDomicilio, emp.Id);
                }
                escuelas = (List<Escuela>) escuelas.Where(x => x.Domicilio.Localidad.Id == idLocalidad &&
                                                               (string.IsNullOrEmpty(barrio) ||
                                                                x.Domicilio.Barrio.Nombre.Contains(barrio)) &&
                                                               (string.IsNullOrEmpty(calle) ||
                                                                x.Domicilio.Calle.Nombre.Contains(calle))
                                                               && (!altura.HasValue || x.Domicilio.Altura == altura))
                                               .ToList();
            }
            
            if (estadoEmpresaEnum != null && estadoEmpresaEnum.Count > 0)
                return escuelas.Where(x => estadoEmpresaEnum.Any(p => p == x.EstadoEmpresa)).ToList();
            return escuelas;


        }
        /*
        public bool ValidarCueyCueAnexo(string cue,int cueAnexo)
        {
            var flag = false;
            var escuela = new Escuela();
            var query = Session.QueryOver<Escuela>(() => escuela);
            if (!string.IsNullOrEmpty(cue))
            {
                query.AndNot(x => x.CUE == cue);
                query.AndNot(x => x.CUEAnexo == cueAnexo);
            }
            if (escuela != null)
                flag = true;
            return flag;
        }
        */
        /// <summary>
        /// Obtiene una lista de escuelas segun filtro avanzado
        /// </summary>
        /// <param name="fechaAltaDesde">Fecha de alta (desde)</param>
        /// <param name="fechaAltaHasta">Fecha de alta (hasta)</param>
        /// <param name="fechaInicioActividadDesde">Fecha de inicio de actividades (desde)</param>
        /// <param name="fechaInicioActividadHasta">Fecha de inicio de actividades (hasta)</param>
        /// <param name="TipoEmpresaEnum">Tipo de empresa</param>
        /// <param name="numeroEscuela">Núero de escuela</param>
        /// <param name="tipoEscuelaEnum">Tipo de escuela</param>
        /// <param name="categoriaEscuelaEnum">Categoría de escuela</param>
        /// <param name="tipoEducacionEnum">Tipo educación </param>
        /// <param name="nivelEducativo">Nivel educativo</param>
        /// <param name="dependenciaEnum">Dependencia</param>
        /// <param name="ambitoEscuelaEnum">Ambito de escuela</param>
        /// <param name="esReligioso">Religioso</param>
        /// <param name="esArancelado">Arancelado</param>
        /// <param name="tipoInspeccionEnum">Tipo de inspección</param>
        /// <param name="idDepartamentoProvincial">Pertenece al domicilio</param>
        /// <param name="idLocalidad">Pertenece al domicilio</param>
        /// <param name="barrio">Pertenece al domicilio</param>
        /// <param name="calle">Pertenece al domicilio</param>
        /// <param name="altura">Pertenece al domicilio</param>
        /// <param name="estadoEmpresaEnum">Estado de la empresa</param>
        /// <param name="idObraSocial">Obra social</param>
        /// <param name="idPeriodoLectivo">Periodo lectivo</param>
        /// <returns>Devuelve una lista de escuelas que cumplan con los parametros de búsqueda</returns>
        public List<Escuela> GetByFiltroAvanzado(DateTime? fechaAltaDesde, DateTime? fechaAltaHasta, DateTime? fechaInicioActividadDesde, DateTime? fechaInicioActividadHasta, TipoEmpresaEnum? tipoEmpresaEnum, int? numeroEscuela, int? tipoEscuelaEnum, CategoriaEscuelaEnum? categoriaEscuelaEnum, TipoEducacionEnum? tipoEducacionEnum, int? nivelEducativo, DependenciaEnum? dependenciaEnum, AmbitoEscuelaEnum? ambitoEscuelaEnum, bool? esReligioso, bool? esArancelado, TipoInspeccionEnum? tipoInspeccionEnum, int? idLocalidad, string barrio, string calle, int? altura, List<EstadoEmpresaEnum> estadoEmpresaEnum, int? idObraSocial, PeriodoLectivo periodoLectivo, Turno turno, string nombre, DateTime? fechaDesdeNotificacion, DateTime? fechaHastaNotificacion, int? idEmpresaUsuarioLogueado, string fltCodigoInspeccion)
        {
            Escuela escuela = null;
            NivelEducativo _nivelEducativo =null;
            NivelEducativoPorTipoEducacion nivelEducativoPorTipoEducacion = null;
            ObraSocial obraSocial = null;
            PeriodoLectivo perLectivo = null;
            Turno turnoN = null;

            var query = Session.QueryOver<Escuela>(() => escuela);
            
            if(idEmpresaUsuarioLogueado.HasValue){
            var empresaRegistro = new EmpresaBase();
            query.JoinQueryOver(x => x.EmpresaRegistro, () => empresaRegistro);
            query.And(x => empresaRegistro.Id == idEmpresaUsuarioLogueado);
            }
            if (!string.IsNullOrEmpty(nombre))
                query.And(x => x.Nombre.IsLike(nombre));

            if(fechaDesdeNotificacion.HasValue)
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

            if (numeroEscuela.HasValue)
                query.And(x => x.NumeroEscuela == numeroEscuela);

            if (categoriaEscuelaEnum.HasValue)
                query.And(x => x.TipoCategoria == categoriaEscuelaEnum);

            if (!string.IsNullOrEmpty(fltCodigoInspeccion))
                query.And(x => x.CodigoInspeccion == fltCodigoInspeccion);

            if (nivelEducativo.HasValue)
            {
                query.JoinQueryOver(x => x.NivelesEducativo, () => nivelEducativoPorTipoEducacion);
                query.JoinQueryOver(x => nivelEducativoPorTipoEducacion.NivelEducativo, () => _nivelEducativo);
                query.And(x => _nivelEducativo.Id == nivelEducativo);
            }
            if (tipoEducacionEnum.HasValue)
            {
                query.JoinQueryOver(x => x.NivelesEducativo, () => nivelEducativoPorTipoEducacion);
                query.And(x => nivelEducativoPorTipoEducacion.TipoEducacion == tipoEducacionEnum);
            }
            
            if (dependenciaEnum.HasValue)
                query.And(x => x.Dependencia == dependenciaEnum);

            if (ambitoEscuelaEnum.HasValue)
                query.And(x => x.Ambito == ambitoEscuelaEnum);

            if (esReligioso.HasValue)
                query.And(Restrictions.Eq("Religioso", esReligioso.Value ? 'Y' : 'N'));

            if(esArancelado.HasValue)
            query.And(Restrictions.Eq("Arancelado", esArancelado.Value ? 'Y' : 'N'));

            if (idObraSocial.HasValue) 
            {
                query.JoinQueryOver<ObraSocial>(() => obraSocial);
                query.And(() => obraSocial.Id == idObraSocial);
            }
            if (periodoLectivo != null)
            {
                query.JoinQueryOver<PeriodoLectivo>(() => perLectivo);
                query.And(() => perLectivo.Id == periodoLectivo.Id);
            }
            if (turno != null)
            {
                query.JoinQueryOver<Turno>(() => turnoN);
                query.And(() => turnoN.Id == turno.Id);
            }
            var listadoEscuelas = (List<Escuela>)query.List<Escuela>();

            var listaEscuela = new List<Escuela>();
            if (estadoEmpresaEnum != null && estadoEmpresaEnum.Count > 0)
            {
                foreach (var est in estadoEmpresaEnum)
                    listaEscuela.AddRange(listadoEscuelas.Where(esc => esc.EstadoEmpresa == est));
                return listaEscuela;
            }
            return listadoEscuelas;
            

            //if (idLocalidad.HasValue || !string.IsNullOrEmpty(barrio) || !string.IsNullOrEmpty(calle) || altura.HasValue)
            //{
            //    foreach (Escuela emp in escuelas)
            //    {
            //        emp.Domicilio = new DaoDomicilio().GetByEntidad(emp.VinculoDomicilio, emp.Id);
            //    }
            //    escuelas = (List<Escuela>)escuelas.Where(x => x.Domicilio.Localidad.Id == idLocalidad &&
            //        x.Domicilio.Barrio.Nombre.Contains(barrio) && x.Domicilio.Calle.Nombre.Contains(calle)
            //        && x.Domicilio.Altura == altura);
            //}
            //return escuelas;

        }

        /// <summary>
        /// Verifica si la escuela pasada por parametro es madre de alguna otra.
        /// </summary>
        /// <param name="id">Id de la escuela madre</param>
        /// <returns>Devuelve true en el caso de que la escuela no sea madre de ninguna o es madre de escuela cerradas.</returns>
        public bool ComprobarEscuelasAsociadas(int id)
        {
            // TODO verificar validacion
            var escuelaMadre = new Escuela();
            var query = Session.QueryOver<Escuela>();
            query.JoinQueryOver<Escuela>(x => x.EscuelaMadre, () => escuelaMadre).Where(() => escuelaMadre.Id == id);
            return query.List<Escuela>().All(x => x.EstadoEmpresa == EstadoEmpresaEnum.CERRADA || x.EstadoEmpresa == EstadoEmpresaEnum.RECHAZADA);
            
            //StringBuilder hql = new StringBuilder("count * from Escuela where Madre.Id=:id and EstadoEmpresa!=:estado");

            //return Session.CreateQuery(hql.ToString())    
            //    .SetInt32("id", id)
            //    .SetString("estado", EstadoEmpresaEnum.CERRADA.ToString()).UniqueResult<int>() <= 0;
        }

        /// <summary>
        /// Verifica si la escuela pasada por parametro es raiz de alguna otra.
        /// </summary>
        /// <param name="id">Id de la escuela raiz</param>
        /// <returns>Devuelve true en el caso de que la escuela no sea raiz de ninguna o es madre de escuela cerradas.</returns>
        public bool ComprobarRaizEscuelas(int id)
        {
            // TODO verificar validacion
            var escuelaRaiz = new Escuela();
            var escuelaMadre = new Escuela();
            var query = Session.QueryOver<Escuela>();
            query.JoinQueryOver<Escuela>(x => x.EscuelaRaiz, () => escuelaRaiz).Where(() => escuelaRaiz.Id == id);
            var noEsRaizDeOtras = query.List<Escuela>().All(x => x.EstadoEmpresa == EstadoEmpresaEnum.CERRADA || x.EstadoEmpresa == EstadoEmpresaEnum.RECHAZADA);

            var query1 = Session.QueryOver<EscuelaAnexo>();
            query1.JoinQueryOver(x => x.EscuelaMadre, () => escuelaMadre).Where(x => escuelaMadre.Id == id);
            var noEsMadreDeOtras = query1.List<EscuelaAnexo>().All(x => x.EstadoEmpresa == EstadoEmpresaEnum.CERRADA || x.EstadoEmpresa == EstadoEmpresaEnum.RECHAZADA);

            return noEsRaizDeOtras && noEsMadreDeOtras;

            //StringBuilder hql = new StringBuilder("count * from Escuela where Escuela.Id=:id and EstadoEmpresa!=:estado");

            //return Session.CreateQuery(hql.ToString())
            //    .SetInt32("id", id)
            //    .SetString("estado", EstadoEmpresaEnum.CERRADA.ToString()).UniqueResult<int>() <= 0;
        }


        public long GetCantidadDeEscuelasPorTipoYLocalidad(int idLocalidad, int tipoEscuela)
        {
            var aplicacion = int.Parse(System.Configuration.ConfigurationManager.AppSettings["AplicacionID"]);
            var mainQuery = Session.CreateSQLQuery(
                @"select count(E.ID_SEQ_EMPRESA) as cantidad 
                from t_em_empresas E inner join dom_manager.VT_DOMICILIOS_TODO_COND DOM on E.id_vin = DOM.id_vin 
                where DOM.ID_APP = :aplicacion and E.ID_SEQ_TIPO_ESCUELA = :tipoEscuela and E.ID_VIN !=0 and DOM.id_localidad = :localidad");

            mainQuery.SetInt32("aplicacion", aplicacion);
            mainQuery.SetInt32("tipoEscuela", tipoEscuela);
            mainQuery.SetInt32("localidad", idLocalidad);

            return long.Parse(mainQuery.UniqueResult().ToString());
        }

        public int GetMaximoNumeroEscuela()
        {
            var criterioMaximoNumeroEscuela =
                QueryOver.Of<Escuela>().SelectList(p => p.SelectMax(e => e.NumeroEscuela));
            var escuelaConMaximoNumeroEscuela =
                Session.QueryOver<Escuela>().Where(
                    Subqueries.WhereProperty<Escuela>(c => c.NumeroEscuela).Eq(criterioMaximoNumeroEscuela)).
                    List();

            return escuelaConMaximoNumeroEscuela != null && escuelaConMaximoNumeroEscuela.Count > 0 ? escuelaConMaximoNumeroEscuela.First().NumeroEscuela : 0;

            
        }

        /// <summary>
        /// Verifica si la escuela pasada por parametro posee alumnos inscriptos en el ciclo lectivo vigente, que no estén en estado cerrada o anulada (la inscripción).
        /// </summary>
        /// <param name="idEmpresa">Id de la escuela </param>
        /// <returns>Devuelve true en el caso de que la escuela tenga algún alumno inscripto y su estado no sea cerrado o anulado.</returns>
        public bool EscuelaPoseeAlumnosInscriptosEnCicloLectivoVigenteNoCerradaOAnulada(int idEmpresa)
        {
            //Busco todas las preinscripciones de la empresa pasada por parámetro que no tengan inscripciones ni cerradas ni anuladas para el ciclo lectivo vigente
            var escuelaPlan = new EscuelaPlan();
            var ins = new Inscripcion();
            var query = Session.QueryOver<Preinscripcion>();
            query.JoinAlias(x => x.EscuelaPlan, () => escuelaPlan)
                .Where(x => escuelaPlan.Escuela.Id == idEmpresa)
                .JoinAlias(x => x.Inscripcion, () => ins)
                .And(
                    x =>
                    ins.EstadoInscripcion != EstadoInscripcionEnum.CERRADA &&
                    ins.EstadoInscripcion != EstadoInscripcionEnum.ANULADA)
                .And(x => ins.CicloLectivo.AñoCiclo == DateTime.Today.Year);
            return query.List().Count > 0;
        }

        public List<DtoEscuelaReporte> GetEscuelasByFiltros(string filtroCUE, string filtroCUEAnexo, string filtroCodigoEmpresa, string filtroNombreEmpresa, CategoriaEscuelaEnum? filtroTipoCategoria,
                                                     TipoEducacionEnum? filtroTipoEducacion, int? filtroNivelEducativo, EstadoEmpresaEnum? filtroEstado, int? filtroDepartamento, 
                                                     int? filtroLocalidad, string filtroOrdenPago, string filtroProgPresupuestario, DependenciaEnum? filtroDependencia, AmbitoEscuelaEnum? filtroAmbito, List<int> listaZonas, bool filtroPublica, bool filtroPrivada)
        {
            var query = Session.QueryOver<DtoEscuelaReporte>();

            if (listaZonas != null && listaZonas.Count > 0)
                query.And(x => x.IdZonaDesfavorable != null && x.IdZonaDesfavorable.Value.IsIn(listaZonas));

            if (!string.IsNullOrEmpty(filtroCUE))
                query.And(x => x.CUE == filtroCUE);

            if (!string.IsNullOrEmpty(filtroCUEAnexo))
                query.And(x => x.CUEAnexo == filtroCUEAnexo);

            if (!string.IsNullOrEmpty(filtroCodigoEmpresa))
                query.And(x => x.CodigoEmpresa == filtroCodigoEmpresa);

            if (!string.IsNullOrEmpty(filtroNombreEmpresa))
                query.And(x => x.NombreEmpresa == filtroNombreEmpresa);

            if (filtroTipoCategoria != null)
                query.And(x => x.TipoCategoria == filtroTipoCategoria);

            if (filtroTipoEducacion != null)
                query.And(x => x.TipoEducacion == filtroTipoEducacion);

            if (filtroNivelEducativo != null)
                query.And(x => x.IdNivelEducativo == filtroNivelEducativo);

            if (filtroEstado != null)
                query.And(x => x.Estado == filtroEstado);

            if (filtroDepartamento.HasValue && filtroDepartamento > 0)
                query.And(x => x.IdDeptoProvincialEmpresa == filtroDepartamento.Value);

            if (filtroLocalidad.HasValue && filtroLocalidad > 0)
                query.And(x => x.IdLocalidadEmpresa == filtroLocalidad.Value);

            if (!string.IsNullOrEmpty(filtroOrdenPago))
                query.And(x => x.CodigoOrdenPago == filtroOrdenPago);

            if (!string.IsNullOrEmpty(filtroProgPresupuestario))
                query.And(x => x.CodigoProgPresupuestario == filtroProgPresupuestario);

            if (filtroDependencia != null)
                query.And(x => x.Dependencia == filtroDependencia);

            if (filtroAmbito != null)
                query.And(x => x.Ambito == filtroAmbito);

            if (filtroPublica && !filtroPrivada)
                query.And(Restrictions.Eq("Privado", 'N'));
            else if (!filtroPublica && filtroPrivada)
                query.And(Restrictions.Eq("Privado", 'Y'));

            return (List<DtoEscuelaReporte>)query.List<DtoEscuelaReporte>();
        }

        public List<DtoEscuelaAnexoReporte> GetEscuelasAnexoByFiltros(string filtroCUE, string filtroCUEAnexo, string filtroCodigoEmpresa, string filtroNombreEmpresa, CategoriaEscuelaEnum? filtroTipoCategoria,
                                                     TipoEducacionEnum? filtroTipoEducacion, int? filtroNivelEducativo, EstadoEmpresaEnum? filtroEstado, int? filtroDepartamento,
                                                     int? filtroLocalidad, string filtroOrdenPago, string filtroProgPresupuestario, DependenciaEnum? filtroDependencia, AmbitoEscuelaEnum? filtroAmbito, List<int> listaZonas, bool filtroPublica, bool filtroPrivada)
        {
            var query = Session.QueryOver<DtoEscuelaAnexoReporte>();

            if (listaZonas != null && listaZonas.Count > 0)
                query.And(x => x.IdZonaDesfavorable != null && x.IdZonaDesfavorable.Value.IsIn(listaZonas));

            if (!string.IsNullOrEmpty(filtroCUE))
                query.And(x => x.CUE == filtroCUE);

            if (!string.IsNullOrEmpty(filtroCUEAnexo))
                query.And(x => x.CUEAnexo == filtroCUEAnexo);

            if (!string.IsNullOrEmpty(filtroCodigoEmpresa))
                query.And(x => x.CodigoEmpresa == filtroCodigoEmpresa);

            if (!string.IsNullOrEmpty(filtroNombreEmpresa))
                query.And(x => x.NombreEmpresa == filtroNombreEmpresa);

            //if (filtroTipoCategoria != null)
            //    query.And(x => x.TipoCategoria == filtroTipoCategoria);

            if (filtroTipoEducacion != null)
                query.And(x => x.TipoEducacion == filtroTipoEducacion);

            if (filtroNivelEducativo != null)
                query.And(x => x.IdNivelEducativo == filtroNivelEducativo);

            if (filtroEstado != null)
                query.And(x => x.Estado == filtroEstado);

            if (filtroDepartamento.HasValue && filtroDepartamento > 0)
                query.And(x => x.IdDeptoProvincialEmpresa == filtroDepartamento.Value);

            if (filtroLocalidad.HasValue && filtroLocalidad > 0)
                query.And(x => x.IdLocalidadEmpresa == filtroLocalidad.Value);

            if (!string.IsNullOrEmpty(filtroOrdenPago))
                query.And(x => x.CodigoOrdenPago == filtroOrdenPago);

            if (!string.IsNullOrEmpty(filtroProgPresupuestario))
                query.And(x => x.CodigoProgPresupuestario == filtroProgPresupuestario);

            if (filtroDependencia != null)
                query.And(x => x.Dependencia == filtroDependencia);

            //if (filtroAmbito != null)
            //    query.And(x => x.Ambito == filtroAmbito);

            if (filtroPublica && !filtroPrivada)
                query.And(Restrictions.Eq("Privado", 'N'));
            else if (!filtroPublica && filtroPrivada)
                query.And(Restrictions.Eq("Privado", 'Y'));

            return (List<DtoEscuelaAnexoReporte>)query.List<DtoEscuelaAnexoReporte>();
        }

        public DtoConsultaEscuela GetDtoById(int id)
        {
            var entidad = GetById(id);
            return new DtoConsultaEscuela
                       {
                           Id = entidad.Id,
                           Nombre = entidad.Nombre,
                           NivelEducativo =
                               new DtoConsultaNivelEducativo
                                   {Id = entidad.NivelEducativo.Id, Nombre = entidad.NivelEducativo.Nombre}
                       };
        }

        #endregion
    }
}