using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoDireccionDeNivel : IDao<DireccionDeNivel, int>
    {
        bool ValidarDireccionNivelSuperior(int idEmpresaLogueado);
        List<DireccionDeNivel> GetByFiltrosBasico(string codigoEmpresa, string nombreEmpresa, int? idLocalidad, string barrio, string calle, int? altura, List<EstadoEmpresaEnum> estadoEmpresaEnum, int? idDireccionDeNivelActual, int? idEmpresaUsuarioLogueado);
        List<DireccionDeNivel> GetByFiltroAvanzado(DateTime? fechaAltaDesde, DateTime? fechaAltaHasta,
                                                   DateTime? fechaInicioActividadDesde,
                                                   DateTime? fechaInicioActividadHasta, TipoEmpresaEnum? tipoEmpresaEnum,
                                                   int? idProgramaPresupuestario,
                                                   List<EstadoEmpresaEnum> estadoEmpresaEnum, int? idLocalidad,
                                                   string barrio, string calle, int? altura,
                                                   TipoEducacionEnum? tipoEducacionEnum, int? IdnivelEducativo,
                                                   string nombre, DateTime? fechaDesdeNotificacion,
                                                   DateTime? fechaHastaNotificacion, int? idEmpresaUsuarioLogueado);
        bool SiglaRepetida(string sigla,int idEmpresa);
        List<NivelEducativo> GetNivelesEducativosByDireccionDeNivelId(int id);
        List<DireccionDeNivel> GetDireccionesDeNivelByNivelEducativo(int nivelId);

        bool EsNivelSuperior(int idEscuelaLogueado);
    }
}

