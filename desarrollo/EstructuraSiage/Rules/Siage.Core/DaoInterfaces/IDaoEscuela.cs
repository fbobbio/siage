using Siage.Base.Dto;
using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoEscuela : IDao<Escuela, int>
    {
        DtoAmbitoEmpresa GetAmbitoByEscuelaMadreId(int idEmpresa);
        int GetIdByEscuelaPlan(int filtroEscuelaPlan);
        bool ValidarNivelEmpresaSeleccionada(int idEscuela ,int idEmpresaLogueado );
        List<Escuela> GetByFiltrosBasico(string cue, int? cueAnexo, string codigoEmpresa, string nombreEmpresa,
                                         int? idLocalidad, string barrio, string calle, int? altura,
                                         List<EstadoEmpresaEnum> estadoEmpresaEnum, bool? esRaiz, int? idEmpresaUsuarioLogueado, int? idEmpresaDependintePadre);
        IList<Escuela> GetEscuelaByTipo(TipoEmpresaEnum tipo);

        List<Escuela> GetByFiltroAvanzado(DateTime? fechaAltaDesde, DateTime? fechaAltaHasta,
                                          DateTime? fechaInicioActividadDesde, DateTime? fechaInicioActividadHasta,
                                          TipoEmpresaEnum? tipoEmpresaEnum, int? numeroEscuela, int? tipoEscuelaEnum,
                                          CategoriaEscuelaEnum? categoriaEscuelaEnum,
                                          TipoEducacionEnum? tipoEducacionEnum, int? nivelEducativo,
                                          DependenciaEnum? dependenciaEnum, AmbitoEscuelaEnum? ambitoEscuelaEnum,
                                          bool? esReligioso, bool? esArancelado, TipoInspeccionEnum? tipoInspeccionEnum,
                                          int? idLocalidad, string barrio, string calle, int? altura,
                                          List<EstadoEmpresaEnum> estadoEmpresaEnum, int? idObraSocial,
                                          PeriodoLectivo periodoLectivo, Turno turnoEnum, string nombre,
                                          DateTime? fechaDesdeNotificacion, DateTime? fechaHastaNotificacion,
                                          int? idEmpresaUsuarioLogueado, string fltCodigoInspeccion);
        List<CicloEducativo> CicloEducativoPorEscuela(int escuelaId);
        bool ComprobarEscuelasAsociadas(int id);
        bool ComprobarRaizEscuelas(int id);
        long GetCantidadDeEscuelasPorTipoYLocalidad(int idLocalidad, int tipoEscuela);
        int GetMaximoNumeroEscuela();
        bool CueRepetido(string cue,int cueAnexo,int id);
        bool EscuelaPoseeAlumnosInscriptosEnCicloLectivoVigenteNoCerradaOAnulada(int idEmpresa);

        List<DtoEscuelaReporte> GetEscuelasByFiltros(string filtroCUE, string filtroCUEAnexo, string filtroCodigoEmpresa, string filtroNombreEmpresa, CategoriaEscuelaEnum? filtroTipoCategoria,
                                                     TipoEducacionEnum? filtroTipoEducacion, int? filtroNivelEducativo, EstadoEmpresaEnum? filtroEstado, int? filtroDepartamento, int? filtroLocalidad,
                                                     string filtroOrdenPago, string filtroProgPresupuestario, DependenciaEnum? filtroDependencia, AmbitoEscuelaEnum? filtroAmbito, List<int> listaZonas, bool filtroPublica, bool filtroPrivada);

        List<DtoEscuelaAnexoReporte> GetEscuelasAnexoByFiltros(string filtroCUE, string filtroCUEAnexo, string filtroCodigoEmpresa, string filtroNombreEmpresa, CategoriaEscuelaEnum? filtroTipoCategoria,
                                                     TipoEducacionEnum? filtroTipoEducacion, int? filtroNivelEducativo, EstadoEmpresaEnum? filtroEstado, int? filtroDepartamento, int? filtroLocalidad,
                                                     string filtroOrdenPago, string filtroProgPresupuestario, DependenciaEnum? filtroDependencia, AmbitoEscuelaEnum? filtroAmbito, List<int> listaZonas, bool filtroPublica, bool filtroPrivada);
        

        DtoConsultaEscuela GetDtoById(int id);
    }
}

