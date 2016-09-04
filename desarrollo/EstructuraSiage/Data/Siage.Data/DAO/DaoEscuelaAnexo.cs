using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Testing.Values;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Mapping;
using Siage.Core.Domain;
using Siage.Core.DaoInterfaces;
using Siage.Base;
using NHibernate.Linq;


namespace Siage.Data.DAO
{
    public class DaoEscuelaAnexo : DaoBase<EscuelaAnexo, int>, IDaoEscuelaAnexo
    {
        public DaoEscuelaAnexo()
        {

        }

        #region IDaoEscuela Members
        public bool ValidarNivelEmpresaSeleccionada(int idEscuela, int idEmpresaLogueado)
        {
            var query = Session.QueryOver<EscuelaAnexo>();
            query.And(x => x.Id == idEscuela);
            var escuelaAnexo = query.SingleOrDefault();
            var nivelAnexo = escuelaAnexo.NivelEducativo.Id;

            var query2 = Session.QueryOver<DireccionDeNivel>();
            query2.And(x => x.Id == idEmpresaLogueado);
            var direccionNivel = query2.SingleOrDefault();
            var nivelDireccionNivel = direccionNivel.NivelesEducativos;

            foreach (var nivelEducativo in nivelDireccionNivel)
            {
                if (nivelAnexo == nivelEducativo.Id)
                    return true;
                
            }
            return false;

        }

        public bool ExisteNumeroEscuelaAnexoParaEscuelaMadre(int numeroEscuelaAnexo, int idEscuelaMadre, int idEscuelaActual)
        {
            var escuela = new Escuela {Id = idEscuelaMadre};
            var query = Session.QueryOver<EscuelaAnexo>().Where(x => x.EscuelaMadre.Id == idEscuelaMadre).And(x => x.NumeroAnexo == numeroEscuelaAnexo);
            
            if (idEscuelaActual != 0)
                query.And(x => x.Id != idEscuelaActual);

            return query.List().Count > 0 ? true : false;
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
        public List<EscuelaAnexo> GetByFiltrosBasico(string cue, int? cueAnexo, string codigoEmpresa, string nombreEmpresa, int? idLocalidad, string barrio, string calle, int? altura, List<EstadoEmpresaEnum> estadoEmpresaEnum, int? idEmpresaUsuarioLogueado)
        {
            var escuela = new EscuelaAnexo();
            var domicilio = new Domicilio();
            var query = Session.QueryOver<EscuelaAnexo>(() => escuela);
            if (!string.IsNullOrEmpty(cue))
                query.And(x => x.CUE.IsLike(cue));
            if (cueAnexo.HasValue)
                query.And(x => x.CUEAnexo == cueAnexo);
            if (!string.IsNullOrEmpty(codigoEmpresa))
                query.And(x => x.CodigoEmpresa.IsLike(codigoEmpresa));
            if (!string.IsNullOrEmpty(nombreEmpresa))
                query.And(x => x.Nombre.IsLike(nombreEmpresa));

            if(idEmpresaUsuarioLogueado.HasValue){
            var empresaRegistro = new EmpresaBase();
            query.JoinQueryOver(x => x.EmpresaRegistro, () => empresaRegistro);
            query.And(x => empresaRegistro.Id == idEmpresaUsuarioLogueado);
                }
            List<EscuelaAnexo> escuelas = (List<EscuelaAnexo>)query.List<EscuelaAnexo>();

            if (idLocalidad.HasValue || !string.IsNullOrEmpty(barrio) || !string.IsNullOrEmpty(calle) ||
                altura.HasValue)
            {
                foreach (EscuelaAnexo emp in escuelas)
                    emp.Domicilio = new DaoDomicilio().GetByEntidad(emp.VinculoDomicilio, emp.Id);
                escuelas = (List<EscuelaAnexo>)escuelas.Where(x => x.Domicilio.Localidad.Id == idLocalidad &&
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
        public List<EscuelaAnexo> GetByFiltroAvanzado(DateTime? fechaAltaDesde, DateTime? fechaAltaHasta, DateTime? fechaInicioActividadDesde, DateTime? fechaInicioActividadHasta, TipoEmpresaEnum? tipoEmpresaEnum, int? numeroEscuela, int? tipoEscuelaEnum, CategoriaEscuelaEnum? categoriaEscuelaEnum, TipoEducacionEnum? tipoEducacionEnum, int? nivelEducativo, DependenciaEnum? dependenciaEnum, AmbitoEscuelaEnum? ambitoEscuelaEnum, bool? esReligioso, bool? esArancelado, TipoInspeccionEnum? tipoInspeccionEnum, int? idLocalidad, string barrio, string calle, int? altura, List<EstadoEmpresaEnum> estadoEmpresaEnum, int? idObraSocial, PeriodoLectivo periodoLectivo, Turno turno, string nombre, DateTime? fechaDesdeNotificacion, DateTime? fechaHastaNotificacion, int? idEmpresaUsuarioLogueado, string fltCodigoInspeccion)
        {
            var escuela = new EscuelaAnexo();
            var _nivelEducativo = new NivelEducativo();
            var obraSocial = new ObraSocial();
            var perLectivo = new PeriodoLectivo();
            var turnoN = new Turno();
            var nivelEducativoPorTipoEducacion=new NivelEducativoPorTipoEducacion();
            var empresaRegistro = new EmpresaBase();

            var query = Session.QueryOver<EscuelaAnexo>(() => escuela);
            if(idEmpresaUsuarioLogueado.HasValue){
            query.JoinQueryOver(x => x.EmpresaRegistro, () => empresaRegistro);
            query.And(x => empresaRegistro.Id == idEmpresaUsuarioLogueado);
            }
            if (fechaAltaDesde.HasValue)
                query.And(x => x.FechaAlta >= fechaAltaDesde);
            if (fechaAltaHasta.HasValue)
                query.And(x => x.FechaAlta <= fechaAltaHasta);
            if (fechaInicioActividadDesde.HasValue)
                query.And(x => x.FechaInicioActividad >= fechaInicioActividadDesde);
            if (fechaInicioActividadHasta.HasValue)
                query.And(x => x.FechaInicioActividad <= fechaInicioActividadHasta);
            if (!string.IsNullOrEmpty(nombre))
                query.And(x => x.Nombre.IsLike(nombre));
            if (fechaDesdeNotificacion.HasValue)
                query.And(x => x.FechaNotificacion >= fechaDesdeNotificacion);
            if (fechaHastaNotificacion.HasValue)
                query.And(x => x.FechaNotificacion <= fechaHastaNotificacion);
            if (tipoEmpresaEnum.HasValue)
                query.And(x => x.TipoEmpresa == tipoEmpresaEnum);
            if (numeroEscuela.HasValue)
                query.And(x => x.NumeroEscuela == numeroEscuela);
            if (categoriaEscuelaEnum.HasValue)
                query.And(x => x.TipoCategoria == categoriaEscuelaEnum);
            if (tipoEducacionEnum.HasValue)
                query.And(x => x.TipoEducacion == tipoEducacionEnum);
            if (nivelEducativo.HasValue)
            {
                query.JoinQueryOver(x => x.NivelesEducativo, () => nivelEducativoPorTipoEducacion);
                query.JoinQueryOver(x => nivelEducativoPorTipoEducacion.NivelEducativo, () => _nivelEducativo);
                query.And(x => _nivelEducativo.Id == nivelEducativo);
            }
            if (!string.IsNullOrEmpty(fltCodigoInspeccion))
                query.And(x => x.CodigoInspeccion == fltCodigoInspeccion);
            if (dependenciaEnum.HasValue)
                query.And(x => x.Dependencia == dependenciaEnum);
            if (ambitoEscuelaEnum.HasValue)
                query.And(x => x.Ambito == ambitoEscuelaEnum);
            if (esReligioso.HasValue)
                query.And(Restrictions.Eq("Religioso", esReligioso.Value ? 'Y' : 'N'));
            if (esArancelado.HasValue)
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
            List<EscuelaAnexo> listadoEscuelas = (List<EscuelaAnexo>)query.List<EscuelaAnexo>();

            List<EscuelaAnexo> listaEscuela = new List<EscuelaAnexo>();
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

        #endregion
    }
}

