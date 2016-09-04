using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoInspeccion : IDao<Inspeccion, int>
    {
        List<Inspeccion> GetByFiltrosBasico(string codigoEmpresa, string nombreEmpresa, int? idLocalidad, string barrio,
                                            string calle, int? altura, List<EstadoEmpresaEnum> estadoEmpresaEnum,
                                            List<TipoEmpresaFiltroBusquedaEnum> empresaFiltro,
                                            int? idDireccionDeNivelActual,
                                            List<TipoInspeccionEnum> tiposInspeccionEnumPermitidos,
                                            int? idEmpresaUsuarioLogueado, int? idEmpresaDependientePadre);

        List<Inspeccion> GetByFiltroAvanzado(DateTime? fechaAltaDesde, DateTime? fechaAltaHasta,
                                             DateTime? fechaInicioActividadDesde, DateTime? fechaInicioActividadHasta,
                                             TipoEmpresaEnum TipoEmpresaEnum, List<TipoInspeccionEnum> tipoInspeccionEnumList,
                                             int? idDepartamentoProvincial, int? idLocalidad, string barrio,
                                             string calle, int? altura, List<EstadoEmpresaEnum> estadoEmpresaEnumstring,
                                             string nombre, DateTime? fechaDesdeNotificacion,
                                             DateTime? fechaHastaNotificacion, int? tipoInspeccionIntermediaId, int? idEmpresaUsuarioLogueado);
        bool EsInspeccionSuperiorDeInspeccionNoCerradaNiRechazada(int id);
        List<Inspeccion> GetByDireccionNivel(DireccionDeNivel direccionDeNivel);
    }
}

