using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoEscuelaAnexo : IDao<EscuelaAnexo, int>
    {
        bool ValidarNivelEmpresaSeleccionada(int idEscuela, int idEmpresaLogueado);
        bool ExisteNumeroEscuelaAnexoParaEscuelaMadre(int numeroEscuelaAnexo, int idEscuelaMadre,int idEscuelaActual);

        List<EscuelaAnexo> GetByFiltrosBasico(string cue, int? cueAnexo, string codigoEmpresa, string nombreEmpresa,
                                              int? idLocalidad, string barrio, string calle, int? altura,
                                              List<EstadoEmpresaEnum> estadoEmpresaEnum, int? idEmpresaUsuarioLogueado);

        List<EscuelaAnexo> GetByFiltroAvanzado(DateTime? fechaAltaDesde, DateTime? fechaAltaHasta,
                                               DateTime? fechaInicioActividadDesde, DateTime? fechaInicioActividadHasta,
                                               TipoEmpresaEnum? tipoEmpresaEnum, int? numeroEscuela,
                                               int? tipoEscuelaEnum, CategoriaEscuelaEnum? categoriaEscuelaEnum,
                                               TipoEducacionEnum? tipoEducacionEnum, int? nivelEducativo,
                                               DependenciaEnum? dependenciaEnum, AmbitoEscuelaEnum? ambitoEscuelaEnum,
                                               bool? esReligioso, bool? esArancelado,
                                               TipoInspeccionEnum? tipoInspeccionEnum, int? idLocalidad, string barrio,
                                               string calle, int? altura, List<EstadoEmpresaEnum> estadoEmpresaEnum,
                                               int? idObraSocial, PeriodoLectivo periodoLectivo, Turno turnoEnum,
                                               string nombre, DateTime? fechaDesdeNotificacion,
                                               DateTime? fechaHastaNotificacion, int? idEmpresaUsuarioLogueado, string fltCodigoInspeccion);
    }
}
