using Siage.Base.Dto;
using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using Siage.Base;
using Siage.Base.Dto;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoEmpresaBase : IDao<EmpresaBase, int>
    {
        List<DtoEmpresaInspeccionadaPorInspeccion> GetEmpresasInspeccionadasPorInspeccionId(int idInspeccion);
        DtoTipoEmpresa GetTipoEmpresaById(int idEmpresa);

        List<EmpresaBase> GetByFiltrosBasico(string codigoEmpresa, string nombreEmpresa, int? idLocalidad, string barrio,
                                             string calle, int? altura, List<EstadoEmpresaEnum> estadoEmpresaEnum,
                                             List<TipoEmpresaFiltroBusquedaEnum> empresaFiltro,
                                             int? idDireccionDeNivelActual, int? idEmpresaUsuarioLogueado);
        List<EmpresaBase> GetEmpresasDependientes(int idEmpresaPadre, string codigoEmpresa, string nombreEmpresa);
        List<EmpresaBase> GetEmpresasDependientes(EmpresaBase empresaBase);
        List<EmpresaBase> GetByFiltroAvanzado(DateTime? fechaAltaDesde, DateTime? fechaAltaHasta,
                                              DateTime? fechaInicioActividadDesde, DateTime? fechaInicioActividadHasta,
                                              TipoEmpresaEnum? tipoEmpresaEnum, int? idProgramaPresupuestario,
                                              List<EstadoEmpresaEnum> estadoEmpresaEnum, int? idLocalidad, string barrio,
                                              string calle, int? altura,string nombre,DateTime? fechaDesdeNotificacion, DateTime? fechaHastaNotificacion,
                                              int? idEmpresaUsuarioLogueado);
        List<string> GetCamposConInstrumentoLegal();                
        void CambiarTipoEmpresa(int idEmpresa, TipoEmpresaEnum tipoEmpresaNueva, string seters);
        bool NombreRepetido(string nombre, int id);
        bool CodigoRepetido(string codigoEmpresa, int id);
        bool ValidarEstructuraDefinitiva(int escuelaId);
        List<VinculoEmpresaEdificio> GetVinculosCompletos(int idEmpresa);
        bool EsEscuela(int idEmpresa);
        bool EsDireccionDeNivel(int idEmpresa);
        List<DtoEmpresaConsultaEdificio> GetEmpresasByEdificios(List<int> listaEdificios);

        DtoEstadoEmpresa GetEstadoByid(int idEscuela);

        bool FechaInicioActividadesValidaPuestoTrabajo(EmpresaBase empresa);
        bool FechaInicioActividadesValidaPlanDeEstudio(EmpresaBase empresa);
        bool EsDireccionDeNivelSuperior(int idEmpresa);
        void ValidarCodigoInspeccionRepetido(int idEmpresa, string codigoInspeccion);
        bool PoseeEmpresasHijasNoCerradasORechazadas(int id);
    }
}

